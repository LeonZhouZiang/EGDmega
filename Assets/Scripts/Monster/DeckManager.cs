using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public Monster monster; // 从 Inspector 中设置
    public UIManager uiManager; // 从 Inspector 中设置

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            monster.DrawNewActionCard();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            monster.ShuffleCards(); // 重新洗牌
        }
    }
}
