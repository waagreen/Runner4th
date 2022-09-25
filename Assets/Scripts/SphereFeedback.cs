using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereFeedback : MonoBehaviour
{
	[SerializeField] private GlobalDataSO globalData;

	private Renderer sphere;

	private void Awake()
	{
		sphere = GetComponent<Renderer>();
	}

	private void FixedUpdate()
	{
		if (globalData.CurrentState == VelocityState.Maximun) sphere.material.color = globalData.MaxStateGradient.colorKeys[1].color;
		else if (globalData.CurrentState == VelocityState.High) sphere.material.color = globalData.HighStateGradient.colorKeys[1].color;
		else if (globalData.CurrentState == VelocityState.Base) sphere.material.color = globalData.BaseStateGradient.colorKeys[1].color;
	}
}
