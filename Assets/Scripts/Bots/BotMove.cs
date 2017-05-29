using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//An enumerator used to determine current movement state of individual bots
public enum MovementMode {Stay, MoveToTarget, MoveToPoint}

//A class that handles the movement of bots
[RequireComponent(typeof(NavMeshAgent))]
public class BotMove : MonoBehaviour {
	//startingMovementMode is the movement mode that the bot starts in
	public MovementMode startingMovementMode;
	//minDistanceToTarget is the closest that this bot can get to its target
	public float minDistanceToTarget;
	//detectionRadius is the radius to check for target detection
	public float detectionRadius;
	//detectionLayer is the layermask to use for detection raycasts
	public LayerMask detectionLayer;
	public Transform tar;

	//CurrentTarget is the current target of the bot
	public Transform CurrentTarget { get; private set; }
	//targetPoint is the position the bot is trying to reach
	private Vector3 targetPoint;

	//navMeshAgent is the navMeshAgent of this object
	private NavMeshAgent navMeshAgent;
	//movementMode is the current movementMode of the bot
	private MovementMode movementMode;
	//bot is the bot script attached to this object
	private Bot bot;

	//----------------------------------------------------------
	//Init()
	//Ensures the botMove script is setup correctly
	//Params:
	//		Bot bot - the bot script that is attached to this object
	//Return:
	//		Void
	//----------------------------------------------------------
	public void Init(Bot bot) {
		this.bot = bot;
		movementMode = startingMovementMode;
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	void Update () {
		tar = CurrentTarget;
		DetectHostile();
		HandleMove();		
	}

	//----------------------------------------------------------
	//GiveTarget()
	//Updates and moves to the given target
	//Param:
	//		Transform target - the target to move to
	//Return:
	//		Void
	//----------------------------------------------------------
	public void GiveTarget(Transform target) {
		CurrentTarget = target;
		movementMode = MovementMode.MoveToTarget;
	}

	//----------------------------------------------------------
	//GivePoint()
	//Updates and moves to the given target
	//Param:
	//		Vector3 point - the point to move to
	//Return:
	//		Void
	//----------------------------------------------------------
	public void GivePoint(Vector3 point) {
		CurrentTarget = null;
		targetPoint = point;
		movementMode = MovementMode.MoveToPoint;
	}

	//----------------------------------------------------------
	//HasTarget()
	//Returns true or false depending on if this bot has a target
	//Return:
	//		Bool - true if this bot has a target, false if not
	//----------------------------------------------------------
	public bool HasTarget(){
		return CurrentTarget != null ? true : false;
	}

	//----------------------------------------------------------
	//DetectHostile()
	//Detects if a hostile unit is within range and will change target
	//if one is found
	//Return:
	//		Void
	private void DetectHostile() {
		//If we are following our player or our movement mode is stay don't do anything
		if (CurrentTarget == bot.OwningPlayer || movementMode == MovementMode.Stay) {
			return;
		}

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

		//Attack priority is as follows: Player, Bot, Drone, Factory
		Transform target = null;
		for (int i = 0; i < hitColliders.Length; i++) {
			//If we found the enemy player we can just break since it is our highest priority target
			if (hitColliders[i].tag == "Player" && hitColliders[i].gameObject.activeInHierarchy && hitColliders[i].GetComponent<Player>() != bot.OwningPlayer) {
				target = hitColliders[i].transform;
				break;
			}

			if (hitColliders[i].tag == "Bot" && hitColliders[i].GetComponent<Bot>().OwningPlayer != bot.OwningPlayer) {
				//if the target we have is not a bot assign the current hitCollider to the target
				//and continue
				if(hitColliders[i].GetComponent<Bot>() == null) {
					target = hitColliders[i].transform;
					continue;
				}

				target = CheckTargetPriority(hitColliders[i].transform, target);
				continue;
			} else if (hitColliders[i].tag == "Drone" && hitColliders[i].GetComponent<Bot>().OwningPlayer != bot.OwningPlayer) {
				target = CheckTargetPriority(hitColliders[i].transform, target);
				continue;
			} else if (hitColliders[i].tag == "Factory" && hitColliders[i].GetComponent<Factory>().owningPlayer != bot.OwningPlayer) {
				target = CheckTargetPriority(hitColliders[i].transform, target);
				continue;
			}
		}

		if(target != null) {
			CurrentTarget = target;
		}

		//If we are moving to a point make sure we update our movement type to that
		//of a target if we now have one
		if (CurrentTarget != null && movementMode == MovementMode.MoveToPoint) {
			movementMode = MovementMode.MoveToTarget;
		}
	}

	//----------------------------------------------------------
	//HandleMove()
	//Updates the bot's movement
	//Return:
	//		Void
	//----------------------------------------------------------
	private void HandleMove() {
		switch (movementMode) {
			case MovementMode.MoveToTarget:
				if(CurrentTarget.gameObject.activeInHierarchy == false || CurrentTarget == null) {
					CheckTarget();
					break;
				}	

				MoveToPoint(CurrentTarget.position);
				break;
			case MovementMode.MoveToPoint:
				MoveToPoint(targetPoint);
				CheckTarget();
				break;
			//Because stay just does nothing we don't declare it in the switch
			//we just let it go through to the default
			default:
				break;
		}
	}

	//----------------------------------------------------------
	//MoveToPoint()
	//Updates the nav mesh agents destination to ensure that it reaches its target
	//Params:
	//		Vector3 point - the target point
	//Return:
	//		Void
	//----------------------------------------------------------
	private void MoveToPoint(Vector3 point) {
		if(Vector3.Distance(transform.position, point) > minDistanceToTarget || CurrentTarget != null && CurrentTarget.tag == "UpgradeStation"){
			navMeshAgent.destination = point;
		} else {
			navMeshAgent.destination = transform.position;
		}
	}

	//----------------------------------------------------------
	//CheckPointProximity()
	//Checks to see if the bot is at its target point for the MoveToPoint
	//setting for movement mode
	//Return:
	//		Void
	//----------------------------------------------------------
	private void CheckTarget() {
		if(Vector3.Distance(transform.position, targetPoint) < minDistanceToTarget) {
			//If we are a drone just stay still
			if(transform.tag == "Drone") {
				movementMode = MovementMode.Stay;
			} else {
				//If we are an upgraded bot make sure we update our target to be the opposing factory
				CurrentTarget = GameController.Instance.GetOpposingFactory(bot.OwningPlayer).transform;
				movementMode = MovementMode.MoveToTarget;
			}
		}
	}

	//----------------------------------------------------------
	//CheckTargetPriority()
	//Checks to see if the given target is a higher priorty than the current target
	//Params:
	//		Transform targetToCheck - the target to check
	//		Transform currentTarget - the target to check against
	//Return:
	//		Transform - the closer target
	//----------------------------------------------------------
	private Transform CheckTargetPriority(Transform targetToCheck, Transform currentTarget) {
		if (currentTarget == null || Vector3.Distance(transform.position, targetToCheck.transform.position) < 
			Vector3.Distance(transform.position, currentTarget.position)) {
			return targetToCheck;
		}

		return currentTarget;
	}
}
