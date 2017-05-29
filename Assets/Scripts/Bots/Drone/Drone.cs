using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Bot {
	protected override void OnTriggerEnter(Collider other) {
		base.OnTriggerEnter(other);

		//If we entered an upgrade station, we are not following the player and it is owned by our player enter it
		if (other.tag == "UpgradeStation" && movement.CurrentTarget != OwningPlayer.transform && other.GetComponent<UpgradeStation>().owningPlayer == OwningPlayer) {
			EnterUpgradeStation(other.GetComponent<UpgradeStation>());
		}
	}
}
