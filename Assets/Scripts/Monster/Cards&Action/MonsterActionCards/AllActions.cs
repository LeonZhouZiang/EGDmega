using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllActions : MonoBehaviour
{
    public List<Effect> EffectList;
}


[System.Serializable]
public class Effect
{
    Unit target;
    public virtual void ActionEffect()
    {

    }
}