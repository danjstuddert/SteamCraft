using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The attack class of the buzzsaw bot
public class BuzzsawAttack : BotAttack {
//----------------------------------------------------------
//	ApplyDamage()
// Applies damage to the targets health component
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	protected virtual void ApplyDamage() {
		attackCount = 0f;
		Health targetHealth = bot.CurrentTarget.GetComponent<Health>();

		if (targetHealth) {
			targetHealth.AdjustHealth(-attackDamage);
		}
	}
}
