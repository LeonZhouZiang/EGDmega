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

    public delegate void PreUpdates();
    public delegate void PostUpdates ();
    public event PreUpdates PreUpdatesHandler;
    public event PostUpdates PostUpdatesHandler;

    void Awake()
    {
        Instance = this;
        combatManager = gameObject.GetComponent<CombatManager>();
        uiManager = gameObject.GetComponent<UIManager>();
        mouseStateManager = gameObject.GetComponent<MouseStateManager>();
        mapManager = gameObject.GetComponent<MapManager>();

        PreUpdatesHandler += combatManager.PreUpdate;
        PreUpdatesHandler += uiManager.PreUpdate;
        PreUpdatesHandler += mouseStateManager.PreUpdate;
        PreUpdatesHandler += mapManager.PreUpdate;

        PostUpdatesHandler += combatManager.PostUpdate;
        PostUpdatesHandler += uiManager.PostUpdate;
        PostUpdatesHandler += mouseStateManager.PostUpdate;
        PostUpdatesHandler += mapManager.PostUpdate;

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
