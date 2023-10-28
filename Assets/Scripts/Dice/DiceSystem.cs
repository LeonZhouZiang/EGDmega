using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DiceSystem : IManager
{
    public GameObject diceObject;
    public GameObject DiceCanvas;

    private Action<int> CurrentWaitingAction;
    private Dice dice;
    private int currentValue = 0;

    public int CurrentValue { set => currentValue = value; get => currentValue; }
    
    public void ConfirmRoll()
    {
        CurrentWaitingAction.Invoke(currentValue);
    }

    public void ShowDice()
    {
        DiceCanvas.SetActive(true);
        diceObject.SetActive(true);
    }

    public void ReceiveAction(Action<int> action)
    {
        CurrentWaitingAction = action;
    }

    public void PostAwake()
    {
        dice = diceObject.GetComponent<Dice>();
    }
    public void PreUpdate()
    {

    }

    public void PostUpdate()
    {
    }

    public void PreLateUpdate()
    {
    }

    public void PostLateUpdate()
    {
    }

    
}
