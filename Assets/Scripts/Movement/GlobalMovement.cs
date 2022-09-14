using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class GlobalMovement : MonoBehaviour
{
    [Header("Speed parameters")]
    [SerializeField][Range(0.1f, 1f)] private float accelerationRate;
    [SerializeField][Range(200f, 500f)] protected float maxRunSpeed;
    [SerializeField][Range(1f, 5f)] protected float maxAcceleration;
    [SerializeField] protected VelocityStates.State vState;

    protected float runAcceleration = 1f;
    private float _actualSpeed = 0f;
    public float ActualSpeed => _actualSpeed;
    
    public void Awake()
    {
        vState = VelocityStates.State.Base;
    }

    protected void FixedUpdate()
    {
        //move forward
        runAcceleration = Mathf.Sqrt(accelerationRate * Time.fixedDeltaTime);
        _actualSpeed += runAcceleration;

        if (_actualSpeed > maxRunSpeed) _actualSpeed = maxRunSpeed;
    }
}
