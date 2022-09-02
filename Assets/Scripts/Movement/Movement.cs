using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float maxRunSpeed = 300f;
    [SerializeField] private float runAcceleration;
    [SerializeField] private float maxAcceleration = 10f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -30f;
    [SerializeField] private VelocityStates.State vState;

    private CharacterController player;
    private Vector3 currentVelocity;
    private Vector3 velocity;
    private bool isGrounded;
    private float horizontalInput;

    public float runSpeed = 10f;

    void Awake()
    {
        player = GetComponent<CharacterController>();
        vState = VelocityStates.State.Base;
    }


    void Update()
    {
        //Input
        horizontalInput = 1;



        
        //face the direction
        transform.forward = new Vector3(horizontalInput, 0, Mathf.Abs(horizontalInput) - 1);




        //isGrounded
        isGrounded = Physics.CheckSphere(transform.position, 0.3f, groundLayers, QueryTriggerInteraction.Ignore);




        //vertical velocity
        if (isGrounded && velocity.y < 0){
            velocity.y = 0;
        }else{
            //add gravity
            velocity.y += gravity * Time.deltaTime;
        }




        //move forward
        if (isGrounded)
        {
            float speedRate = runSpeed/maxRunSpeed;
            runAcceleration = maxAcceleration * (1 - speedRate);
            runSpeed += runAcceleration;

            if (runSpeed > maxRunSpeed) runSpeed = maxRunSpeed;

            Debug.Log(runSpeed);
        }




        //Set the runSpeed based on it's state
        if (isGrounded)
        {
			runSpeed = new VelocityStates().SetSpeed(vState, runSpeed);
		}
		



		//move the player
		currentVelocity = new Vector3(0, velocity.y, 0) * Time.deltaTime;
        player.Move(currentVelocity);




    }




    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed){
            if(isGrounded){
                velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
        }
    }
}
