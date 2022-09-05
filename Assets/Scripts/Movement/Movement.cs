using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Speed parameters")]
    [Range(0.5f, 2f)] public float accelerationRate;
    [SerializeField][Range(300f, 1000f)] protected float maxRunSpeed;
    [SerializeField][Range(2f, 10f)] protected float maxAcceleration;
    [SerializeField] protected VelocityStates.State vState;

    [Space]
    [Header("Jump parameters")]
    [SerializeField] protected LayerMask groundLayers;
    [SerializeField] protected float jumpHeight = 2f, inputGravity = -30f;
    [HideInInspector] public bool isGrounded => Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);

    protected CharacterController playerJP;
    protected PlayerInput referenceJP;
    protected Vector3 desiredGravity;

    protected float gravity => desiredGravity.y < 0 ? inputGravity * 3f : inputGravity;
    protected float runAcceleration = 2f;
    public float actualSpeed = 0f;

    public void Awake()
    {
        playerJP = GetComponent<CharacterController>();
        referenceJP = new PlayerInput();

        vState = VelocityStates.State.Base;
        referenceJP.Keyboard.Jump.started += Jump;
    }

    protected void FixedUpdate()
    {
        //move forward
        if (isGrounded)
        {
            runAcceleration = Mathf.Sqrt(accelerationRate * Time.fixedDeltaTime);
            actualSpeed += runAcceleration;

            if (actualSpeed > maxRunSpeed) actualSpeed = maxRunSpeed;
        }

        if(isGrounded && desiredGravity.y < 0f) desiredGravity.y = 0f;
        else desiredGravity.y += gravity * Time.fixedDeltaTime;

        playerJP.Move(desiredGravity * Time.fixedDeltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            desiredGravity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
