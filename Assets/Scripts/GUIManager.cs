using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {
	public GameObject pause;
	public GameObject gameOver;
	public GameObject levelComplete;
	public GameObject buttons;
	public GameObject revive;
	public Text bananaNumberText;
	public Text strawberryNumberText;
	public Text scoreText;

	private static GUIManager _instance;
	private bool modalDialogAlreadyPoppedUp;

	public static GUIManager Instance {
		get {
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<GUIManager>();
			return _instance;
		}
	}

	public void SetPause(bool state) {
		ToggleModalDialog(pause, state);
	}

	public void SetGameOver(bool state) {
		ToggleModalDialog(gameOver, state);
	}

	public void SetLevelComplete (bool state) {
		ToggleModalDialog(levelComplete, state);
	}

	public void SetBananaNumber(int number) {
		bananaNumberText.text = "x " + number.ToString();
	}

	public void SetStrawberryNumber (int number) {
		strawberryNumberText.text = "x " + number.ToString();
	}

	public void RefreshScore(int number) {
		scoreText.text = "Score: " + number.ToString();
	}

	public void DisableButtons() {
		buttons.SetActive(false);
	}

	public void EnableButtons() {
		buttons.SetActive(true);
	}

	public void SetRevive(bool state) {
		ToggleModalDialog(revive, state);
	}

	private void ToggleModalDialog(GameObject dialog, bool state) {
		if (!modalDialogAlreadyPoppedUp && state) {
			dialog.SetActive(true);
			modalDialogAlreadyPoppedUp = true;
		} else if (modalDialogAlreadyPoppedUp && !state && dialog.activeInHierarchy) {
			dialog.SetActive(false);
			modalDialogAlreadyPoppedUp = false;
		}
	}
}
