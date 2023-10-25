using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour, IManager
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    GameObject grid;

    private GameObject[,] grids;
    private GameObject gridMap;

    public GameObject GridMap => gridMap;
    public GameObject[,] Grids { get { return grids; } }


    public void Awake()
    {
        gridMap = new GameObject("GridMap");
        grids = new GameObject[width,height];


        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                GameObject g = Instantiate(grid);
                Vector3 pos = new Vector3(i, 0, j);
                g.transform.position = pos;
                g.transform.SetParent(gridMap.transform);

                grids[i, j] = g;
            }
        }
    }

    public void PostLateUpdate()
    {
        
    }

    public void PostUpdate()
    {
        
    }

    public void PreLateUpdate()
    {
        
    }

    public void PreUpdate()
    {
        
    }
}
