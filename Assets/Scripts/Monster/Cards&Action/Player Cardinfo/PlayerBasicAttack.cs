using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour, IEffect
{
    protected int damage = 1;

    public bool GetTarget()
    {

        return true;
    }

    public void ActionEffect()
    {
       
    }

    public bool GetTargets(Vector3[] range)
    {
        throw new System.NotImplementedException();
    }
}
