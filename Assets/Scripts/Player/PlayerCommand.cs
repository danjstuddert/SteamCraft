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
	//controlledBots is the list of currently controlled bots
	private List<Bot> controlledBots;

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

		if (controlledBots == null) {
			controlledBots = new List<Bot>();
		}
	}

	void Update(){
		CheckTargetActive ();
		MoveTarget ();
	}

	//----------------------------------------------------------
	//GiveBot()
	//Gives the player a bot to command
	//Params:
	//		Bot bot - The bot to store
	//Return:
	//		Void
	//----------------------------------------------------------
	public void GiveBot(Bot bot) {
		if(controlledBots.Exists(b => b == bot) == false) {
			controlledBots.Add(bot);
		}
	}

	//----------------------------------------------------------
	//GiveUpgradeStation()
	//Gives the player a station to store for use on trigger release
	//Params:
	//		UpgradeStation station - The station to store
	//Return:
	//		Void
	//----------------------------------------------------------
	public void GiveUpgradeStation(UpgradeStation station) {
		selectedStation = station;
	}

	//----------------------------------------------------------
	//RemoveUpgradeStation()
	//Removes the selected upgrade station if it matches the given station
	//Params:
	//		UpgradeStation station - The station to check
	//Return:
	//		Void
	//----------------------------------------------------------
	public void RemoveUpgradeStation(UpgradeStation station) {
		if(station == selectedStation) {
			selectedStation = null;
		}
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

			ProcessCommand ();
		}
	}

	//----------------------------------------------------------
	//MoveTarget()
	//Moves the target gameobject restricting the distance from the player
	//it can go
	//Return:
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

	private void ProcessCommand(){
		if(selectedStation != null){
			//Get the first drone we can and send it to the station
			for (int i = 0; i < controlledBots.Count; i++) {
				if(controlledBots[i].tag == "Drone") {
					controlledBots[i].GiveTarget(selectedStation.transform);
					controlledBots.Remove(controlledBots[i]);
					break;
				}
			}

			//make sure we reset the selected station to null to prevent Drones moving
			//to the same station the next time a command is issued to any point on the map
			selectedStation = null;
		}

		else {

		}
	}
}
