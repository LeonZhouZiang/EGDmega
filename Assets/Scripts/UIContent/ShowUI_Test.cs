using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowUI_Test : MonoBehaviour
{
    public TMP_Text UIText;
    // Start is called before the first frame update
    void Start()
    {
        UIText.text = UIContentDatabase.GetContentById(1);
    }
   
}
