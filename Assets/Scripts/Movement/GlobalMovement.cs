using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.Events;

public enum VelocityState : int
{ 
    Idle = -1,
    Base = 0,
    High = 1, 
    Maximun = 2, 
    Immovable = 3,
}

public class GlobalMovement : MonoBehaviour
{

    [Header("Speed parameters")]
    [SerializeField][Range(0.01f, 1f)] private float accelerationRate;
    [SerializeField][Range(1f, 50f)] protected float maxRunSpeed;
    [SerializeField] protected Transform PlayerTransform;

    [Header("State Colors")]
    public Gradient[] velocityGradients;

    public VelocityState CurrentState => GetSpeedState();
    
    public const float kMinSpeed = 1.5f;
    public float distance = 0;
    public float CurrentSpeed => _currentSpeed;
    
    private UnityEvent deathEvent;
    private float runAcceleration = 1f;
    private float _currentSpeed = kMinSpeed;
    
    private int totalShieldCharges => (int)DataManager.Events.passiveSkills.shieldCharges;
    private int currentShieldCharges = 0;

    private void Start()
    {
        if(!DataManager.isGameplay) return;

        deathEvent = DataManager.Events.OnPlayerDeath;
        deathEvent.AddListener(SetSpeedToZero);

        maxRunSpeed += DataManager.Events.passiveSkills.maxSpeed;
        accelerationRate += DataManager.Events.passiveSkills.maxAcceleration;
        
        currentShieldCharges = totalShieldCharges;
    }

    protected void FixedUpdate()
    {
        if(!DataManager.isGameplay) return;

        if(CurrentState == VelocityState.Idle) deathEvent.Invoke();
        else
        {
            //move forward
            _currentSpeed += runAcceleration / 3f;
            distance += _currentSpeed * Time.deltaTime;
        }

        runAcceleration = OnSlope(PlayerTransform) ? Mathf.Sqrt((accelerationRate * 10f) * Time.fixedDeltaTime) : runAcceleration = Mathf.Sqrt(accelerationRate * Time.fixedDeltaTime);

		if (CurrentState == VelocityState.Maximun) _currentSpeed = maxRunSpeed;
    }
    
    public VelocityState GetSpeedState()
    {
        //Switch btween the states of velocity
        VelocityState inState;

        if(CurrentSpeed >= maxRunSpeed) inState = VelocityState.Maximun;
        else if(CurrentSpeed >= maxRunSpeed / 2f) inState = VelocityState.High;
        else if(CurrentSpeed < kMinSpeed) inState = VelocityState.Idle;
        else inState = VelocityState.Base;

        return inState;
    }

    public void SetSpeedToZero() => _currentSpeed = 0f;
    public void ReduceSpeed()
    {
        if (currentShieldCharges > 0) 
        {
            currentShieldCharges--;
            return;
        } 
        else _currentSpeed /= 2f;
    }
    
    private bool OnSlope(Transform t)
    {
        if(t == null) return false;
        
        RaycastHit slopeHit;

        if(Physics.Raycast(t.position, Vector3.down, out slopeHit, transform.localScale.x / 2 + 0.5f))
        {
            return slopeHit.normal != Vector3.up ? true : false;
        }
        else return false;
    }
}
