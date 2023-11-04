using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public Monster monster; // �� Inspector ������
    public UIManager uiManager; // �� Inspector ������

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            monster.DrawNewActionCard();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            monster.ShuffleCards(); // ����ϴ��
        }
    }
}
