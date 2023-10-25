using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSystem : MonoBehaviour
{
    public GameObject dice;
    public GameObject DiceCanvas;


    public int Roll()
    {
        int value = 1;
        return value;
    }

    public void ShowDice()
    {
        DiceCanvas.SetActive(true);
        dice.SetActive(true);
    }
}
