using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Draws a gizmo on the screen to assist with placement of spawn points
public class SpawnPoint : MonoBehaviour {
	//Is this spawn point occupied?
	public bool IsOccupied { get; private set;}

	//
	private Bot occupiedBy;

	//----------------------------------------------------------
	//OnDrawGizmos()
	//Draws a gizmo sphere at the location of this transform
	//Return:
	//		Void
	//----------------------------------------------------------
	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (transform.position, 0.1f);
	}

	//----------------------------------------------------------
	//BecomeOccupied()
	//
	//Params:
	//		Drone drone - the drone to store
	//Return:
	//		Void
	//----------------------------------------------------------
	public void BecomeOccupied(Bot drone){
		IsOccupied = true;
		occupiedBy = drone;
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Drone" && other.GetComponent<Bot> () == occupiedBy) {
			Debug.Log ("Exit");
			IsOccupied = false;
			occupiedBy = null;
		}
	}
}
