using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using IncredibleCode;
public class Movement : MonoBehaviour, IDataMovement
{
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float inputGravity = -30f;

    private CharacterController player;
    private Vector3 velocity;
    private bool isGrounded => Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);
    private float horizontalInput;
    private Vector3 initialPosition;
    private float gravity => velocity.y < 0 ? inputGravity * 3f : inputGravity; 

    [SerializeField] private float runSpeed = 8f, jumpHeight = 2f,inputGravity = -30f;
    [HideInInspector] public bool isGrounded => Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);
    private CharacterController playerJP;
    private PlayerInput referenceJP;
    private Vector3 velocity;
    private float gravity => velocity.y < 0 ? inputGravity * 3f : inputGravity;
    private float horizontalInput;

    void Awake(){
        playerJP = GetComponent<CharacterController>();
        referenceJP = new PlayerInput();

        referenceJP.Keyboard.Jump.started += Jump;
    } 
    
    void Update()
    {
        horizontalInput = 1;

        //face the direction
        transform.forward = new Vector3(horizontalInput, 0, Mathf.Abs(horizontalInput) - 1);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
        else
        {
            //add gravity
            velocity.y += gravity * Time.deltaTime;
        }

        //vertical velocity
        playerJP.Move(velocity * Time.deltaTime);
    }

    public void Jump(InputAction.CallbackContext context){
        if(context.performed){
            if(isGrounded){
                velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
                CameraManager.SetNoise(ShakeMode.strong);
            }
        }
    } 
}
