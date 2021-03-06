﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    //[Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 15;
    public int rows = 15;
    public Count wallCount = new Count(3, 6);
    public Count foodCount = new Count(1, 3);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] monsterTiles1;
    public GameObject[] monsterTiles2;
    public GameObject[] monsterTiles3;
    public GameObject[] monsterTiles4;
    public GameObject[] monsterTiles5;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < rows - 1; x++)
        {
            for (int z = 1; z < columns - 1; z++)
            {
                gridPositions.Add(new Vector3(x, 0f, z));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        
        for (int x = -1; x < rows + 1; x++)
        {
            for (int z = -1; z < columns + 1; z++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == rows || z == -1 || z == columns)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, 0f, z), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int Maximum)
    {
        int objectCount = Random.Range(minimum, Maximum + 1);
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        InitialiseList();
        BoardSetup();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int monsterCount = (int)Mathf.Log(level, 2);
        if (level < 10)
        {
            LayoutObjectAtRandom(monsterTiles1, monsterCount, monsterCount);
        }
        else if (10 <= level && level < 20)
        {
            LayoutObjectAtRandom(monsterTiles2, monsterCount, monsterCount);
        }
        else if (20 <= level && level < 30)
        {
            LayoutObjectAtRandom(monsterTiles3, monsterCount, monsterCount);
        }
        else if (30 <= level && level < 40)
        {
            LayoutObjectAtRandom(monsterTiles4, monsterCount, monsterCount);
        }
        else if (40 <= level && level <= 50)
        {
            LayoutObjectAtRandom(monsterTiles5, monsterCount, monsterCount);
        }
        Instantiate(exit, new Vector3(rows - 1, 0F, columns - 1), Quaternion.identity);
    }
}