using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyWork : ComponentBase, IHoldComponent
{
    private float currentEnergy;
    
    [SerializeField] private float maxEnergy;

    [SerializeField] private float force;
    private static readonly int IsWorking = Animator.StringToHash("IsWorking");

    private void Awake()
    {
        currentEnergy = maxEnergy;
    }

    public float EnergyProgress => currentEnergy/maxEnergy;

    public void OnHold()
    {
        if(currentEnergy<=0)return; 
        currentEnergy -= Time.deltaTime;
        
        Rb.AddForce(transform.up * force);

        if (currentEnergy <= 0)
        {
            Sr.color=Color.gray;
        }
        
        Ani.SetBool(IsWorking,true);
    }

    protected override void HandleInput(KeyCode key)
    {
        if (Input.GetKey(key))
        {
            OnHold();

        }
        else
        {
            Ani.SetBool(IsWorking,false);
        }
    }
}
