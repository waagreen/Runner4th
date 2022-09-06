using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : PoolingObjectReturner
{
    private float movingSpeed => DataManager.globalMovement.ActualSpeed;
    private float screenWidth => Screen.width;

    void FixedUpdate()
    {
        transform.position += (Vector3.left * movingSpeed) * Time.fixedDeltaTime;

        if (transform.position.x < 0) gameObject.SetActive(false);
    }
}
