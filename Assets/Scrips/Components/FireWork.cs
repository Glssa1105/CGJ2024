using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWork : ComponentBase,ITriggerComponent
{
    private float currentEnergy;
    
    [SerializeField]private float maxEnergy;
    [SerializeField]private float force;

    private bool isStarted;

    public override void Start()
    {
        base.Start();
        
        currentEnergy = maxEnergy;

    }

    protected void Update()
    {
        
        
        if (isStarted)
        {
            
            currentEnergy -= Time.deltaTime;    

            Sr.color = Color.Lerp(Color.white, Color.red, currentEnergy / maxEnergy);
            
            Rb.AddForce(transform.up*force);
            
            if (currentEnergy <= 0)
            {
                Sr.color=Color.grey;
                isStarted = false;;
            }
            
        }
    }


    public void OnTrigger()
    {
        isStarted = true;
    }
}

