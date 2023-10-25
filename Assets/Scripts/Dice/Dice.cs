using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public bool inHand = false;
    public float height;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (inHand)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = new Vector3(mouseWorldPos.x, height, mouseWorldPos.y);
            transform.position = position;
        }
    }


}
