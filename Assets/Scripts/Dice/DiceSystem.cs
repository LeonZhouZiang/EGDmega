using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DiceSystem : IManager
{
    public GameObject diceObject;
    public GameObject DiceCanvas;
    public Camera diceCamera;

    private Action<int> CurrentWaitingAction;
    private Dice dice;
    private int currentValue = 0;

    public int CurrentValue { set => currentValue = value; get => currentValue; }

    public override void PostAwake()
    {
        dice = diceObject.GetComponent<Dice>();
    }

    public void ShowDice()
    {
        diceCamera.gameObject.SetActive(true);
        DiceCanvas.SetActive(true);
        diceObject.SetActive(true);
    }

    public void ReceiveAction(Action<int> action)
    {
        CurrentWaitingAction = action;
    }
    public void ConfirmRoll()
    {
        CurrentWaitingAction.Invoke(currentValue);
        diceCamera.gameObject.SetActive(false);
        DiceCanvas.SetActive(false);
        diceObject.SetActive(false);
    }


    
}
