using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MovementMode {Stay, MoveToPoint}

[RequireComponent(typeof(NavMeshAgent))]
public class BotMove : MonoBehaviour {
	//The movement mode that the bot starts in
	public MovementMode startingMovementMode;
	//The closest that this bot can get to its target
	public float minDistanceToTarget;

	//The current target of the bot
	public Transform CurrentTarget { get; private set; }

	//The navMeshAgent of this object
	private NavMeshAgent navMeshAgent;
	//The current movementMode of the bot
	private MovementMode movementMode;

	//----------------------------------------------------------
	//Init()
	//Ensures the botMove script is setup correctly
	//Params:
	//		float movementSpeed - the movement speed of this particular bot
	//Return:
	//		Void
	//----------------------------------------------------------
	public void Init() {
		movementMode = startingMovementMode;
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	void Update () {
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
		movementMode = MovementMode.MoveToPoint;
	}

	public bool HasTarget(){
		return CurrentTarget != null ? true : false;
	}

	//----------------------------------------------------------
	//HandleMove()
	//Updates the bot's movement
	//Return:
	//		Void
	//----------------------------------------------------------
	private void HandleMove() {
		switch (movementMode) {
			case MovementMode.MoveToPoint:
				MoveToPoint();
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
	//Return:
	//		Void
	//----------------------------------------------------------
	private void MoveToPoint() {
		if(CurrentTarget != null) {
			if(Vector3.Distance(transform.position, CurrentTarget.position) > minDistanceToTarget || CurrentTarget.tag == "UpgradeStation"){
				navMeshAgent.destination = CurrentTarget.position;
			} else {
				navMeshAgent.destination = transform.position;
			}
		}
	}
}
