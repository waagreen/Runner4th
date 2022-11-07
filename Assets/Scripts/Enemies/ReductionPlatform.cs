using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.Events;

public class ReductionPlataform : MonoBehaviour
{
    [SerializeField] private int timeReduceCooldown;
    [ConditionalField(nameof(needToReduce))] public VelocityState desiredVelocity;
    private GlobalMovement globalMove => DataManager.GlobalMovement;
    private UnityEvent deathEvent;

    private float timeForReduce;
    private bool isReducing = false;
    public bool needToReduce = false;
    private void Start() {
        deathEvent = DataManager.Events.OnPlayerDeath;
        deathEvent.AddListener(globalMove.SetSpeedToZero);
    }

    private void Update() {
        timeForReduce = Time.time;
        VelocityState theCurrentState;

        if(isReducing && CheckReduceTime()){
            timeForReduce = 0;
            isReducing = false;
            if(globalMove.CurrentState == VelocityState.High) theCurrentState = VelocityState.Maximun;
            else if(globalMove.CurrentState == VelocityState.Base) theCurrentState = VelocityState.High;
            else if(globalMove.CurrentState == VelocityState.Idle) deathEvent.Invoke();
        }
    }
    private void OnCollisionEnter(Collision other) {
        if(needToReduce){
            if(other.gameObject.tag == "Player" && desiredVelocity >= globalMove.CurrentState){
                isReducing = true;
                globalMove.ReduceSpeed();
            }
        }
    }

    private bool CheckReduceTime(){
        return timeForReduce >= timeReduceCooldown;
    }
}
