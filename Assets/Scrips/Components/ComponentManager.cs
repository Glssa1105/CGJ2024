using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{
    private static ComponentManager instance;
    public static ComponentManager Instance => instance;

    public List<Item> itemDetails=new List<Item>();
    
    public List<ComponentBase> components = new List<ComponentBase>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public SpriteRenderer CreateComponent(Grid grid,int id)
    {
        var item=itemDetails.Select((i) => i.id == id);
        
        return null;
    }

    public void GenerateComponentsJoint(List<GameObject> objects,Grid[,] grid)
    {
        
    }
    
}
