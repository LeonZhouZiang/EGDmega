using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartGraph : MonoBehaviour
{
    public Survivor currentSurvivor;
    public Monster monster;

    public Button HeadBtn;
    public Button BodyBtn;
    public Button ArmsBtn;
    public Button LegsBtn;

    public void AttackHead()
    {
        GameManager.Instance.uiManager.playerAttackBtn.interactable = false;
        GameManager.Instance.combatManager.monster.GetComponent<Monster>().partToBeHit = "Head";
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

    public void AttackArms()
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
