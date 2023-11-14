using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Astar : IManager
{
    public Node startNode;
    public Node targetNode;

    private Node[,] nodes;
    private Dictionary<Vector3Int, Node> nodesDict = new();
    private MapManager map;

    public Node[,] Nodes { get => nodes; set => nodes = value; }
    public Dictionary<Vector3Int, Node> NodesDict => nodesDict;

    public override void PostAwake()
    {
        map = GameManager.Instance.mapManager;
    }

    public Node NodeFromWorldPosition(Vector2 position)
    {
        Vector3Int pos = new Vector3Int((int)(position.x * 100), 5, (int)(100 * position.y));
        if (!nodesDict.ContainsKey(pos)) Debug.LogError("Invalid unit position");
        return nodesDict[pos];
    }
    public Node NodeFromWorldPosition(Vector3 position)
    {
        Vector3Int pos = new Vector3Int((int)(position.x * 100), 5, (int)(100 * position.z));
        if (!nodesDict.ContainsKey(pos)) Debug.LogError("Invalid unit position");
        return nodesDict[pos];
    }


    public void ResetColor()
    {
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                map.GridsArray[i, j].GetComponent<SpriteRenderer>().color = nodes[i, j].color;
            }
        }
    }

    public Node[] TryFindPath(Vector3 startPos, Vector3 targetPos, int steps)
    {
        startNode = NodeFromWorldPosition(startPos);
        targetNode = NodeFromWorldPosition(targetPos);

        if (startNode != null && targetNode != null)
        {
            return FindPath(steps);
        }
        else Debug.LogError("start nodes not valid");
            return null;
    }

    public Node[] FindPath(int steps)
    {
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new();
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            //find best node
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
                //result found check steps
                Node[] path = Retrace(startNode, targetNode, steps);
                return path;
            }

            //find neighbors
            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if ((!neighbor.walkable && neighbor != targetNode)|| closedList.Contains(neighbor))
                {
                    continue;
                }
                else
                {
                    //get distance
                    int newMovementCostToNeighbor = currentNode.gCost + 1;//GetDistanceBetweenNodes(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistanceBetweenNodes(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }

            }
        }
        return null;
    }

    public int GetDistanceBetweenNodes(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return dstX + dstY;
    }
    public int GetDistanceBetweenWorldPos(Vector3 a, Vector3 b)
    {
        Node nodeA = NodeFromWorldPosition(a);
        Node nodeB = NodeFromWorldPosition(b);
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return dstX + dstY;
    }
    private Node[] Retrace(Node start, Node end, int steps)
    {
        List<Node> path = new List<Node>();
        Node currentNode = end;
        while (currentNode != start)
        {
            if (currentNode.walkable)
                path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        List<Node> result = new();
        steps = Mathf.Min(steps, path.Count);
        for(int i = 0; i < steps; i++)
        {
            result.Add(path[i]);
        }
        return result.ToArray();
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (Mathf.Abs(x) == Mathf.Abs(y)) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < nodes.GetLength(0) && checkY >= 0 && checkY < nodes.GetLength(1))
                {
                    neighbors.Add(nodes[checkY, checkX]);
                }
            }
        }

        return neighbors;
    }
}


public class Node
{
    public bool walkable;
    private Vector2 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public Color color;

    public Vector3 WorldPosition { get => new Vector3(worldPosition.x, 0, worldPosition.y); set => worldPosition = value; }
    public Node(bool walkable, Vector3 position, int gridX, int gridY, Color color)
    {
        this.walkable = walkable;
        this.worldPosition = new Vector2(position.x, position.z);
        this.gridX = gridX;
        this.gridY = gridY;
        this.color = color;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

}
