using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MovementMode {Stay, MoveToTarget, MoveToPoint}

[RequireComponent(typeof(NavMeshAgent))]
public class BotMove : MonoBehaviour {
	//startingMovementMode is the movement mode that the bot starts in
	public MovementMode startingMovementMode;
	//minDistanceToTarget is the closest that this bot can get to its target
	public float minDistanceToTarget;

	//CurrentTarget is the current target of the bot
	public Transform CurrentTarget { get; private set; }
	//targetPoint is the position the bot is trying to reach
	private Vector3 targetPoint;

	//navMeshAgent is the navMeshAgent of this object
	private NavMeshAgent navMeshAgent;
	//movementMode is the current movementMode of the bot
	private MovementMode movementMode;
	//
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
	//HandleMove()
	//Updates the bot's movement
	//Return:
	//		Void
	//----------------------------------------------------------
	private void HandleMove() {
		switch (movementMode) {
			case MovementMode.MoveToTarget:
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
}
