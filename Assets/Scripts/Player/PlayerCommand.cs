using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

// Handles the command target object
public class PlayerCommand : MonoBehaviour {
// commandTarget is the command target object that is owned by this player
	public GameObject commandTarget;
// commandSpeed is the speed the command target moves at
	public float commandSpeed;
// commandMaxDistance is the maximum distance that the command target can be from the player
	public float commandMaxDistance;
// resetTime is the time it takes until the callCircle resets its position
	public float resetTime;

// controller is the controller that controls the object
	private XboxController controller;

// selectedStation is the last station touched by the command target
	private UpgradeStation selectedStation;
// controlledBots is the list of currently controlled bots
	private List<Bot> controlledBots;
// resetCounter is a counter to compare resetTime against
	private float resetCounter;
// shouldReset is a check to see if the circle should reset on spawn
	private bool shouldReset;

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
		CheckTargetReset ();
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
			if(shouldReset){
				commandTarget.transform.position = transform.position;	
			}

			commandTarget.SetActive(true);
		}

		else if (XCI.GetAxis(XboxAxis.LeftTrigger, controller) == 0 && commandTarget.activeInHierarchy) {
			commandTarget.SetActive(false);
			ProcessCommand ();

			shouldReset = false;
			resetCounter = 0f;
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
//	CheckCircleReset()
// Checks if the call circle should be reset
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	private void CheckTargetReset(){
		if(shouldReset == true){
			return;
		}

		resetCounter += Time.deltaTime;

		if(resetCounter >= resetTime){
			shouldReset = true;
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
		if (selectedStation != null) {
			float distance = Mathf.Infinity;
			Bot selectedBot = null;

			for (int i = controlledBots.Count - 1; i >= 0 ; i--) {
				if(controlledBots[i].tag == "Drone"){
					float botDistance = Vector3.Distance (selectedStation.transform.position, controlledBots [i].transform.position);

					if(botDistance < distance) {
						selectedBot = controlledBots [i];
						distance = botDistance;
					}
				}
			}

			if(selectedBot != null){
				selectedBot.GiveTarget(selectedStation.transform);
				controlledBots.Remove(selectedBot);
			}

			selectedStation = null;
		} else {
			bool foundUpgraded = false;
			float distance = Mathf.Infinity;
			Bot selectedBot = null;

			for (int i = controlledBots.Count - 1; i >= 0 ; i--) {
				if(controlledBots[i].tag == "Bot"){
					float botDistance = Vector3.Distance (commandTarget.transform.position, controlledBots [i].transform.position);

					if(botDistance < distance) {
						selectedBot = controlledBots [i];
						distance = botDistance;
					}
				}
			}

			if(selectedBot != null){
				selectedBot.GivePoint(commandTarget.transform.position);
				controlledBots.Remove(selectedBot);
				foundUpgraded = true;
			}


			if(foundUpgraded == false) {
				distance = Mathf.Infinity;
				selectedBot = null;

				for (int i = controlledBots.Count - 1; i >= 0 ; i--) {
					if(controlledBots[i].tag == "Drone"){
						float botDistance = Vector3.Distance (commandTarget.transform.position, controlledBots [i].transform.position);

						if(botDistance < distance) {
							selectedBot = controlledBots [i];
							distance = botDistance;
						}
					}
				}

				if(selectedBot != null){
					selectedBot.GivePoint(commandTarget.transform.position);
					controlledBots.Remove(selectedBot);
				}

			}
		}
	}
}
