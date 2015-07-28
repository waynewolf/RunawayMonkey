using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


public class InputManager : Singleton<InputManager> {
	private static MonkeyBehaviour _player;

	void Start() {
		if (GameManager.Instance.Player != null) {
			_player = GameManager.Instance.Player;
		}
	}

	void Update() {		
		if (_player == null) {
			if (GameManager.Instance.Player != null) {
				if (GameManager.Instance.Player.GetComponent<MonkeyBehaviour> () != null)
					_player = GameManager.Instance.Player;
			}
			else {
				return;
			}
		}

		if (CrossPlatformInputManager.GetButtonDown("Pause")) {
			Debug.Log ("Pause button down");
			GameManager.Instance.Pause();
		}
		
		if (GameManager.Instance.Paused)
			return;	
		
		if (!GameManager.Instance.CanMove)
			return;
		
		if (CrossPlatformInputManager.GetButtonDown("Attack")) {
			Debug.Log ("Attack button down");
			_player.Attack();
		}

		if (CrossPlatformInputManager.GetButtonDown ("Jump")) {
			Debug.Log ("Jump button down");
			if (_player.IsRunning())
				_player.JumpOn();
			else if (_player.IsHaning())
				_player.JumpOff();
		}
	}	
}

