using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public class GroundManager : SingletonMono<GroundManager>
{
    public List<GameObject> groundPrefabs;
    public int groundWidth;
    public int groundNum;
    private int[] randomSequence;


    public SpriteShape ssp1, ssp2;
    
    private void Start()
    {
        SpawnGround();
    }

    public void SpawnGround()
    {
        // 初始化随机数序列
        randomSequence = new int[groundNum];
        for (int i = 0; i < groundNum; i++)
        {
            randomSequence[i] = Random.Range(0, groundPrefabs.Count);
        }

        // 清除现有地面
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 生成新的地面
        for (int i = 0; i < groundNum; i++)
        {
            int prefabIndex = randomSequence[i];
            GameObject groundPrefab = groundPrefabs[prefabIndex];
            Vector3 spawnPosition = new Vector3(i * groundWidth, 0, 0);
            Instantiate(groundPrefab, spawnPosition, Quaternion.identity, transform);
            var obj2=Instantiate(groundPrefab, spawnPosition+new Vector3(0,100,0), Quaternion.identity, transform);
            obj2.GetComponent<SpriteShapeController>().spriteShape = ssp2;
        }
    }
}