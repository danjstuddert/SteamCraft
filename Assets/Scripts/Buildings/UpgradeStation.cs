using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the upgrading of Bots
public class UpgradeStation : MonoBehaviour {
	//upgradeTime is the time that it will take to upgrade
	public float upgradeTime;
	//botToSpawn is the prefab that this station will spawn
	public GameObject botToSpawn;
	//spawnLocation is the location to spawn the upgradedBot
	public Transform spawnLocation;
	//owningPlayer is the player that owns this upgrade station
	public Player owningPlayer;
	//botParent is the object to parent spawned bots to
	public Transform botParent;

	//The number of items to left to spawn
	private int spawnCounter;
	//A counter for spawning
	private float upgradeCounter;
	//The factory object of our team
	private Factory factory;

	public void Init(){
		factory = GameController.Instance.GetOwningFactory (owningPlayer);
	}

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
			//Spawn the bot
			Bot b = SimplePool.Spawn(botToSpawn, spawnLocation.position, Quaternion.identity).GetComponent<Bot>();
			b.Init(factory);
			b.GiveTarget(GameController.Instance.GetOpposingFactory(owningPlayer).transform);
			b.transform.SetParent (botParent);
			upgradeCounter = 0f;
			spawnCounter--;

			//Make sure we tell the factory that we have spawned something
			factory.AddBot (b);
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "CommandTarget" && other.GetComponentInParent<Player>() == owningPlayer){
			other.GetComponentInParent<Player>().GiveUpgradeStation(this);
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "CommandTarget" && other.GetComponentInParent<Player>() == owningPlayer) {
			other.GetComponentInParent<Player>().RemoveUpgradeStation(this);
		}
	}
}
