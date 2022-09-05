using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpInput : MonoBehaviour
{
    [Header("Jump parameters")]
    [SerializeField] protected LayerMask groundLayers;
    [SerializeField] protected float jumpHeight = 2f, inputGravity = -30f;
    [HideInInspector] public bool isGrounded => Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);

    protected CharacterController playerJP;
    protected Vector3 currentVelocity;

    protected float gravity => currentVelocity.y < 0 ? inputGravity * 3f : inputGravity;
    protected float runAcceleration = 2f;
    public float actualSpeed = 0f;
    void Awake()
    {
        playerJP = GetComponent<CharacterController>();

    }

    void FixedUpdate()
    {
        if (isGrounded && currentVelocity.y < 0) currentVelocity.y =0;
        

        playerJP.Move(currentVelocity * Time.fixedDeltaTime);
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            currentVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log(currentVelocity.y);
        }
    }
}
