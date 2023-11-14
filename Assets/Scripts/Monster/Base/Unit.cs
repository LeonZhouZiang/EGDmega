using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [HideInInspector]
    public string unitName;

    [HideInInspector]
    public Vector3 orientation = Vector3.back;

    public Vector2Int startGridIndex;

    private TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
    public virtual void Start()
    {
        transform.position = GameManager.Instance.mapManager.GridsArray[startGridIndex.y, startGridIndex.x].transform.position;
        GameManager.Instance.astar.Nodes[startGridIndex.y, startGridIndex.x].walkable = false;
    }

    public void UpdateLeftRight()
    {
        if(orientation == Vector3.left)
        {

        }
        else
        {

        }
    }
    public void SetOrientation(Vector3 dir)
    {
        if(dir == Vector3.left || dir == Vector3.right)
        {
            orientation = dir;
        }
    }

    public async Task MovePath(List<Vector3> path)
    {
        completionSource = new();
        StartCoroutine(MoveTo(path));
        await completionSource.Task;
    }

    IEnumerator MoveTo(List<Vector3> path)
    {
        GameManager.Instance.mouseStateManager.allowedToClick = false;
        Vector3 target = path[0];
        GameManager.Instance.astar.NodeFromWorldPosition(transform.position).walkable = true;
        GameManager.Instance.mapManager.ShowCheckerBoard();

        int i = 0;
        while (Vector3.SqrMagnitude(transform.position - path[^1]) > 0.01f)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, target, Time.deltaTime * 10);
            transform.position = newPosition;
            //reached tmp
            if(Vector3.SqrMagnitude(transform.position - target) < 0.01f)
            {
                i++;
                if(i < path.Count)target = path[i];
                CameraManager.Instance.MoveToTarget(target);
                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }
        transform.position = path[^1];
        //Debug.Log(GameManager.Instance.astar.NodeFromWorldPosition(transform.position).gridX + " "+ GameManager.Instance.astar.NodeFromWorldPosition(transform.position).gridY);
        GameManager.Instance.astar.NodeFromWorldPosition(transform.position).walkable = false;
        GameManager.Instance.mouseStateManager.allowedToClick = true;
        completionSource.SetResult(true);
    }
}
