using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : PoolingObjectReturner
{
    private float movingSpeed => DataManager.globalMovement.ActualSpeed;
    
    private float screenWidth => Screen.width;
    private Vector3 objectPosition => Camera.main.WorldToScreenPoint(transform.position + transform.localScale); 

    private void FixedUpdate()
    {
        transform.position += (Vector3.left * movingSpeed) * Time.fixedDeltaTime;

        float positionSquared = Mathf.Pow(objectPosition.x, 2f);
        if(objectPosition.x < screenWidth - positionSquared) gameObject.SetActive(false);
    }
}
