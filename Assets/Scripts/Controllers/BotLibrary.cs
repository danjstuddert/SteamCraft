using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Stores and distributes bot prefabs
public class BotLibrary : Singleton<BotLibrary> {
	//A list of all of the available bot prefabs
	public List<GameObject> botPrefabs;

	//----------------------------------------------------------
	//SpawnDrone()
	//Spawns a drone at a random
	//Return:
	//		Requested drone prefab gameobject, 
	//      null if no prefab was found
	//----------------------------------------------------------
	public GameObject GetBotByType(string botName) {
		//Look through our bot prefabs to see if
		//we have a botPrefab called botName
		foreach(GameObject bot in botPrefabs){
			//If we do return it
			if(bot.name == botName){
				return bot;
			}
		}

		//If we did not find a bot of botName print an error to the console
		//and return null
		Debug.LogError (string.Format ("{0} is not a valid Bot type!", botName));
		return null;
	}
}
