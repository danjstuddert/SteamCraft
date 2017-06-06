using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Handles the starting and running of the game, 
// including despawning of killed units and win conditions
public class GameController : Singleton<GameController> {
// factoryList is a list of all of the factories in the game
	private List<Factory> factoryList;

//----------------------------------------------------------
//	Start()
// Runs during initialisation to set up the game correctly
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	void Start () {
		Cursor.visible = false;

		factoryList = new List<Factory>();
		foreach(Factory f in FindObjectsOfType(typeof(Factory))){
			factoryList.Add(f);
			f.Init ();
		}

		foreach (UpgradeStation station in FindObjectsOfType(typeof(UpgradeStation))) {
			station.Init();
		}

		PlayerController.Instance.Init();
		CanvasController.Instance.Init();
	}

//----------------------------------------------------------
//	HandleDespawn()
// Correctly handles the despawning of each object type
//
// Param:
//		GameObject obj - the object to despawn
// Return:
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
			case "Drone":
				DespawnBot(obj.GetComponent<Bot>());
				break;
			case "Factory":
				EndGame(obj);
				break;
			default:
				Debug.LogError (string.Format("{0} does not contain a health script but despawn was requested", obj.name));
				break;
		}
	}

//----------------------------------------------------------
//	GetOwningFactory()
// Gets the factory owned by the player
//
// Param:
//		Player p - the player to check
// Return:
//		Factory - the factory that is owned by the player, null
//				  if no factory was found
//----------------------------------------------------------
	public Factory GetOwningFactory(Player p){
		foreach(Factory f in factoryList){
			if(f.owningPlayer == p){
				return f;
			}
		}

		Debug.LogError (string.Format ("{0} does not own a factory!", p.transform.name));
		return null;
	}

//----------------------------------------------------------
//	GetOpposingFactory()
// Gets the factory not owned by the player
//
// Param:
//		Player p - the player to check
// Return:
//		Factory - the factory that is owned by the player, null
//				  if no factory was found
//----------------------------------------------------------
	public Factory GetOpposingFactory(Player p) {
		foreach (Factory f in factoryList) {
			if(f.owningPlayer != p) {
				return f;
			}
		}

		Debug.LogError (string.Format ("No opposing factory found for {0}", p.transform.name));
		return null;
	}

//----------------------------------------------------------
//	ResetLevel()
// Resets the game level
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	public void ResetLevel() {
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

//----------------------------------------------------------
//	Quit()
// Quits the game
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	public void Quit() {
		Application.Quit();
	}

//----------------------------------------------------------
//	DespawnBot()
// Tells the factory that the bot comes from that it is gone
//
// Param:
//		Bot bot - the bot to despawn
// Return:
//		Void
//----------------------------------------------------------
	private void DespawnBot(Bot bot) {
		bot.HomeFactory.RemoveBot(bot);
		SimplePool.Despawn (bot.gameObject);	
	}

//----------------------------------------------------------
//	EndGame()
// Ensures the game ends correctly
//
// Param:
//		GameObject obj - the winning player
// Return:
//		Void
//----------------------------------------------------------
	private void EndGame(GameObject obj) {
		Transform winningPlayer = GetOpposingFactory(obj.GetComponent<Factory>().owningPlayer).owningPlayer.transform;
		obj.SetActive(false);
		CanvasController.Instance.ShowWinScreen(winningPlayer.name);
		Time.timeScale = 0f;
	}
}
