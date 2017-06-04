using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A base class that handles bot attacking
public abstract class BotAttack : MonoBehaviour {
// attackDamage is the damage inflicted with each attack
	public int attackDamage;
// attackRate is how fast the bot attacks
	public float attackRate;
// attackRange is the range that the bot has to be in to attack
	public float attackRange;
// attackingMask is the layermask to check attacks against
	public LayerMask attackingMask;

// bot is the bot script attached to this object
	protected Bot bot;
// attackCount is a timer to use when checking to see if the bot can attack
	protected float attackCount;

//----------------------------------------------------------
//	Init()
// Ensures the BotAttack script is setup correctly
//
// Params:
//		Bot bot - the bot script that is attached to this object
// Return:
//		Void
//----------------------------------------------------------
	public virtual void Init(Bot bot) {
		this.bot = bot;

		//Set attack count to attack rate so we can attack right out of the gate
		//if we are within range
		attackCount = attackRate;
	}

//--------------------------------------------------------------------------------------
//	Update()
// Runs every frame
//
// Param:
//		None
// Return:
//		Void
//--------------------------------------------------------------------------------------
	void Update(){
		CheckAttack ();
	}

//----------------------------------------------------------
//CheckAttack()
//Checks to see if this bot should attack
//
//Param:
//		None
//Return:
//		Void
//----------------------------------------------------------
	private void CheckAttack(){
		if(bot.CurrentTarget == null || bot.CurrentTarget.tag == "Player" && bot.CurrentTarget.GetComponent<Player>().playerFaction == bot.OwningPlayer.playerFaction){
			return;
		}

		if (Vector3.Distance (transform.position, bot.CurrentTarget.position) <= attackRange && attackCount >= attackRate) {
			ApplyDamage();
		}

		attackCount += Time.deltaTime;
	}

//----------------------------------------------------------
//ApplyDamage()
//Applies damage to the current target
//
//Param:
//		None
//Return:
//		Void
//----------------------------------------------------------
	protected virtual void ApplyDamage(){
		attackCount = 0f;
		Health targetHealth = bot.CurrentTarget.GetComponent<Health> ();

		if(targetHealth){
			targetHealth.AdjustHealth (-attackDamage);
		}
	}
}