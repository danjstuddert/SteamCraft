using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

//Handles the movement of the player objects
public class PlayerMove : MonoBehaviour {
	//The speed that the player moves at
	public float movementSpeed;
	//The maximum speed that the player can move at
	public float maxMovementSpeed;
	//The speed the player rotates at
	public float rotationSpeed;

	//The controller that controls the object
	private XboxController controller;
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
	public void Init (XboxController controller) {
		rBody = GetComponent<Rigidbody>();
		previousRotationDirection = Vector3.forward;
		this.controller = controller;
	}

	void Update () {
		UpdateMoveVelocity();
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
		if(rBody != null) {
			rBody.AddForce(moveVelocity * movementSpeed);

			if (rBody.velocity.magnitude > maxMovementSpeed) {
				rBody.velocity = rBody.velocity.normalized * maxMovementSpeed;
			}
		}
	}
}
