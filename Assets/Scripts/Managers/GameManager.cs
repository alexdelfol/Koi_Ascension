using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class GameManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    public GameStatus gameStatus { get; private set; }

    [Header("Variables Globales")]
    public float Speed;
    public float speedMod;

    [Header("Koi")]
    public Koi koi;

    [Header("Movimiento de Background")]
    public GameObject fondoIzq01;
    public GameObject fondoIzq02;
    public GameObject fondoMid01;
    public GameObject fondoMid02;
    public GameObject fondoDer01;
    public GameObject fondoDer02;

    [Header("Skins del Koi")]
    public Sprite originalCenter;
    public Sprite originalLeft;
    public Sprite originalRight;
    public Sprite verdeCenter;
    public Sprite verdeLeft;
    public Sprite verdeRight;
    public Sprite ninjaCenter;
    public Sprite ninjaLeft;
    public Sprite ninjaRight;

    private const string SKIN_KEY = "Skin";

    [Header("Transicion de Noche")]
    public GameObject efecto_noche;
    private Color ncolor;
    private float alphaNoche;

    public GameObject transformacion_animation;

    //private bool InGame = false;
    private int Score = 0;
    private float distance = 0;
    private float initSpeed = 1;

    private const string TOPSCORE = "TopScore";
    private int bestScore;

    //Star effect
    private bool staractive = false;
    private bool staraway = false;
    private float oldSpeed = 1;
    private float tempSpeed = 1;

    public float distanceMod = 1.5f;


    //Espacio
    [Header("Espacio")]
    public Dragon dragon;

    private bool inSpace = false;
    public GameObject fondoEspacio01;
    public GameObject fondoEspacio02;

    public GameObject estrellas01;
    public GameObject estrellas01s;
    public GameObject estrellas02;

    public GameObject botonSkip;



    [Header("Debug")]
    public bool debugMode;
    public GameObject debugButton;
    public GameObject debugPanel;

    private int dbCount=0;

    // Start is called before the first frame update
    void IGameManager.Startup()
    {
        status = ManagerStatus.Initializing;
        Debug.Log("Game manager starting...");
        Speed = initSpeed;
        Score = 0;

        efecto_noche.SetActive(true);
        ncolor = efecto_noche.GetComponent<SpriteRenderer>().color;
        alphaNoche = ncolor.a;
        ncolor.a = 0;
        efecto_noche.GetComponent<SpriteRenderer>().color = ncolor;

        inSpace = false;

        transformacion_animation.SetActive(false);

        fondoIzq01.SetActive(true);
        fondoIzq02.SetActive(true);
        fondoDer01.SetActive(true);
        fondoDer02.SetActive(true);
        fondoMid01.SetActive(true);
        fondoMid02.SetActive(true);

        fondoEspacio01.SetActive(false);
        fondoEspacio02.SetActive(false);
        estrellas01.SetActive(false);
        estrellas01s.SetActive(false);
        estrellas02.SetActive(false);

        dragon.gameObject.SetActive(false);

        string skin = PlayerPrefs.GetString(SKIN_KEY);
        if (skin != null)
        {
            ChangeSkin(skin);
            actualizarVisualPanelCustominazion(skin);
        }

        bestScore = PlayerPrefs.GetInt(TOPSCORE);

        debugButton.SetActive(debugMode);
        dbCount = 0;

        botonSkip.SetActive(false);

       

        gameStatus = GameStatus.Init;
        status = ManagerStatus.Started;
    }

    // Update is called once per frame
    void Update()
    {
        if(status == ManagerStatus.Started)
        {
            if(gameStatus == GameStatus.Playing)
            {
                distance = distance + Time.deltaTime * Speed * distanceMod;

                //Noche
                if(distance > 125 && distance < 300)
                {
                    ncolor.a = alphaNoche * ((distance - 125) / (250 - 125));
                    efecto_noche.GetComponent<SpriteRenderer>().color = ncolor;

                    if(distance > 230 && Managers.Temp.IsSpawnActive())
                    {
                        Managers.Temp.DeactivateSpawn();
                    }

                    if(distance > 250)
                    {
                        gameStatus = GameStatus.Trasformation;
                        transformacion_animation.SetActive(true);
                        transformacion_animation.GetComponent<Trasformacion>().InitTansform();

                    }
                    
                }


                Score = (int) distance;
                if(staractive)
                {
                    Speed = tempSpeed;
                } else if(staraway)
                {
                    if(tempSpeed>oldSpeed)
                    {
                        tempSpeed -= Time.deltaTime * (oldSpeed/4);
                        Speed = tempSpeed;
                    } else
                    {
                        staraway = false;
                        Speed = oldSpeed;
                    }
                }
                else
                {
                    Speed = Speed + Time.deltaTime * speedMod;
                }

                //Movimiento del background------------------------------------------------------------------------
                if (inSpace)
                {
                    fondoEspacio01.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * Speed * 0.2f);
                    if (fondoEspacio01.transform.position.y < -16.38)
                    {
                        fondoEspacio01.transform.Translate(new Vector3(0, 16.38f*2f, 0));
                    }

                    fondoEspacio02.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * Speed * 0.2f);
                    if (fondoEspacio02.transform.position.y < -16.38)
                    {
                        fondoEspacio02.transform.Translate(new Vector3(0, 16.38f * 2f, 0));
                    }

                    estrellas01.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * Speed * 0.5f);
                    if (estrellas01.transform.position.y < -18)
                    {
                        estrellas01.transform.Translate(new Vector3(0, (18 + 22), 0));
                    }

                    estrellas01s.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * Speed * 0.5f);
                    if (estrellas01s.transform.position.y < -18)
                    {
                        estrellas01s.transform.Translate(new Vector3(0, (18 + 22), 0));
                    }

                    estrellas02.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * Speed * 0.1f);
                    if (estrellas02.transform.position.y < -18)
                    {
                        estrellas02.transform.Translate(new Vector3(0, (18+22), 0));
                    }
                   
                }
                else
                {
                    fondoIzq01.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * Speed);
                    if (fondoIzq01.transform.position.y < -12)
                    {
                        fondoIzq01.transform.Translate(new Vector3(0, 24, 0));
                    }

                    fondoIzq02.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * Speed);
                    if (fondoIzq02.transform.position.y < -12)
                    {
                        fondoIzq02.transform.Translate(new Vector3(0, 24, 0));
                    }

                    fondoMid01.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * Speed);
                    if (fondoMid01.transform.position.y < -12)
                    {
                        fondoMid01.transform.Translate(new Vector3(0, 24, 0));
                    }

                    fondoMid02.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * Speed);
                    if (fondoMid02.transform.position.y < -12)
                    {
                        fondoMid02.transform.Translate(new Vector3(0, 24, 0));
                    }

                    fondoDer01.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * Speed);
                    if (fondoDer01.transform.position.y < -12)
                    {
                        fondoDer01.transform.Translate(new Vector3(0, 24, 0));
                    }

                    fondoDer02.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * Speed);
                    if (fondoDer02.transform.position.y < -12)
                    {
                        fondoDer02.transform.Translate(new Vector3(0, 24, 0));
                    }
                }

                //Debug.Log("Speed: " + Speed);
            }

            //Debug.Log("Distancia: " + distancia_recorrida);
        }
    }

    public void SetGameStatus(GameStatus gs)
    {
        gameStatus = gs;
    }

    public void SetGameStatusPlaying()
    {
        gameStatus = GameStatus.Playing;
    }

    public void SetGameStatusPause()
    {
        if (gameStatus == GameStatus.Playing)
        {
            gameStatus = GameStatus.Pause;
        }

    }

    public void TogglePause()
    {
        if(gameStatus == GameStatus.Pause)
        {
            gameStatus = GameStatus.Playing;
        } else if(gameStatus == GameStatus.Playing)
        {
            gameStatus = GameStatus.Pause;
        }
    }

    public void Resume()
    {
        if (gameStatus == GameStatus.Pause)
        {
            gameStatus = GameStatus.Playing;
        }
    }


    public GameStatus GetGameStatus()
    {
        return gameStatus;
    }

    public bool IsPlaying()
    {
        return (gameStatus == GameStatus.Playing);
    }

    public int GetScore()
    {
        return Score;
    }

    public void StarUp()
    {
        staractive = true;
        staraway = false;
        oldSpeed = Speed;
        tempSpeed = Speed * 2;
    }

    public void StarDown()
    {
        staractive = false;
        staraway = true;
    }

    public void GameOver()
    {
        gameStatus = GameStatus.GameOver;
        if(GetScore() > bestScore )
        {
            
            Managers.UI.ChangeScoreGMText("NUEVO RECORD \n\nDistancia: " + GetScore() + " m \n\nAntiguo Record: " + bestScore + " m");
            bestScore = GetScore();
            PlayerPrefs.SetInt(TOPSCORE, bestScore);
        } else
        {
            Managers.UI.ChangeScoreGMText("Distancia: " + GetScore() + " m \n\nRecord: " + bestScore + " m");
        }
        
        Managers.UI.gameOverPanel.SetActive(true);
        Managers.UI.scorePanel.SetActive(false);
        Managers.Sound.PlayDeathMusic();
        //Managers.Sound.StopMusic();     // Cambiar por musica de fin de partida
        SavePlayerPref();
    }

    public void ResetGame()
    {
        gameStatus = GameStatus.Init;
        foreach (Rock o in FindObjectsOfType<Rock>()){
            Destroy(o.gameObject);
        }

        foreach (Branch o in FindObjectsOfType<Branch>())
        {
            Destroy(o.gameObject);
        }

        foreach (Angry_Fish o in FindObjectsOfType<Angry_Fish>())
        {
            Destroy(o.gameObject);
        }

        foreach (Shield o in FindObjectsOfType<Shield>())
        {
            Destroy(o.gameObject);
        }

        foreach (Star o in FindObjectsOfType<Star>())
        {
            Destroy(o.gameObject);
        }

        foreach (Coin o in FindObjectsOfType<Coin>())
        {
            Destroy(o.gameObject);
        }

        Score = 0;
        distance = 0;
        Managers.UI.scoreText.GetComponent<Text>().text = Managers.Game.GetScore() + " m";

        staractive = false;
        staraway = false;
        Speed = initSpeed;

        koi.ResetPos();


        Managers.Sound.StopMusic();


        //Quitar Espacio
        Debug.Log("Set Active");
        fondoIzq01.SetActive(true);
        fondoIzq02.SetActive(true);
        fondoDer01.SetActive(true);
        fondoDer02.SetActive(true);
        fondoMid01.SetActive(true);
        fondoMid02.SetActive(true);

        fondoEspacio01.SetActive(false);
        fondoEspacio02.SetActive(false);
        estrellas01.SetActive(false);
        estrellas01s.SetActive(false);
        estrellas02.SetActive(false);

        inSpace = false;
        dragon.gameObject.SetActive(false);

        efecto_noche.SetActive(true);
        ncolor.a = 0;
        efecto_noche.GetComponent<SpriteRenderer>().color = ncolor;

        Managers.Temp.RiverLevelConf();
        
    }

    public void SavePlayerPref()
    {
        PlayerPrefs.Save();
    }


    public bool TransformationIsActive()
    {
        if (bestScore > 600)
        {
            botonSkip.SetActive(true);
        }
        else
        {
            botonSkip.SetActive(false);
        }

        return GameStatus.Trasformation == gameStatus;
    }

    public void PostTrasformation()
    {
        botonSkip.SetActive(false);

        distance = 301f;
        inSpace = true;
        efecto_noche.SetActive(false);
        SetGameStatusPlaying();
        fondoEspacio01.SetActive(true);
        fondoEspacio02.SetActive(true);
        estrellas01.SetActive(true);
        estrellas01s.SetActive(true);
        estrellas02.SetActive(true);

        fondoIzq01.SetActive(false);
        fondoIzq02.SetActive(false);
        fondoDer01.SetActive(false);
        fondoDer02.SetActive(false);
        fondoMid01.SetActive(false);
        fondoMid02.SetActive(false);

        Managers.Sound.PlayClip(Managers.Sound.SpaceTheme);

        dragon.gameObject.SetActive(true);
        dragon.ResetPos();
      

        foreach (Branch o in FindObjectsOfType<Branch>())
        {
            Destroy(o.gameObject);
        }

        foreach (Angry_Fish o in FindObjectsOfType<Angry_Fish>())
        {
            Destroy(o.gameObject);
        }

        foreach (Shield o in FindObjectsOfType<Shield>())
        {
            Destroy(o.gameObject);
        }

        foreach (Star o in FindObjectsOfType<Star>())
        {
            Destroy(o.gameObject);
        }

        foreach (Coin o in FindObjectsOfType<Coin>())
        {
            Destroy(o.gameObject);
        }


       

    }



    

    public void ChangeSkin(string skinName)
    {

        switch (skinName)
        {
            case "original":
                koi.originalKoiSprite = originalCenter;
                koi.leftKoi = originalLeft;
                koi.rightKoi = originalRight;
                PlayerPrefs.SetString(SKIN_KEY, skinName);
                break;
            case "verde":
                koi.originalKoiSprite = verdeCenter;
                koi.leftKoi = verdeLeft;
                koi.rightKoi = verdeRight;
                PlayerPrefs.SetString(SKIN_KEY, skinName);
                break;
            case "ninja":
                koi.originalKoiSprite = ninjaCenter;
                koi.leftKoi = ninjaLeft;
                koi.rightKoi = ninjaRight;
                PlayerPrefs.SetString(SKIN_KEY, skinName);
                break;
            default:
                koi.originalKoiSprite = originalCenter;
                koi.leftKoi = originalLeft;
                koi.rightKoi = originalRight;
                PlayerPrefs.SetString(SKIN_KEY, "original");
                break;
        }
        
        
    }



    private void actualizarVisualPanelCustominazion(string skin)
    {
        if (skin != "original")
        {
            FindObjectOfType<Managers>().GetComponent<UIManager>().originalSkin.SetActive(false);
            switch (skin)
            {
                case "verde":
                    FindObjectOfType<Managers>().GetComponent<UIManager>().verdeSkin.SetActive(true);
                    break;
                case "ninja":
                    FindObjectOfType<Managers>().GetComponent<UIManager>().ninjaSkin.SetActive(true);
                    break;
                default:
                    FindObjectOfType<Managers>().GetComponent<UIManager>().originalSkin.SetActive(true);
                    break;
            }
        }
    }
    


    //------------------------------------Debug---------------------------

    public void DebugDeleteAll()
    {
        /*if (EditorUtility.DisplayDialog("Delete all player preferences.",
             "Are you sure you want to delete all the player preferences? " +
             "This action cannot be undone.", "Yes", "No"))
         {
             Debug.Log("yes");*/
        PlayerPrefs.DeleteAll();
        //}
    }

    public void DebugTo200()
    {
        distance = 229;
    }

    public void DebugToSpace()
    {
        Managers.Temp.SpaceTime();
        koi.gameObject.SetActive(false);
        Managers.UI.optionPanel.SetActive(false);
        debugPanel.SetActive(false);

    }

    public void DebugActivate(int idb)
    {
       
        switch (idb)
        {
            case 0:
                if (dbCount < 6)
                {
                    dbCount++;
                }
                else if (dbCount == 11)
                {
                    //debugMode = true;
                    debugPanel.SetActive(true);
                }
                else
                {
                    dbCount = 0;
                }
                break;
            case 1:
                if (dbCount > 5 && dbCount < 9 )
                {
                    dbCount++;
                }
                else
                {
                    dbCount = 0;
                }
                break;
            case 2:
                if (dbCount > 8 && dbCount < 11 )
                {
                    dbCount++;
                }
                else
                {
                    dbCount = 0;
                }
                break;

        }
        Debug.Log("db: " + dbCount);
    }

    public void DebugDisable()
    {
        debugMode = false;
    }

}
