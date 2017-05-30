using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the controls of the drone
public class Drone : Bot {

	//----------------------------------------------------------
	//Init()
	//Ensures that this Drone is setup correctly
	//Param:
	//		Factory homeFactory - the factory that spawned this bot
	//Return:
	//		Void
	//----------------------------------------------------------
	public override void Init (Factory homeFactory) {
		base.Init (homeFactory);

		MaterialController.Instance.UpdateMaterial (GetComponent<Renderer> (), ObjectType.Drone, OwningPlayer.playerFaction);
	}

	protected override void OnTriggerEnter(Collider other) {
		base.OnTriggerEnter(other);

		//If we entered an upgrade station, we are not following the player and it is owned by our player enter it
		if (other.tag == "UpgradeStation" && movement.CurrentTarget != OwningPlayer.transform && other.GetComponent<UpgradeStation>().owningPlayer == OwningPlayer) {
			EnterUpgradeStation(other.GetComponent<UpgradeStation>());
		}
	}
}
