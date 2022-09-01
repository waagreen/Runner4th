using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slide : MonoBehaviour
{
    private CharacterController player;
    private PlayerInput reference;

    private Vector3 originalHeight;
    [SerializeField] private float reducedHeight;

    [SerializeField] private float inputHoldTime = 2f;
    private float slideInputStartTime;
    private bool doingSlide = false;

    private void Awake(){
        player = GetComponent<CharacterController>();
        reference = new PlayerInput();

        reference.Keyboard.Slide.started += Sliding;
        originalHeight = player.transform.localScale;
    }

    private void Update(){
        slideInputStartTime += Time.deltaTime;

        if(doingSlide && CheckSlideTime()){
            player.transform.localScale = originalHeight;
            slideInputStartTime = 0;
            doingSlide = false;
        }
    }
    public void Sliding(InputAction.CallbackContext context){
        doingSlide = true;
        slideInputStartTime = 0;
        player.transform.localScale = new Vector3(1,reducedHeight,1);
    }

    private bool CheckSlideTime(){
        return slideInputStartTime > inputHoldTime;
    }
}
