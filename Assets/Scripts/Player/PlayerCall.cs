using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerCall : MonoBehaviour {
	//The command circle object associated with the player
	public GameObject callCircle;
	//The speed the call circle moves at
	public float callCircleSpeed;
	//The maximum distance that the call circle can be from the player
	public float callCircleMaxDistance;

	//The controller that is assigned to this player
	private XboxController controller;

	public void Init(XboxController controller) {
		this.controller = controller;

		if (callCircle.activeInHierarchy) {
			callCircle.SetActive(false);
		}
	}

	void Update() {
		CheckCircleActive();
		MoveCircle();
	}

	//----------------------------------------------------------
	//CheckCircleActive()
	//Turns on and off the call circle depending on player input
	//Return:
	//		Void
	//----------------------------------------------------------
	private void CheckCircleActive() {
		if (XCI.GetAxis(XboxAxis.RightTrigger, controller) != 0 && callCircle.activeInHierarchy == false && 
			XCI.GetAxis(XboxAxis.LeftTrigger, controller) == 0 ) {
			callCircle.transform.position = transform.position;
			callCircle.SetActive(true);
		}

		else if (XCI.GetAxis(XboxAxis.RightTrigger, controller) == 0 && callCircle.activeInHierarchy) {
			callCircle.SetActive(false);
		}
	}

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
}
