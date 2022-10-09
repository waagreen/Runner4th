using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    public bool IsPlaying => particles.isPlaying;
    public bool IsStopped => particles.isStopped;

    private ParticleSystem particles;
    private ParticleSystem.ColorOverLifetimeModule colorOverLifeTime;
    private ParticleSystem.EmissionModule emissionModule;

    private VelocityState currentState => DataManager.GlobalMovement.CurrentState; 
    private Gradient[] grads => DataManager.GlobalMovement.velocityGradients;
    private int currentStateIndex;
   
    private void Start()
    {
        particles = GetComponent<ParticleSystem>();

        colorOverLifeTime = particles.colorOverLifetime;
        emissionModule = particles.emission;    
    }
    
    void FixedUpdate() => SetStateGradient();

    private void SetStateGradient()
    {
        int stateIndex = (int)currentState;
        if(stateIndex == currentStateIndex) return;

        switch(stateIndex)
        {
            case 0:
                colorOverLifeTime.color = grads[0];
                emissionModule.rateOverTime = 50f;
                break;
            case 1:
                colorOverLifeTime.color = grads[1];
                emissionModule.rateOverTime = 150f;
                CameraManager.SetFov(45);
                break;
            case 2:
                colorOverLifeTime.color = grads[2];
                emissionModule.rateOverTime = 450f;
                CameraManager.SetFov(50);
                break;
        }

        currentStateIndex = stateIndex;
    }

    public void StartEmission() => particles.Play();
    public void StopEmission() => particles.Stop();
}
