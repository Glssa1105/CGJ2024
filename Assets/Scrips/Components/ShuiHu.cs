using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuiHu : ComponentBase, IHoldComponent
{
    private float currentEnergy;
    
    [SerializeField] private float maxEnergy;

    [SerializeField] private float force;


    private void Awake()
    {
        currentEnergy = maxEnergy;
    }

    public float EnergyProgress => currentEnergy/maxEnergy;

    public void OnHold()
    {
        if(currentEnergy<=0)return; 
        currentEnergy -= Time.deltaTime;
        
        Rb.AddForce(-transform.right * force);

        if (currentEnergy <= 0)
        {
            Sr.color=Color.gray;
        }
        

    }

    protected override void HandleInput(KeyCode key)
    {
        if (Input.GetKey(key))
        {
            OnHold();

        }
        
    }
}