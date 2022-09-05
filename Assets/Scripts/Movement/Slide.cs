using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slide : MonoBehaviour
{
    [SerializeField] private float reducedHeight, inputHoldTime = 2f;
    private CharacterController playerSD;
    private PlayerInput referenceSD;
    private Vector3 originalHeight;
    private float slideInputStartTime;
    private bool doingSlide = false;
    private void Awake(){
        playerSD = GetComponent<CharacterController>();
        referenceSD = new PlayerInput();

        referenceSD.Keyboard.Slide.started += Sliding;
        originalHeight = playerSD.transform.localScale;
    }

    private void Update(){
        slideInputStartTime += Time.deltaTime;

        if(doingSlide && CheckSlideTime()){
            playerSD.transform.localScale = originalHeight;
            slideInputStartTime = 0;
            doingSlide = false;
        }
    }
    public void Sliding(InputAction.CallbackContext context){
        doingSlide = true;
        slideInputStartTime = 0;
        playerSD.transform.localScale = new Vector3(1,reducedHeight,1);
    }

    private bool CheckSlideTime(){
        return slideInputStartTime > inputHoldTime;
    }
}
