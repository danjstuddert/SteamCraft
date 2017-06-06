using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

// Handles the logic of the calling circle
public class PlayerCall : MonoBehaviour {
// callCircle is the call circle object associated with the player
	public GameObject callCircle;
// callCircleSpeed is the speed the call circle moves at
	public float callCircleSpeed;
// callCircleMaxDistance is the maximum distance that the call circle can be from the player
	public float callCircleMaxDistance;
// resetTime is the time it takes until the callCircle resets its position
	public float resetTime;

// controller is the controller that is assigned to this player
	private XboxController controller;
// resetCounter is a counter to compare resetTime against
	private float resetCounter;
// shouldReset is a check to see if the circle should reset on spawn
	private bool shouldReset;

//----------------------------------------------------------
//	Init()
// Turns on and off the call circle depending on player input
//
// Param:
//		XboxController controller - the controller that is assigned to this player
// Return:
//		Void
//----------------------------------------------------------
	public void Init(XboxController controller) {
		this.controller = controller;

		if (callCircle.activeInHierarchy) {
			callCircle.SetActive(false);
		}
	}

//--------------------------------------------------------------------------------------
//	Update()
// Runs every frame
//
// Param:
//		None
// Return:
//		Void
//--------------------------------------------------------------------------------------
	void Update() {
		CheckCircleActive();
		MoveCircle();
		CheckCircleReset ();
	}

//----------------------------------------------------------
//	CheckCircleActive()
// Turns on and off the call circle depending on player input
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	private void CheckCircleActive() {
		if (XCI.GetAxis(XboxAxis.RightTrigger, controller) != 0 && callCircle.activeInHierarchy == false && 
			XCI.GetAxis(XboxAxis.LeftTrigger, controller) == 0 ) {
			if(shouldReset){
				callCircle.transform.position = transform.position;	
			}

			callCircle.SetActive(true);
		}

		else if (XCI.GetAxis(XboxAxis.RightTrigger, controller) == 0 && callCircle.activeInHierarchy) {
			callCircle.SetActive(false);
			shouldReset = false;
			resetCounter = 0f;
		}
	}

//----------------------------------------------------------
//	MoveCircle()
// Moves the call circle depending on player input
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	private void MoveCircle() {
		Vector3 movement = new Vector3(XCI.GetAxis(XboxAxis.RightStickX, controller), 0f, XCI.GetAxis(XboxAxis.RightStickY, controller)) * callCircleSpeed * Time.deltaTime;

		callCircle.transform.position += movement;

		float distance = Vector3.Distance(callCircle.transform.position, transform.position);
		if (distance > callCircleMaxDistance) {
			Vector3 heading = (callCircle.transform.position - transform.position);
			Vector3 direction = heading / distance;
			callCircle.transform.localPosition = direction * callCircleMaxDistance;
		}
	}

//----------------------------------------------------------
//	CheckCircleReset()
// Checks if the call circle should be reset
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	private void CheckCircleReset(){
		if(shouldReset == true){
			return;
		}

		resetCounter += Time.deltaTime;

		if(resetCounter >= resetTime){
			shouldReset = true;
		}
	}
}
