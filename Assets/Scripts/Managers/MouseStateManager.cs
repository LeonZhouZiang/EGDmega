using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MouseStateManager : IManager
{
    /// <summary>
    /// Unit/Grid/Move are used for combat selection
    /// Normal select and shows unit/card info.
    /// </summary>
    public enum State{ UNIT, GRID, MOVE, CARD, NORMAL }
    public static State state = State.NORMAL;

    public static Action<Unit> MonsterCallback;
    public static Action<Node> GridCallback;
    public static Action<Node[]> MoveCallback;

    private Vector2 startPos;
    private int range;

    public override void PostAwake()
    {

    }

    public override void PostUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

        OnMouseAction(ray);
    }

    private void UpdateColor(RaycastHit hit)
    {

    }
    public void TrySelectGrid(Ray ray)
    {
        if (Input.GetMouseButtonDown(1))
        {
            CleanState();
            GameManager.Instance.mapManager.HideCheckerBoard();
        }

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("Grid")))
        {
            if (hit.collider.CompareTag("Grid"))
            {
                UpdateColor(hit);

                //click to select
                if (Input.GetMouseButtonDown(0))
                {
                    Node g = GameManager.Instance.astar.NodeFromWorldPosition(hit.collider.transform.position);
                    GridCallback.Invoke(g);
                    Debug.Log("Get node");
                }
            }
        }
        
    }

    public void TrySelectDestination(Ray ray)
    {
        if (Input.GetMouseButtonDown(1))
        {
            CleanState();
            GameManager.Instance.mapManager.HideCheckerBoard();
            return;
        }

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("Grid")))
        {
            UpdateColor(hit);
            //click to select
            if (Input.GetMouseButtonDown(0))
            {
                Node[] path = GameManager.Instance.astar.TryFindPath(startPos, hit.collider.transform.position, range);
                if(path.Length != 0)
                {
                    //reachable
                    GameManager.Instance.mapManager.UpdatePathColor(path, path[0]);
                    MoveCallback.Invoke(path);
                    CleanState();
                }
            }
        }
    }

    public void TryGetMonster(Ray ray)
    {
        if (Input.GetMouseButtonDown(1))
        {
            CleanState();
        }

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("Unit")))
        {
            if (hit.collider.CompareTag("Monster"))
            {
                //select target
                if (Input.GetMouseButtonDown(0))
                {
                    var monster = hit.collider.GetComponent<Monster>();
                    MonsterCallback.Invoke(monster);
                    CleanState();
                }
                //only hover
                else { GameManager.Instance.uiManager.SetReticle(hit.transform.position, 0.5f, hit.transform.localScale.x); }
                
            }
            else
                GameManager.Instance.uiManager.CleanReticle();
        }
    }

    public void TryGetUnitInfo(Ray ray)
    {
        if (Input.GetMouseButtonDown(1))
        {
            CleanState();
            GameManager.Instance.uiManager.HideMonsterInfo();
            GameManager.Instance.uiManager.HideSurvivorInfo();
            return;
        }

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("Unit")))
        {
            if (hit.collider.CompareTag("Monster") && Input.GetMouseButtonDown(0))
            {
                Debug.Log("Show monster info");
                GameManager.Instance.uiManager.ShowMonsterInfo(hit.collider.gameObject.GetComponent<Monster>());
            }
            else if (hit.collider.CompareTag("Player") && Input.GetMouseButtonDown(0))
            {
                GameManager.Instance.uiManager.ShowSurvivorInfo(hit.collider.gameObject.GetComponent<Survivor>());
            }
        }

    }

    public void CardDisplayState(Ray ray)
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            CleanState();
            return;
        }

    }

    private void CleanState()
    {
        GameManager.Instance.uiManager.UpdateStateText("");
        GameManager.Instance.uiManager.CleanReticle();
        GameManager.Instance.mapManager.ResetColor();

        state = State.NORMAL;
    }

    public void OnMouseAction(Ray ray)
    {
        if (state == State.GRID)
        {
            TrySelectGrid(ray);
            return;
        }
        else if(state == State.MOVE)
        {
            TrySelectDestination(ray);
            return;
        }
        else if (state == State.UNIT)
        {
            TryGetMonster(ray);
            return;
        }
        else if(state == State.NORMAL)
        {
           
            TryGetUnitInfo(ray);
            return;
        }
        else if(state == State.CARD)
        {
            CardDisplayState(ray);
        }
        else
        {
            Debug.Log("No state");
        }
    }

    public void RequireMove(Vector2 start, int distance, Action<Node[]> moveCallback)
    {
        range = distance;
        startPos = start;
        MoveCallback = moveCallback;
        state = State.MOVE;

        GameManager.Instance.mapManager.ShowCheckerBoard();
        GameManager.Instance.uiManager.UpdateStateText("Select a grid");
    }
    public void RequireGrid(int distance, Action<Node> gridCallback)
    {
        range = distance;
        GridCallback = gridCallback;
        state = State.GRID;

        GameManager.Instance.mapManager.ShowCheckerBoard();
        GameManager.Instance.uiManager.UpdateStateText("Select a grid");
    }
    public void RequireUnit(int distance, Action<Unit> unitCallback)
    {
        range = distance;
        MonsterCallback = unitCallback;
        state = State.UNIT;

        GameManager.Instance.uiManager.UpdateStateText("Select a target");
    }

}
