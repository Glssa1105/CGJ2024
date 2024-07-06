using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable,CreateAssetMenu(fileName = "Item",menuName ="Scripts/Item")]
public class Item:ScriptableObject
{
    public int id;
    public Sprite icon;
}
