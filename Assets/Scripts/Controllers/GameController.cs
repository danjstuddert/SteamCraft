using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles the starting and running of the game, 
//including despawning of killed units and win conditions
public class GameController : Singleton<GameController> {
	//All of the factories in the game
	private List<Factory> factoryList;

	void Start () {
		//Make sure we initalise our factories correctly
		factoryList = new List<Factory>();
		foreach(Factory f in FindObjectsOfType(typeof(Factory))){
			factoryList.Add(f);
			f.Init ();
		}

		PlayerController.Instance.Init();
	}

	//----------------------------------------------------------
	//HandleDespawn()
	//Correctly handles the despawning of each object type
	//Param:
	//		GameObject obj - the object to despawn
	//Return:
	//		Void
	//----------------------------------------------------------
	public void HandleDespawn(GameObject obj) {
		switch (obj.tag) {
			case "Player":
				PlayerController.Instance.PlayerDead(obj);
				break;
			case "Bot":
				DespawnBot(obj.GetComponent<Bot>());
				break;
			case "Factory":
				break;
			default:
				Debug.LogError (string.Format("{0} does not contain a health script but despawn was requested", obj.name));
				break;
		}

		//Despawn the object using the object pooling script
		SimplePool.Despawn (obj);	
	}

	public Factory GetOpposingFactory(Player p) {
		foreach (Factory f in factoryList) {
			if(f.owningPlayer != p) {
				return f;
			}
		}

		return null;
	}

	//----------------------------------------------------------
	//DespawnBot()
	//Tells the factory that the bot comes from that it is gone
	//Param:
	//		Bot bot - the bot to despawn
	//Return:
	//		Void
	//----------------------------------------------------------
	private void DespawnBot(Bot bot) {
		bot.HomeFactory.RemoveBot(bot);
	}

	//----------------------------------------------------------
	//DespawnBot()
	//Correctly handles when a player wins the game and then resets the scene
	//Param:
	//		Bot bot - the bot to despawn
	//Return:
	//		Void
	//----------------------------------------------------------
	private void PlayerWon(Player player) {
		Debug.Log(string.Format("{0} has won!", player.name));

		StartCoroutine(ReloadScene(1f));
	}

	private IEnumerator ReloadScene(float t) {
		float timer = 0f;
		while (timer < t) {
			timer += Time.deltaTime;
			yield return null;
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
