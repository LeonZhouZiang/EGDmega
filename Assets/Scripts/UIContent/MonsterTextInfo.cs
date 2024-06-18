using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterTextInfo : MonoBehaviour
{
    public TextMeshProUGUI monsterName;
    public TextMeshProUGUI totalHealth;

    public TextMeshProUGUI headHealth;
    public TextMeshProUGUI bodyHealth;
    public TextMeshProUGUI clawHealth;
    public TextMeshProUGUI legsHealth;
    
    public void UpdateInfo(Monster monster)
    {
        totalHealth.text = "Total health: " + monster.TotalHealth.ToString();
        headHealth.text = "Head: " + monster.bodyParts["Head"].health.ToString();
        bodyHealth.text = "Body: " + monster.bodyParts["Body"].health.ToString();
        clawHealth.text = "Claws: " + monster.bodyParts["Claws"].health.ToString();
        legsHealth.text = "Legs: " + monster.bodyParts["Legs"].health.ToString();
    }
}
