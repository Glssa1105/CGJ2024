using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComponentManager : MonoBehaviour
{
    private static ComponentManager instance;
    public static ComponentManager Instance => instance;

    public List<Item> itemDetails=new List<Item>();
    
    public List<ComponentBase> components = new List<ComponentBase>();
    public EDirType type = EDirType.UP;
    public EGridRotate Direction = EGridRotate.RIGHT;
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {


            
            var dirMap = GridRotater.RotateDirMap(type, Direction);
            
            Debug.Log(dirMap);
            Debug.Log(dirMap.ToString());
            Debug.Log(Convert.ToString((int)dirMap, 2));
        }
    }


    /// <summary>
    /// 创造配件
    /// </summary>
    /// <param name="id">配件的id</param>
    /// <param name="position">生成位置</param>
    /// <param name="parent">父物体</param>
    /// <returns></returns>
    public ComponentBase CreateComponent(int id,Vector3 position,Transform parent)
    {
        var item=itemDetails.Find((i) => i.id == id);

        var obj = Instantiate(item.prefeb,position,Quaternion.identity,parent);

        var cmp = obj.GetComponent<ComponentBase>();

        cmp.detail = item;
        
        return cmp;
    }

    public void GenerateComponentsJoint(List<GameObject> objects,Grid[,] grids)
    {
        foreach (var grid in grids)
        {
            if (grid.Object_index != -1)
            {

                var cmp=objects[grid.Object_index].GetComponent<ComponentBase>();

                var dirMap = GridRotater.RotateDirMap(cmp.detail.type, cmp._Direction);
                

            }
                
                
                
            objects[grid.Object_index].AddComponent<FixedJoint2D>();
        }
    }
    
}
