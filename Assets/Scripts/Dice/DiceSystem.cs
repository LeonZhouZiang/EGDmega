using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DiceSystem : IManager
{
    public GameObject diceObject;
    public GameObject dicePlate;
    public GameObject diceCanvas;
    public Camera diceCamera;
    [Header("UI")]
    public TextMeshProUGUI diceValueText;
    /// <summary>
    /// show current checking content
    /// </summary>
    public TextMeshProUGUI hintText;
    public GameObject partExplaination;
    public GameObject deadExplaination;
    [Header("Buttons")]
    public Button confirmRollBtn;
    public Button fastRollBtn;

    private Action<int> CurrentWaitingAction;
    private Dice dice;
    private int currentValue = 0;

    public int CurrentValue { set => currentValue = value; get => currentValue; }

    public override void PostAwake()
    {
        dice = diceObject.GetComponent<Dice>();
        confirmRollBtn.onClick.AddListener(ConfirmRoll);
        fastRollBtn.onClick.AddListener(dice.FastGetValue);
    }

    public void UpdateValueText(int value)
    {
        diceValueText.gameObject.SetActive(true);
        diceValueText.text = "You got: " + value.ToString();
    }
    public void HideValueText()
    {
        diceValueText.gameObject.SetActive(false);
    }

    public void RequireAction(Action<int> action, string textContent)
    {
        hintText.text = textContent;
        diceValueText.text = "(Require a roll)";
        CurrentWaitingAction = action;
        ShowDice();
    }

    public void ShowDice()
    {
        GameManager.Instance.mouseStateManager.allowedToClick = false;

        dice.isFinished = false;
        diceCamera.gameObject.SetActive(true);
        diceCanvas.SetActive(true);
        dicePlate.SetActive(true);
        diceObject.SetActive(true);
        fastRollBtn.gameObject.SetActive(true);
        //UpdateValueText(dice.value);
    }

    public void ConfirmRoll()
    {
        GameManager.Instance.mouseStateManager.allowedToClick = true;

        diceCamera.gameObject.SetActive(false);
        confirmRollBtn.gameObject.SetActive(false);
        diceValueText.gameObject.SetActive(false);
        diceCanvas.SetActive(false);
        diceObject.SetActive(false);
        dicePlate.SetActive(false);

        HideValueText();
        HideHintText();

        CurrentWaitingAction.Invoke(currentValue);
    }


    public void ReceiveValueFromDice(int value)
    {
        CurrentValue = value;
        confirmRollBtn.gameObject.SetActive(true);
        UpdateValueText(value);
    }

    public void HideHintText()
    {
        hintText.text = "";
    }

    /// <summary>
    /// 0 = taking injure part check
    /// 1 = death check
    /// </summary>
    /// <param name="index"></param>
    public void UpdateExplanation(int index)
    {
        if(index == 0)
        {
            partExplaination.SetActive(true);
        }
        else if(index == 1)
        {
            deadExplaination.SetActive(true);
        }
    }
    public void HideExplanation()
    {
        partExplaination.SetActive(false);
        deadExplaination.SetActive(false);
    }
}
