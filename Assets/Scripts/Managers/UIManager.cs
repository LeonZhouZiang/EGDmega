using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class UIManager : IManager
{
    TextMeshProUGUI stateText;

    public GameObject reticle;
    public override void PostAwake()
    {

    }
    
    public override void PreUpdate()
    {

    }
    public override void PostUpdate()
    {
    }


    public void UpdateStateText(string content)
    {
        stateText.text = content;
    }

    public void SetReticle(Vector2 pos, float height = 0.5f, float scale = 1f)
    {
        reticle.SetActive(true);
        Vector3 position = new(pos.x, height, pos.y);
        reticle.transform.position = position;
        reticle.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void CleanReticle()
    {
        reticle.SetActive(false);
    }
}
