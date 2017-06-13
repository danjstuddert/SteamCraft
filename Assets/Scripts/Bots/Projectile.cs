using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the control of projectiles
public class Projectile : MonoBehaviour {
// damage is the amount of damage the projectile causes
	private int damage;
// moveSpeed is the speed that the projectile travels at
	private float moveSpeed;
// target is the target that the projectile is firing towards
	private Transform target;

//----------------------------------------------------------
//	Init()
// Initialises the projectile including damage, speed and target variables
// as well as setting the colour of the trail renderer of this object to the 
// colour of the owning faction
//
// Param:
//		int damage - the damage that this projectile will cause
//		float moveSpeed - the speed the projectile moves at
//		Transform target - the target of the projectile
//		Faction owningFaction - the faction that owns the projectile
// Return:
//		Void
//----------------------------------------------------------
	public void Init(int damage, float moveSpeed, Transform target, Faction owningFaction) {
		this.damage = damage;
		this.moveSpeed = moveSpeed;
		this.target = target;
		MaterialController.Instance.UpdateMaterial(GetComponent<TrailRenderer>(), ObjectType.Projectile, owningFaction);
	}

//----------------------------------------------------------
//	Update()
// Runs every frame
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	void Update() {
		CheckTarget();
		MoveToTarget();
	}

//----------------------------------------------------------
//	CheckTarget()
// Checks to see if the target has been removed or if we are at the same position
// as our target and we haven't been removed for some reason
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	private void CheckTarget() {
		if (target == null) {
			SimplePool.Despawn(gameObject);
		} else if (transform.position == target.position) {
			target.GetComponent<Health>().AdjustHealth(-damage);
			SimplePool.Despawn(gameObject);
		}
	}

	private void MoveToTarget() {
		transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) {
		if(other.transform == target && other.transform.GetComponent<Health>()) {
			other.transform.GetComponent<Health>().AdjustHealth(-damage);
			SimplePool.Despawn(gameObject);
		}
	}
}
