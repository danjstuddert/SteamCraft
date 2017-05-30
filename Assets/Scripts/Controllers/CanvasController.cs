using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls the games canvas objects
public class CanvasController : Singleton<CanvasController> {
// winScreen is the games win screen
	public GameObject winScreen;

// winText is the text on the win screen
	private Text winText;

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
	}
}
