using UnityEngine;

[System.Serializable]
public class MapManager : IManager
{
    [SerializeField]
    private int widthCount;
    [SerializeField]
    private int heightCount;
    [SerializeField]
    [Range(1.0f,5.0f)]
    private float scale;
    [SerializeField]
    [Range(1f, 3f)]
    private float gridSize;
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

    public override void PostAwake()
    {

        GenerateMap();
    }

    public override void PreUpdate()
    {

    }

    public override void PostUpdate()
    {
        
    }


    //generate map
    public void GenerateMap()
    {
        gridMap = new GameObject("GridMap");
        gridsArray = new GameObject[widthCount, heightCount];
        GameManager.Instance.astar.Nodes = new Node[heightCount, widthCount];

        for (int i = 0; i < widthCount; i++)
        {
            for (int j = 0; j < heightCount; j++)
            {
                GameObject g = Object.Instantiate(grid);
                Vector3 pos = new((i + 0.5f - widthCount / 2f) * scale  , 0.05f, (j - heightCount / 2f + 0.5f) * scale);
                g.transform.position = pos;
                g.transform.SetParent(gridMap.transform);
                g.transform.localScale *= gridSize;

                gridsArray[i, j] = g;
                Node node = new Node(true, pos, j, i, normalColor);
                GameManager.Instance.astar.Nodes[j, i] = node;
                GameManager.Instance.astar.NodesDict.Add(pos, node);

            }
        }
    }

    public void ShowGrid()
    {
        gridMap.SetActive(true);
        foreach(var node in GameManager.Instance.astar.Nodes)
        {
            if (node.walkable)
            {
                gridsArray[node.gridX, node.gridY].SetActive(true);
            }
        }
    }

    public void HideGrid()
    {
        foreach (var node in GameManager.Instance.astar.Nodes)
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
