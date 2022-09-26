using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class MovingObject : PoolingObjectReturner
{   
    private float speed => DataManager.GlobalMovement.ActualSpeed;
    
    private Camera principalCamera;
    private BoxCollider myExtension;

    private void Awake() {
        if(principalCamera == null) principalCamera = Camera.main;
        if(myExtension == null) myExtension = GetComponent<BoxCollider>();
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3.left * speed) * Time.fixedDeltaTime;
        
        float dist = (transform.position + transform.localScale - principalCamera.transform.position).z;
        Vector3 leftBorder = principalCamera.ViewportToWorldPoint(new Vector3(0, 0, dist));

        if(transform.position.x < leftBorder.x - (myExtension.size.x + 2.5f)) gameObject.SetActive(false);
    }

}
