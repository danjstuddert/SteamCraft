using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the upgrading of Bots
public class UpgradeStation : MonoBehaviour {
	//The time that it will take to upgrade
	public float upgradeTime;
	//The prefab that this station will spawn
	public GameObject botPrefab;
	//The location to spawn the upgradedBot
	public Transform spawnLocation;

	//The number of items to left to spawn
	private int spawnCounter;
	//A counter for spawning
	private float upgradeCounter;

	void Update() {
		if(spawnCounter > 0) {
			ProcessSpawn();
		}
	}

	//----------------------------------------------------------
	//Adds to the upgrade count
	//Return:
	//		Void
	//----------------------------------------------------------
	public void AddUpgrade() {
		spawnCounter++;
	}

	//----------------------------------------------------------
	//Handles the spawning of Bots from this Upgrade station
	//Return:
	//		Void
	//----------------------------------------------------------
	private void ProcessSpawn() {
		if(upgradeCounter <= upgradeTime) {
			upgradeCounter += Time.deltaTime;
		} else {
			SimplePool.Spawn(botPrefab, spawnLocation.position, botPrefab.transform.rotation);
			upgradeCounter = 0f;
		}
	}


}
