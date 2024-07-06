using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMono<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;

    public static T Instance => instance;
    
    protected virtual void Awake()
    {
           if (Instance != null)
            Destroy(gameObject);
           else
           {
               instance = this as T;
           }
    }
}
