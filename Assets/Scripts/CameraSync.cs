using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSync : MonoBehaviour
{
    private Camera cameraToSync;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraToSync = GameObject.FindGameObjectWithTag("OutlineCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        cameraToSync.fieldOfView = mainCamera.fieldOfView;
        cameraToSync.transform.position = mainCamera.transform.position;
        cameraToSync.transform.rotation = mainCamera.transform.rotation;
    }
}
