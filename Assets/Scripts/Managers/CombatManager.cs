using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[System.Serializable]
public class CombatManager : IManager
{
    public GameObject player1;
    public GameObject player2;
    public GameObject monster;

    public MonsterActionCard currentActionCard;
    //single action now
    private Action<int> currentExecutingAction;

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

        //do all actions
        StartNextActionInQueueRecursively();
    }

    public void StartNextActionInQueueRecursively()
    {
        if(currentActionQueue.Count == 0)
        {
            StartOwnerAction();
            return;
        }
        else
        {
            SingleAction action = currentActionQueue.Dequeue();
            //more...
        }
    }

    private void StartOwnerAction()
    {
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
        //not found, need new card
        return true;
    }

    public void EndSurviorTurn()
    {
        StartNewTurn();
    }

    public void WaitingForDice()
    {
        GameManager.Instance.diceSystem.ReceiveAction(currentExecutingAction);
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
                StartNextActionInQueueRecursively();
            }
            else
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
    public void FindNearestSurvivor(Monster monster)
    {

    }
}
