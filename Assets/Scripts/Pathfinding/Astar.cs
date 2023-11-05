using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Astar : IManager
{
    public Node startNode;
    public Node targetNode;

    private Node[,] nodes;
    private Dictionary<Vector3, Node> nodesDict = new();
    private MapManager map;

    public Node[,] Nodes { get => nodes; set => nodes = value; }
    public Dictionary<Vector3, Node> NodesDict => nodesDict;

    public override void PostAwake()
    {
        map = GameManager.Instance.mapManager;
    }

    public Node NodeFromWorldPosition(Vector2 position)
    {
        Vector3 pos = new Vector3(position.x, 0, position.y);
        return nodesDict[pos];
    }
    public Node NodeFromWorldPosition(Vector3 position)
    {
        return nodesDict[position];
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

    public Node[] TryFindPath(Vector2 startPos, Vector2 targetPos, int steps)
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
                Node[] path = Retrace(startNode, targetNode);
                if (path.Length > steps) path = new Node[0];
                return path;
            }

            //find neighbors
            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedList.Contains(neighbor) || GetDistanceBetweenNodes(startNode,neighbor) > steps)
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
                        map.GridsArray[neighbor.gridX, neighbor.gridY].GetComponent<SpriteRenderer>().color = Color.yellow;
                    }
                }
            }
        }
        return new Node[0];
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
    private Node[] Retrace(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currentNode = end;

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        return path.ToArray();
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
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public Color color;
    public Node(bool walkable, Vector2 position, int gridX, int gridY, Color color)
    {
        this.walkable = walkable;
        this.worldPosition = position;
        this.gridX = gridX;
        this.gridY = gridY;
        this.color = color;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

}
