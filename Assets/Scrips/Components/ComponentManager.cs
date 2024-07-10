using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    public GameObject player1, player2;

    public TMP_Text distanceText1, distanceText2;

    private float playe1xTemp, playe2xTemp;
    private float distance1, distance2;
    private float lastDistance1, lastDistance2;

    public SlotSystem slotSystem1, slotSystem2;
    public MarketSystem MarketSystem1, MarketSystem2;

    private float timer;
    private float timer1;
    private float timer2;
    private float baseFontSize = 20;
    
    private List<ComponentBase> components = new List<ComponentBase>();

    public GameObject EndPanel;

    public GameObject num;

    public Image playerImage;
    public Sprite player1Win, player2Win;
    
    private bool started;
    
    
    
    private void Start()
    {
        //CreateComponent(0,Vector3.zero, transform, true);
        playe1xTemp = player1.transform.position.x;
        playe2xTemp = player2.transform.position.x;
        baseFontSize = distanceText1.fontSize;

        int money = Random.Range(2, 12);
        MarketSystem1.Money = money * 5;
        MarketSystem2.Money = money * 5;

    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     GenerateComponentsJoint(GetComponent<SlotSystem>().m_objectList, GetComponent<SlotSystem>().GridMap);
        // }
        //
        if (Input.GetKey(KeyCode.F5))
        {
            RestartGame();
        }
        
        if(!started)return;

        distance1 = player1.transform.position.x-playe1xTemp;
        distance2 = player2.transform.position.x-playe2xTemp;
        distanceText1.text="距离："+distance1.ToString("f2")+"m";
        distanceText2.text="距离："+distance2.ToString("f2")+"m";
        timer += Time.deltaTime;
        
        
        distanceText1.fontSize = baseFontSize + (int) (distance1 /10)+Mathf.Sin(timer)*5;
        
        distanceText2.fontSize = baseFontSize + (int) (distance2 /10)+Mathf.Sin(timer)*5;

        Debug.Log((lastDistance1 - distance1)/Time.deltaTime);
        if((Mathf.Abs(lastDistance1 - distance1)/Time.deltaTime)<1.25f)
        {
            timer1 += Time.deltaTime;
        }
        else
        {
            timer1 = 0;
        }
        if((Mathf.Abs(lastDistance2 - distance2)/Time.deltaTime)<1.25f)
        {
            timer2 += Time.deltaTime;
        }
        else
        {
            timer2 = 0;
        }

        if (timer1 >= 8&&timer2>=8&&distance1!=0&&distance2!=0)
        {
            EndGame();
        }
        
        
        lastDistance1 = distance1;
        lastDistance2 = distance2;
        
        


    }

    public void PressStartBtn()
    {
        StartCoroutine(IStartGame());
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator IStartGame()
    {

        
        
        yield return new WaitForSeconds(3f);
        
        num.SetActive(false);
        
        StartGame();
    }
    private void EndGame()
    {
        EndPanel.SetActive(true);

        started = false;

        Score.MaxScore = distance1;
        Score.MaxScore = distance2;
        
        if (distance1 > distance2)
        {
            playerImage.sprite = player1Win;
        }
        else
        {
            playerImage.sprite = player2Win;
            
        }
            EndPanel.GetComponentInChildren<TMP_Text>().text =$"历史最高成绩：{Score.MaxScore}";
        
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

        Debug.Log(item);
        Debug.Log(id);
        
        var obj = Instantiate(item.prefeb, position, Quaternion.identity, parent);

        var cmp = obj.GetComponent<ComponentBase>();

        cmp.detail = item;

        onGameStart += cmp.OnGameStart;

        cmp.isPlayer1 = isPlayer1;


        components.Add(cmp);
        


        return cmp;
    }
    
    
    
    

    public void DestroyComponent(GameObject SeletingObject)
    {
        if (components.Contains(SeletingObject.GetComponent<ComponentBase>()))
        {
            Debug.Log(123);
            components.Remove(SeletingObject.GetComponent<ComponentBase>());
            Destroy(SeletingObject);
        }
    }
    public void StartGame()
    {
        GenerateComponentsJoint(slotSystem1.m_objectList, slotSystem1.GridMap);
        GenerateComponentsJoint(slotSystem2.m_objectList, slotSystem2.GridMap);
        onGameStart?.Invoke();
            player1.GetComponent<CircleCollider2D>().enabled = true;
            player2.GetComponent<CircleCollider2D>().enabled = true;
            player1.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            player2.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            distanceText1.enabled = true;
            distanceText2.enabled = true;

            foreach (var component in components)
            {
                if (component.isPlayer1)
                {
                    skillMgr1.SpawnSkill(component.detail, player1KeyMap[player1Index % player1KeyMap.Length],component);
                    component.keyCode = player1KeyMap[player1Index % player1KeyMap.Length];
                    if(component is ITriggerComponent|| component is IHoldComponent)player1Index++;
                }
                else
                {
                    skillMgr2.SpawnSkill(component.detail, player2KeyMap[player2Index%player2KeyMap.Length],component);
                    component.keyCode = player2KeyMap[player2Index % player2KeyMap.Length];
                    if(component is ITriggerComponent|| component is IHoldComponent)player2Index++;
                }

            }

            started = true;
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