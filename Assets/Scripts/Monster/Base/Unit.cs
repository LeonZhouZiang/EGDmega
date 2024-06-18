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

    private bool facing;
    private TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
    public virtual void Start()
    {
        facing = GetComponentInChildren<SpriteRenderer>().flipX;
        transform.position = GameManager.Instance.mapManager.GridsArray[startGridIndex.y, startGridIndex.x].transform.position;
        GameManager.Instance.astar.Nodes[startGridIndex.y, startGridIndex.x].walkable = false;
    }

    public void UpdateLeftRight(float xDiff)
    {
        if (xDiff != 0)
            GetComponentInChildren<SpriteRenderer>().flipX = xDiff > 0 ? facing : !facing;
    }

    public async Task MovePath(List<Vector3> path)
    {
        completionSource = new();
        StartCoroutine(MoveTo(path));
        await completionSource.Task;
    }

    IEnumerator MoveTo(List<Vector3> path)
    {
        Vector3 target = path[0];
        UpdateLeftRight(target.x - transform.position.x);

        GameManager.Instance.mouseStateManager.allowedToClick = false;
        GameManager.Instance.astar.NodeFromWorldPosition(transform.position).walkable = true;
        GameManager.Instance.mapManager.ShowCheckerBoard();

        int i = 0;
        while (Vector3.SqrMagnitude(transform.position - path[^1]) > 0.001f)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, target, Time.deltaTime * 10);
            transform.position = newPosition;
            //reached tmp
            if(Vector3.SqrMagnitude(transform.position - target) < 0.001f)
            {
                i++;
                if(i < path.Count)target = path[i];
                UpdateLeftRight(target.x - transform.position.x);

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
