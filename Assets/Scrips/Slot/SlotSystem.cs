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
[System.Flags]
public enum EDirType
{
    UP=1,
    DOWN =2,
    LEFT =4,
    RIGHT =8
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
   
    private int m_index;

    public bool[,] ableToPlace;
    public Grid[,] GridMap;
    public List<GameObject> m_objectList;

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

        for(int i = 0;i<m_GirdMap_X;i++)
        {
            
        }

    }

    private void Start()
    {
        CreateGridMap();
    }

    private void Update()
    {
        for (int j = 0; j < m_GirdMap_Y; j++)
        {
            for (int i = 0; i < m_GirdMap_X; i++)
            {
                if(i == ActiveGrid.x&& j == ActiveGrid.y)
                {
                    m_objectList[j * m_GirdMap_Y + i].GetComponent<SpriteRenderer>().color = Color.blue;
                }
                else if (ableToPlace[j, i] == true)
                {
                    m_objectList[j * m_GirdMap_Y + i].GetComponent<SpriteRenderer>().color = Color.red;
                }


            }
        }
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
        m_objectList.Add(Instantiate(testObject, StartPos+new Vector3(pos.x*distance, pos.y*distance, 0.0f), Quaternion.identity));
    }

    public void CreateGridMap()
    {
        for(int j = 0;j<m_GirdMap_Y;j++)
        {
            for(int i = 0;i<m_GirdMap_X;i++)
            {
                GridList.Add(Instantiate(testObject, StartPos + new Vector3(i * distance, j * distance, 0.0f), Quaternion.identity));
            }
        }
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
