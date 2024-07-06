using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ComponentManager : SingletonMono<ComponentManager>
{
    public List<Item> itemDetails = new List<Item>();

    
    public EDirType type = EDirType.UP;
    public EGridRotate Direction = EGridRotate.RIGHT;

    public Action onGameStart;

    [FormerlySerializedAs("endForce")] public float breakForce;

    public SkillManager skillMgr1, skillMgr2;

    private KeyCode[] player1KeyMap = {
        KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T,
        KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P
    };

    private KeyCode[] player2KeyMap = {
        KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5,
        KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Plus
    };

    private int player1Index;
    private int player2Index;

    public SlotSystem slotSystem1, slotSystem2;
    
    private void Start()
    {
        //CreateComponent(0,Vector3.zero, transform, true);
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     GenerateComponentsJoint(GetComponent<SlotSystem>().m_objectList, GetComponent<SlotSystem>().GridMap);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     onGameStart?.Invoke();
        // }
    }


    /// <summary>
    /// 创造配件
    /// </summary>
    /// <param name="id">配件的id</param>
    /// <param name="position">生成位置</param>
    /// <param name="parent">父物体</param>
    /// <returns></returns>
    public ComponentBase CreateComponent(int id, Vector3 position, Transform parent, bool isPlayer1)
    {
        var item = itemDetails.Find((i) => i.id == id);

        var obj = Instantiate(item.prefeb, position, Quaternion.identity, parent);

        var cmp = obj.GetComponent<ComponentBase>();

        cmp.detail = item;

        onGameStart += cmp.OnGameStart;

        cmp.isPlayer1 = isPlayer1;


        if (isPlayer1)
        {
            skillMgr1.SpawnSkill(item, player1KeyMap[player1Index % player1KeyMap.Length],cmp);
            cmp.keyCode = player1KeyMap[player1Index % player1KeyMap.Length];
            player1Index++;
        }
        else
        {
            skillMgr2.SpawnSkill(item, player2KeyMap[player2Index%player2KeyMap.Length],cmp);
            cmp.keyCode = player2KeyMap[player2Index % player2KeyMap.Length];
            player2Index++;
        }

            
        
        

        return cmp;
    }


    public void StartGame()
    {
        GenerateComponentsJoint(slotSystem1.m_objectList, slotSystem1.GridMap);
        GenerateComponentsJoint(slotSystem2.m_objectList, slotSystem2.GridMap);
        onGameStart?.Invoke();
        
    }

    
    public void GenerateComponentsJoint(List<GameObject> objects, Grid[,] grids)
    {
        HashSet<KeyValuePair<int, int>> gridSet = new HashSet<KeyValuePair<int, int>>();
        int index = 0;
        foreach (var grid in grids)
        {

            if (grid.Object_index != -1)
            {
                

                var cmp = objects[grid.Object_index].GetComponent<ComponentBase>();

                var dirMap = GridRotater.RotateDirMap(cmp.detail.type, cmp._Direction);

               // Debug.Log(dirMap);
                var gridList = GridRotater.AccessMultiplePoints(new Vector2Int(index%(grids.GetLength(1)),index/(grids.GetLength(1))), dirMap, grids);

                //Debug.Log(gridList.Count);
                
                foreach (var nigger in gridList)
                {
                    Debug.Log(gridSet.Count);

                    if (gridSet.Contains(new KeyValuePair<int, int>(grid.Object_index, nigger.Value.Object_index))
                        || gridSet.Contains(new KeyValuePair<int, int>(nigger.Value.Object_index, grid.Object_index)))
                        continue;

                    if (nigger.Value.Object_index == -1 || nigger.Value.Object_index == grid.Object_index) continue;

                    Debug.Log(nigger.Value.Object_index);
                    Debug.Log(objects.Count);
                    var niggerCmp = objects[nigger.Value.Object_index].GetComponent<ComponentBase>();

                    var niggerDirMap = GridRotater.RotateDirMap(niggerCmp.detail.type, niggerCmp._Direction);

                    switch (nigger.Key)
                    {
                        case EDirType.UP:
                            if ((niggerDirMap & EDirType.DOWN) != 0)
                            {
                                var fixedJoint2D = cmp.AddComponent<FixedJoint2D>();
                                fixedJoint2D.connectedBody = niggerCmp.GetComponent<Rigidbody2D>();
                                fixedJoint2D.anchor = new Vector2(0, 0.5f);
                                fixedJoint2D.breakForce = breakForce;
                            }

                            break;
                        case EDirType.DOWN:
                            if ((niggerDirMap & EDirType.UP) != 0)
                            {
                                var fixedJoint2D = cmp.AddComponent<FixedJoint2D>();
                                fixedJoint2D.connectedBody = niggerCmp.GetComponent<Rigidbody2D>();
                                fixedJoint2D.anchor = new Vector2(0, -0.5f);
                                fixedJoint2D.breakForce = breakForce;
                            }

                            break;
                        case EDirType.LEFT:
                            if ((niggerDirMap & EDirType.RIGHT) != 0)
                            {
                                var fixedJoint2D = cmp.AddComponent<FixedJoint2D>();
                                fixedJoint2D.connectedBody = niggerCmp.GetComponent<Rigidbody2D>();
                                fixedJoint2D.anchor = new Vector2(-0.5f, 0);
                                fixedJoint2D.breakForce = breakForce;
                            }

                            break;
                        case EDirType.RIGHT:
                            if ((niggerDirMap & EDirType.LEFT) != 0)
                            {
                                var fixedJoint2D = cmp.AddComponent<FixedJoint2D>();
                                fixedJoint2D.connectedBody = niggerCmp.GetComponent<Rigidbody2D>();
                                fixedJoint2D.anchor = new Vector2(0.5f, 0);
                                fixedJoint2D.breakForce = breakForce;
                                
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    gridSet.Add(new KeyValuePair<int, int>(grid.Object_index, nigger.Value.Object_index));
                }

                //if(objects.Count>grid.Object_index)objects[grid.Object_index].AddComponent<FixedJoint2D>();
            }
            index++;
        }
    }
}