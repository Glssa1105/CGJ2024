using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWork : ComponentBase,ITriggerComponent
{
    private float currentEnergy;
    
    [SerializeField]private float maxEnergy;
    [SerializeField]private float force;

    private bool isTriggerd;

    private void Awake()
    {
        currentEnergy = maxEnergy;
    }

    // public override void Start()
    // {
    //     base.Start();
    //     
    //     
    //
    // }

    protected override void Update()
    {
        base.Update();
        
        if (isTriggerd)
        {
            
            currentEnergy -= Time.deltaTime;    

            Sr.color = Color.Lerp(Color.white, Color.red, currentEnergy / maxEnergy);
            
            Rb.AddForce(transform.up*force);
            
            if (currentEnergy <= 0)
            {
                Sr.color=Color.grey;
                isTriggerd = false;;
            }
            
        }
    }

    protected override void HandleInput(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            OnTrigger();
            Debug.Log(key);
        }
    }


    public float EnergyProgress => currentEnergy/maxEnergy;

    public void OnTrigger()
    {
        isTriggerd = true;
    }
}

