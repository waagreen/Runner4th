using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereFeedback : MonoBehaviour
{
	private Gradient[] grads => DataManager.GlobalMovement.velocityGradients;
	private VelocityState currentState => DataManager.GlobalMovement.CurrentState;

	private Renderer sphere;

	private void Awake()
	{
		sphere = GetComponent<Renderer>();
	}

	private void FixedUpdate()
	{
		if (currentState == VelocityState.Maximun) sphere.material.color = grads[2].colorKeys[1].color;
		else if (currentState == VelocityState.High) sphere.material.color = grads[1].colorKeys[1].color;
		else if (currentState == VelocityState.Base) sphere.material.color = grads[0].colorKeys[1].color;
	}
}
