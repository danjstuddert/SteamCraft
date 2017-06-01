using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Internal logic of the factory object
public class Factory : MonoBehaviour {
// droneString is the name of the drone prefab
	public string droneString;
// startingDroneNumber is the initial number of drones to spawn
	public int startingDroneNumber;
// maxBotNumber is the maximum number of bots this factory is allowed to have
	public int maxBotNumber;
// botSpawnTime is the time it takes to spawn a bot
	public float botSpawnTime;
// owningPlayer is the player that owns this factory
	public Player owningPlayer;
// botParent is the object to parent the spawned bots to
	public Transform botParent;
// pointCheckRadius is the radius of the overlapsphere when checking points
	public float pointCheckRadius;
// droneLayer is the layer the drones are on
	public LayerMask droneLayer;

// spawnLocations is a list of all of our spawn locations
	private List<Vector3> spawnLocations;
// currentBots is a list of all of our current bots
	private List<Bot> currentBots;
// spawnCount is used as a counter to compare botSpawnTime against
	private float spawnCount;


//----------------------------------------------------------
//	Init()
// Ensures correct setup of the factory
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	public void Init () {
		if(transform.childCount > 0) {
			if(spawnLocations == null){
				spawnLocations = new List<Vector3> ();		
			}
				
			for (int i = 0; i < transform.childCount; i++) {
				if(transform.GetChild(i).tag == "SpawnLocation"){
					spawnLocations.Add (transform.GetChild (i).position);	
				}
			}
		} else {
			Debug.LogError (string.Format ("{0} requires locations for spawn points but none were found!", name));
		}
			
		if(currentBots == null){
			currentBots = new List<Bot> ();	
		}
			
		while (currentBots.Count != startingDroneNumber) {
			if(SpawnDrone() == false){
				break;
			}
		}
		GetComponent<Health>().Init();
		MaterialController.Instance.UpdateMaterial(transform.GetComponentInChildren<Renderer>(), ObjectType.Factory, owningPlayer.playerFaction);
	}

//--------------------------------------------------------------------------------------
//	Update()
// Runs every frame
//
// Param:
//		None
// Return:
//		Void
//--------------------------------------------------------------------------------------
	void Update(){
		CheckSpawn ();
	}

//----------------------------------------------------------
//	RemoveBot()
// Removes a bot from the current Bot list
//
// Param:
//		Bot bot - the bot to remove from the current bot list
// Return:
//		Void
//----------------------------------------------------------
	public void RemoveBot(Bot bot){
		if(currentBots.Exists(b => b == bot)) {
			currentBots.Remove (bot);
		}
	}

//----------------------------------------------------------
//	AddBot()
// Adds a bot from the current Bot list
//
// Param:
//		Bot bot - the bot to add to the current bot list
// Return:
//		Void
//----------------------------------------------------------
	public void AddBot(Bot bot){
		if(currentBots.Exists(b => b == bot) == false){
			currentBots.Add (bot);
		}
	}

//----------------------------------------------------------
//	CheckSpawn()
// Checks if it is time to spawn a drone
//
// Param:
//		None
// Return:
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
//	SpawnDrone()
// Spawns a drone at a given location and adds it to the 
// current bot list.
//
// Param:
//		Vector3 location - the location to spawn the Drone
// Return:
//		bool - If placement was successful
//----------------------------------------------------------
	private bool SpawnDrone() {
		for (int i = 0; i < spawnLocations.Count; i++) {
			Vector3 location = spawnLocations [i];

			if(SpawnLocationOccupied(location) == false) {
				Bot bot = SimplePool.Spawn (BotLibrary.Instance.GetBotByType(droneString), location, 
					owningPlayer.playerFaction == Faction.Blue ? Quaternion.Euler(0f,90f,0f) : Quaternion.Euler(0f, -90f, 0f)).GetComponent<Bot>();

				bot.transform.SetParent (botParent);
				bot.Init (this);

				currentBots.Add (bot);
				return true;
			}
		}
			
		return false;
	}

//----------------------------------------------------------
//	SpawnLocationOccupied()
// Checks to see if the given spawn point is empty
//
// Param:
//		Vector3 point - the spawn point to check
// Return:
//		Void
//----------------------------------------------------------
	private bool SpawnLocationOccupied(Vector3 point) {
		Collider[] hitColliders = Physics.OverlapSphere (point, pointCheckRadius, droneLayer);
		return hitColliders.Length > 0 ? true : false;
	}
}
