using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class UIManager : IManager
{
    TextMeshProUGUI stateText;

    

    [Header("Survivor info")]
    GameObject survivorInfoPanel;
    TextMeshProUGUI survivorText;
    SpriteRenderer survivorItem;

    [Header("Monster info")]

    GameObject monsterInfoPanel;
    TextMeshProUGUI monsterInfo;
    GameObject monsterCardDesk;

    public GameObject monsterReticle;
    public override void PostAwake()
    {

    }
    
    public override void PreUpdate()
    {

    }
    public override void PostUpdate()
    {
    }
    //œ‘ æΩ«…´ Ù–‘
    public void ShowSurvivorInfo(Survivor survivor)
    {

        survivorInfoPanel.SetActive(true);
        survivorText.text = survivor.name;
        survivorItem.sprite = survivor.items.image;

    }
    public void CloseSurvivorInfo()
    {
        survivorInfoPanel.SetActive(false);
    }

    //monster
    public void ShowMonsterInfo(GameObject monster)
    {
        Monster m = monster.GetComponent<Monster>();
        monsterInfoPanel.SetActive(true);
    }

    public void UpdateStateText(string content)
    {
        stateText.text = content;
    }

    internal void ShuffleCards()
    {
        throw new NotImplementedException();
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
