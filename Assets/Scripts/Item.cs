using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public SingleAction[] actions;

    public int dexOffset;
    public int strOffset;

    public Sprite image;
}
