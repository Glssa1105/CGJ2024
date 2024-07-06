using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MarketSystem : MonoBehaviour
{


    public int Money;
    public SlotSystem slotSystem;
    public List<Item> itemList;
    public bool ifUsingMarket;

    public int m_CurrentIndex;

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
    }

    private void Update()
    {
        if(ifUsingMarket)
        {
            MoveActive();
            FreshUI();
            SelectUI();
        }
    }

    public void MoveActive()
    {
        int dx = 0;
        if(Input.GetKeyDown(KeyCode.W))
        {
            dx += 1;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            dx-= 1;
        }
        var nIndex = m_CurrentIndex + dx;
        if(nIndex < 0 || nIndex > itemList.Count-1) { return; }
        m_CurrentIndex = nIndex;
    }

    //Ë¢ÐÂUI
    private void FreshUI()
    {

    }

    private void SelectUI()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (TryToBuy(itemList[m_CurrentIndex]))
            {
                Debug.Log($"try to buy,no money = {Money}");
                m_CurrentIndex = 0;
                ifUsingMarket = false;
                slotSystem.EnterSlotSystem();
                slotSystem.EnterAndSelectItem(itemList[m_CurrentIndex], EGridRotate.UP);

            }
        }
    }

}
