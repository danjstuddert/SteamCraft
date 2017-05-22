using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Draws a gizmo on the screen to assist with placement of spawn points
public class SpawnPoint : MonoBehaviour {
	public bool IsOccupied { get; private set; }
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

	public void SetOccupied(){
		IsOccupied = true;
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Drone"){
			IsOccupied = true;	
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Drone"){
			IsOccupied = false;	
		}
	}
}
