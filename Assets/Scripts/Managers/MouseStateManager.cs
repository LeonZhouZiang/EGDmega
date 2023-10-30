using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MouseStateManager : IManager
{
    public enum State{ UNIT, GRID, MOVE, CARD, NORMAL }
    public static State state;

    public static Action<Unit> UnitCallback;
    public static Action<Node> GridCallback;
    public static Action<Node[]> MoveCallback;

    private Vector2 startPos;
    private int range;

    public override void PostAwake()
    {

    }
    public override void PreUpdate()
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
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Grid"))
            {
                UpdateColor(hit);

                //click to select
                if (Input.GetMouseButtonDown(0))
                {
                    Node g = GameManager.Instance.astar.NodeFromPosition(hit.collider.transform.position);
                    GridCallback.Invoke(g);
                    Debug.Log("Get node");
                }
            }
        }
        
    }

    public void TrySelectDestination(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, LayerMask.GetMask("Grid")))
        {
            UpdateColor(hit);
            //click to select
            if (Input.GetMouseButtonDown(0))
            {
                Node[] path = GameManager.Instance.astar.TryFindPath(startPos, hit.collider.transform.position, range);
                if(path.Length != 0)
                {
                    //reachable
                    MoveCallback.Invoke(path);
                    CleanState();
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                CleanState();
            }
        }
    }

    public void TryGetUnit(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, LayerMask.GetMask("Unit")))
        {
            if (hit.collider.CompareTag("Monster"))
            {
                
            }
        }
    }

    public void CardDisplayState()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            CleanState();
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
            TryGetUnit(ray);
            return;
        }
        else if (state == State.UNIT)
        {
            TryGetUnit(ray);
            return;
        }
        else if(state == State.NORMAL)
        {
            
        }
        else if(state == State.CARD)
        {
            CardDisplayState();
        }
        else
        {
            Debug.Log("No state");
        }
        //exit selection
        if (Input.GetMouseButton(1))
        {
            CleanState();
        }
    }

    public void RequireMove(Vector2 start, int distance, Action<Node[]> moveCallback)
    {
        range = distance;
        startPos = start;
        MoveCallback = moveCallback;
        state = State.MOVE;

        GameManager.Instance.uiManager.UpdateStateText("Select a target");
    }
    public void RequireGrid(int distance, Action<Node> gridCallback)
    {
        range = distance;
        GridCallback = gridCallback;
        state = State.GRID;

        GameManager.Instance.mapManager.ShowGrid();
        GameManager.Instance.uiManager.UpdateStateText("Select a target");
    }
    public void RequireUnit(int distance, Action<Unit> unitCallback)
    {
        range = distance;
        UnitCallback = unitCallback;
        state = State.UNIT;

        GameManager.Instance.uiManager.UpdateStateText("Select a target");
    }

    public void RequireShowCard(Vector2[] gridPos)
    {
        GameManager.Instance.mapManager.ShowGrid();
    }
}
