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
	protected BotMove movement;
	//attack is the player's attack script
	protected BotAttack attack;

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
		movement.Init(this);

		if (GetComponent<BotAttack>() != null) {
			attack = GetComponent<BotAttack>();
			attack.Init(this);
		}
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
	//GiveTarget()
	//Gives a point to the attached movement script
	//Param:
	//		Vector3 point - the target to give the movement script
	//Return:
	//		Void
	//----------------------------------------------------------
	public virtual void GivePoint(Vector3 point) {
		movement.GivePoint(point);
	}

	//----------------------------------------------------------
	//HasTarget()
	//Checks the movement script to see if the bot currently
	//has a target
	//Return:
	//		Bool - True if the bot has a target, false otherwise
	//----------------------------------------------------------
	public bool HasTarget(){
		return movement.HasTarget ();
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
		//If we were touched by our call circle start following the player
		if(other.tag == "CallCircle" && other.GetComponent<CallCircle>().owningPlayer == OwningPlayer) {
			movement.GiveTarget(OwningPlayer.transform);
			OwningPlayer.GiveBot(this);
		}
	}
}
