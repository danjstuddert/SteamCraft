using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

//Handles the movement of the player objects
public class PlayerMove : MonoBehaviour {
	//movementSpeed is the speed that the player moves at
	public float movementSpeed;
	//maxMovementSpeed is the maximum speed that the player can move at
	public float maxMovementSpeed;
	//rotationSpeed is the speed the player rotates at
	public float rotationSpeed;

	//controller is the controller that controls the object
	private XboxController controller;
	//rBody is the player's rigidbody component
	private Rigidbody rBody;
	//moveVelocity is the velocity to apply every fixed update
	private Vector3 moveVelocity;

	//----------------------------------------------------------
	//Init()
	//Ensures correct setup of the PlayerMove component
	//Return:
	//		Void
	//----------------------------------------------------------
	public void Init (XboxController controller) {
		rBody = GetComponent<Rigidbody>();
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
