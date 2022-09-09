using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [Header("Jump parameters")]
    [SerializeField] protected LayerMask groundLayers;
    [SerializeField] protected float jumpHeight = 2f, inputGravity = -30f;
    [HideInInspector] public bool isGrounded => Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);

    [Header("Slide parameters")]
    [SerializeField] private float reducedHeight, inputHoldTime = 2f;
    private Vector3 originalHeight;
    private float slideInputStartTime;
    private bool doingSlide = false;

    protected PlayerInput inputMap;
    protected CharacterController playerJP;
    protected Vector3 desiredGravity;
    protected float gravity => desiredGravity.y < 0 ? inputGravity * 3f : inputGravity;
    
    private RaycastHit hit;
    private Ray ray => new Ray(transform.position, Vector3.down);
    private const int kMaxLayers = 31;


    void Awake()
    {
        playerJP = GetComponent<CharacterController>();
        inputMap = new PlayerInput();
        
        originalHeight = playerJP.transform.localScale; 
    }

    private void OnEnable()
    {
        inputMap.Enable();
        
        inputMap.Keyboard.Jump.started += Jump; 
        inputMap.Keyboard.Slide.started += Sliding;  
    }
    
    void FixedUpdate()
    {   
        // handle slide cooldown
        slideInputStartTime += Time.deltaTime;

        if(doingSlide && CheckSlideTime()){
            transform.localScale = originalHeight;
            slideInputStartTime = 0;
            doingSlide = false;
            CameraManager.SetNoise(ShakeMode.moderate);
        }

        // handle gravity
        if (isGrounded && desiredGravity.y < 0f) desiredGravity.y = 0f;
        else desiredGravity.y += gravity * Time.fixedDeltaTime;

        //raycast for collision
        if(Physics.Raycast(ray, out hit, 25f, groundLayers)) {
            Debug.DrawRay(new Vector3(transform.position.x + 0.8f, transform.position.y + 1f, transform.position.z), Vector3.forward, Color.yellow); // just to see the ray
            for(int i=0; i <= kMaxLayers; i++){
                var layerN = LayerMask.LayerToName(i); //name of the layer 
            }
        } 

        // player jump
        playerJP.Move(desiredGravity * Time.fixedDeltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded) desiredGravity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void Sliding(InputAction.CallbackContext context)
    {
        doingSlide = true;
        slideInputStartTime = 0;
        transform.localScale = new Vector3(1,reducedHeight,1);
    }

    private bool CheckSlideTime(){
        return slideInputStartTime >= inputHoldTime;
    }

    private void OnDisable() 
    {
        inputMap.Disable();

        inputMap.Keyboard.Jump.started -= Jump;  
        inputMap.Keyboard.Slide.started -= Sliding;    
    }
}
