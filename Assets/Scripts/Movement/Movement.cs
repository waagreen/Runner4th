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
    [SerializeField] private float runSpeed = 8f, jumpHeight = 2f,inputGravity = -30f;
    [HideInInspector] public bool isGrounded => Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);
    
    private CharacterController playerJP;
    private PlayerInput referenceJP;

    private CharacterController player;
    private Vector3 currentVelocity;
    private Vector3 velocity;
    
    private float gravity => velocity.y < 0 ? inputGravity * 3f : inputGravity;
    private float horizontalInput;
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
        //Input
        horizontalInput = 1;
     
        //face the direction
        transform.forward = new Vector3(horizontalInput, 0, Mathf.Abs(horizontalInput) - 1);

        //vertical velocity
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
        else
        {
            //add gravity
            velocity.y += gravity * Time.deltaTime;
        }
        
        //move forward
        if (isGrounded)
        {   
            runAcceleration = Mathf.Sqrt(accelerationRate * Time.fixedDeltaTime); 
            actualSpeed += runAcceleration;

            if (actualSpeed > maxRunSpeed) actualSpeed = maxRunSpeed;

            Debug.Log($"ACTUAL SPEEd: {runAcceleration}");
            Debug.Log(actualSpeed);
        }
		
        //move the player
		currentVelocity = new Vector3(0, velocity.y * actualSpeed, 0) * Time.deltaTime;
        player.Move(currentVelocity);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(isGrounded){
            velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
        }  
    } 
}
