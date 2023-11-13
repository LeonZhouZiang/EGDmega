using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SurvivorInfoPanel: MonoBehaviour
{
    public TextMeshProUGUI survivorName;

    public TextMeshProUGUI headHP;
    public TextMeshProUGUI bodyHP;
    public TextMeshProUGUI armsHP;
    public TextMeshProUGUI legsHP;
    public TextMeshProUGUI totalHealth;
    public Image weaponSlot;

    public GameObject actionPanel;
    public GameObject infoPanel;

    public void UpdateInfo(Survivor survivor)
    {
        survivorName.text = survivor.survivorName;
        headHP.text = survivor.bodyParts["Head"].health.ToString();
        bodyHP.text = survivor.bodyParts["Body"].health.ToString();
        armsHP.text = survivor.bodyParts["Arms"].health.ToString();
        legsHP.text = survivor.bodyParts["Legs"].health.ToString();
        totalHealth.text = (survivor.bodyParts["Head"].health + survivor.bodyParts["Body"].health + survivor.bodyParts["Arms"].health + survivor.bodyParts["Legs"].health).ToString();

        weaponSlot.sprite = survivor.weapon.image;
    }

}
