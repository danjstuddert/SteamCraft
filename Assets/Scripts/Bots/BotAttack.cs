using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BotAttack : MonoBehaviour {
	//attackDamage is the damage inflicted with each attack
	public int attackDamage;
	//attackRate is how fast the bot attacks
	public float attackRate;
	//attackRange is the range that the bot has to be in to attack
	public float attackRange;

	//bot is the bot script attached to this object
	protected Bot bot;

	//----------------------------------------------------------
	//Init()
	//Ensures the BotAttack script is setup correctly
	//Params:
	//		Bot bot - the bot script that is attached to this object
	//Return:
	//		Void
	//----------------------------------------------------------
	public virtual void Init(Bot bot) {
		this.bot = bot;
	}
}
