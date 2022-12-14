using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[RequireComponent(typeof(BoxCollider))]
public class MovingObject : PoolingObjectReturner
{   
    private float speed => DataManager.GlobalMovement.CurrentSpeed;
    
    private Camera principalCamera;
    private BoxCollider myExtension;
    Vector3 leftBorder;

    private void Start() {
        if(principalCamera == null) principalCamera = Camera.main;
        if(myExtension == null) myExtension = GetComponent<BoxCollider>();
        
        float dist = (transform.position - principalCamera.transform.position).z;
        leftBorder = principalCamera.ViewportToWorldPoint(new Vector3(0, 0, dist));
    }

    private void FixedUpdate()
    {
        if (DataManager.isPlayingCutscene == true) return;

        transform.position += (Vector3.left * speed) * Time.fixedDeltaTime;

        if(transform.position.x < leftBorder.x - (myExtension.size.x + 2.5f)) gameObject.SetActive(false);
    }

}
