using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public Rigidbody rb;
    public Camera diceCamera;
    public float height;
    public float maxVelocityMagnitude;
    [HideInInspector]
    public int value;

    private Vector3 velocity;
    private bool inHand = false;
    private bool stable = false;

    void Start()
    {
        
    }

    private void SetIgnore()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero; 

        Transform[] all = GetComponentsInChildren<Transform>();
        int LayerIgnore = LayerMask.NameToLayer("Ignore All");
        foreach(var a in all)
        {
            a.gameObject.layer = LayerIgnore;
        }
    }

    private void ResetIgnore()
    {
        if(velocity.magnitude > maxVelocityMagnitude)
        {
            velocity = velocity.normalized * maxVelocityMagnitude;
        }
        Vector3 newVelocity = velocity;
        rb.velocity = velocity;
        rb.useGravity = true;

        Transform[] all = GetComponentsInChildren<Transform>();
        int layer = LayerMask.NameToLayer("Dice");
        foreach (var a in all)
        {
            a.gameObject.layer = layer;
        }
    }

    void Update()
    {
        if (stable) 
        {
            Ray ray = diceCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.collider.gameObject == gameObject && Input.GetMouseButton(0))
                {
                    inHand = true;
                    SetIgnore();
                }
            }
        }

        if (inHand)
        {
            Ray ray = diceCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("Dice")))
            {
                Vector3 position = Vector3.Lerp(transform.position, hit.point + hit.normal * height, 0.8f);
                velocity = (position - transform.position) / Time.deltaTime;
                transform.position = position;
            }
            

            if (Input.GetMouseButtonUp(0) && rb.velocity.magnitude + rb.angularVelocity.magnitude >= 5)
            {
                Debug.Log(rb.velocity.magnitude + rb.angularVelocity.magnitude);
                inHand = false;
                stable = false;
                ResetIgnore();
            }
            else if(Input.GetMouseButtonUp(0))
            {
                inHand = false;
                ResetIgnore();
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
