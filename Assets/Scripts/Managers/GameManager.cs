using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CombatManager combatManager;
    public UIManager uiManager;
    public MouseStateManager mouseStateManager;
    public MapManager mapManager;
    public DiceSystem diceSystem;

    public delegate void PreUpdates();
    public delegate void PostUpdates ();
    public delegate void PostAwakes();
    public event PreUpdates PreUpdatesHandler;
    public event PostUpdates PostUpdatesHandler;
    public event PostAwakes PostAwakesHandler;

    void Awake()
    {
        Instance = this;
        combatManager = new CombatManager();
        uiManager = new UIManager();
        mouseStateManager = new MouseStateManager();
        diceSystem = new DiceSystem();

        PreUpdatesHandler += combatManager.PreUpdate;
        PreUpdatesHandler += uiManager.PreUpdate;
        PreUpdatesHandler += mouseStateManager.PreUpdate;
        PreUpdatesHandler += mapManager.PreUpdate;

        PostUpdatesHandler += combatManager.PostUpdate;
        PostUpdatesHandler += uiManager.PostUpdate;
        PostUpdatesHandler += mouseStateManager.PostUpdate;
        PostUpdatesHandler += mapManager.PostUpdate;

        PostAwakesHandler += combatManager.PostAwake;
        PostAwakesHandler += uiManager.PostAwake;
        PostAwakesHandler += mouseStateManager.PostAwake;
        PostAwakesHandler += mapManager.PostAwake;

        PostAwakesHandler.Invoke();
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
