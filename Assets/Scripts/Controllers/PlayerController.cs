using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to declare the faction of an object
public enum Faction { Red, Blue }

// A script that manages the instances of player objects in the game
public class PlayerController : Singleton<PlayerController> {
// playerRespawnTime is the time it takes for players to respawn
	public float playerRespawnTime;
// redSpawnPoint is the point where the red player respawns
	public Transform redSpawnPoint;
// blueSpawnPoint is the point where the blue player respawns
	public Transform blueSpawnPoint;

// players is a list of the current players
	private List<Player> players;

//----------------------------------------------------------
//	Init()
// Ensures correct setup of the PlayerController component and
// starts initialisation of all player objects in the game
//
// Param:
// 		None
// Return:
//		Void
//----------------------------------------------------------
	public void Init() {
		players = new List<Player>();

		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			Player p = player.GetComponent<Player> ();
			p.Init ();
			players.Add (p);
		}
	}

//----------------------------------------------------------
//	PlayerDead()
// Handles player death
//
// Param:
//		None
//	Return:
//		Void
//----------------------------------------------------------
	public void PlayerDead(GameObject player) {
		Player playerScript = player.GetComponent<Player>();

		if(players.Exists(p => p == playerScript)) {
			players.Remove (playerScript);
			playerScript.gameObject.SetActive (false);
			StartCoroutine (RespawnPlayer(playerScript));

		} else {
			Debug.LogError(string.Format("{0} is not a recognised player!", player.name));
		} 
	}

//----------------------------------------------------------
//	RespawnPlayer()
// Respawns a player to their given point
//
// Params:
//		Player player - the player to respawn
// Return:
//		Void
//----------------------------------------------------------
	private IEnumerator RespawnPlayer(Player player){
		yield return new WaitForSeconds (playerRespawnTime);

		player.gameObject.SetActive (true);

		player.Init ();
		players.Add (player);
		player.transform.position = player.name == "PlayerBlue" ? blueSpawnPoint.position : redSpawnPoint.position;
	}
}
