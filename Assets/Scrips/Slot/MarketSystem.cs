using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MarketSystem : MonoBehaviour
{
    public bool isPlayer1;

    public int Money;
    
    public TMP_Text MoneyText;
    
    public SlotSystem slotSystem;
    public List<Item> itemList;
    public bool ifUsingMarket;


    public int m_maxX;
    public int m_maxY;
    public float Distance;
    public GameObject testObject;



    public Transform StartPos;
    public int m_CurrentIndex;
    public Vector2Int CurrentGrid;
    public List<GameObject> m_objectList;

    public Sprite normalSprite;
    
    public Sprite activeSprite;

    public TMP_Text description;
    
    private void Start()
    {
        itemList = ComponentManager.Instance.itemDetails;
        CreateStore();
        ifUsingMarket = false;
    }

    public void SellItem(Item item)
    {
        Money += item.price;
    }

    public bool TryToBuy(Item item)
    {
        if(Money >= item.price)
        {
            Money -= item.price;
            return true;
        }
        return false;
    }
    
    public void CreateStore()
    {
        //Éú³ÉUI
        for(int i = 0;i< itemList.Count;i++)
        {
            var trans = StartPos;
            var position = new Vector3(StartPos.position.x + Distance*(i%m_maxX),StartPos.position.y + Distance*(i/m_maxX),trans.position.z);
            m_objectList.Add(Instantiate(testObject, position, Quaternion.identity));
        }
        FreshUI();

    }

    private void Update()
    {
        if(ifUsingMarket)
        {
            MoveActive();
            FreshUI();
            SelectUI();
        }

        MoneyText.text = Money + "£¤";
    }

    public void MoveActive()
    {
        int dx = 0;
        int dy = 0;
        if(isPlayer1)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                dy += 1;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                dy -= 1;
            }
            if(Input.GetKeyDown(KeyCode.D))
            {
                dx += 1;
            }
            if( Input.GetKeyDown(KeyCode.A))
            {
                dx -= 1;
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                dy += 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                dy -= 1;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                dx += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                dx -= 1;
            }
        }

        var nx = dx + CurrentGrid.x;
        var ny = dy + CurrentGrid.y;
        if(nx < 0 || ny< 0||nx>=m_maxX||ny>=m_maxY || ny*m_maxX +nx >= itemList.Count)
        {
            return;
        }
        
        var nIndex = ny * m_maxX + nx;
        CurrentGrid = new Vector2Int(nx, ny);
        m_CurrentIndex = nIndex;
    }

    //Ë¢ÐÂUI
    private void FreshUI()
    {
        for(int i =0; i < itemList.Count; i++)
        {
            m_objectList[i].GetComponent<SpriteRenderer>().sprite = itemList[i].icon;
            m_objectList[i].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text =
                itemList[i].name + ":" + itemList[i].price + "$";
            if(i == m_CurrentIndex)
            {
                m_objectList[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = activeSprite;
            }
            else
            {
                m_objectList[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = normalSprite;
            }

            description.text = itemList[i].description;
        }
    }

    private void SelectUI()
    {
        if((Input.GetKeyDown(KeyCode.Space) && isPlayer1 )||(Input.GetKeyDown(KeyCode.KeypadEnter)&&!isPlayer1))
        {
            if (TryToBuy(itemList[m_CurrentIndex]))
            {
                //m_CurrentIndex = 0;
                ifUsingMarket = false;
                slotSystem.EnterSlotSystem();
                slotSystem.EnterAndSelectItem(itemList[m_CurrentIndex], EGridRotate.UP);

            }
            CurrentGrid.x = -1;
            CurrentGrid.y = -1;
            m_CurrentIndex = -1;
            FreshUI();
        }
    }

}
