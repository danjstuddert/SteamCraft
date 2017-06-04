using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the control of projectiles
public class Projectile : MonoBehaviour {
	private int damage;
	private float moveSpeed;
	private Transform target;

	public void Init(int damage, float moveSpeed, Transform target, Faction owningFaction) {
		this.damage = damage;
		this.moveSpeed = moveSpeed;
		this.target = target;
		MaterialController.Instance.UpdateMaterial(GetComponent<TrailRenderer>(), ObjectType.Projectile, owningFaction);
	}
	
	void Update() {
		transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) {
		if(other.transform == target && other.transform.GetComponent<Health>()) {
			other.transform.GetComponent<Health>().AdjustHealth(-damage);
			SimplePool.Despawn(gameObject);
		}
	}
}
