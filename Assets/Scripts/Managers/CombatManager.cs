using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[System.Serializable]
public class CombatManager : IManager
{
    public bool isPreActionPhase = true;

    public GameObject player1;
    public GameObject player2;
    public GameObject monster;
    [HideInInspector]
    public MonsterActionCard currentActionCard;
    //single action now
    private SingleAction currentAction;
    private Survivor currentTargetPlayer;
    private Queue<Survivor> hitQueue;

    private MonsterPartition partition;
    //all incoming actions
    public List<Queue<SingleAction>> allActionsQueue;
    private Queue<SingleAction> currentActionQueue;
    //all units
    public Queue<GameObject> allUnits = new();
    [HideInInspector]
    public GameObject turnOwner;

    public override void PostAwake()
    {
        allUnits.Enqueue(monster);
        allUnits.Enqueue(player1);
        allUnits.Enqueue(player2);
        //initialize list
        allActionsQueue = new();
        for(int i = 0; i < 5; i++)
        {
            allActionsQueue.Add(new Queue<SingleAction>());
        }

        StartGame();
    }

    public void StartGame()
    {
        turnOwner = monster;
        GameManager.Instance.uiManager.UpdateOwnerText(turnOwner.name);
        monster.GetComponent<Monster>().ShuffleCard();
        StartNewTurn();
    }
    public override void PreUpdate()
    {
    }

    
    public async Task EndCurrentTurn()
    {
        var tmp = allUnits.Dequeue();
        allUnits.Enqueue(tmp);
        //set owner
        turnOwner = allUnits.Peek();
        GameManager.Instance.uiManager.UpdateOwnerText(turnOwner.name);

        CameraManager.Instance.MoveToTarget(turnOwner.transform.position);
        await Task.Delay(TimeSpan.FromSeconds(0.8));
        StartNewTurn();
    }

    public void StartNewTurn()
    {
        Debug.Log(monster.GetComponent<Monster>().shuffledDeck.Count + " cards remaining in deck, Start new unit turn");
        //check setup time 0 cards
        isPreActionPhase = true;
        ActivatePreparedAction();
        //unit action
    }

    private void ActivatePreparedAction()
    {
        currentActionQueue = allActionsQueue[0];
        allActionsQueue.RemoveAt(0);
        Debug.Log(currentActionQueue.Count.ToString() + "actions to be done");

        //rescale list, set last item clean
        for(int i = 1; i < allActionsQueue.Count; i++)
        {
            allActionsQueue[i-1] = allActionsQueue[i];
        }
        allActionsQueue[^1] = new();

        //do all ready actions
        DoActionInQueueRecursively();
    }

    public async Task DoActionInQueueRecursively()
    {
        if(currentActionQueue.Count == 0)
        {
            StartOwnerTurnAsync();
            return;
        }
        else
        {
            SingleAction action = currentActionQueue.Dequeue();
            CameraManager.Instance.MoveToTarget(action.Owner.transform.position);

            action.GetTargets();
            await (action.effect as IEffect).ActionEffect();

        }
    }

    public async Task DoMonsterInstanceActionsRecursively()
    {
        if (currentActionQueue.Count == 0)
        {
            EndCurrentTurn();
            return;
        }
        else
        {
            Debug.Log(currentActionQueue.Peek().actionName);
            SingleAction action = currentActionQueue.Dequeue();

            await CameraManager.Instance.MoveToTarget(monster.transform.position);
            currentAction = action;

            //if has target
            currentAction.GetTargets();
            await currentAction.ActionEffects();
        }
    }

    private async Task StartOwnerTurnAsync()
    {
        await GameManager.Instance.coroutineHelper.NewTurnAnimation();
        isPreActionPhase = false;
        Debug.Log("Owner: " + turnOwner.name);
        await CameraManager.Instance.MoveToTarget(turnOwner.transform.position);

        if(turnOwner.CompareTag("Player"))
        {
            turnOwner.GetComponent<Survivor>().canAttack = true;
            turnOwner.GetComponent<Survivor>().canMove = true;
            GameManager.Instance.uiManager.ShowSurvivorActionPanel(turnOwner.GetComponent<Survivor>());
        }
        else
        {
            GameManager.Instance.uiManager.HideSurvivorActionPanel();
            GameManager.Instance.uiManager.ShowMonsterInfo(turnOwner.GetComponent<Monster>());
            //GameManager.Instance.uiManager.survivorInfoPanel.gameObject.SetActive(false);
            monster.GetComponent<Monster>().CheckCurrentActionCard();
        }
    }

    public bool CheckCardComplete(MonsterActionCard currentActionCard)
    {
        foreach(var q in allActionsQueue)
        {
            foreach(var action in q)
            {
                if (currentActionCard.lastAction == action)
                    return false;
            }
        }
        //last action used already, need new card
        return true;
    }


    public void Activate(MonsterActionCard actionCard)
    {
        
        foreach(var phase in actionCard.phaseList)
        {
            if(phase.prepareTime == 0)
            {
                Queue<SingleAction> queue = new Queue<SingleAction>(phase.actions);
                currentActionQueue = queue;
                Debug.Log(currentActionQueue.Count.ToString() + " instant actions");
                DoMonsterInstanceActionsRecursively();
            }
            else//register actions
            {
                for(int i = 0; i < allActionsQueue.Count; i++)
                {
                    if(i+1 == phase.prepareTime)
                    {
                        foreach(var action in phase.actions)
                        {
                            allActionsQueue[i].Enqueue(action);
                        }
                    }
                }
            }
        }
    }


    //helper
    #region combat functions

    public List<Survivor> GetSurvivorsInRange(Vector3[] range)
    {
        List<Survivor> survivors = new();
        foreach (var svr in allUnits)
        {
            if (svr.CompareTag("Player"))
            {
                foreach (var location in range)
                {
                    if (svr.transform.position == location)
                    {
                        survivors.Add(svr.GetComponent<Survivor>());
                    }
                }
            }
        };
        return survivors;
    }

    public bool GetNearestPlayerPos(int searchRange, out Vector3 target)
    {
        int dis1 = GameManager.Instance.astar.GetDistanceBetweenWorldPos(player1.transform.position, monster.transform.position);
        int dis2 = GameManager.Instance.astar.GetDistanceBetweenWorldPos(player2.transform.position, monster.transform.position);
        if(dis1 <= searchRange && dis2 <= searchRange)
        {
            if (dis1 == dis2)
            {
                target = UnityEngine.Random.value < 0.5f ? player1.transform.position : player2.transform.position;
                return true;
            }
            else
            {
                target = dis1 < dis2 ? player1.transform.position : player2.transform.position;
                return true;
            }
        }
        else if (dis1 <= searchRange)
        {
            target = player1.transform.position;
            return true;
        }
        else if(dis2 <= searchRange)
        {
            target = player2.transform.position;
            return true;
        }
        else
        {
            target = Vector3.zero;
            return false;
        }
    }

    

    public void PlayerEvadeCheck_(int diceValue)
    {
        if(diceValue == 6)
        {

        }
        else
        {
            //not hit
            if(diceValue <= currentTargetPlayer.survivorInfo.dex)
            {
                Debug.Log("Evaded!");
                //process next
                ProcessTargetsOneByOne(myDamage);
            }
            else
            {
                currentTargetPlayer.waitingDamage = myDamage;
                GameManager.Instance.diceSystem.RequireAction(currentTargetPlayer.ChooseInjurePart);
                
            }
        }
    }
    

    public void SetupHitQueue(List<Survivor> survivors, int damage)
    {
        hitQueue = new(survivors);
        ProcessTargetsOneByOne(damage);
    }

    private int myDamage;
    private async Task ProcessTargetsOneByOne(int damage)
    {
        myDamage = damage;
        if(hitQueue.Count != 0)
        {
            currentTargetPlayer = hitQueue.Dequeue();
            await CameraManager.Instance.MoveToTarget(currentTargetPlayer.transform.position);
            GameManager.Instance.uiManager.UpdateStateText(currentTargetPlayer.name + "Evasion check.");
            GameManager.Instance.diceSystem.RequireAction(PlayerEvadeCheck_);
        }
        else
        {
            Debug.Log("Effect done");
            if (isPreActionPhase)
                //check if still has effects
                DoActionInQueueRecursively();
            else
                DoMonsterInstanceActionsRecursively();
        }
    }

    #endregion
}
