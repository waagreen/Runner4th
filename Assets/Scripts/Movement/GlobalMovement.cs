using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public enum VelocityState : int
{ 
    Idle = -1,
    Base = 0,
    High = 1, 
    Maximun = 2, 
}

public class GlobalMovement : MonoBehaviour
{
    [Header("Speed parameters")]
    [SerializeField][Range(0.01f, 1f)] private float accelerationRate;
    [SerializeField][Range(40f, 300f)] protected float maxRunSpeed;
    [SerializeField] protected Renderer sphereFeedback;
    
    private const float kMinSpeed = 5f;
    
    public VelocityState CurrentState => GetSpeedState();
    protected float runAcceleration = 1f;
    private float _actualSpeed = kMinSpeed;
    public float ActualSpeed => _actualSpeed;


    protected void FixedUpdate()
    {
        //move forward
        runAcceleration = Mathf.Sqrt(accelerationRate * Time.fixedDeltaTime);
        _actualSpeed += runAcceleration / 1.5f;

        if (CurrentState == VelocityState.Maximun)
        { 
            _actualSpeed = maxRunSpeed;
            sphereFeedback.material.color = new Color(255, 0, 0);
        }
        else if (CurrentState == VelocityState.High) sphereFeedback.material.color = new Color(0, 128, 0);
        else  sphereFeedback.material.color = new Color(50, 50, 50);
    }
    
    public VelocityState GetSpeedState()
    {
        //Switch btween the states of velocity
        VelocityState inState;

        if(ActualSpeed >= maxRunSpeed) inState = VelocityState.Maximun;
        else if(ActualSpeed >= maxRunSpeed / 2f) inState = VelocityState.High;
        else if(ActualSpeed < kMinSpeed) inState = VelocityState.Idle;
        else inState = VelocityState.Base;

        return inState;
    }

    public void ReduceSpeed() => _actualSpeed = _actualSpeed / 2f;
}
