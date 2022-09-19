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
    [SerializeField][Range(10f, 300f)] protected float maxRunSpeed;
    [SerializeField] protected Renderer sphereFeedback;
    [SerializeField] protected Transform PlayerTransform;

    [Header("State Colors")]
    public Gradient baseStateGradient;
    public Gradient highStateGradient;
    public Gradient maxStateGradient;
    
    private const float kMinSpeed = 5f;
    
    public VelocityState CurrentState => GetSpeedState();
    protected float runAcceleration = 1f;
    private float _actualSpeed = kMinSpeed;
    public float ActualSpeed => _actualSpeed;


    protected void FixedUpdate()
    {

        //move forward
        if(OnSlope(PlayerTransform)) runAcceleration = Mathf.Sqrt((accelerationRate * 3f) * Time.fixedDeltaTime);
        else runAcceleration = Mathf.Sqrt(accelerationRate * Time.fixedDeltaTime); 
        
        _actualSpeed += runAcceleration / 1.5f;

        if (CurrentState == VelocityState.Maximun)
        { 
            _actualSpeed = maxRunSpeed;
            sphereFeedback.material.color = maxStateGradient.colorKeys[1].color;
        }
        else if (CurrentState == VelocityState.High) sphereFeedback.material.color = highStateGradient.colorKeys[1].color;
        else  if (CurrentState == VelocityState.Base) sphereFeedback.material.color = baseStateGradient.colorKeys[1].color;
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
    
    private bool OnSlope(Transform t)
    {
        RaycastHit slopeHit;

        if(Physics.Raycast(t.position, Vector3.down, out slopeHit, transform.localScale.x / 2 + 0.5f))
        {
            return slopeHit.normal != Vector3.up ? true : false;
        }
        else return false;
    }
}
