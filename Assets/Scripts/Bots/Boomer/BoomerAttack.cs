using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the attack sequence of the boomer bot
public class BoomerAttack : BotAttack {
	public float explosionRadius;
	public ParticleSystem explosionParticles;

//----------------------------------------------------------
//	ApplyDamage()
// Applies damage to every collider on the attacking mask within explosionRadius,
// removing self in the process
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	protected override void ApplyDamage () {
		Collider[] hitColliders = Physics.OverlapSphere (transform.position, explosionRadius, attackingMask);

		foreach (Collider col in hitColliders) {
			col.GetComponent<Health> ().AdjustHealth (-attackDamage);
		}

		ParticleSystem p = SimplePool.Spawn (explosionParticles.gameObject, transform.position, Quaternion.identity).GetComponent<ParticleSystem> ();
		GetComponent<Health> ().AdjustHealth (int.MaxValue);
	}
}
