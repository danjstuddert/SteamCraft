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
		movement.Init();
	}

	//----------------------------------------------------------
	//GiveTarget()
	//Gives a target to the attached movement script
	//Param:
	//		Transform target - the target to give the movement script
	//Return:
	//		Void
	//----------------------------------------------------------
	public virtual void GiveTarget(Transform target) {
		movement.GiveTarget(target);
	}

	//----------------------------------------------------------
	//EnterUpgradeStation()
	//Tells the given station to spawn its upgrade and handles despawning
	//of this bot
	//Param:
	//		UpgradeStation station - the station to add a spawn command to
	//Return:
	//		Void
	//----------------------------------------------------------
	protected virtual void EnterUpgradeStation(UpgradeStation station){
		station.AddUpgrade ();

		GameController.Instance.HandleDespawn (gameObject);
	}

	protected virtual void OnTriggerEnter(Collider other){
		//If we entered an upgrade station and it is owned by our player enter it
		if(other.tag == "UpgradeStation" && other.GetComponent<UpgradeStation>().owningPlayer == OwningPlayer) {
			EnterUpgradeStation (other.GetComponent<UpgradeStation>());
		}
	}
}
