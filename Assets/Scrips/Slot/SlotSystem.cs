using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;

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
    public ItemState item;
    public Grid(int x,int y,int index)
    {
        this.x = x;
        this.y = y;
        this.Object_index = index;
        item = new ItemState();
    }
}

public class ItemState
{
    EGridRotate rotation = EGridRotate.UP;
}

public class SlotSystem : MonoBehaviour
{
    [Tooltip("是否为玩家1，否则玩家2")]
    public bool ifPlayer1;

    public bool EditingSlotSystem;

    public int m_GirdMap_X;
    public int m_GirdMap_Y;
    public Grid ActiveGrid;

    //选中与控制
    [NonSerialized]
    public bool isSeleting;
    [NonSerialized]
    public GameObject SeletingObject;
    [NonSerialized] public ComponentBase SeletingCB;
    [NonSerialized] public int SeletingIndex;
    [NonSerialized] public int SeleteRotate;


    private int m_index;

    [NonSerialized]
    public bool[,] ableToPlace;
    [NonSerialized]
    public Grid[,] GridMap;

    [NonSerialized] public List<GameObject> m_objectList;

    [SerializeField]
    private float distance;


    //绘制GridMap
    public Transform StartPos;
    public MarketSystem marketSystem;

    public GameObject testObject;
    public Item testItem;
    public List<GameObject> GridList;



    private void Awake()
    {
        ableToPlace = new bool[m_GirdMap_Y, m_GirdMap_X];
        GridMap = new Grid[m_GirdMap_Y, m_GirdMap_X];
        m_objectList = new List<GameObject>();
        marketSystem = GetComponent<MarketSystem>();

        ActiveGrid = new Grid(0,0,-1);
        for(int i = 0;i<m_GirdMap_X;i++)
        {
            for(int j =0;j<m_GirdMap_Y;j++)
            {
                GridMap[j, i].Object_index = -1;
            }
        }

    }


    private void Start()
    {
        CreateGridMap();


        EnterSlotSystem();
    }
    private void Update()
    {
        if(EditingSlotSystem)
        {
            MoveActiveGrid();
            SeletctTarget();
        }

        DrawActiveGrid();
    }

    public bool CheckPlaceable(Item item,EGridRotate rotate)
    {
        var pos = ActiveGrid;
        var bias = item.biasList;
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

    public void PlaceGrid(Item item,EGridRotate rotate,bool ifCreate = false)
    {
        var bias = item.biasList;
        var pos = ActiveGrid;
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
            GridMap[ny, nx] = new Grid(pos.x, pos.y, ifCreate ? m_index : SeletingIndex);
        }
        //m_objectList内添加生成的物体
        if(ifCreate)
        {
            m_index++;
            var obj = ComponentManager.Instance.CreateComponent(item.id, transform.position + new Vector3(pos.x * distance, pos.y * distance, 0.0f), transform.root,ifPlayer1);
            obj.Rotate(rotate);
            m_objectList.Add(obj.gameObject);
        }        
    }

    public void EnterAndSelectItem(Item item, EGridRotate rotate)
    {
        isSeleting = true;
        var obj = ComponentManager.Instance.CreateComponent(item.id, transform.position + new Vector3(ActiveGrid.x * distance, ActiveGrid.y * distance, 0.0f), transform.root, ifPlayer1);
        obj.Rotate(rotate);
        m_objectList.Add(obj.gameObject);
        SeletingIndex = m_index;
        SeletingObject = m_objectList[m_index];
        m_index++;
        SeletingCB = SeletingObject.GetComponent<ComponentBase>();

    }

    public void RemoveGrid(Item item, EGridRotate rotate)
    {
        var bias = item.biasList;
        var pos = ActiveGrid;
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
            ableToPlace[ny, nx] = false;
            GridMap[ny, nx] = new Grid(nx, ny, -1);
        }
    }

    public void CreateGridMap()
    {
        for(int j = 0;j<m_GirdMap_Y;j++)
        {
            for(int i = 0;i<m_GirdMap_X;i++)
            {
                GridList.Add(Instantiate(testObject, transform.position + new Vector3(i * distance, j * distance, 0.0f), Quaternion.identity));
            }
        }
    }

    public void MoveActiveGrid()
    {
        var x = ActiveGrid.x;
        var y = ActiveGrid.y;
        int dx = 0;
        int dy = 0;

        if (ifPlayer1)
        {
            if (Input.GetKeyDown(KeyCode.A))
                dx += -1;
            if (Input.GetKeyDown(KeyCode.S))
                dy += -1;
            if (Input.GetKeyDown(KeyCode.D))
                dx += 1;
            if (Input.GetKeyDown(KeyCode.W))
                dy += 1;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                dx += -1;
            if (Input.GetKeyDown(KeyCode.DownArrow))
                dy += -1;
            if (Input.GetKeyDown(KeyCode.RightArrow))
                dx += 1;
            if (Input.GetKeyDown(KeyCode.UpArrow))
                dy += 1;
        }



        var nx = x +dx;
        var ny = y +dy; 

        if(nx < 0)
        {
            ChangeToMarket();
            return;
        }

        if (nx < 0 || nx >= m_GirdMap_X || ny >= m_GirdMap_Y || ny < 0)
        {
            return;
        }
        

        if (x == nx && y == ny)
            return;
        ActiveGrid.x = nx;
        ActiveGrid.y = ny;
    }

    public void DrawActiveGrid()
    {
        for(int i = 0;i<m_GirdMap_X;i++)
        {
            for(int j = 0;j<m_GirdMap_Y;j++)
            {
                if(i == ActiveGrid.x && j == ActiveGrid.y)
                GridList[j * m_GirdMap_X + i].GetComponent<SpriteRenderer>().color = Color.white;
                else
                GridList[j * m_GirdMap_X + i].GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }

    }

    public void SeletctTarget()
    {
        if((Input.GetKeyDown(KeyCode.Space) && ifPlayer1) || (Input.GetKeyDown(KeyCode.KeypadEnter) && !ifPlayer1))
        {
            if(isSeleting == false && ableToPlace[ActiveGrid.y, ActiveGrid.x] == true)
            {
                isSeleting = true;
                ActiveGrid = GridMap[ActiveGrid.y, ActiveGrid.x];
                SeletingObject = m_objectList[ActiveGrid.Object_index];
                SeletingCB = SeletingObject.GetComponent<ComponentBase>();
                SeletingIndex = ActiveGrid.Object_index; 
                RemoveGrid(SeletingCB.detail,SeletingCB._Direction);
                
            }
            else if(isSeleting == true && CheckPlaceable(SeletingCB.detail,SeletingCB._Direction))
            {
                SeletingCB.SetColorWhite();                
                isSeleting = false;
                PlaceGrid(SeletingCB.detail, SeletingCB._Direction, false);
                SeletingIndex = -1;
                SeletingCB = null;
                SeletingObject = null;
                SeleteRotate = 0;
            }
        }

        if (isSeleting == true)
        {
            SeletingObject.transform.position = GridList[m_GirdMap_X * ActiveGrid.y + ActiveGrid.x].transform.position;
            if(CheckPlaceable(SeletingCB.detail,SeletingCB._Direction))
            {
                SeletingCB.OnCanSet();
            }
            else
            {
                SeletingCB.OnCannotSet();
            }
            if((Input.GetKeyDown(KeyCode.R)&&ifPlayer1) || (Input.GetKeyDown(KeyCode.Keypad0)&&!ifPlayer1))
            {
                SeleteRotate = (SeleteRotate+1)%4;
                switch (SeleteRotate)
                {
                    case 0:
                        SeletingCB.Rotate(EGridRotate.UP);
                        break;
                    case 1:
                        SeletingCB.Rotate(EGridRotate.RIGHT);
                        break;
                    case 2:
                        SeletingCB.Rotate(EGridRotate.DOWN);
                        break;
                    case 3:
                        SeletingCB.Rotate(EGridRotate.LEFT);
                        break;
                    default:
                        break;
                }
            }

        }
    }


    public void ChangeToMarket()
    {
        if(isSeleting)
        {
            isSeleting = false;
            SeletingIndex = -1;
            SeletingCB = null;
            Destroy(SeletingObject);
            SeleteRotate = 0;
            marketSystem.SellItem(testItem);
        }
        ActiveGrid.x = -1; 
        ActiveGrid.y = -1;
        EditingSlotSystem = false;
        marketSystem.ifUsingMarket = true;

        marketSystem.CurrentGrid.x = 0;
        marketSystem.CurrentGrid.y = 0;
        marketSystem.m_CurrentIndex = 0;
    }



    public void EnterSlotSystem()
    {
        ActiveGrid.x = 0;
        ActiveGrid.y = 0;
        EditingSlotSystem = true;
        //test
    }
}
