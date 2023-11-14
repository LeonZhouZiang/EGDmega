using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIManager : IManager
{
    [Header("Game Info")]
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI ownerText;

    [Header("Survivor info")]
    public SurvivorInfoPanel survivorInfoPanel;
    public GameObject survivorActionPanel;
    public Button playerAttackBtn;
    public Button playerMoveBtn;
    public Button endTurnBtn;
    //public SpriteRenderer survivorItem;

    [Header("Monster info")]

    public MonsterTextInfo monsterInfo;
    public GameObject monsterDesk;
    public BodyPartGraph monsterBodyPartPanel;
    public TextMeshProUGUI deckCountText;
    public Monster monster;

    public GameObject reticle;
    [Header("Dice")]
    public Button confirmRollBtn;
    public Button fastRollBtn;
    public TextMeshProUGUI diceValueText;
    public override void PostAwake()
    {
        endTurnBtn.onClick.AddListener(() => { if(GameManager.Instance.mouseStateManager.allowedToClick) GameManager.Instance.combatManager.EndCurrentTurn();});
    }
    

    public void UpdateValueText(int value)
    {
        diceValueText.gameObject.SetActive(true);
        diceValueText.text = "Roll Result: " + value.ToString();
    }

    public void UpdateOwnerText(string turnOwner)
    {
        ownerText.text = turnOwner + "'s turn";
    }
    public void HideDiceText()
    {
        diceValueText.gameObject.SetActive(false); 
    }

    //显示角色属性


    public async Task ShowSurvivorInfo(Survivor survivor)
    {
        survivorInfoPanel.gameObject.SetActive(true);
        survivorInfoPanel.UpdateInfo(survivor);

        await CameraManager.Instance.MoveToTarget(survivor.transform.position);
    }
    public async Task HideSurvivorInfo()
    {
        survivorInfoPanel.gameObject.SetActive(false);
        await CameraManager.Instance.ResetPosition();
    }
    //显示角色属性 + 行动
    public void ShowSurvivorActionPanel(Survivor survivor)
    {
        survivorActionPanel.SetActive(true);
        GameManager.Instance.uiManager.playerMoveBtn.interactable = true;
        GameManager.Instance.uiManager.playerAttackBtn.interactable = true;

        playerMoveBtn.onClick.RemoveAllListeners();
        playerAttackBtn.onClick.RemoveAllListeners();
        playerAttackBtn.onClick.AddListener(survivor.RequireAttack);
        playerMoveBtn.onClick.AddListener(survivor.RequireMove);

        ShowSurvivorInfo(survivor);
    }
    public void HideSurvivorActionPanel()
    {
        survivorActionPanel.SetActive(false);
    }

    //monster
    public async Task ShowMonsterInfo(Monster monster)
    {
        
        monsterDesk.SetActive(true);
        monsterInfo.gameObject.SetActive(true);
        monsterInfo.UpdateInfo(monster);
        await CameraManager.Instance.MoveToTarget(monster.transform.position);
    }
    public async Task HideMonsterInfo()
    {
        monsterInfo.gameObject.SetActive(false);
        monsterDesk.SetActive(false);
        await CameraManager.Instance.ResetPosition();
    }

    public void UpdateStateText(string content)
    {
        stateText.text = content;
    }

   internal void UpdateDeckCount()
    {
        // Check if the deckCountText or monster reference is null before trying to access them
        if (deckCountText == null)
        {
            Debug.LogWarning("deckCountText is not set on the UIManager.");
            return;
        }
        if (monster == null || monster.shuffledDeck == null)
        {
            Debug.LogWarning("Monster or shuffledDeck is not initialized.");
            return;
        } 

        // Update the UI text with the current count of the shuffled deck
        deckCountText.text = monster.shuffledDeck.Count.ToString();
        // Add any additional UI updates for shuffling here
    }


    //reticle
    public void SetReticle(Vector3 pos, float height = 0.05f, float scale = 1f)
    {
        if (!reticle.activeInHierarchy)
        {
            reticle.SetActive(true);
            Vector3 position = new(pos.x, height, pos.z);
            reticle.transform.position = position;
            reticle.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public void CleanReticle()
    {
        if(reticle.activeInHierarchy)
            reticle.SetActive(false);
    }

    internal void ShowMonsterBodyParts(Survivor s)
    {
        monsterBodyPartPanel.currentSurvivor = s;
        monsterBodyPartPanel.gameObject.SetActive(true);
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
