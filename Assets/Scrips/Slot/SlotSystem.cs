using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UIElements;

public enum EGridRotate
{
    UP, DOWN, LEFT, RIGHT
}

[Serializable]
public struct Grid {
    public int x, y;
    public int Object_index;
    public Grid(int x,int y,int index)
    {
        this.x = x;
        this.y = y;
        this.Object_index = index;
    }
}

public class SlotSystem : MonoBehaviour
{
    public int m_GirdMap_X;
    public int m_GirdMap_Y;
    public Grid ActiveGrid;

    private List<GameObject> m_objectList;
    private int m_index;

    bool[,] ableToPlace;
    Grid[,] GridMap;

    [SerializeField]
    private float distance;


    //绘制GridMap
    public Vector3 StartPos;
    public GameObject testObject;
    public List<GameObject> GridList;

    private void Awake()
    {
        ableToPlace = new bool[m_GirdMap_Y, m_GirdMap_X];
        GridMap = new Grid[m_GirdMap_Y, m_GirdMap_X];
        m_objectList = new List<GameObject>();

    }

    public bool CheckPlaceable(Grid pos, Grid[] bias,EGridRotate rotate)
    {
        foreach(var v in bias)
        {
            int cosAngle;
            int sinAngle;

            switch (rotate)
            {
                case EGridRotate.UP:
                    cosAngle = 1;
                    sinAngle = 0;
                    break;
                case EGridRotate.DOWN:
                    cosAngle = -1;
                    sinAngle = 0;
                    break;
                case EGridRotate.LEFT:
                    cosAngle = 0;
                    sinAngle = -1;
                    break;
                case EGridRotate.RIGHT:
                    cosAngle = 0;
                    sinAngle = 1;
                    break;

                default:
                    cosAngle = -1; sinAngle = -1;
                    break;
            }
            int nx = pos.x+v.x*cosAngle - v.y*sinAngle; 
            int ny = pos.y+v.x*sinAngle + v.y*cosAngle;

            if(nx <0||nx>=m_GirdMap_X||ny>=m_GirdMap_Y||ny<0)
            {
                return false;
            }
            if (ableToPlace[ny,nx] == true)
            {
                return false;
            }
        }
        return true;
    }

    public void PlaceGrid(Grid pos, Grid[] bias,Item item,EGridRotate rotate)
    {
        foreach (var v in bias)
        {
            int cosAngle;
            int sinAngle;

            switch (rotate)
            {
                case EGridRotate.UP:
                    cosAngle = 1;
                    sinAngle = 0;
                    break;
                case EGridRotate.DOWN:
                    cosAngle = -1;
                    sinAngle = 0;
                    break;
                case EGridRotate.LEFT:
                    cosAngle = 0;
                    sinAngle = -1;
                    break;
                case EGridRotate.RIGHT:
                    cosAngle = 0;
                    sinAngle = 1;
                    break;

                default:
                    cosAngle = -1; sinAngle = -1;
                    break;
            }
            int nx = pos.x + v.x * cosAngle - v.y * sinAngle;
            int ny = pos.y + v.x * sinAngle + v.y * cosAngle;
            ableToPlace[ny, nx] = true;
            GridMap[ny, nx] = new Grid(pos.x, pos.y, m_index);
        }

        m_index++;
        //m_objectList内添加生成的物体
        m_objectList.Add(Instantiate(testObject, new Vector3(pos.x, pos.y, 0.0f), Quaternion.identity));
    }

    public void CreateGridMap()
    {
        for(int i = 0;i<m_GirdMap_X;i++)
        {
            for(int j = 0;j<m_GirdMap_Y;j++)
            {
                GridList.Add(Instantiate(testObject, StartPos + new Vector3(i * distance, j * distance, 0.0f), Quaternion.identity));
            
            }
        }
    }
    private void Update()
    {

    }

    public void Change2Market()
    {

    }

    public void OnSlotSystemEnable()
    {
        
    }

    public void OnSlotSystemDisable()
    {

    }

    
}
