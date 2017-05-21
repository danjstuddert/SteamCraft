using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MovementMode {Stay, MoveToPoint}

[RequireComponent(typeof(NavMeshAgent))]
public class BotMove : MonoBehaviour {
	//The movement mode that the bot starts in
	public MovementMode startingMovementMode;

	//The navMeshAgent of this object
	private NavMeshAgent navMeshAgent;
	//The current movementMode of the bot
	private MovementMode movementMode;
	//The current target of the bot
	private Transform currentTarget;

	//----------------------------------------------------------
	//Init()
	//Ensures the botMove script is setup correctly
	//Params:
	//		float movementSpeed - the movement speed of this particular bot
	//Return:
	//		Void
	//----------------------------------------------------------
	public void Init(Player p) {
		movementMode = startingMovementMode;
		navMeshAgent = GetComponent<NavMeshAgent>();

		//This will have to be altered when we expand into the control functionality 
		//but it will work for now
		currentTarget = p.transform;
	}

	void Update () {
		HandleMove();		
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
		if(currentTarget != null && currentTarget.position != navMeshAgent.destination) {
			navMeshAgent.destination = currentTarget.position;
		}
	}
}
