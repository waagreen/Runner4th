using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RepeatEnvironment : MonoBehaviour
{
	[SerializeField] private float xSpeed;

	private float xLimit;
	private Vector3 initialPos;
	private new Collider collider;

	private void Start()
	{
		collider = GetComponent<Collider>();
		xLimit = collider.bounds.size.x / 2;
		initialPos = transform.localPosition;
	}

	private void Update()
	{
		//move the background and repeats it
		transform.localPosition += Vector3.left * xSpeed * Time.deltaTime;
		
		if (transform.localPosition.x < initialPos.x - xLimit)
		{
			transform.position = initialPos;
		}
	}
}

