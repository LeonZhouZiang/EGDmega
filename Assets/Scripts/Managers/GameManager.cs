using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CombatManager combatManager;
    public UIManager uiManager;
    public MouseStateManager mouseStateManager;
    public MapManager mapManager;

    public DiceSystem diceSystem;
    public Astar astar;

    public delegate void PreUpdates();
    public delegate void PostUpdates ();
    public delegate void PostAwakes();

    public event PreUpdates PreUpdatesHandler;
    public event PostUpdates PostUpdatesHandler;
    public event PostAwakes PostAwakesHandler;

    private List<IManager> mySystems = new();

    void Awake()
    {
        Instance = this;

        combatManager = new CombatManager();
        mySystems.Add(combatManager);
        uiManager = new UIManager();
        mySystems.Add(uiManager);

        mouseStateManager = new MouseStateManager();
        mySystems.Add(mouseStateManager);
        mapManager = new MapManager();
        mySystems.Add(mapManager);
        diceSystem = new DiceSystem();
        mySystems.Add(diceSystem);
        astar = new Astar();
        mySystems.Add(astar);

        RegisterSystems();
        
        PostAwakesHandler.Invoke();
    }

    private void RegisterSystems()
    {
        Type type = typeof(IManager);
        foreach(var manager in mySystems)
        {
            PreUpdatesHandler += manager.PreUpdate;
            PostUpdatesHandler += manager.PostUpdate;
            PostAwakesHandler += manager.PostAwake;
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
