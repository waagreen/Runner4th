using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class DestructableObject : PoolingObjectReturner
{
    [SerializeField] private GlobalDataSO globalData;

    public bool isDestructable = false;
    [ConditionalField(nameof(isDestructable))] public VelocityState desiredVelocity;

    private void OnCollisionEnter(Collision other) 
    {
        if(isDestructable)
        {
            if(other.transform.tag == "Player" && (int)desiredVelocity <= (int)globalData.CurrentState)
            {
				gameObject.SetActive(false);
			}
            else if(other.transform.tag == "Player" && (int)desiredVelocity > (int)globalData.CurrentState )
            {
                globalData.ReduceSpeed();
            }
            
            if(other.transform.tag == "Player" && transform.tag == "Obstacle")
            {
				globalData.ReduceSpeed();
			}
        }
    }
}
