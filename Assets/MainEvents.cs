using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEvents : MonoBehaviour
{

    public float[] resources;

    public Material mat_normal;
    public Material mat_selected;
    public Material mat_collected;

    public GameObject current_selected;

    
    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Soldier")))
            {
                if (current_selected != null)
                {
                    MeshRenderer oldMeshRenderer = current_selected.GetComponent<MeshRenderer>();
                    if (oldMeshRenderer != null)
                    {
                        oldMeshRenderer.material = mat_normal;
                    }
                }
                current_selected = hit.transform.gameObject;
                MeshRenderer newMeshRenderer = current_selected.GetComponent<MeshRenderer>();

                if (newMeshRenderer != null)
                {
                    newMeshRenderer.material = mat_selected;
                }

            }
            else
            {
                if (current_selected != null)
                {
                    MeshRenderer oldMeshRenderer = current_selected.GetComponent<MeshRenderer>();
                    if (oldMeshRenderer != null)
                    {
                        oldMeshRenderer.material = mat_normal;
                    }
                    current_selected = null;
                }

            }
        }

        else if (Input.GetMouseButton(1))
        {
            if (current_selected != null)
            {
                LayerMask layerMask = LayerMask.GetMask("Soldier");
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // Right-clicked on a resource?
                if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Kaynak")))
                {
                    Soldier soldier = current_selected.GetComponentInParent<Soldier>();
                    if (soldier != null)
                    {
                        Resource resource = hit.transform.gameObject.GetComponent<Resource>();
                        if (resource != null)
                        {
                            // If the unit can collect resources, send it to the resource; otherwise, move to the clicked point
                            if (soldier.can_collect)
                            {
                                soldier.GoToResource(resource);
                            }
                            else
                            {
                                soldier.Walk(hit.point);    
                            }
                        }
                    }
                }

                else if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Ground")))
                {
                   Soldier soldier = current_selected.GetComponentInParent<Soldier>();
                    if (soldier != null)
                    {
                        soldier.Walk(hit.point);
                    }
                }
            }
        }
    }
}