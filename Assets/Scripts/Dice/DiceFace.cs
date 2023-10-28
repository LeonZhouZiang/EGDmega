using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceFace : MonoBehaviour
{
    public Dice dice;
    public int value;
    private void OnTriggerEnter(Collider other)
    {
        Transform parent = transform.parent;
        if (parent.position.y > other.ClosestPointOnBounds(parent.position).y)
            dice.value = value;
    }
}
