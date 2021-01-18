using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TempManager))]


public class Managers : MonoBehaviour
{
    public static GameManager Game { get; private set; }
    public static TempManager Temp { get; private set; }
    public static UIManager UI { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    private List<IGameManager> _startSequence;

    public static SoundManager Sound{ get; private set; }

    private void Awake()
    {

        Game = GetComponent<GameManager>();
        Temp = GetComponent<TempManager>();
        Inventory = GetComponent<InventoryManager>();
        UI = GetComponent<UIManager>();
        Sound = GetComponent<SoundManager>(); 

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Game);
        _startSequence.Add(Temp);
        _startSequence.Add(Inventory);
        _startSequence.Add(Sound);
        _startSequence.Add(UI);
        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        foreach (IGameManager manager in _startSequence)
        {
            manager.Startup();
        }
        yield return null;
        int numModules = _startSequence.Count;
        int numReady = 0;
        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;
            foreach (IGameManager manager in _startSequence)
                if (manager.status == ManagerStatus.Started)
                    numReady++;
            if (numReady > lastReady)
                Debug.Log("Progress: " + numReady +
                "/" + numModules);
            yield return null;
        }
        Debug.Log("All managers started up");

    }
    private void Update()
    {
        if (Game.IsPlaying())
        {
            
        }
    }
}