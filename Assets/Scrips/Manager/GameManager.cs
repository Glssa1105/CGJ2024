using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMono<GameManager>
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
    
    
    
    
}
