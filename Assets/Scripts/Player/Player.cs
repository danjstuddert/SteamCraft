using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A script that handles all of the player components
[RequireComponent(typeof(PlayerMove))]
public class Player : MonoBehaviour {
	//The player's movement script
	private PlayerMove move;

	//----------------------------------------------------------
	//Init()
	//Ensures correct setup of the Player component
	//Return:
	//		Void
	//----------------------------------------------------------
	public void Init() {
		move = GetComponent<PlayerMove>();
		move.Init();
	}
}
