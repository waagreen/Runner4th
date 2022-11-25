using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class JumpingPlataform : MonoBehaviour
{
    [SerializeField] private float jumpForce;//Force of the jump

	private float jumpDuration = .4f; //Prevents the playerController.isGrounded to stop the jump before it even happens
	private float jumpBeginTime = -Mathf.Infinity; //Time of the beginning of the jump
    private float heightValue = 8f; //A provisory solution for sum of the force of the player jump to the plataform jump force
	private float maxHeight; //The plataform position plus the heightValue
    private float actualHeight; //Distance between the player and the Platform
    private bool isJumping = false; //Is the platform jump ocurring?
    private bool performed = false; //Prevents the collision to be detected two times in a row
    private Rigidbody playerRb;
    private PlayerController playerController;

    private void Awake()
    {
        if (playerRb == null) playerRb = GameObject.Find("Main_Character").GetComponent<Rigidbody>();
        if (playerController == null) playerController = GameObject.Find("Main_Character").GetComponent<PlayerController>();

        //Calculates the maxHeight value
        maxHeight = heightValue;
	}

    private void FixedUpdate()
    {
        if (playerRb == null) return;

        //Executes the jump
		if (isJumping)
		{
            //Distance between the player and the platform
			actualHeight = playerRb.transform.position.y - transform.position.y;

            //Checks if the player is grounded after the jump time
            if (Time.time >= jumpBeginTime + jumpDuration && playerController.isGrounded)
			{
				isJumping = false;
                performed = false;
			}
			else if (actualHeight < maxHeight) //Adds the force of the jump if the player Height is under the maximum height
			{
				playerRb?.AddForce(Vector3.up * jumpForce, ForceMode.Force);
			}
		}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && performed == false) //if player collides, the jump starts
        {
			jumpBeginTime = Time.time;
            isJumping = true;
			performed = true;
        }
	}
}
