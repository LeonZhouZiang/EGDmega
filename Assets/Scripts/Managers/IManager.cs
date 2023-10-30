using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IManager
{
    protected virtual void Awake() { }
    public virtual void PostAwake() { }
    public virtual void PreUpdate() { }
    public virtual void PostUpdate() { }

    public virtual void PreLateUpdate() { }
    public virtual void PostLateUpdate() { }

}
