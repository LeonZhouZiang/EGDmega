using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartGraph : MonoBehaviour
{
    [HideInInspector]
    public Survivor currentSurvivor;
    [HideInInspector]
    public Monster monster;

    public Button HeadBtn;
    public Button BodyBtn;
    public Button ClawBtn;
    public Button LegsBtn;

    public void Awake()
    {
        HeadBtn.onClick.AddListener(AttackHead);
        BodyBtn.onClick.AddListener(AttackBody);
        ClawBtn.onClick.AddListener(AttackClaw);
        LegsBtn.onClick.AddListener(AttackLegs);
    }
    public void AttackHead()
    {
        currentSurvivor.canAttack = false;
        monster.partToBeHit = "Head";
        GameManager.Instance.uiManager.playerAttackBtn.interactable = false;
        GameManager.Instance.uiManager.UpdateStateText("Hit Check");
        GameManager.Instance.diceSystem.RequireAction(currentSurvivor.MonsterEvadeCheck);
    }

    public void AttackBody()
    {
        GameManager.Instance.uiManager.playerAttackBtn.interactable = false;
        GameManager.Instance.combatManager.monster.GetComponent<Monster>().partToBeHit = "Body";
        GameManager.Instance.uiManager.UpdateStateText("Hit Check");
        GameManager.Instance.diceSystem.RequireAction(currentSurvivor.MonsterEvadeCheck);
    }

    public void AttackClaw()
    {
        GameManager.Instance.uiManager.playerAttackBtn.interactable = false;
        GameManager.Instance.combatManager.monster.GetComponent<Monster>().partToBeHit = "Claws";
        GameManager.Instance.uiManager.UpdateStateText("Hit Check");
        GameManager.Instance.diceSystem.RequireAction(currentSurvivor.MonsterEvadeCheck);
    }

    public void AttackLegs()
    {
        GameManager.Instance.uiManager.playerAttackBtn.interactable = false;
        GameManager.Instance.combatManager.monster.GetComponent<Monster>().partToBeHit = "Legs";
        GameManager.Instance.uiManager.UpdateStateText("Hit Check");
        GameManager.Instance.diceSystem.RequireAction(currentSurvivor.MonsterEvadeCheck);
    }
}
