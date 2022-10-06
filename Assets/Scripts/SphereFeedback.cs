using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereFeedback : MonoBehaviour
{
	private GlobalMovement globalData => DataManager.GlobalMovement;

	private Renderer sphere;

	private void Awake()
	{
		sphere = GetComponent<Renderer>();
	}

	private void FixedUpdate()
	{
		if (globalData.CurrentState == VelocityState.Maximun) sphere.material.color = globalData.maxStateGradient.colorKeys[1].color;
		else if (globalData.CurrentState == VelocityState.High) sphere.material.color = globalData.highStateGradient.colorKeys[1].color;
		else if (globalData.CurrentState == VelocityState.Base) sphere.material.color = globalData.baseStateGradient.colorKeys[1].color;
	}
}
