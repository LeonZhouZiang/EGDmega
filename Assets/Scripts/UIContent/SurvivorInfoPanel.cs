using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class SurvivorInfoPanel: MonoBehaviour
{
    public TextMeshProUGUI survivorName;

    public TextMeshProUGUI headHP;
    public TextMeshProUGUI bodyHP;
    public TextMeshProUGUI handHP;
    public TextMeshProUGUI legHP;
    public TextMeshProUGUI totalHealth;

    public SpriteRenderer weaponSlot;
}
