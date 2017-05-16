using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the starting and running of the game, 
//including despawning of killed units and win conditions
public class GameController : Singleton<GameController> {
	void Start () {
		//Make sure we initalise our factories correctly
		foreach(Factory f in FindObjectsOfType(typeof(Factory))){
			f.Init ();
		}
	}

	//----------------------------------------------------------
	//Correctly handles the despawning of each object type
	//Param:
	//		GameObject obj - the object to despawn
	//Return:
	//		Void
	//----------------------------------------------------------
	public void HandleDespawn(GameObject obj) {
		switch (obj.tag) {
		default:
			Debug.LogError (string.Format("{0} does not contain a health script but despawn was requested", obj.name));
			break;
		}

		//Despawn the object using the object pooling script
		SimplePool.Despawn (obj);	
	}
}
