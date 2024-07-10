using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable,CreateAssetMenu(fileName = "Item",menuName ="Scripts/Item")]
public class Item:ScriptableObject
{
    public int id;
    [FormerlySerializedAs("name")] public string itemName;
    public Sprite icon;
    public EDirType type;
    public GameObject prefeb;
    public Grid[] biasList;
    public int price;
    public string description;
}
