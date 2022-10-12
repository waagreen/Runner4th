using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.Events;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Jump parameters")]
    [SerializeField] protected LayerMask groundLayers;
    [SerializeField] protected float jumpHeight = 2f, inputGravity = -30f;
    [SerializeField] private float inputJumpTime = 2f;
    [SerializeField] private float jumpButtonGracePeriod;
    
    private float jumpInputStartTime;
    private bool isJumping = false;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    
    public bool isGrounded => Physics.CheckSphere(transform.position, .2f, groundLayers);

    [Header("Slide parameters")]
    [SerializeField] private float reducedHeight, inputHoldTime = 2f;
    private float slideInputStartTime;
    private bool doingSlide = false;
    private float originalColliderHeight;
    private Vector3 originalColliderCenter;
    [SerializeField] private CapsuleCollider mCollider;
    
    [Header("Animation & Particles")]
    [SerializeField] private Animator playerAnim;
    [SerializeField] private TrailController trail;

    private UnityEvent deathEvent;
    private Rigidbody rb;
    private PlayerInput inputMap;
    private Vector3 desiredGravity;
    private float gravity => desiredGravity.y > 0f ? inputGravity : inputGravity * 3f;

    void Start()
    {   
        inputMap = new PlayerInput();
        inputMap.Enable();

        inputMap.Keyboard.Jump.performed += Jump;
        inputMap.Keyboard.Slide.started += Sliding;
        inputMap.Keyboard.Pause.started += Pause;
        
        deathEvent = DataManager.Events.OnPlayerDeath;
        deathEvent.AddListener(Death);
        
        rb = GetComponent<Rigidbody>();

        originalColliderCenter = mCollider.center;
        originalColliderHeight = mCollider.height;
    }


    private void FixedUpdate()
    {
        if(!isGrounded && trail.IsPlaying) trail.StopEmission();
        else if (isGrounded && trail.IsStopped) trail.StartEmission();

        // handle slide cooldown
        slideInputStartTime += Time.deltaTime;

        if (doingSlide && CheckSlideTime())
        {
            mCollider.center = originalColliderCenter;
            mCollider.height = originalColliderHeight;

            playerAnim.Play("Running");
            slideInputStartTime = 0;
            doingSlide = false;
        }

        //handle jump bool
        jumpInputStartTime += Time.deltaTime;

        if(isJumping && CheckJumpTime()){
            playerAnim.Play("Running");
            jumpInputStartTime = 0;
            isJumping = false;
        }

        //coyote time with jump buffer
        if(isGrounded) lastGroundedTime = Time.time;
 
        // handle gravity
        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod && desiredGravity.y < 0f) desiredGravity.y = 0f;
        desiredGravity.y += gravity * Time.fixedDeltaTime;
        rb.velocity = desiredGravity;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed) jumpButtonPressedTime = Time.time;
        if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod && Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        { 
            float lerp = Mathf.Lerp(inputGravity / 5f, inputGravity, 0.15f);
            desiredGravity.y += Mathf.Sqrt(jumpHeight * -3.0f * lerp);

            playerAnim.Play("Jump");
            isJumping = true;
            jumpInputStartTime = 0;

            jumpButtonPressedTime = null;
            lastGroundedTime = null;
        }
    }

    public void Sliding(InputAction.CallbackContext context)
    {
        mCollider.center = new Vector3(0f, 0.3f, 0f); 
        mCollider.height = 0.5f;

        doingSlide = true;
        playerAnim.Play("Female Action Pose");
        slideInputStartTime = 0;
        CameraManager.SetNoise(ShakeMode.weak);
    }

    private bool CheckSlideTime()
    {
        return slideInputStartTime >= inputHoldTime;
    }

    private bool CheckJumpTime(){
        return jumpInputStartTime >= inputJumpTime;
    }
    
    private void Death() => gameObject.SetActive(false);
    
    private void Pause(InputAction.CallbackContext context) => DataManager.Events.HandlePause();

    private void OnBecameInvisible() => deathEvent.Invoke();
    
    private void OnDestroy()
    {
        if(inputMap != null)
        {
            inputMap.Disable();

            inputMap.Keyboard.Jump.performed -= Jump;
            inputMap.Keyboard.Slide.started -= Sliding;
            inputMap.Keyboard.Pause.started -= Pause;
        }
    }
}
