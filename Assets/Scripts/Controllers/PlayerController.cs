using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A script that manages the instances of player objects in the game
public class PlayerController : Singleton<PlayerController> {
	private List<Player> players;

	//----------------------------------------------------------
	//Init()
	//Ensures correct setup of the PlayerController component and
	//starts initialisation of all player objects in the game
	//
	//Return:
	//		Void
	//----------------------------------------------------------
	public void Init() {
		players = new List<Player>();

		foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Player p = player.GetComponent<Player>();
			p.Init();
			players.Add(p);
		}
	}

	//----------------------------------------------------------
	//PlayerDead()
	//Handles player death
	//
	//Return:
	//		Void
	//----------------------------------------------------------
	public void PlayerDead(Player player) {
		if(players.Exists(p => p == player)) {

		} else {
			Debug.LogError(string.Format("{0} is not a recognised player!", player.name));
		} 
	}
}
