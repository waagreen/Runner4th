using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class GlobalMovement : MonoBehaviour
{
    [Header("Speed parameters")]
    [SerializeField][Range(0.5f, 2f)] private float accelerationRate;
    [SerializeField][Range(300f, 1000f)] protected float maxRunSpeed;
    [SerializeField][Range(2f, 10f)] protected float maxAcceleration;
    [SerializeField] protected VelocityStates.State vState;

    protected float runAcceleration = 2f;
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
