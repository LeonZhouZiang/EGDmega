using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CombatManager combatManager = new();
    public UIManager uiManager = new();
    public MouseStateManager mouseStateManager = new();
    public MapManager mapManager = new();
    public DiceSystem diceSystem = new();
    public Astar astar = new();

    public delegate void MyHandler();

    public event MyHandler PreUpdatesHandler;
    public event MyHandler PostUpdatesHandler;
    public event MyHandler PostAwakesHandler;

    private List<IManager> mySystems = new();

    void Awake()
    {
        Instance = this;

        mySystems.Add(combatManager);
        mySystems.Add(uiManager);
        mySystems.Add(mouseStateManager);
        mySystems.Add(mapManager);
        mySystems.Add(diceSystem);
        mySystems.Add(astar);
        RegisterSystems();
        
        PostAwakesHandler.Invoke();
    }

    private void RegisterSystems()
    {
        

        foreach (var manager in mySystems)
        {
            Debug.Log(manager.GetType().Name + " registered!");

            PostAwakesHandler += manager.PostAwake;
            PreUpdatesHandler += manager.PreUpdate;
            PostUpdatesHandler += manager.PostUpdate;
        }
    }
    private void Start()
    {
    }

    void Update()
    {
        PreUpdatesHandler.Invoke();
        PostUpdatesHandler.Invoke();
    }
    private void LateUpdate()
    {
        
    }
}
