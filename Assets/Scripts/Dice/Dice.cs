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
    public bool isFinished;

    private Vector3 velocity;
    private bool inHand = false;
    private bool stable = true;


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
        if (stable && !isFinished && !inHand) 
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

        if (inHand && !isFinished)
        {
            Ray ray = diceCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("Dice")))
            {
                Vector3 position = Vector3.Lerp(transform.position, hit.point + hit.normal * height, 0.8f);
                velocity = (position - transform.position) / Time.deltaTime;
                transform.position = position;
            }

            if (Input.GetMouseButtonUp(0) && velocity.magnitude >= 4)
            {
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
            //rolled, and stopped, pass value!
            if(!stable && rb.velocity.magnitude + rb.angularVelocity.magnitude <= 0.1f)
            {
                stable = true;
                isFinished = true;
                GameManager.Instance.diceSystem.ReceiveValueFromDice(value);
            }
        }
    }

    public void FastGetValue()
    {
        inHand = false;
        stable = true;
        GameManager.Instance.diceSystem.ReceiveValueFromDice(Random.Range(1, 7));
        
    }
}
