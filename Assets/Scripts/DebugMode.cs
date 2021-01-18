using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NextRoca()
    {
        Managers.Temp.SpawnThing(SpawnItem.Rock);
    }

    public void NextRama()
    {
        Managers.Temp.SpawnThing(SpawnItem.Branch);
    }

    public void NextPiranya()
    {
        Managers.Temp.SpawnThing(SpawnItem.Angry_Fish);
    }

    public void NextShield()
    {
        Managers.Temp.SpawnThing(SpawnItem.Booster);
    }
}
