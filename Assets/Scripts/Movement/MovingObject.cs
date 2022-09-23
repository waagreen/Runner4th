using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class MovingObject : PoolingObjectReturner
{   
    private GlobalMovement globalMove => DataManager.globalMovement;
    private Camera mainCam => globalMove.MainCamera;

    private float screenWidth => Screen.width;
    private Vector3 myRightEdge;
    private BoxCollider myExtension;

    private void Awake() {
       if(myExtension == null) myExtension = GetComponent<BoxCollider>();
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3.left * globalMove.ActualSpeed) * Time.fixedDeltaTime;

        myRightEdge = mainCam.ScreenToViewportPoint(transform.position);
        
        float dist = (transform.position + transform.localScale - mainCam.transform.position).z;
        Vector3 leftBorder = mainCam.ViewportToWorldPoint(new Vector3(0, 0, dist));

        if(transform.position.x < leftBorder.x - (myExtension.size.x + 2.5f)) gameObject.SetActive(false);
    }

}
