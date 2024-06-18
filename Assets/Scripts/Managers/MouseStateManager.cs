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

    public Action<List<Vector3>> MoveCallback; 
    public Action<Monster> MonsterCallback;
    [HideInInspector]
    public bool allowedToClick;

    private int range;
    private Unit sender;
    public override void PostAwake()
    {

    }

    public override void PostUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

        OnMouseAction(ray);
    }

    public void TrySelectGrid(Ray ray)
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    CleanState();
        //    GameManager.Instance.mapManager.HideCheckerBoard();
        //}

        //if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("Grid")))
        //{
        //    if (hit.collider.CompareTag("Grid"))
        //    {
        //        //in range
        //        if (GameManager.Instance.astar.GetDistanceBetweenWorldPos(hit.transform.position, sender.worldPostition) < range)
        //        {
        //            Node g = GameManager.Instance.astar.NodeFromWorldPosition(hit.collider.transform.position);
        //            GameManager.Instance.mapManager.UpdateHoverColor(g);
        //            //click to select
        //            if (Input.GetMouseButtonDown(0))
        //            {
        //                GridCallback.Invoke(g);
        //                CleanState();
        //                GameManager.Instance.mapManager.HideCheckerBoard();
        //            }
        //        }
        //        //out range
        //        else
        //        {
        //            GameManager.Instance.mapManager.ResetColor();
        //        }
        //    }
        //    //not hit
        //    else
        //    {
        //        GameManager.Instance.mapManager.ResetColor();
        //    }
        //}
        
    }

    public void TrySelectDestination(Ray ray)
    {
        if (allowedToClick && Input.GetMouseButtonDown(1))
        {
            CleanState();
            GameManager.Instance.mapManager.HideCheckerBoard();
            return;
        }
        if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("Grid")))
        {
            //update range color first, add path color on it
            GameManager.Instance.mapManager.UpdateRangeColor(sender.transform.position, range);

            //in range
            if (GameManager.Instance.astar.GetDistanceBetweenWorldPos(hit.transform.position, sender.transform.position) <= range)
            {
                Node[] path = GameManager.Instance.astar.TryFindPath(sender.transform.position, hit.collider.transform.position, range);
                GameManager.Instance.mapManager.UpdatePathColor(path);

                //click to select & reachable
                if (allowedToClick && Input.GetMouseButtonDown(0) && path != null)
                {
                    List<Vector3> pathList = new();
                    foreach(var node in path)
                    {
                        pathList.Add(node.WorldPosition);
                    }
                    MoveCallback.Invoke(pathList);
                    CleanState();
                }
            }
            //out range
            else
            {
                GameManager.Instance.mapManager.ResetColor();
                GameManager.Instance.mapManager.UpdateRangeColor(sender.transform.position, range);
            }
        }
        //not hit
        else
        {
            GameManager.Instance.mapManager.ResetColor();
            GameManager.Instance.mapManager.UpdateRangeColor(sender.transform.position, range);
        }
    }

    public void TryGetMonster(Ray ray)
    {
        if (allowedToClick && Input.GetMouseButtonDown(1))
        {
            CleanState();
            return;
        }

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("Unit")))
        {
            if (hit.collider.CompareTag("Monster"))
            {
                //in range
                if (GameManager.Instance.astar.GetDistanceBetweenWorldPos(hit.transform.position, sender.transform.position) <= range)
                {
                    //select target
                    if (Input.GetMouseButtonDown(0) && allowedToClick)
                    {
                        var monster = hit.collider.GetComponent<Monster>();
                        MonsterCallback.Invoke(monster);
                        CleanState();
                    }
                    //only hover
                    else { GameManager.Instance.uiManager.SetReticle(hit.transform.position, hit.transform.localScale.x); }
                }
                //not in range
                else GameManager.Instance.uiManager.CleanReticle();

            }
            else
            {   //not unit
                GameManager.Instance.uiManager.CleanReticle();
            }
        }
        //not hit
        else GameManager.Instance.uiManager.CleanReticle();
    }

    public void TryGetUnitInfo(Ray ray)
    {
        if (allowedToClick && Input.GetMouseButtonDown(1))
        {
            CleanState();
            GameManager.Instance.uiManager.CloseAllTabs();
            return;
        }

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("Unit")))
        {
            if (hit.collider.CompareTag("Monster") )
            {
                GameManager.Instance.mapManager.UpdateUnitHover(hit.collider.gameObject);

                if (allowedToClick && Input.GetMouseButtonDown(0))
                {

                    CameraManager.Instance.MoveToTarget(hit.transform.position);
                    GameManager.Instance.uiManager.ShowMonsterInfo(hit.collider.gameObject.GetComponent<Monster>());
                }
            }
            else if (hit.collider.CompareTag("Player"))
            {
                GameManager.Instance.mapManager.UpdateUnitHover(hit.collider.gameObject);

                if (allowedToClick && Input.GetMouseButtonDown(0))
                {
                    CameraManager.Instance.MoveToTarget(hit.transform.position);
                    GameManager.Instance.uiManager.ShowSurvivorInfo(hit.collider.gameObject.GetComponent<Survivor>());
                }
            }
        }

    }

    public void CardDisplayState(Ray ray)
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || allowedToClick)
        {
            CleanState();
            return;
        }

    }


    public void CleanState()
    {
        GameManager.Instance.uiManager.UpdateStateText("");
        GameManager.Instance.astar.ResetColor();
        GameManager.Instance.mapManager.HideCheckerBoard();
        CameraManager.Instance.ResetPosition();
        GameManager.Instance.uiManager.CleanReticle();
        state = State.NORMAL;
    }

    public void OnMouseAction(Ray ray)
    {
        if (state == State.GRID)
        {
            GameManager.Instance.mapManager.UpdateRangeColor(sender.transform.position, range);
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
            GameManager.Instance.mapManager.UpdateRangeColor(sender.transform.position, range);
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

    public void RequireMove(Unit sender, int distance, Action<List<Vector3>> moveCallback)
    {
        range = distance;
        MoveCallback = moveCallback;
        this.sender = sender;
        state = State.MOVE;

        GameManager.Instance.mapManager.ShowCheckerBoard();
        GameManager.Instance.astar.ResetColor();
        GameManager.Instance.uiManager.UpdateStateText("Select a grid");
    }
    //public void RequireGrid(Survivor sender, int distance, Action<Node> gridCallback)
    //{
    //    range = distance;
    //    GridCallback = gridCallback;
    //    this.sender = sender;
    //    state = State.GRID;

    //    GameManager.Instance.mapManager.ShowCheckerBoard();
    //    GameManager.Instance.uiManager.UpdateStateText("Select a grid");
    //}
    public void RequireAttack(Survivor sender, int distance, Action<Monster> unitCallback)
    {
        range = distance;
        MonsterCallback = unitCallback;
        this.sender = sender;
        state = State.UNIT;

        GameManager.Instance.mapManager.ShowCheckerBoard();
        GameManager.Instance.astar.ResetColor();
        GameManager.Instance.uiManager.UpdateStateText("Select a target");
    }

}
