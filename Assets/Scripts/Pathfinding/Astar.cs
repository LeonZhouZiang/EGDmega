using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    public static Astar Instance;

    public Node startNode;
    public Node targetNode;
    public Node[,] nodes;

    public MapManager map;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
    }


    public Node NodeFromPosition(Vector2 position)
    {
        foreach (var node in nodes)
        {
            if (node.position == position)
            {
                return node;
            }
        }
        return null;
    }

    public void ResetColor()
    {
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                map.Grids[i, j].GetComponent<SpriteRenderer>().color = nodes[i, j].color;
            }
        }
    }

    public void StartFind()
    {
        //nodes = level.ReturnNodes();
        if (startNode != null && targetNode != null)
        {
            StopCoroutine(nameof(FindPath));
            StartCoroutine(nameof(FindPath));
        }
    }

    IEnumerator FindPath()
    {

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new();
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                Retrace(startNode, targetNode);
                StopAllCoroutines();
            }

            //find neighbors
            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedList.Contains(neighbor))
                {
                    continue;
                }

                //get distance
                int newMovementCostToNeighbor = currentNode.gCost + GetDistanceBetweenNodes(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistanceBetweenNodes(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                        map.Grids[neighbor.gridX, neighbor.gridY].GetComponent<SpriteRenderer>().color = Color.yellow;
                    }
                    yield return new WaitForSeconds(0.005f);
                }
            }
        }
        yield return null;
    }

    public int GetDistanceBetweenNodes(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return dstX + dstY;
    }


    private void Retrace(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currentNode = end;

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        foreach (Node node in path)
        {
            map.Grids[node.gridX, node.gridY].GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < nodes.GetLength(0) && checkY >= 0 && checkY < nodes.GetLength(1))
                {
                    neighbors.Add(nodes[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }
}


public class Node
{
    public bool walkable;
    public Vector2 position;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public Color color;
    public Node(bool walkable, Vector2 position, int gridX, int gridY, Color color)
    {
        this.walkable = walkable;
        this.position = position;
        this.gridX = gridX;
        this.gridY = gridY;
        this.color = color;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

}
