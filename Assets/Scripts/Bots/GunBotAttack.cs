using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBotAttack : BotAttack {
// projectile is the projectile prefab to instantiate
	public GameObject projectile;
// projectileForce is the amount of force to apply to the projectile
	public float projectileSpeed;
// turretRotateSpeed is how fast the bots turret rotates
	public float turretRotateSpeed;
// turret is the bot's turret
	public Transform turret;
// projectilePoint is the point where the projectile will spawn
	public Transform projectilePoint;

//----------------------------------------------------------
//	ApplyDamage()
// Applies damage to the current target if the bot's turret is facing it
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	protected override void ApplyDamage() {
		Vector3 targetDirection = bot.CurrentTarget.position - turret.position;
		Vector3 newDirection = Vector3.RotateTowards(turret.forward, targetDirection, turretRotateSpeed * Time.deltaTime, 0f);
		turret.rotation = Quaternion.LookRotation(newDirection);

		RaycastHit[] hits = Physics.RaycastAll(projectilePoint.position, projectilePoint.forward, Mathf.Infinity);

		Debug.DrawRay(projectilePoint.position, projectilePoint.forward * 100);

		for (int i = 0; i < hits.Length; i++) {
			if (hits[i].transform == bot.CurrentTarget) {
				Projectile p = SimplePool.Spawn(projectile, projectilePoint.position, projectilePoint.transform.rotation).GetComponent<Projectile>();
				p.Init(attackDamage, projectileSpeed, bot.CurrentTarget, bot.OwningPlayer.playerFaction);

				attackCount = 0f;
			}
		}
	}
}
