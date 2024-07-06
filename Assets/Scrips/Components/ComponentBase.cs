using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
public class ComponentBase : MonoBehaviour
{

    public Item detail;
    private EGridRotate _direction=EGridRotate.UP;
    public EGridRotate _Direction => _direction;

    public SpriteRenderer Sr
    {
        get => sr??=GetComponent<SpriteRenderer>();
    }

    private SpriteRenderer sr;

    

    public void Start()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Static;
    }

    public void OnGameStart()
    {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Dynamic;
        
        Sr.color=Color.white;
        
    }
    


    public void Rotate(EGridRotate target)
    {
        _direction = target;
        switch (target)
        {
            case EGridRotate.UP:
                transform.localRotation=Quaternion.identity;
                break;
            case EGridRotate.DOWN:
                transform.localRotation=Quaternion.Euler(0,0,180);
                break;
            case EGridRotate.LEFT:
                transform.localRotation=Quaternion.Euler(0,0,-90);
                break;
            case EGridRotate.RIGHT:
                transform.localRotation=Quaternion.Euler(0,0,90);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(target), target, null);
        }
    }

    public void OnCannotSet()
    {
        Sr.color=new Color(255f,0f,0f,0.5f);
    }

    public void OnCanSet()
    {
        Sr.color=new Color(0f,255f,0f,1f);
    }
    
    

    
}
