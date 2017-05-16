using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
	//The starting amount of health an object has
	public int startingHealth;

	//The objects current health
	public int CurrentHealth {get; private set;}

	//The maximum health an object can have
	private int maxHealth;

	//Sets up the objects health
	public void Init() {
		//Set our health Current and max health values to starting health
		CurrentHealth = startingHealth;
		maxHealth = startingHealth;
	}

	//Adjusts the objects health by a given amount
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
