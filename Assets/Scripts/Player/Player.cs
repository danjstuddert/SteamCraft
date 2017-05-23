using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

//A script that handles all of the player components
[RequireComponent(typeof(PlayerMove))]
//[RequireComponent(typeof(PlayerCommand))]
[RequireComponent(typeof(PlayerCall))]
public class Player : MonoBehaviour {
	//The controller that controls the object
	public XboxController controller;

	//The player's movement script
	private PlayerMove move;
	//The player's command script
	private PlayerCommand command;
	//The player's call script
	private PlayerCall call;
	//controlledBots is the list of currently controlled bots
	private List<Bot> controlledBots;

	//----------------------------------------------------------
	//Init()
	//Ensures correct setup of the Player component
	//Return:
	//		Void
	//----------------------------------------------------------
	public void Init() {
		move = GetComponent<PlayerMove>();
		move.Init(controller);

		command = GetComponent<PlayerCommand> ();
		command.Init (controller);

		call = GetComponent<PlayerCall> ();
		call.Init (controller);

		if(controlledBots == null){
			controlledBots = new List<Bot> ();
		}
	}
}
