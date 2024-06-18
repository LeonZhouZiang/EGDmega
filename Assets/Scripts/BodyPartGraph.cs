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

        HeadBtn.image.alphaHitTestMinimumThreshold = 0.5f;
        BodyBtn.image.alphaHitTestMinimumThreshold = 0.5f;
        ClawBtn.image.alphaHitTestMinimumThreshold = 0.5f;
        LegsBtn.image.alphaHitTestMinimumThreshold = 0.5f;
    }
    public void Start()
    {
        monster = GameManager.Instance.combatManager.monster.GetComponent<Monster>();
    }
    public void AttackHead()
    {
        currentSurvivor.canAttack = false;
        currentSurvivor.aimmingPartition = "Head";
        monster.partToBeHit = "Head";

        GameManager.Instance.uiManager.HideMonsterBodyParts();
        GameManager.Instance.uiManager.playerAttackBtn.interactable = false;
        GameManager.Instance.diceSystem.RequireAction(currentSurvivor.PlayerAccuracyCheck, "Accuracy check");
    }

    public void AttackBody()
    {
        currentSurvivor.canAttack = false;
        currentSurvivor.aimmingPartition = "Body";
        monster.partToBeHit = "Body";

        GameManager.Instance.uiManager.HideMonsterBodyParts();
        GameManager.Instance.uiManager.playerAttackBtn.interactable = false;
        GameManager.Instance.diceSystem.RequireAction(currentSurvivor.PlayerAccuracyCheck, "Accuracy check");
    }

    public void AttackClaw()
    {
        currentSurvivor.canAttack = false;
        currentSurvivor.aimmingPartition = "Claws";
        monster.partToBeHit = "Claws";

        GameManager.Instance.uiManager.HideMonsterBodyParts();
        GameManager.Instance.uiManager.playerAttackBtn.interactable = false;
        GameManager.Instance.diceSystem.RequireAction(currentSurvivor.PlayerAccuracyCheck, "Accuracy check");
    }

    public void AttackLegs()
    {
        currentSurvivor.canAttack = false;
        currentSurvivor.aimmingPartition = "Legs";
        monster.partToBeHit = "Legs";

        GameManager.Instance.uiManager.HideMonsterBodyParts();
        GameManager.Instance.uiManager.playerAttackBtn.interactable = false;
        GameManager.Instance.diceSystem.RequireAction(currentSurvivor.PlayerAccuracyCheck, "Accuracy check");
    }

    public void DisableButton(string name)
    {
        if (name == "Head") HeadBtn.interactable = false;
        else if(name == "Body") BodyBtn.interactable = false;
        else if(name == "Claws") ClawBtn.interactable = false;
        else if(name == "Legs") LegsBtn.interactable = false;
    }
}
