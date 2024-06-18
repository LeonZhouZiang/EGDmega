using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    Color rangeColor;

    private GameObject[,] gridsArray;
    private GameObject checkerBoard;

    public float Scale => scale;
    public GameObject CheckerBoard => checkerBoard;
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
        checkerBoard = new GameObject("GridMap");
        gridsArray = new GameObject[heightCount, widthCount];
        GameManager.Instance.astar.Nodes = new Node[heightCount, widthCount];

        for (int i = 0; i < heightCount; i++)
        {
            for (int j = 0; j < widthCount; j++)
            {
                GameObject g = Object.Instantiate(grid);
                Vector3 pos = new((j + 0.5f - widthCount / 2f) * scale  , 0.05f, (i - heightCount / 2f + 0.5f) * scale);
                g.transform.position = pos;
                g.transform.SetParent(checkerBoard.transform);
                g.transform.localScale *= gridSize;

                gridsArray[i, j] = g;
                Node node = new Node(true, pos, j, i, normalColor);
                GameManager.Instance.astar.Nodes[i, j] = node;
                Vector3Int modPos = new Vector3Int((int)(pos.x * 100), 5, (int)(pos.z * 100));
                GameManager.Instance.astar.NodesDict.Add(modPos, node);

            }
        }

        checkerBoard.SetActive(false);
    }

    public void ShowCheckerBoard()
    {
        checkerBoard.SetActive(true);
        foreach(var node in GameManager.Instance.astar.Nodes)
        {
            if (node.walkable)
            {
                gridsArray[node.gridY, node.gridX].SetActive(true);
            }
        }
    }

    //color------
    public void HideCheckerBoard()
    {
        GameManager.Instance.mapManager.ResetColor();
        foreach (var node in GameManager.Instance.astar.Nodes)
        {
            gridsArray[node.gridY, node.gridX].SetActive(false);
        }
        checkerBoard.SetActive(false);
    }

    public void UpdatePathColor(Node[] nodes)
    {
        foreach(var node in nodes)
        {
            gridsArray[node.gridY, node.gridX].GetComponent<MeshRenderer>().material.color = pathColor;
        }
        if(nodes.Length > 0)
            UpdateHoverColor(nodes[^1]);
    }

    public void UpdateHoverColor(Node node)
    {
        gridsArray[node.gridY, node.gridX].GetComponent<MeshRenderer>().material.color = hoverColor;
    }

    public void UpdateRegionColor(Node[] nodes)
    {
        foreach (var node in nodes)
        {
            gridsArray[node.gridY, node.gridX].GetComponent<MeshRenderer>().material.color = rangeColor;
        }
    }

    public void UpdateRangeColor(Vector3 position, int range)
    {
        GameManager.Instance.mapManager.ShowCheckerBoard();

        Node origin = GameManager.Instance.astar.NodeFromWorldPosition(position);
        for(int i = -range; i < range + 1; i++)
        {
            for(int j = -(range - Mathf.Abs(i)); j < range - Mathf.Abs(i) + 1; j++)
            {
                //check if out of bound
                if(0 <= i + origin.gridX && i + origin.gridX < GridsArray.GetLength(1) && 0 <= j + origin.gridY && j + origin.gridY < GridsArray.GetLength(0))
                    gridsArray[j + origin.gridY, i + origin.gridX].GetComponent<MeshRenderer>().material.color = rangeColor;
            }
        }
    }

    public void UpdateUnitHover(GameObject target)
    {
        Image sr = target.GetComponentInChildren<Image>();
        
    }

    public void ResetColor()
    {
        foreach(var grid in gridsArray)
        {
            grid.GetComponent<MeshRenderer>().material.color = normalColor;
        }
    }
}
