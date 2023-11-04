using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIManager : IManager
{
    public TextMeshProUGUI stateText;


    [Header("Survivor info")]
    public GameObject survivorInfoPanel;
    public TextMeshProUGUI survivorText;
    public SpriteRenderer survivorItem;

    [Header("Monster info")]

    public GameObject monsterInfoPanel;
    public TextMeshProUGUI monsterInfo;
    public GameObject monsterCardDesk;
    public TextMeshProUGUI monsterCardNum;

    public GameObject monsterReticle;
    [Header("Dice")]
    public Button confirmRollBtn;
    public Button fastRollBtn;
    public TextMeshProUGUI diceValueText;
    public override void PostAwake()
    {

    }
    

    public void UpdateValueText(int value)
    {
        diceValueText.gameObject.SetActive(true);
        diceValueText.text = "Roll Result: " + value.ToString();
    }
    public void HideDice()
    {
        diceValueText.gameObject.SetActive(false); 
    }

    //œ‘ æΩ«…´ Ù–‘
    public void ShowSurvivorInfo(Survivor survivor)
    {
        Debug.Log("Show player info");
        survivorInfoPanel.SetActive(true);
        survivorText.text = survivor.name;
        survivorItem.sprite = survivor.items.image;

    }
    public void HideSurvivorInfo()
    {
        survivorInfoPanel.SetActive(false);
    }

    //monster
    public void ShowMonsterInfo(GameObject monster)
    {
        Monster m = monster.GetComponent<Monster>();
        monsterInfoPanel.SetActive(true);
        Debug.Log("Show monster info");
    }
    public void HideMonsterInfo()
    {
        monsterInfoPanel.SetActive(false);
    }

    public void UpdateStateText(string content)
    {
        stateText.text = content;
    }

    internal void ShuffleCards()
    {
        monsterCardNum.text = "6";
    }

    //reticle
    public void SetReticle(Vector2 pos, float height = 0.5f, float scale = 1f)
    {
        monsterReticle.SetActive(true);
        Vector3 position = new(pos.x, height, pos.y);
        monsterReticle.transform.position = position;
        monsterReticle.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void CleanReticle()
    {
        monsterReticle.SetActive(false);
    }

    //public void SetMonsterTarget(GameObject target)
    //{
    //    monsterReticle.SetActive(true);
    //    monsterReticle.transform.position = target.transform.position;
    //}
    //public void CleanMonsterTarget()
    //{
    //    monsterReticle.SetActive(false);
    //}
}
