using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviourPun
{
    public float panSpeed = 100f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;

    public float scrollSpeed = 50f;
    public float minY = 8f;
    public float maxY = 48f;

    [SerializeField] private Transform green_initial_position;
    [SerializeField] private Transform red_initial_position;

    private void Awake()
    {
        if (!photonView.IsMine) return;

        transform.position = MultiplayerHelper.MasterPlayer.IsLocal ? green_initial_position.position : red_initial_position.position;
    }


    // Update is called once per frame
    private void Update()
    {
        //if (!photonView.IsMine) return;
        var pos = transform.position;
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
            pos += transform.forward * (panSpeed * Time.deltaTime);
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
            pos -= transform.forward * (panSpeed * Time.deltaTime);
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
            pos += transform.right * (panSpeed * Time.deltaTime);
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
            pos -= transform.right * (panSpeed * Time.deltaTime);
        if (Input.GetMouseButton(2))
        {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");

            var initialRotation = transform.rotation;

            transform.Rotate(Vector3.up, mouseX * panSpeed, 0);
            transform.Rotate(Vector3.left, mouseY * panSpeed, 0);

            var newEulerAngles = transform.rotation.eulerAngles;
            newEulerAngles.z = initialRotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(newEulerAngles);
        }

        var scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);


        transform.position = pos;
    }
}