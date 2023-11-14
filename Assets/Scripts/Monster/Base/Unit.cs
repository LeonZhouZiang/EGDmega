using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [HideInInspector]
    public string unitName;
    [HideInInspector]
    public Vector3 worldPostition;

    [HideInInspector]
    public Vector3 orientation = Vector3.back;

    public Vector2Int startIndex;

    private void Start()
    {
        transform.position = GameManager.Instance.mapManager.GridsArray[startIndex.y, startIndex.x].transform.position;
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

    public void MovePath(List<Vector3> path)
    {
        StartCoroutine(nameof(MoveTo), path);
    }

    IEnumerator MoveTo(List<Vector3> path)
    {
        int i = 0;
        Vector3 target = path[0];
        
        while (Vector3.SqrMagnitude(transform.position - path[^1]) > 0.01f)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, target, Time.deltaTime * 10);
            transform.position = newPosition;
            //reached tmp
            if(Vector3.SqrMagnitude(transform.position - target) > 0.01f)
            {
                i++;
                if(i < path.Count)target = path[i];
            }
            yield return new WaitForSeconds(0.5f);
        }

        if (gameObject.CompareTag("Monster"))
        {
            if (GameManager.Instance.combatManager.isPreActionPhase)
                GameManager.Instance.combatManager.DoActionInQueueRecursively();
            else
                GameManager.Instance.combatManager.DoMonsterInstanceActionsRecursively();
        }
    }
}
