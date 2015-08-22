using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {
	public GameObject _pause;
	public GameObject _levelComplete;
	public GameObject _buttons;
	public GameObject _revive;
	public GameObject _countDown;
	public Text _bananaNumberText;
	public Text _strawberryNumberText;
	public Text _scoreText;

	private static GUIManager _instance;
	private bool _modalDialogAlreadyPoppedUp;

	public static GUIManager Instance {
		get {
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<GUIManager>();
			return _instance;
		}
	}

	public void SetPause(bool state) {
		ToggleModalDialog(_pause, state);
	}

	public void SetLevelComplete (bool state) {
		ToggleModalDialog(_levelComplete, state);
	}

	public void SetBananaNumber(int number) {
		_bananaNumberText.text = "x " + number.ToString();
	}

	public void SetStrawberryNumber (int number) {
		_strawberryNumberText.text = "x " + number.ToString();
	}

	public void RefreshScore(int number) {
		_scoreText.text = "Score: " + number.ToString();
	}

	public void DisableButtons() {
		_buttons.SetActive(false);
	}

	public void EnableButtons() {
		_buttons.SetActive(true);
	}

	public void SetRevive(bool state) {
		ToggleModalDialog(_revive, state);
	}

	public void SetCountDown(bool state) {
		ToggleModalDialog(_countDown, state);
	}

	private void ToggleModalDialog(GameObject dialog, bool state) {
		if (!_modalDialogAlreadyPoppedUp && state) {
			dialog.SetActive(true);
			_modalDialogAlreadyPoppedUp = true;
		} else if (_modalDialogAlreadyPoppedUp && !state && dialog.activeInHierarchy) {
			dialog.SetActive(false);
			_modalDialogAlreadyPoppedUp = false;
		}
	}
}
