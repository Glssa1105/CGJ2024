using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;


public struct Grid {
    int x, y;
}

public class SlotSystem : MonoBehaviour
{
    public Grid ActiveGrid;
    public List<List<bool>> gridmap { get; set; }

    public bool CheckPlaceable(Grid pos, Grid bias)
    {
        foreach(var v in gridmap)
        {

        }
        return true;
    }

    
}
