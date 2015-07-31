using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	public GameObject pause;
	public GameObject gameOver;
	public GameObject levelComplete;

	private static GUIManager _instance;

	public static GUIManager Instance {
		get {
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<GUIManager>();
			return _instance;
		}
	}

	public void SetPause(bool state) {
		pause.SetActive(state);
	}

	public void SetGameOver(bool state) {
		gameOver.SetActive(state);
	}

	public void SetLevelComplete (bool state) {
		levelComplete.SetActive(state);
	}
}
