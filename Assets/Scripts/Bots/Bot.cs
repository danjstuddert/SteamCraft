using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The base class that every Bot type inherits from
[RequireComponent(typeof(BotMove))]
public abstract class Bot : MonoBehaviour {
	//homeFactory is the factory that this bot was spawned from
	public Factory HomeFactory {get;private set;}
	//OwningPlayer is the player that owns the bot
	public Player OwningPlayer { get; private set; }

	//The BotMovement script for this particular bot
	private BotMove movement;

	//----------------------------------------------------------
	//Init()
	//Ensures that this bot is setup correctly
	//Param:
	//		Factory homeFactory - the factory that spawned this bot
	//Return:
	//		Void
	//----------------------------------------------------------
	public virtual void Init(Factory homeFactory){
		HomeFactory = homeFactory;
		OwningPlayer = homeFactory.owningPlayer;

		movement = GetComponent<BotMove>();
		movement.Init(OwningPlayer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
