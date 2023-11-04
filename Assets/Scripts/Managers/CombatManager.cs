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
    public Action<int> currentExecutingAction;

    //all incoming actions
    public Queue<KeyValuePair<int, Queue<SingleAction>>> allActionsQueue = new();
    private Queue<SingleAction> currentActionQueue;
    //all units
    public Queue<GameObject> allUnits = new();

    public GameObject turnOwner;

    public override void PostAwake()
    {
        allUnits.Enqueue(player1);
        allUnits.Enqueue(player2);
        allUnits.Enqueue(monster);
    }

    public override void PreUpdate()
    {
        int a  = 0;
    }

    
    public async Task EndCurrentTurn()
    {
        var tmp = allUnits.Dequeue();
        allUnits.Enqueue(tmp);

        await Task.Delay(TimeSpan.FromSeconds(2));
        turnOwner = allUnits.Peek();
        StartNewTurn();
        
    }

    public void StartNewTurn()
    {
        //check setup time 0 cards
        ActivatePreparedAction();
        //unit action
        if (turnOwner.CompareTag("Monster"))
        {
            monster.GetComponent<Monster>().CheckCurrentActionCard();
        }
        else if(turnOwner.CompareTag("Player"))
        {

        }
    }

    private void ActivatePreparedAction()
    {
        currentActionQueue = allActionsQueue.Dequeue().Value;
        SingleAction action = currentActionQueue.Dequeue();
        Debug.Log("Do action:" + action.actionName);

    }

    public void WaitingForDice()
    {
        GameManager.Instance.diceSystem.ReceiveAction(currentExecutingAction);
    }

    internal void Activate(MonsterActionCard actionCard)
    {
        currentActionCard = actionCard;

        //throw new NotImplementedException();
    }
}
