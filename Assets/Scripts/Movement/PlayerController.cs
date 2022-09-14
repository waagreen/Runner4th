using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    #region Jump Variables

    [Header("Jump parameters")]
    [SerializeField] protected LayerMask groundLayers;
    [SerializeField] protected float jumpHeight = 2f, inputGravity = -30f;
    [HideInInspector] public bool isGrounded => Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);

    #endregion

    #region Slide Variables

    [Header("Slide parameters")]
    [SerializeField] private float reducedHeight, inputHoldTime = 2f;
    private Vector3 originalHeight;
    private float slideInputStartTime;
    private bool doingSlide = false;

    #endregion

    #region Input Variables

    protected PlayerInput inputMap;
    protected CharacterController playerJP;
    protected Vector3 desiredGravity;
    //protected float gravity => desiredGravity.y > 0f ? inputGravity : inputGravity * 3f;

    #endregion
    
    #region Raycast Variables

    private RaycastHit hit;
    private Ray ray => new Ray(transform.position, Vector3.down);
    private const int kMaxLayers = 31;

    #endregion

    #region Basic Unity Methods

    void Awake()
    {
        playerJP = GetComponent<CharacterController>();
        inputMap = new PlayerInput();

        originalHeight = playerJP.transform.localScale;
    }


    void FixedUpdate()
    {
        // handle slide cooldown
        slideInputStartTime += Time.deltaTime;

        if (doingSlide && CheckSlideTime())
        {
            transform.localScale = originalHeight;
            slideInputStartTime = 0;
            doingSlide = false;
            CameraManager.SetNoise(ShakeMode.moderate);
        }

        //raycast for collision
        if (Physics.Raycast(ray, out hit, 10f))
        {
            Debug.DrawRay(new Vector3(transform.position.x + 0.8f, transform.position.y + 1f, transform.position.z), Vector3.down, Color.yellow); // just to see the ray

            //check what layer value is hitting the player
            LayerMask layerHit = hit.transform.gameObject.layer;
            CheckFeedback(layerHit.value);
        }
 
        // handle gravity
        if (isGrounded && desiredGravity.y <= 0f) desiredGravity.y = 0f;
        else desiredGravity.y += inputGravity * Time.fixedDeltaTime;

        // player jump
        playerJP.Move(desiredGravity * Time.fixedDeltaTime);
    }

    #endregion

    #region Input Events

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        { 
            float lerp = Mathf.Lerp(inputGravity / 5f, inputGravity, 0.15f);
            desiredGravity.y += Mathf.Sqrt(jumpHeight * -3.0f * lerp);
        }
    }

    public void Sliding(InputAction.CallbackContext context)
    {
        doingSlide = true;
        slideInputStartTime = 0;
        transform.localScale = new Vector3(1, reducedHeight, 1);
    }
    
    private void OnEnable()
    {
        inputMap.Enable();

        inputMap.Keyboard.Jump.performed += Jump;
        inputMap.Keyboard.Slide.started += Sliding;
    }
    private void OnDisable()
    {
        inputMap.Disable();

        inputMap.Keyboard.Jump.performed -= Jump;
        inputMap.Keyboard.Slide.started -= Sliding;
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

    #endregion
}
