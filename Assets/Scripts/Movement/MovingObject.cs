using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class MovingObject : PoolingObjectReturner
{   
    
    private GlobalMovement globalMove;
    
    private float screenWidth => Screen.width;
    private Vector3 objectPosition => Camera.main.WorldToScreenPoint(transform.position + transform.localScale);

    private void Awake()
    {
        globalMove = GameObject.Find("-- MANAGER").GetComponent<GlobalMovement>();
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3.left * globalMove.ActualSpeed) * Time.fixedDeltaTime;

        float positionSquared = Mathf.Pow(objectPosition.x, 2f);
        if(objectPosition.x < screenWidth - 4800) gameObject.SetActive(false);
    }

}
