using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores and distributes bot prefabs
public class BotLibrary : Singleton<BotLibrary> {
// A list of all of the available bot prefabs
	public List<GameObject> botPrefabs;

//----------------------------------------------------------
//	SpawnDrone()
// Spawns a drone at a random
//
// Param:
//		string botName - the name of the bot to get
// Return:
//		GameObject Requested drone prefab gameobject, 
//      null if no prefab was found
//----------------------------------------------------------
	public GameObject GetBotByType(string botName) {
		foreach(GameObject bot in botPrefabs){
			//If we do return it
			if(bot.name == botName){
				return bot;
			}
		}

		Debug.LogError (string.Format ("{0} is not a valid Bot type!", botName));
		return null;
	}
}
