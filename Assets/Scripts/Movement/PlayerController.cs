using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    #region Jump Variables

    [Header("Jump parameters")]
    [SerializeField] protected LayerMask groundLayers;
    [SerializeField] protected float jumpHeight = 2f, inputGravity = -30f;
    [HideInInspector] public bool isGrounded => Physics.CheckSphere(transform.position, .25f, groundLayers, QueryTriggerInteraction.Ignore);
    [SerializeField] float jumpButtonGracePeriod;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    #endregion

    #region Slide Variables

    [Header("Slide parameters")]
    [SerializeField] private float reducedHeight, inputHoldTime = 2f;
    private Vector3 originalHeight;
    private float slideInputStartTime;
    private bool doingSlide = false;

    [Header("Particles")]
    [SerializeField] private ParticleSystem particles;
    private ParticleSystem.ColorOverLifetimeModule colorOverLifeTime;
    private ParticleSystem.EmissionModule emissionModule;
    private int curretStateIndex;

    #endregion

    #region Input Variables

    private GlobalMovement globalMove => DataManager.GlobalMovement;
    protected Rigidbody rb;
    protected PlayerInput inputMap;
    protected Vector3 desiredGravity;
    protected float gravity => desiredGravity.y > 0f ? inputGravity : inputGravity * 3f;

    #endregion
    
    #region Raycast Variables

    private RaycastHit hit;
    private Ray ray => new Ray(transform.position, Vector3.down);
    private const int kMaxLayers = 31;

    #endregion

    #region Basic Unity Methods

    void Awake()
    {
        // playerJP = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        
        inputMap = new PlayerInput();

        originalHeight = transform.localScale;
        colorOverLifeTime = particles.colorOverLifetime;
        emissionModule = particles.emission;
    }


    private void FixedUpdate()
    {
        if(globalMove.CurrentState == VelocityState.Idle) DisableAndShowRestartScreen(); // TODO: implementar ciclo de morte e evento para notificar UI
        
        if(!isGrounded && particles.isPlaying) particles.Stop();
        else if (isGrounded && particles.isStopped) particles.Play();
        SetStateGradient();

        // handle slide cooldown
        slideInputStartTime += Time.deltaTime;

        if (doingSlide && CheckSlideTime())
        {
            transform.localScale = originalHeight;
            slideInputStartTime = 0;
            doingSlide = false;
        }

        //raycast for collision
        if (Physics.Raycast(ray, out hit, 10f))
        {
            Debug.DrawRay(new Vector3(transform.position.x + 0.8f, transform.position.y + 1f, transform.position.z), Vector3.down, Color.yellow); // just to see the ray

            //check what layer value is hitting the player
            LayerMask layerHit = hit.transform.gameObject.layer;
            CheckFeedback(layerHit.value);
        }

        //coyote time with jump buffer
        if(isGrounded) lastGroundedTime = Time.time;
 
        // handle gravity
        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod && desiredGravity.y < 0f) desiredGravity.y = 0f;
        desiredGravity.y += gravity * Time.fixedDeltaTime;
        rb.velocity = desiredGravity;
    }

    #endregion

    #region Input Events

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed) jumpButtonPressedTime = Time.time;
        if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod && Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        { 
            float lerp = Mathf.Lerp(inputGravity / 5f, inputGravity, 0.15f);
            desiredGravity.y += Mathf.Sqrt(jumpHeight * -3.0f * lerp);

            jumpButtonPressedTime = null;
            lastGroundedTime = null;
        }
    }

    public void Sliding(InputAction.CallbackContext context)
    {
        doingSlide = true;
        slideInputStartTime = 0;
        transform.localScale = new Vector3(1, reducedHeight, 1);
        CameraManager.SetNoise(ShakeMode.weak);
    }

    public void Quit(InputAction.CallbackContext context)
    {
        Application.Quit();
        Debug.Log("QUIT THE GAME");
    }
    
    private void OnEnable()
    {
        inputMap.Enable();

        inputMap.Keyboard.Jump.performed += Jump;
        inputMap.Keyboard.Slide.started += Sliding;
        inputMap.Keyboard.Quit.performed += Quit;
    }
    private void OnDisable()
    {
        inputMap.Disable();

        inputMap.Keyboard.Jump.performed -= Jump;
        inputMap.Keyboard.Slide.started -= Sliding;
        inputMap.Keyboard.Quit.performed -= Quit;
    }

    #endregion

    #region Check Methods

    private bool CheckSlideTime()
    {
        return slideInputStartTime >= inputHoldTime;
    }

    private void CheckFeedback(int layerHit){
        switch(layerHit){
            case 3:
                //Debug.Log("TA OLHANDO PARA O CHÃO");
                break;
            case 6:
                //Debug.Log("VOCÊ CONSEGUIU PULAR!!");
                return;
            default: 
                break;
        }
    }
    
    private void SetStateGradient()
    {
        int stateIndex = (int) globalMove.CurrentState;
        if(stateIndex == curretStateIndex) return;

        switch(stateIndex)
        {
            case 0:
                colorOverLifeTime.color = globalMove.baseStateGradient;
                emissionModule.rateOverTime = 50f;
                break;
            case 1:
                colorOverLifeTime.color = globalMove.highStateGradient;
                emissionModule.rateOverTime = 150f;
                Camera.main.DOFieldOfView(45f, 2f).SetEase(Ease.OutCubic);
                break;
            case 2:
                colorOverLifeTime.color = globalMove.maxStateGradient;
                emissionModule.rateOverTime = 450f;
                Camera.main.DOFieldOfView(50f, 2f).SetEase(Ease.OutCubic);
                break;
        }

        curretStateIndex = stateIndex;
    }

    private void OnBecameInvisible() => DisableAndShowRestartScreen();

    private void DisableAndShowRestartScreen()
    {
        gameObject.SetActive(false);
        DataManager.GlobalMovement.ShowRestartScreen();
    }
    #endregion
}
