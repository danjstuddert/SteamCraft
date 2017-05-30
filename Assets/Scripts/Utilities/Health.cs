using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles the health of an object
public class Health : MonoBehaviour {
	//startingHealth is the starting amount of health an object has
	public int startingHealth;

	//CurrentHealth is the objects current health
	public int CurrentHealth {get; private set;}

	//maxHealth is the maximum health an object can have
	private int maxHealth;

	//----------------------------------------------------------
	//Init()
	//Sets up the health of the object
	//Return:
	//		void
	//----------------------------------------------------------
	public void Init() {
		//Set our health Current and max health values to starting health
		CurrentHealth = startingHealth;
		maxHealth = startingHealth;
	}

	//----------------------------------------------------------
	//AdjustHealth()
	//Adjusts the objects health by a given amount
	//Return:
	//		int amount, the amount to adjust health by
	//----------------------------------------------------------

	public void AdjustHealth(int amount) {
		//If the given amount will bring us over max health
		//do nothing
		if(CurrentHealth + amount > maxHealth) {
			return;
		}

		//If the given amount will bring us below 0 health
		//get the game manager to correctly despawn us
		if(CurrentHealth + amount <= 0) {
			GameController.Instance.HandleDespawn (gameObject);
			return;
		}

		//Add the given amount to our current health
		CurrentHealth += amount;
	}
}
