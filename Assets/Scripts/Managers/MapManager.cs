using UnityEngine;

public class MapManager : MonoBehaviour, IManager
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    GameObject grid;
    [SerializeField]
    Color normalColor;

    private GameObject[,] gridsArray;
    private GameObject gridMap;

    public GameObject GridMap => gridMap;
    public GameObject[,] GridsArray { get { return gridsArray; } }


    public void Awake()
    {
        GenerateMap();
    }
    public void PreUpdate()
    {

    }

    public void PostUpdate()
    {
        
    }

    public void PostLateUpdate()
    {
        
    }
    public void PreLateUpdate()
    {
        
    }

    //generate map
    public void GenerateMap()
    {
        gridMap = new GameObject("GridMap");
        gridsArray = new GameObject[width, height];
        Astar.Instance.Nodes = new Node[height, width];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject g = Instantiate(grid);
                Vector3 pos = new Vector3(i - width / 2f, 0, j - height / 2);
                g.transform.position = pos;
                g.transform.SetParent(gridMap.transform);

                gridsArray[i, j] = g;
                Node node = new Node(true, pos, j, i, normalColor);
                Astar.Instance.Nodes[j, i] = node;
                Astar.Instance.NodesDict.Add(pos, node);

            }
        }
    }

    public void ShowGrid()
    {
        foreach(var node in Astar.Instance.Nodes)
        {
            if (node.walkable)
            {
                gridsArray[node.gridX, node.gridY].SetActive(true);
            }
        }
    }

    public void ShowPath(Node[] nodes)
    {

    }
}
