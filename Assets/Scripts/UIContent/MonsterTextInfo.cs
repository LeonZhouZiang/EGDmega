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
        totalHealth.text = monster.totalHealth.ToString();
        headHealth.text = monster.bodyParts["Head"].health.ToString();
        bodyHealth.text = monster.bodyParts["Head"].health.ToString();
        clawHealth.text = monster.bodyParts["Head"].health.ToString();
        legsHealth.text = monster.bodyParts["Head"].health.ToString();
    }
}
