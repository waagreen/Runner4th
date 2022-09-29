using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class DestructableObject : PoolingObjectReturner
{
    private GlobalMovement globalMove => DataManager.GlobalMovement;

    public bool isDestructable = false;
    [ConditionalField(nameof(isDestructable))] public VelocityState desiredVelocity;

    private void OnCollisionEnter(Collision other) 
    {
        if(isDestructable)
        {
            if(other.transform.tag == "Player" && (int)desiredVelocity <= (int)globalMove.CurrentState)
            {
				gameObject.SetActive(false);
			}
            else if(other.transform.tag == "Player" && (int)desiredVelocity > (int)globalMove.CurrentState)
            {
                globalMove.ReduceSpeed();
            }
        }
    }
}
