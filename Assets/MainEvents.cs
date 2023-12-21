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

    private Resource currentResource; // Soldier tarafýndan toplanan kaynaðý saklamak için deðiþken


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
            // Sað týklama ile "Ground" katmanýndaki bir obje mi seçildi?
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
        // Kaynaðý topla
        resource.collectResource(soldier);

        // Kaynaðý sakla ve tekrar oluþma süresini baþlat
        currentResource = resource;
        resourceTimer = resourceRespawnTime;
    }

    void RespawnResource()
    {
        GameObject resourceObject = Instantiate(Resources.Load("ResourcePrefab"), Vector3.zero, Quaternion.identity) as GameObject;
        Resource newResource = resourceObject.GetComponent<Resource>();

        // Yeni kaynaðý baþka nesnelerle çakýþmasýný önlemek için Soldier ve Ground katmanlarýna ekleyin
        if (newResource != null)
        {
            newResource.gameObject.layer = LayerMask.NameToLayer("Soldier");
            
        }

        // Kaynaðýn tekrar oluþma süresini sýfýrla
        resourceTimer = 0f;

        // Kaynak nesnesini temizle
        currentResource = null;
    }

    public void AddCollectedResource(float amount)
    {
        // Toplanan kaynak miktarýný kaynak dizisine ekleyebilir veya baþka bir iþlem yapabilirsiniz.
        // Bu örnekte kaynaklarý sýfýrdan baþlatýyorum, sizin senaryonuza göre uygun iþlemleri yapmalýsýnýz.
        resources[0] += amount;
    }
}




