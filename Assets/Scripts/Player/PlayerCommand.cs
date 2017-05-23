using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerCommand : MonoBehaviour {
	public GameObject commandTarget;
	//The speed the command target moves at
	public float commandSpeed;
	//The maximum distance that the command target can be from the player
	public float commandMaxDistance;

	//The controller that controls the object
	private XboxController controller;

	//The last station touched by the command target
	private UpgradeStation selectedStation;

	//----------------------------------------------------------
	//Init()
	//Ensures correct setup of the PlayerCall component
	//Param:
	//		XboxController controller - the controller that controls this object
	//Return:
	//		Void
	//----------------------------------------------------------
	public void Init(XboxController controller){
		this.controller = controller;
	}

	void Update(){
		CheckTargetActive ();
		MoveTarget ();
	}

	//----------------------------------------------------------
	//CheckCircleActive()
	//Turns on and off the command target depending on player input
	//on deactivate calls CheckCommandObject()
	//Return:
	//		Void
	//----------------------------------------------------------
	private void CheckTargetActive() {
		if (XCI.GetAxis(XboxAxis.LeftTrigger, controller) != 0 && commandTarget.activeInHierarchy == false && XCI.GetAxis(XboxAxis.RightTrigger, controller) == 0) {
			commandTarget.transform.position = transform.position;
			commandTarget.SetActive(true);
		}

		else if (XCI.GetAxis(XboxAxis.LeftTrigger, controller) == 0 && commandTarget.activeInHierarchy) {
			commandTarget.SetActive(false);

			CheckCommandObject ();
		}
	}

	private void MoveTarget(){
		Vector3 movement = new Vector3(XCI.GetAxis(XboxAxis.RightStickX, controller), 0f, XCI.GetAxis(XboxAxis.RightStickY, controller)) * commandSpeed * Time.deltaTime;

		commandTarget.transform.position += movement;

		float distance = Vector3.Distance(commandTarget.transform.position, transform.position);
		if (distance > commandMaxDistance) {
			Vector3 heading = (commandTarget.transform.position - transform.position);
			Vector3 direction = heading / distance;
			commandTarget.transform.localPosition = direction * commandMaxDistance;
		}
	}

	private void CheckCommandObject(){
		if(selectedStation != null){
			Debug.Log ("Working");
		}
	}
}
