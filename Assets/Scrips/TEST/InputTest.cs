using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    // Start is called before the first frame update
    InputController controller;
    void Start()
    {
        controller = InputController.Instance;
        controller.pauseAction += pauseTest;
        controller.resumeAction += resumeTest;
    }

    private void Update()
    {
         
    }

    public void pauseTest()
    {
        Debug.Log("pause");
    }

    public void resumeTest()
    {
        Debug.Log("resume");
    }
}
