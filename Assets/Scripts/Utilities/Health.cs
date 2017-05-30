using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the health of an object
public class Health : MonoBehaviour {
// startingHealth is the starting amount of health an object has
	public int startingHealth;

// CurrentHealth is the objects current health
	public int CurrentHealth {get; private set;}

// maxHealth is the maximum health an object can have
	private int maxHealth;

//----------------------------------------------------------
//	Init()
// Sets up the health of the object
//
// Params:
//		None
// Return:
//		void
//----------------------------------------------------------
	public void Init() {
		CurrentHealth = startingHealth;
		maxHealth = startingHealth;
	}

//----------------------------------------------------------
// AdjustHealth()
// Adjusts the objects health by a given amount, tells the game controller
// to despawn the object if health is below 0
//
// Params:
//		int amount the amount to adjust health by
// Return:
//		void
//----------------------------------------------------------
	public void AdjustHealth(int amount) {
		if(CurrentHealth + amount > maxHealth) {
			return;
		}
			
		if(CurrentHealth + amount <= 0) {
			GameController.Instance.HandleDespawn (gameObject);
			return;
		}

		CurrentHealth += amount;
	}
}
