using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : PoolingObjectReturner
{
    private float movingSpeed => DataManager.globalMovement.ActualSpeed;
    
    private float screenWidth => Screen.width;
    private Vector3 objectPosition => Camera.main.WorldToScreenPoint(transform.position); 

    private void Awake()
    {
        Debug.Log("WORLD TRANSFORM: " + objectPosition.x);
        Debug.Log("THIS TRANSFORM: " + screenWidth);
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3.left * movingSpeed) * Time.fixedDeltaTime;

        if(objectPosition.x < screenWidth - objectPosition.x / 2f) gameObject.SetActive(false);
    }
}
