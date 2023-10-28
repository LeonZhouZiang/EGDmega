using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public Rigidbody rb;

    public bool inHand = false;
    private bool stable = false;

    public float height;
    public int value;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (stable) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.collider.gameObject == gameObject && Input.GetMouseButton(0))
                {
                    inHand = true;
                }
            }
        }

        if (inHand)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = new Vector3(mouseWorldPos.x, Mathf.Lerp(transform.position.y, height ,0.8f), mouseWorldPos.y);
            transform.position = position;

            if (Input.GetMouseButtonUp(0) && rb.velocity.magnitude + rb.angularVelocity.magnitude >= 5)
            {
                Debug.Log(rb.velocity.magnitude + rb.angularVelocity.magnitude);
                inHand = false;
                stable = false;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                inHand = false;
            }
        }
        else
        {
            if(!stable && rb.velocity.magnitude + rb.angularVelocity.magnitude <= 0.1f)
            {
                stable = true;
                GameManager.Instance.diceSystem.CurrentValue = value;
            }
        }
    }

    public void FastGetValue()
    {
        inHand = false;
        stable = true;
        GameManager.Instance.diceSystem.CurrentValue = Random.Range(1, 7);
        
    }
}
