using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Internal logic of the factory object
public class Factory : MonoBehaviour {
	//droneString is the name of the drone prefab
	public string droneString;
	//startingDroneNumber is the initial number of drones to spawn
	public int startingDroneNumber;
	//maxBotNumber is the maximum number of bots this factory is allowed to have
	public int maxBotNumber;
	//botSpawnTime is the time it takes to spawn a bot
	public float botSpawnTime;

	//spawnLocations is a list of all of our spawn locations
	private List<Vector3> spawnLocations;
	//currentBots is a list of all of our current bots
	private List<Bot> currentBots;

	//----------------------------------------------------------
	//Init()
	//Ensures correct setup of the factory
	//Return:
	//		Void
	//----------------------------------------------------------
	public void Init () {
		//Get our spawn locations from the children of this object
		//If there are no children print an error to the console saying that some are required
		if(transform.childCount > 0) {
			//Make sure our spawn location list is initialised
			if(spawnLocations == null){
				spawnLocations = new List<Vector3> ();		
			}

			for (int i = 0; i < transform.childCount; i++) {
				spawnLocations.Add (transform.GetChild (i).position);
			}
		} else {
			Debug.LogError (string.Format ("{0} requires locations for spawn points but none were found!", name));
		}

		//Make sure our bot list is initialised
		if(currentBots == null){
			currentBots = new List<Bot> ();	
		}

		while (currentBots.Count != startingDroneNumber) {
			SpawnDrone ();
		}
	}

	void Update () {
		
	}

	//----------------------------------------------------------
	//SpawnDrone()
	//Spawns a drone at a random spawnLocation and adds it to the 
	//current bot list. Will not spawn a bot if a free location
	//cannot be found
	//Return:
	//		Void
	//----------------------------------------------------------
	private void SpawnDrone() {
		//Pick a spawn location at random
		Vector3 location = spawnLocations[Random.Range(0, spawnLocations.Count)];



		Bot b = SimplePool.Spawn (BotLibrary.Instance.GetBotByType(droneString),
			              		  location,
						  		  Quaternion.identity).GetComponent<Bot>();

		b.Init (this);
		currentBots.Add (b);
	}

	//----------------------------------------------------------
	//SpawnDrone()
	//Spawns a drone at a random spawnLocation and adds it to the 
	//Param:
	//		Vector3 location - the location to check
	//Return:
	//		Void
	//----------------------------------------------------------
	private bool SpawnLocationEmpty(Vector3 location) {
		
	}
}
