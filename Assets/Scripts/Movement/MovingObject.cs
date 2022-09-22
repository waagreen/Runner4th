using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class MovingObject : PoolingObjectReturner
{   
    
    private GlobalMovement globalMove;
    
    private float screenWidth => Screen.width;
    private Vector3 myRightEdge;
    private float screenLeftEdge;
    private Vector3 objectPosition => Camera.main.WorldToScreenPoint(transform.position + transform.localScale);

    private void Awake()
    {
        globalMove = GameObject.Find("-- MANAGER").GetComponent<GlobalMovement>();
        myRightEdge = GetComponent<BoxCollider>().bounds.max;
        screenLeftEdge = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3.left * globalMove.ActualSpeed) * Time.fixedDeltaTime;

        var updatedEdge = myRightEdge.x + transform.position.x + 0.5f;
        if(updatedEdge < screenLeftEdge) gameObject.SetActive(false);
    }

}
