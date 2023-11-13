using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CameraManager cameraManager;

    public CombatManager combatManager = new();
    public UIManager uiManager = new();
    public MouseStateManager mouseStateManager = new();
    public MapManager mapManager = new();
    public DiceSystem diceSystem = new();
    public Astar astar = new();

    public CoroutineHelper coroutineHelper;

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
        cameraManager.Initialize();

        PostAwakesHandler.Invoke();
    }
    private void RegisterSystems()
    {
        foreach (var manager in mySystems)
        {
            PostAwakesHandler += manager.PostAwake;
            PreUpdatesHandler += manager.PreUpdate;
            PostUpdatesHandler += manager.PostUpdate;
        }
    }

    public void Start()
    {
    }

    public void EndGame()
    {
        SceneManager.LoadScene(0);
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
