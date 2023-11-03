using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        GameManager.Instance.uiManager.confirmRollBtn.onClick.AddListener(ConfirmRoll);
        GameManager.Instance.uiManager.fastRollBtn.onClick.AddListener(dice.FastGetValue);
    }


    public void ReceiveAction(Action<int> action)
    {
        CurrentWaitingAction = action;
        ShowDice();
    }

    public void ShowDice()
    {
        diceCamera.gameObject.SetActive(true);
        DiceCanvas.SetActive(true);
        diceObject.SetActive(true);

        GameManager.Instance.uiManager.UpdateValueText(dice.value);
    }

    public void ConfirmRoll()
    {
        CurrentWaitingAction.Invoke(currentValue);
        diceCamera.gameObject.SetActive(false);
        DiceCanvas.SetActive(false);
        diceObject.SetActive(false);

        GameManager.Instance.uiManager.HideDice();
    }

    public void ReceiveValueFromDice(int value)
    {
        CurrentValue = value;
        GameManager.Instance.uiManager.confirmRollBtn.gameObject.SetActive(true);
        GameManager.Instance.uiManager.UpdateValueText(value);
    }

    
}
