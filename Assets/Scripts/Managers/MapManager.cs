using UnityEngine;

[System.Serializable]
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
    [SerializeField]
    Color pathColor;
    [SerializeField]
    Color hoverColor;

    private GameObject[,] gridsArray;
    private GameObject gridMap;

    public GameObject GridMap => gridMap;
    public GameObject[,] GridsArray { get { return gridsArray; } }

    public void PostAwake()
    {
        Debug.Log(1);
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
                Vector3 pos = new Vector3(i - width / 2f + 0.5f, 0.05f, j - height / 2f + 0.5f);
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
        gridMap.SetActive(true);
        foreach(var node in Astar.Instance.Nodes)
        {
            if (node.walkable)
            {
                gridsArray[node.gridX, node.gridY].SetActive(true);
            }
        }
    }

    public void HideGrid()
    {
        foreach (var node in Astar.Instance.Nodes)
        {
            gridsArray[node.gridX, node.gridY].SetActive(false);
        }
        gridMap.SetActive(false);
    }

    public void UpdatePathColor(Node[] nodes, Node hoverNode)
    {
        foreach(var node in nodes)
        {
            gridsArray[node.gridY, node.gridX].GetComponent<SpriteRenderer>().color = pathColor;
        }

        gridsArray[hoverNode.gridY, hoverNode.gridX].GetComponent<SpriteRenderer>().color = hoverColor;
    }

    public void UpdateRegionColor()
    {

    }

    public void UpdateGridHover()
    {

    }

    public void UpdateUnitHover()
    {

    }

    public void ResetColor()
    {
        foreach(var grid in gridsArray)
        {
            grid.GetComponent<SpriteRenderer>().color = normalColor;
        }

    }
}
