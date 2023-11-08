using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[System.Serializable]
public class CombatManager : IManager
{
    private bool preActionPhase = true;

    public GameObject player1;
    public GameObject player2;
    public GameObject monster;

    public MonsterActionCard currentActionCard;
    //single action now
    private SingleAction currentAttackAction;
    private Survivor currentTargetPlayer;
    private Action<int> ConditionChecking;
    private Queue<Survivor> hitQueue;
    private MonsterPartition partition;
    //all incoming actions
    public List<Queue<SingleAction>> allActionsQueue;
    private Queue<SingleAction> currentActionQueue;
    //all units
    public Queue<GameObject> allUnits = new();

    public GameObject turnOwner;

    public override void PostAwake()
    {
        allUnits.Enqueue(player1);
        allUnits.Enqueue(player2);
        allUnits.Enqueue(monster);
        //initialize list
        allActionsQueue = new();
        for(int i = 0; i < 5; i++)
        {
            allActionsQueue.Add(new Queue<SingleAction>());
        }
    }


    public override void PreUpdate()
    {
    }

    
    public async Task EndCurrentTurn()
    {
        var tmp = allUnits.Dequeue();
        allUnits.Enqueue(tmp);
        turnOwner = allUnits.Peek();

        CameraManager.Instance.MoveToTarget(turnOwner.transform.position);
        await Task.Delay(TimeSpan.FromSeconds(0.8));
        StartNewTurn();
    }

    public void StartNewTurn()
    {
        //check setup time 0 cards
        preActionPhase = true;
        ActivatePreparedAction();
        //unit action
        if (turnOwner.CompareTag("Monster"))
        {
            
        }
        else if(turnOwner.CompareTag("Player"))
        {

        }
    }

    private void ActivatePreparedAction()
    {
        currentActionQueue = allActionsQueue[0];
        allActionsQueue.RemoveAt(0);
        Debug.Log(allActionsQueue[0].Count.ToString() + "actions to be done");

        //rescale list
        for(int i = 1; i < allActionsQueue.Count; i++)
        {
            allActionsQueue[i-1] = allActionsQueue[i];
        }
        allActionsQueue[~1] = new();

        //do all ready actions
        DoActionInQueueRecursively();
    }

    public void DoActionInQueueRecursively()
    {
        if(currentActionQueue.Count == 0)
        {
            StartOwnerTurn();
            return;
        }
        else
        {
            SingleAction action = currentActionQueue.Dequeue();
            if (action.effect.GetTargets())
                action.effect.ActionEffect();
        }
    }

    public void DoMonsterInstanceActionsRecursively()
    {
        if (currentActionQueue.Count == 0)
        {
            EndCurrentTurn();
            return;
        }
        else
        {
            SingleAction action = currentActionQueue.Dequeue();
            currentAttackAction = action;
            //if has target
            if(currentAttackAction.effect.GetTargets())
                currentAttackAction.effect.ActionEffect();
        }
    }

    private void StartOwnerTurn()
    {
        preActionPhase = false;

        CameraManager.Instance.MoveToTarget(turnOwner.transform.position);
        
        if(turnOwner.CompareTag("Player"))
        {
            GameManager.Instance.uiManager.ShowSurvivorActionPanel(turnOwner.GetComponent<Survivor>());
        }
        else
        {
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
        currentActionCard = actionCard;

        foreach(var phase in currentActionCard.phaseList)
        {
            if(phase.prepareTime == 0)
            {
                Queue<SingleAction> queue = new Queue<SingleAction>(phase.actions);
                currentActionQueue = queue;
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

    #region combat functions
    public void WaitingForDice()
    {
        GameManager.Instance.diceSystem.ReceiveAction(ConditionChecking);
    }

    public void WaitingForPart()
    {
        GameManager.Instance.uiManager.ShowMonsterBodyParts();
    }

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

    public Vector3 GetNearestPlayerPos()
    {
        int dis1 = GameManager.Instance.astar.GetDistanceBetweenWorldPos(player1.transform.position, monster.transform.position);
        int dis2 = GameManager.Instance.astar.GetDistanceBetweenWorldPos(player2.transform.position, monster.transform.position);
        if(dis1 == dis2)
        {
            return UnityEngine.Random.value < 0.5f ? player1.transform.position : player2.transform.position;
        }
        else
        {
            return dis1 < dis2 ? player1.transform.position : player2.transform.position;
        }
    }

    public void MonsterEvadeCheck(int diceValue)
    {
        if (diceValue == 6)
        {
            monster.GetComponent<Monster>().React(diceValue, partition);
        }
        else
        {
            if (diceValue >= currentAttackAction.Owner.gameObject.GetComponent<Survivor>().weapon.dexOffset
                             - currentAttackAction.Owner.gameObject.GetComponent<Survivor>().survivorInfo.dex)
            {
                monster.GetComponent<Monster>().React(diceValue, partition);
            }
        }
    }

    public void PlayerEvadeCheck_(int diceValue)
    {
        if(diceValue == 6)
        {

        }
        else
        {
            
            if(diceValue <= currentAttackAction.Owner.gameObject.GetComponent<Survivor>().survivorInfo.dex)
            {

            }
        }
    }
    public void StrengthCheck(int diceValue)
    {
        if (diceValue == 6)
        {
            
        }
        else
        {
            if (diceValue <= currentAttackAction.Owner.gameObject.GetComponent<Survivor>().survivorInfo.dex)
            {

            }
        }
    }

    public void SetupHitQueue(List<Survivor> survivors, int damage)
    {
        hitQueue = new(survivors);
        ProcessTargetsOneByOne(damage);
    }

    private void ProcessTargetsOneByOne(int damage)
    {
        if(hitQueue.Count != 0)
        {
            currentTargetPlayer = hitQueue.Dequeue();
            CameraManager.Instance.MoveToTarget(currentTargetPlayer.transform.position);
            GameManager.Instance.uiManager.UpdateStateText(currentTargetPlayer.name + "evasion check.");
            GameManager.Instance.diceSystem.ReceiveAction(PlayerEvadeCheck_);
        }
        else
        {
            Debug.Log("Effect done");
            if (preActionPhase)
                //check if still has effects
                DoActionInQueueRecursively();
            else
                DoMonsterInstanceActionsRecursively();
        }
    }

    #endregion
}
