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
    public SurvivorInfoPanel survivorInfoPanel;
    public GameObject survivorActionPanle;
    //public SpriteRenderer survivorItem;

    [Header("Monster info")]

    public GameObject monsterInfoPanel;
    public GameObject monsterDesk;
    public TextMeshProUGUI monsterInfo;
    public TextMeshProUGUI deckCountText;
    public Monster monster;
    public GameObject monsterBodyPartPanel;

    public GameObject reticle;
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
    public void HideDiceText()
    {
        diceValueText.gameObject.SetActive(false); 
    }

    //显示角色属性
    public void ShowSurvivorInfo(Survivor survivor)
    {
        Debug.Log("Show player info");
        survivorInfoPanel.gameObject.SetActive(true);
        survivorInfoPanel.survivorName.text = survivor.name;
        survivorInfoPanel.weaponSlot.sprite = survivor.items.image;

        CameraManager.Instance.MoveToTarget(survivor.transform.position);
    }
    public void HideSurvivorInfo()
    {
        survivorInfoPanel.gameObject.SetActive(false);
        CameraManager.Instance.ResetPosition();
    }
    //显示角色属性 + 行动
    public void ShowSurvivorActionPanel(Survivor survivor)
    {
        ShowSurvivorInfo(survivor);
        survivorActionPanle.SetActive(true);
    }
    //monster
    public void ShowMonsterInfo(Monster monster)
    {
        monsterInfoPanel.SetActive(true);
        monsterDesk.SetActive(true);

        CameraManager.Instance.MoveToTarget(monster.transform.position);
    }
    public void HideMonsterInfo()
    {
        monsterInfoPanel.SetActive(false);
        monsterDesk.SetActive(false);
        CameraManager.Instance.ResetPosition();
    }

    public void UpdateStateText(string content)
    {
        stateText.text = content;
    }

   internal void ShuffleCards()
    {
        // Check if the deckCountText or monster reference is null before trying to access them
        if (deckCountText == null)
        {
            Debug.LogError("deckCountText is not set on the UIManager.");
            return;
        }
        if (monster == null || monster.shuffledDeck == null)
        {
            Debug.LogError("Monster or shuffledDeck is not initialized.");
            return;
        } 

        // Update the UI text with the current count of the shuffled deck
        deckCountText.text = monster.shuffledDeck.Count.ToString();
        // Add any additional UI updates for shuffling here
    }


    //reticle
    public void SetReticle(Vector2 pos, float height = 0.5f, float scale = 1f)
    {
        reticle.SetActive(true);
        Vector3 position = new(pos.x, height, pos.y);
        reticle.transform.position = position;
        reticle.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void CleanReticle()
    {
        reticle.SetActive(false);
    }

    internal void ShowMonsterBodyParts()
    {

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
