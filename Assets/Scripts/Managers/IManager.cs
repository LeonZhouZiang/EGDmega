using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager
{
    public abstract void PreUpdate();
    public abstract void PostUpdate();

    public abstract void PreLateUpdate();
    public abstract void PostLateUpdate();
}
