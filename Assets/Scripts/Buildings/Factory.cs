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
	//owningPlayer is the player that owns this factory
	public Player owningPlayer;
	//botParent is the object to parent the spawned bots to
	public Transform botParent;

	//spawnLocations is a list of all of our spawn locations
	private List<SpawnPoint> spawnLocations;
	//currentBots is a list of all of our current bots
	private List<Bot> currentBots;
	//spawnCount is used as a counter to compare botSpawnTime against
	private float spawnCount;


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
				spawnLocations = new List<SpawnPoint> ();		
			}
				
			for (int i = 0; i < transform.childCount; i++) {
				if(transform.GetChild(i).GetComponent<SpawnPoint>()){
					spawnLocations.Add (transform.GetChild (i).GetComponent<SpawnPoint>());	
				}
			}
		} else {
			Debug.LogError (string.Format ("{0} requires locations for spawn points but none were found!", name));
		}

		//Make sure our bot list is initialised
		if(currentBots == null){
			currentBots = new List<Bot> ();	
		}

		//Spawn in our starting bot number, making sure we break out
		//if a valid spawn location cannot be found
		while (currentBots.Count != startingDroneNumber) {
			if(SpawnDrone() == false){
				break;
			}
		}
	}

	void Update(){
		CheckSpawn ();
	}

	//----------------------------------------------------------
	//RemoveBot()
	//Removes a bot from the current Bot list
	//Param:
	//		Bot bot - the bot to remove from the current bot list
	//Return:
	//		Void
	//----------------------------------------------------------
	public void RemoveBot(Bot bot){
		if(currentBots.Exists(b => b == bot)) {
			currentBots.Remove (bot);
		}
	}

	//----------------------------------------------------------
	//AddBot()
	//Adds a bot from the current Bot list
	//Param:
	//		Bot bot - the bot to add to the current bot list
	//Return:
	//		Void
	//----------------------------------------------------------
	public void AddBot(Bot bot){
		if(currentBots.Exists(b => b == bot) == false){
			currentBots.Add (bot);
		}
	}

	//----------------------------------------------------------
	//CheckSpawn()
	//Checks if it is time to spawn a drone
	//Return:
	//		Void
	//----------------------------------------------------------
	private void CheckSpawn(){
		if(spawnCount <= botSpawnTime){
			spawnCount += Time.deltaTime;
		}

		if(currentBots.Count < maxBotNumber && spawnCount >= botSpawnTime){
			//If we can spawn a drone do it and reset spawnCount
			if(SpawnDrone()){
				spawnCount = 0f;
			}
		}
	}

	//----------------------------------------------------------
	//SpawnDrone()
	//Spawns a drone at a given location and adds it to the 
	//current bot list.
	//Param:
	//		Vector3 location - the location to spawn the Drone
	//Return:
	//		bool - If placement was successful
	//----------------------------------------------------------
	private bool SpawnDrone() {
		//Go through the spawn locations until we find a free one
		Vector3 location = Vector3.zero;
		bool locationFound = false;

		for (int i = 0; i < spawnLocations.Count; i++) {
			SpawnPoint s = spawnLocations [i];
			location = spawnLocations [i].transform.position;

			if(SpawnLocationEmpty(s)){
				s.SetOccupied ();
				locationFound = true;
				break;
			}
		}

		//If we found a location spawn a Bot there, add it to the
		//current bot list and return true
		if(locationFound){
			Bot b = SimplePool.Spawn (BotLibrary.Instance.GetBotByType(droneString), location, Quaternion.identity).GetComponent<Bot>();
			b.transform.SetParent (botParent);
			b.Init (this);
			currentBots.Add (b);
			return true;
		}

		//If we could not find an empty location return false
		return false;
	}

	//----------------------------------------------------------
	//SpawnLocationEmpty()
	//Spawns a drone at a random spawnLocation and adds it to the 
	//Param:
	//		SpawnPoint point - the spawn point to check
	//Return:
	//		Void
	//----------------------------------------------------------
	private bool SpawnLocationEmpty(SpawnPoint point) {
		return !point.IsOccupied;
	}
}
