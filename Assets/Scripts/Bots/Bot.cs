using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The base class that every Bot type inherits from
public abstract class Bot : MonoBehaviour {
	//homeFactory is the factory that this bot was spawned from
	public Factory HomeFactory {get;private set;}

	//----------------------------------------------------------
	//Init()
	//Spawns a drone at a random
	//Param:
	//		Factory homeFactory - the factory that spawned this bot
	//Return:
	//		Void
	//----------------------------------------------------------
	public virtual void Init(Factory homeFactory){
		HomeFactory = homeFactory;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
