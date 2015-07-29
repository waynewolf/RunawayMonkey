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
			GameManager.Instance.Pause();
		}
		
		if (GameManager.Instance.Paused)
			return;	
		
		if (!GameManager.Instance.CanMove)
			return;
		
		if (CrossPlatformInputManager.GetButtonDown("Attack")) {
			_player.Attack();
		}

		if (CrossPlatformInputManager.GetButtonDown ("Jump")) {
			if (_player.IsRunning())
				_player.ShortJump();
			else if (_player.IsFloating())
				_player.LongJump();
			else if (_player.IsHanging())
				_player.JumpOff ();
		}
	}	
}

