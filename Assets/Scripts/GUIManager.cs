using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	public GameObject PauseMenu;

	private static GUIManager _instance;

	public static GUIManager Instance {
		get {
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<GUIManager>();
			return _instance;
		}
	}

	public void SetPause(bool state) {
		PauseMenu.SetActive(state);
	}

}
