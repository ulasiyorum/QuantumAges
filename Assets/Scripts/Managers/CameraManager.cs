using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviourPun
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;  
    public Vector2 panLimit;  
    
    public float scrollSpeed = 20f;
    public float minY = 20f;
    public float maxY = 120f;
    

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        
        Vector3 pos = transform.position;
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness )
        {
            pos.z += panSpeed * Time.deltaTime;   //move forward
        }
        if (Input.GetKey("s") || Input.mousePosition.y <=  panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;   //move back
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;   //move right
        }
        if (Input.GetKey("a") || Input.mousePosition.x <=  panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;   //move left
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");  //zoom in and out
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);  //limit the camera movement   
        pos.y = Mathf.Clamp(pos.y, minY, maxY);  //limit the camera movement    
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);  //limit the camera movement


        transform.position = pos;
    }
}
