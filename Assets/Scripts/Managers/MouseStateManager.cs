using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStateManager : MonoBehaviour, IManager
{
    public bool selectMode = false;

    public bool SelectMode => selectMode;

    public void PreUpdate()
    {
        
        
    }

    public void PostUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        OnMouseAction(ray);
    }

    public void PreLateUpdate()
    {

    }
    public void PostLateUpdate()
    {

    }

    public void TrySelectGrid(Ray ray)
    {
        if (true)
        {
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Grid"))
                {
                    //hit.collider.transform.position
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }
    }

    private void HoverCheck() 
    {

    }
    public void OnMouseAction(Ray ray)
    {
        HoverCheck();
        if (Input.GetMouseButton(0))
        {
            if (SelectMode)
            {
                TrySelectGrid(ray);
            }
        }
    }


}
