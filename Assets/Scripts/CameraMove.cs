using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMove : MonoBehaviour
{
    private Camera cam;
    private Transform targetTransform;
    private Vector3 startPosition;

    private const float kVerticalScreenLimit = -4f;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetTransform = GameObject.FindGameObjectWithTag(DataManager.playerTag).transform;
    }

    void Update()
    {
        Vector3 updatedPosition = new Vector3(cam.transform.position.x, targetTransform.position.y + 1.15f, cam.transform.position.z);
        
        if(targetTransform.position.y > kVerticalScreenLimit) cam.transform.position = Vector3.MoveTowards(cam.transform.position, updatedPosition, 5f);
    }
}
