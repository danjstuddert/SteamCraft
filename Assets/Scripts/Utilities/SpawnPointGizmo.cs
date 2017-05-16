using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Draws a gizmo on the screen to assist with placement of spawn points
public class SpawnPointGizmo : MonoBehaviour {
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
}
