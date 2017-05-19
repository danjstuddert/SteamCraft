using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

//Handles the movement of the player objects
public class PlayerMove : MonoBehaviour {
	//The controller that controls the object
	public XboxController controller;
	//The speed that the player moves at
	public float movementSpeed;
	//The maximum speed that the player can move at
	public float maxMovementSpeed;
	//The speed the player rotates at
	public float rotationSpeed;

	//The player's rigidbody component
	private Rigidbody rBody;
	//The velocity to apply every fixed update
	private Vector3 moveVelocity;
	//The rotation of the player on the previous frame
	private Vector3 previousRotationDirection;

	//----------------------------------------------------------
	//Init()
	//Ensures correct setup of the PlayerMove component
	//Return:
	//		Void
	//----------------------------------------------------------
	public void Init () {
		rBody = GetComponent<Rigidbody>();
		previousRotationDirection = Vector3.forward;
	}

	void Update () {
		UpdateMoveVelocity();
		RotatePlayer();
	}

	void FixedUpdate() {
		MovePlayer();
	}

	//----------------------------------------------------------
	//UpdateMoveVelocity()
	//Assigns moveVelocity based on player input
	//Return:
	//		Void
	//----------------------------------------------------------
	private void UpdateMoveVelocity() {
		moveVelocity = new Vector3(XCI.GetAxis(XboxAxis.LeftStickX, controller), 0f, XCI.GetAxis(XboxAxis.LeftStickY, controller));
	}

	//----------------------------------------------------------
	//MovePlayer()
	//Applies moveVelocity to the player's rigidbody
	//Return:
	//		Void
	//----------------------------------------------------------
	private void MovePlayer() {
		rBody.AddForce(moveVelocity * movementSpeed);

		if(rBody.velocity.magnitude > maxMovementSpeed) {
			rBody.velocity = rBody.velocity.normalized * maxMovementSpeed;
		}
	}

	//----------------------------------------------------------
	//RotatePlayer()
	//Rotates the player using the current input
	//Return:
	//		Void
	//----------------------------------------------------------
	private void RotatePlayer() {
		Vector3 directionVector = new Vector3(XCI.GetAxis(XboxAxis.RightStickX, controller), 0f, XCI.GetAxis(XboxAxis.RightStickY, controller));

		//If the right thumbstick is not being used, make sure we stay facing the previous direction
		if(directionVector.magnitude < 0.1f) {
			directionVector = previousRotationDirection;
		}

		directionVector = directionVector.normalized;
		previousRotationDirection = directionVector;

		Vector3 newDirection = Vector3.RotateTowards(transform.forward, directionVector, rotationSpeed * Time.deltaTime, 0f);

		transform.rotation = Quaternion.LookRotation(newDirection);
	}
}
