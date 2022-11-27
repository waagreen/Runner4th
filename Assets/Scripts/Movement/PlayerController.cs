using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.Events;
using System;
using DG.Tweening;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    [SerializeField] SphereCollider magneticField;

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
    [SerializeField] private CharacterAudio cAudio;
    [SerializeField] private GameObject shield;
    private Vector3 shieldOriginalScale = Vector3.zero;
    private Vector3 ShieldOriginalPosition = Vector3.zero;
    private const float kAnimShildTime = 1.1f;

    private UnityEvent deathEvent;
    private UnityEvent shieldEvent;
    private UnityEvent<int> collectEvent;

    private Rigidbody rb;
    private PlayerInput inputMap;
    private Vector3 desiredGravity;
    private float gravity => desiredGravity.y > 0f ? inputGravity : inputGravity * 3f;
    private bool isDead = false;

    public List<Transform> caughtTransforms = new List<Transform>();
    private CharacterSheet passiveSkills => DataManager.Events.passiveSkills;

    void Start()
    {   
        inputMap = new PlayerInput();
        inputMap.Enable();

        inputMap.Keyboard.Jump.performed += Jump;
        inputMap.Keyboard.Slide.started += Sliding;
        inputMap.Keyboard.Pause.started += Pause;

        deathEvent = DataManager.Events.OnPlayerDeath;
        collectEvent = DataManager.Events.OnCollectCoin;
        shieldEvent = DataManager.Events.OnShieldHit;

        deathEvent.AddListener(Death);
        collectEvent.AddListener(RemoveCaughtTransform);
        shieldEvent.AddListener(UptadeShield);
        deathEvent = DataManager.Events.OnPlayerDeath;
        deathEvent.AddListener(Death);

        rb = GetComponent<Rigidbody>();

        originalColliderCenter = mCollider.center;
        originalColliderHeight = mCollider.height;

        shieldOriginalScale = shield.transform.localScale;
        ShieldOriginalPosition = shield.transform.localPosition;

        if (passiveSkills.magForce < 1f) magneticField.enabled = false;
        else 
        {
            magneticField.enabled = true;
            magneticField.radius += passiveSkills.magForce;
        }

        Debug.Log(passiveSkills.shieldCharges);
    }


    private void FixedUpdate()
    {
        //Detects if the player is grounded
        Vector3 actualRayPosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        Ray playerRay = new Ray(actualRayPosition, Vector3.down);
        if (Physics.Raycast(playerRay, 1.7f, groundLayers.value)) isGrounded = true;
        else isGrounded = false;
        trail.ControlEmission(isGrounded);
        if(isGrounded && !cAudio.isPlaying) cAudio.PlaySound(SoundType.running);

        if (caughtTransforms != null && caughtTransforms.Count > 0)
        {
            foreach (var t in caughtTransforms)
            {
                float coinDist = Vector3.Distance(t.position, transform.position);
                t.position = Vector3.MoveTowards(t.position, transform.position, 15 * Time.deltaTime);
            }
        }

        // handle slide cooldown
        slideInputStartTime += Time.deltaTime;

        if (doingSlide && CheckSlideTime())
        {
            mCollider.center = originalColliderCenter;
            mCollider.height = originalColliderHeight;
            
            shield.transform.DOScale(shieldOriginalScale, kAnimShildTime - 0.5f).SetEase(Ease.OutQuint);
            shield.transform.DOLocalMove(ShieldOriginalPosition, kAnimShildTime).SetEase(Ease.OutQuint);

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
            
            cAudio.PlaySound(SoundType.jump);
        }
    }

    public void Sliding(InputAction.CallbackContext context)
    {
        mCollider.center = new Vector3(0f, 0.3f, 0f); 
        mCollider.height = 0.5f;

        Vector3 newShieldScale = new Vector3(150f, 105f, 150f);
        Vector3 newShieldPosition = new Vector3(0.2f, 0.3f, 0f);

        shield.transform.DOScale(newShieldScale, kAnimShildTime).SetEase(Ease.OutQuint);
        shield.transform.DOLocalMove(newShieldPosition, kAnimShildTime).SetEase(Ease.OutQuint);

        doingSlide = true;
        playerAnim.Play("Slide_Dalla");
        slideInputStartTime = 0;
        CameraManager.SetNoise(ShakeMode.weak);
        cAudio.PlaySound(SoundType.slide);
    }

    private bool CheckSlideTime()
    {
        return slideInputStartTime >= inputHoldTime;
    }

    private bool CheckJumpTime(){
        return jumpInputStartTime >= inputJumpTime;
    }
    
    private void Death()
    {
        if(passiveSkills.reviveCharges < 1)
        {
            isDead = true;
            gameObject.SetActive(false);
        }

    }
    
    private void UptadeShield() => shield.SetActive(DataManager.GlobalMovement.CurrentShieldCharges > 0);

    private void Pause(InputAction.CallbackContext context)
    {
        if(isDead) return;
        DataManager.Events.HandlePause();
    }

    private void OnBecameInvisible() => deathEvent.Invoke();
    
    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Obstacle") cAudio.PlaySound(SoundType.collision);   
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(!caughtTransforms.Contains(other.transform) && other.tag == "Collectable")
        {
            //Add Transform
            caughtTransforms.Add(other.transform);
        }
    }
    private void RemoveCaughtTransform(int dontUse)
    {
        Transform lastT = caughtTransforms.LastOrDefault();
        caughtTransforms.Remove(lastT);
    }
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
