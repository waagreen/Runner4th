using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.InputSystem;
using IncredibleCode;

public class Movement : MonoBehaviour, IDataMovement
{   
    [Header("Speed parameters")]
    [Range(0.5f, 2f)]public float accelerationRate;
    [SerializeField][Range(300f, 1000f)]private float maxRunSpeed;
    [SerializeField][Range(2f, 10f)] private float maxAcceleration;
    [SerializeField] private VelocityStates.State vState;
    
    [Space]
    [Header("Jump parameters")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float jumpHeight = 2f, inputGravity = -30f;
    [HideInInspector] public bool isGrounded => Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);
    
    private CharacterController playerJP;
    private PlayerInput referenceJP;

    private Vector3 currentVelocity;
    
    private float gravity => currentVelocity.y < 0 ? inputGravity * 3f : inputGravity;
    private float runAcceleration = 2f;
    public float actualSpeed = 0f;
    
    private void Awake()
    {
        playerJP = GetComponent<CharacterController>();
        referenceJP = new PlayerInput();
        
        vState = VelocityStates.State.Base;
        referenceJP.Keyboard.Jump.started += Jump;
    } 

    private void FixedUpdate()
    {
        //move forward
        if (isGrounded)
        {   
            runAcceleration = Mathf.Sqrt(accelerationRate * Time.fixedDeltaTime); 
            actualSpeed += runAcceleration;

            if (actualSpeed > maxRunSpeed) actualSpeed = maxRunSpeed;

            Debug.Log($"ACTUAL SPEED: {runAcceleration}");
        }  

        //move the player
		currentVelocity = (Vector3.right * Mathf.Abs(actualSpeed)) * Time.fixedDeltaTime;
        var desiredGraviy = (new Vector3(0f, gravity, 0f)) * Time.fixedDeltaTime;

        playerJP.Move(currentVelocity);
        if(!isGrounded) playerJP.Move(desiredGraviy);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(isGrounded){
            Debug.Log("JUMP TOKEN");
            currentVelocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
        }  
    } 
}
