using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    public SpawnItem lastItem { get; private set; }

    [Header("Obstaculos prefabs")]
    public Rock rock;
    public Branch branch;
    public Angry_Fish angry_fish;
    public Rock blackHole;
    public Branch asteroid;

    [Header("Booster prefabs")]
    public Shield shield;
    public Star star;
    public Coin coin;
    //private float speed = 1f;
    private float spawnCoolDown = 1f;
    private bool firstSpawn = true;



    //Variables Director
    private int numSpawns = 0;
    private int numBoosterSpawn = 0;

    private bool activateSpawn = true;


    //Espacio
    private bool inSpace = false;


    //Debug
    private bool dbNextP = false;
    private int dbItem = 0;

    // Start is called before the first frame update
    void IGameManager.Startup()
    {
        status = ManagerStatus.Initializing;
        Debug.Log("Temp manager starting...");

        //speed = Managers.Game.Speed;
        firstSpawn = true;
        numSpawns = 0;
        numBoosterSpawn = 0;
        inSpace = false;

        status = ManagerStatus.Started;
    }

    // Update is called once per frame
    void Update()
    {
        if (status == ManagerStatus.Started && Managers.Game.IsPlaying())
        {
            if(activateSpawn)
            {
                DirectorSpawnLauncher();
            }
            
        }
        
    }




    private void RandomSpawn()
    {
        if (spawnCoolDown <= 0)
        {
            spawnCoolDown = 4f; //Default Spawn Cooldown

            int rspawn = UnityEngine.Random.Range(1, 14);
            //rspawn = 13;

            if (dbNextP)
            {
                rspawn = dbItem;
                dbNextP = false;
            }

            //TO DO: Actualizar con un switch, cooldown boost
            if (rspawn < 6 || firstSpawn)
            {
                Console.WriteLine("Case Rock");
                Instantiate(rock, new Vector3(UnityEngine.Random.Range(-1.7f, 1.7f), 6, 0), Quaternion.identity);
            }
            else if (rspawn < 8)
            {
                Console.WriteLine("Case Branch");
                Instantiate(branch, new Vector3(UnityEngine.Random.Range(-1.7f, 1.7f), 6, 0), Quaternion.identity);
            }
            else if (rspawn < 12)
            {
                Console.WriteLine("Case Angry Fish");
                Instantiate(angry_fish, new Vector3(UnityEngine.Random.Range(-1.7f, 1.7f), -5, 0), Quaternion.identity);
                spawnCoolDown += 4f;
            }
            else
            {
                Console.WriteLine("Case Shiedl");
                Instantiate(shield, new Vector3(UnityEngine.Random.Range(-1.7f, 1.7f), 6, 0), Quaternion.identity);

            }
            //Instantiate(rock, new Vector3(UnityEngine.Random.Range(-1.7f, 1.7f), 6, 0), Quaternion.identity);

            firstSpawn = false;

        }
        else
        {
            spawnCoolDown -= Time.deltaTime * Managers.Game.Speed;
        }
    }


    private void DirectorSpawnLauncher()
    {
        if (spawnCoolDown <= 0)
        {
            spawnCoolDown = 4f;
            //Special Spawns
            switch (numSpawns)
            {
                case 0:
                    SpawnThing(SpawnItem.Rock);
                    break;
                case 5:
                    if(numBoosterSpawn == 0)
                    {
                        SpawnThing(SpawnItem.Booster);
                    } else
                    {
                        SpawnThing(SpawnItem.Angry_Fish);
                        spawnCoolDown = 2f;
                        if (!inSpace)
                        {
                            Instantiate(rock, new Vector3(0, 6, 0), Quaternion.identity);
                        }
                        else
                        {

                            Instantiate(blackHole, new Vector3(0, 6, 0), Quaternion.identity);
                        }
                    }
                    break;
                case 14:
                    SpawnThing(SpawnItem.Angry_Fish);
                    spawnCoolDown = 2f;
                    if (!inSpace)
                    {
                        Instantiate(rock, new Vector3(0, 6, 0), Quaternion.identity);
                    }
                    else
                    {

                        Instantiate(blackHole, new Vector3(0, 6, 0), Quaternion.identity);
                    }
                    break;
                case 27:
                    Instantiate(star, new Vector3(UnityEngine.Random.Range(-1.7f, 1.7f), 6, 0), Quaternion.identity);
                    numSpawns++;
                    numBoosterSpawn++;
                    break;
                case 30:
                    numSpawns = 0;
                    numBoosterSpawn = 0;
                    SpawnThing(SpawnItem.Angry_Fish);
                    break;
                default:
                    DirectorSpawn();
                    break;

            }

        } else
        {
            spawnCoolDown -= Time.deltaTime * Managers.Game.Speed;
        }
    }

    private void DirectorSpawn()
    {
        SpawnItem nextItem = SpawnItem.Rock;
        //Depende del ultimo objeto invocado
        switch (lastItem) {
            case SpawnItem.Rock:
                nextItem = Random2Spawn(50,25,15,10);
                break;
            case SpawnItem.Branch:
               nextItem = Random2Spawn(40,30,5,10);
                break;
            case SpawnItem.Angry_Fish:
                nextItem = Random2Spawn(60, 20, 5, 15);
                break;
            case SpawnItem.Booster:
                nextItem = Random2Spawn(70, 10, 10, 0);
                break;
            default:
                break;

        }

        //Spawn Item
        SpawnThing(nextItem);
        
        
    }


    public void SpawnThing(SpawnItem si)
    {
        bool scoin = true;
        float rnumber = UnityEngine.Random.Range(-1.7f, 1.7f);
        switch (si)
        {
            case SpawnItem.Rock:
                //Console.WriteLine("Case Rock");
                if (!inSpace)
                {
                    Instantiate(rock, new Vector3(rnumber, 6, 0), Quaternion.identity);
                }
                else
                {

                    Instantiate(blackHole, new Vector3(rnumber, 6, 0), Quaternion.identity);
                }
                break;
            case SpawnItem.Branch:
                //Console.WriteLine("Case Branch");
                if (!inSpace)
                {
                    Instantiate(branch, new Vector3(rnumber, 6, 0), Quaternion.identity);
                }
                else
                {

                    Instantiate(asteroid, new Vector3(rnumber, 6, 0), Quaternion.identity);
                }
                Instantiate(coin, new Vector3(UnityEngine.Random.Range(-1.7f, 1.7f), 7, 0), Quaternion.identity);
                Instantiate(coin, new Vector3(UnityEngine.Random.Range(-1.7f, 1.7f), 5, 0), Quaternion.identity);
                scoin = false;
                break;
            case SpawnItem.Angry_Fish:
                //Console.WriteLine("Case Angry Fish");
                if (!inSpace)
                {
                    Instantiate(angry_fish, new Vector3(rnumber, -5, 0), Quaternion.identity);
                }
                else
                {

                    //Instantiate(blackHole, new Vector3(rnumber, 6, 0), Quaternion.identity);
                }
                break;
            case SpawnItem.Booster:
                //Console.WriteLine("Case Shiedl");
                SpawnBooster();
                numBoosterSpawn += 1;
                scoin = false;
                break;
            default:
                break;
         
        }

        numSpawns += 1;
        lastItem = si;

        if(scoin)
        {
            if(rnumber > 0.4f)
            {
                Instantiate(coin, new Vector3(UnityEngine.Random.Range(-1.7f, -0.2f), 6, 0), Quaternion.identity);
            } else if (rnumber < -0.4f)
            {
                Instantiate(coin, new Vector3(UnityEngine.Random.Range(0, 1.7f), 6, 0.2f), Quaternion.identity);
            } else
            {
                if(UnityEngine.Random.Range(0,1) == 1)
                {
                    Instantiate(coin, new Vector3(1.2f, 6, 0), Quaternion.identity);
                } else
                {
                    Instantiate(coin, new Vector3(-1.2f, 6, 0), Quaternion.identity);
                }
            }
            
        }
    }
    
    private SpawnItem Random2Spawn(int rock,int branch,int angry_fish,int booster)
    {
        SpawnItem res = SpawnItem.Rock;
        int total = rock + branch + angry_fish + booster;
        branch = rock + branch;
        angry_fish = branch + angry_fish;
        
        int rspawn = UnityEngine.Random.Range(1, total);
        if(rspawn < rock)
        {
            res = SpawnItem.Rock;
        } else if(rspawn < branch)
        {
            res = SpawnItem.Branch;
        } else if(rspawn < angry_fish)
        {
            res = SpawnItem.Angry_Fish;
        } else
        {
            res = SpawnItem.Booster;
        }
        //Branch rate
        //Bad Fish Rate
        //Booster Rate

        return res;
    }

    private void SpawnBooster()
    {
        int rb = UnityEngine.Random.Range(1, 5);
        if(rb < 4)
        {
            Instantiate(shield, new Vector3(UnityEngine.Random.Range(-1.7f, 1.7f), 6, 0), Quaternion.identity);
        } else
        {
            Instantiate(star, new Vector3(UnityEngine.Random.Range(-1.7f, 1.7f), 6, 0), Quaternion.identity);
        }
    }


    //Debug
    public void DbSetNextItem(int i)
    {
        dbItem = i;
        dbNextP = true;
    }

    public void ActivateSpawn()
    {
        activateSpawn = true;
    }

    public void DeactivateSpawn()
    {
        activateSpawn = false;
    }

    public bool IsSpawnActive()
    {
        return activateSpawn;
    }

    public void SpaceTime()
    {
        //Cambiar Skins
        activateSpawn = true;
        inSpace = true;
        Managers.Game.PostTrasformation();


    }

    public void RiverLevelConf()
    {
        inSpace = false;
    }

    public void DebugSpawnStar()
    {
        Instantiate(star, new Vector3(UnityEngine.Random.Range(-1.7f, 1.7f), 6, 0), Quaternion.identity);
    }

}
