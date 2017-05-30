using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

// A script that handles all of the player components
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerCommand))]
[RequireComponent(typeof(PlayerCall))]
public class Player : MonoBehaviour {
// controller is the controller that controls the object
	public XboxController controller;
// playerFaction is the faction this player belongs to
	public Faction playerFaction;

// move is the player's movement script
	private PlayerMove move;
// command is thee player's command script
	private PlayerCommand command;
// call is the player's call script
	private PlayerCall call;


//----------------------------------------------------------
//	Init()
// Ensures correct setup of the Player component
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	public void Init() {
		move = GetComponent<PlayerMove>();
		move.Init(controller);

		command = GetComponent<PlayerCommand> ();
		command.Init (controller);

		call = GetComponent<PlayerCall> ();
		call.Init (controller);

		GetComponent<Health> ().Init ();
	}

//----------------------------------------------------------
//	GiveUpgradeStation()
// Gives the player a station to store for use on trigger release
//
// Param:
//		UpgradeStation station - The station to send
// Return:
//		Void
//----------------------------------------------------------
	public void GiveUpgradeStation(UpgradeStation station) {
		command.GiveUpgradeStation(station);
	}

//----------------------------------------------------------
//	RemoveUpgradeStation()
// Passes the command script of this object to remove from its
// stored station
//
// Param:
//		UpgradeStation station - The station to send
// Return:
//		Void
//----------------------------------------------------------
	public void RemoveUpgradeStation(UpgradeStation station) {
		command.RemoveUpgradeStation(station);
	}

//----------------------------------------------------------
//	GiveBot()
// Gives the bot that just started following the player to the
// command script ascociated with this player
//
// Param:
//		Bot bot - The bot to store
// Return:
//		Void
//----------------------------------------------------------
	public void GiveBot(Bot bot) {
		command.GiveBot(bot);
	}
}
