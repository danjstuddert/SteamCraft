using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerCommand : MonoBehaviour {
// commandTarget is the command target object that is owned by this player
	public GameObject commandTarget;
// commandSpeed is the speed the command target moves at
	public float commandSpeed;
// commandMaxDistance is the maximum distance that the command target can be from the player
	public float commandMaxDistance;

// controller is the controller that controls the object
	private XboxController controller;

// selectedStation is the last station touched by the command target
	private UpgradeStation selectedStation;
// controlledBots is the list of currently controlled bots
	private List<Bot> controlledBots;

//----------------------------------------------------------
//	Init()
// Ensures correct setup of the PlayerCall component
//
// Param:
//		XboxController controller - the controller that controls this object
// Return:
//		Void
//----------------------------------------------------------
	public void Init(XboxController controller){
		this.controller = controller;

		if (controlledBots == null) {
			controlledBots = new List<Bot>();
		}
	}

//------------------------------------------------------------
//	Update()
// Runs every frame
//
// Param:
//		None
// Return:
//		Void
//-----------------------------------------------------------
	void Update(){
		CheckTargetActive ();
		MoveTarget ();
	}

//----------------------------------------------------------
//	GiveBot()
// Gives the player a bot to command
//
// Param:
//		Bot bot - The bot to store
// Return:
//		Void
//----------------------------------------------------------
	public void GiveBot(Bot bot) {
		if(controlledBots.Exists(b => b == bot) == false) {
			controlledBots.Add(bot);
		}
	}

//----------------------------------------------------------
//	GiveUpgradeStation()
// Gives the player a station to store for use on trigger release
//
// Param:
//		UpgradeStation station - The station to store
// Return:
//		Void
//----------------------------------------------------------
	public void GiveUpgradeStation(UpgradeStation station) {
		selectedStation = station;
	}

//----------------------------------------------------------
//	RemoveUpgradeStation()
// Removes the selected upgrade station if it matches the given station
//
// Param:
//		UpgradeStation station - The station to check
// Return:
//		Void
//----------------------------------------------------------
	public void RemoveUpgradeStation(UpgradeStation station) {
		if(station == selectedStation) {
			selectedStation = null;
		}
	}

//----------------------------------------------------------
//	CheckCircleActive()
// Turns on and off the command target depending on player input
// on deactivate calls CheckCommandObject()
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	private void CheckTargetActive() {
		if (XCI.GetAxis(XboxAxis.LeftTrigger, controller) != 0 && commandTarget.activeInHierarchy == false && XCI.GetAxis(XboxAxis.RightTrigger, controller) == 0) {
			commandTarget.transform.position = transform.position;
			commandTarget.SetActive(true);
		}

		else if (XCI.GetAxis(XboxAxis.LeftTrigger, controller) == 0 && commandTarget.activeInHierarchy) {
			commandTarget.SetActive(false);

			ProcessCommand ();
		}
	}

//----------------------------------------------------------
//	MoveTarget()
// Moves the target gameobject restricting the distance from the player
// it can go
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
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

//----------------------------------------------------------
//	ProcessCommand()
// Work out what command should be issued to following bots depending
// on the position of the command target
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	private void ProcessCommand(){
		if (selectedStation != null){
			for (int i = 0; i < controlledBots.Count; i++) {
				if(controlledBots[i].tag == "Drone") {
					controlledBots[i].GiveTarget(selectedStation.transform);
					controlledBots.Remove(controlledBots[i]);
					break;
				}
			}
			selectedStation = null;
		} else {
			bool foundUpgraded = false;
			for (int i = 0; i < controlledBots.Count; i++) {
				if(controlledBots[i].tag == "Bot") {
					controlledBots[i].GivePoint(commandTarget.transform.position);
					controlledBots.Remove(controlledBots[i]);
					foundUpgraded = true;
					break;
				}
			}

			if(foundUpgraded == false) {
				for (int i = 0; i < controlledBots.Count; i++) {
					if (controlledBots[i].tag == "Drone") {
						controlledBots[i].GivePoint(commandTarget.transform.position);
						controlledBots.Remove(controlledBots[i]);
						break;
					}
				}
			}
		}
	}
}
