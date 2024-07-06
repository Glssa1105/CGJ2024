using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MarketSystem : MonoBehaviour
{

    public int Monny;
    public SlotSystem slotSystem;
    private void Awake()
    {
        SlotSystem slotSystem = GetComponent<SlotSystem>();
    }

    public void SellItem(Item item)
    {
        Monny += item.price;
    }

    public bool TryToBuy(Item item)
    {
        if(Monny > item.price)
        {
            Monny -= item.price;
            return true;
        }
        return false;
    }
    


}
