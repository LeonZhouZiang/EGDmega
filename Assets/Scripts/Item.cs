using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string type;
    public string name;
    public SingleAction[] actions;

    public Sprite image;
}
