using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Controls the games canvas objects
public class CanvasController : Singleton<CanvasController> {
// winScreen is the games win screen
	public GameObject winScreen;
// restartButton is the button that restarts the level
	public Button restartButton;

// winText is the text on the win screen
	private Text winText;
// eventSystem is the scenes event system object
	private EventSystem eventSystem;
	

//----------------------------------------------------------
//	Init()
// Correctly handles the setting up of the canvas
//
// Param:
//		None
// Return:
//		Void
//----------------------------------------------------------
	public void Init() {
		if (winScreen.activeInHierarchy) {
			winScreen.SetActive(false);
		}

		winText = winScreen.transform.GetChild(0).GetComponent<Text>();
		eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	}

//----------------------------------------------------------
//	ShowWinScreen()
// Shows the win screen updating it to display the winning players name
//
// Params:
//		string winningPlayer - the name of the winning player
// Return:
//		Void
//----------------------------------------------------------
	public void ShowWinScreen(string winningPlayer) {
		if(winScreen.activeInHierarchy == false) {
			winScreen.SetActive(true);
		}

		winText.text = string.Format("{0} wins!", winningPlayer);

		StartCoroutine(SelectRestart());
	}

//----------------------------------------------------------
//	SelectRestart()
// Makes sure that the restart button is selected by reselecting it
//
// Params:
//		none
// Return:
//		WaitForEndOfFrame
//----------------------------------------------------------
	private IEnumerator SelectRestart() {
		eventSystem.SetSelectedGameObject(null);
		yield return new WaitForEndOfFrame();

		eventSystem.SetSelectedGameObject(restartButton.gameObject);
	}
}
