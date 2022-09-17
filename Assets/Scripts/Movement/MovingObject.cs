using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovingObject : PoolingObjectReturner
{
    public VelocityState desiredVelocity;
    private GlobalMovement globalMove => DataManager.globalMovement;
    
    private float screenWidth => Screen.width;
    private Vector3 objectPosition => Camera.main.WorldToScreenPoint(transform.position + transform.localScale); 

    private void FixedUpdate()
    {
        transform.position += (Vector3.left * globalMove.ActualSpeed) * Time.fixedDeltaTime;

        float positionSquared = Mathf.Pow(objectPosition.x, 2f);
        if(objectPosition.x < screenWidth - positionSquared) gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.transform.tag == "Player" && desiredVelocity == globalMove.CurrentState)
        {
            gameObject.SetActive(false);
        }
        else if(other.transform.tag == "Player" && desiredVelocity != globalMove.CurrentState)
        {
            globalMove.ReduceSpeed();
        }
    }
}
