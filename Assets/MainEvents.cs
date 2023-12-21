/*
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

    private float resourceRespawnTime = 10f;
    private float resourceTimer = 0f;

    private Resource currentResource; // Soldier taraf�ndan toplanan kayna�� saklamak i�in de�i�ken
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }

        if (currentResource == null && resourceTimer > 0)
        {
            resourceTimer -= Time.deltaTime;
        }
        if (currentResource == null && resourceTimer <= 0)
        {
            RespawnResource();
        }
    }

    void HandleLeftClick()
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
    void HandleRightClick()
    {
        if (current_selected != null)
        {
            LayerMask layerMask = LayerMask.GetMask("Soldier");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Resource")))
            {
                Soldier soldier = current_selected.GetComponent<Soldier>();
                if (soldier != null)
                {
                    Resource resource = hit.transform.gameObject.GetComponent<Resource>();

                    if (resource != null)
                    {
                        if (soldier.can_collect)
                        {
                            CollectResource(soldier, resource);
                        }
                        else
                        {
                            soldier.Walk(hit.point);
                        }
                    }

                }
            }
            // Sa� t�klama ile "Ground" katman�ndaki bir obje mi se�ildi?
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

    void CollectResource(Soldier soldier, Resource resource)
    {
        // Kayna�� topla
        resource.collectResource(soldier);

        // Kayna�� sakla ve tekrar olu�ma s�resini ba�lat
        currentResource = resource;
        resourceTimer = resourceRespawnTime;
    }

    void RespawnResource()
    {
        GameObject resourceObject = Instantiate(Resources.Load("ResourcePrefab"), Vector3.zero, Quaternion.identity) as GameObject;
        Resource newResource = resourceObject.GetComponent<Resource>();

        // Yeni kayna�� ba�ka nesnelerle �ak��mas�n� �nlemek i�in Soldier ve Ground katmanlar�na ekleyin
        if (newResource != null)
        {
            newResource.gameObject.layer = LayerMask.NameToLayer("Soldier");
            
        }

        // Kayna��n tekrar olu�ma s�resini s�f�rla
        resourceTimer = 0f;

        // Kaynak nesnesini temizle
        currentResource = null;
    }

    public void AddCollectedResource(float amount)
    {
        // Toplanan kaynak miktar�n� kaynak dizisine ekleyebilir veya ba�ka bir i�lem yapabilirsiniz.
        // Bu �rnekte kaynaklar� s�f�rdan ba�lat�yorum, sizin senaryonuza g�re uygun i�lemleri yapmal�s�n�z.
        resources[0] += amount;
    }
}
*/




