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

		// CrossPlatformI.M must make sure button down and up come in pair,
		// otherwise you lose next button down, if you're going to pause,
		// resume, restart, or load another scene, please follow this rule
		// that is known to be working:
		// 1. Use Event Trigger script, not Button script
		// 2. Set both PointerDown and PointerUp event type in event trigger script
		// 3. Call CrossPlatformInputManager.GetButtonUp, not GetButtonDown.
		if (CrossPlatformInputManager.GetButtonUp ("MainScreen")) {
			GameManager.Instance.ResumeGame();
			GameManager.Instance.LoadLevel(0);
		}
		
		if (CrossPlatformInputManager.GetButtonUp("Restart")) {
			GameManager.Instance.ResumeGame ();
			GUIManager.Instance.SetPause(false);
			LevelManager.Instance.RestartLevel();
		}
		
		if (CrossPlatformInputManager.GetButtonUp("Resume")) {
			GameManager.Instance.ResumeGame ();
			GUIManager.Instance.SetPause(false);
			LevelManager.Instance.FreezeCharacters();
			GUIManager.Instance.SetCountDown(true);
		}

		if (CrossPlatformInputManager.GetButtonUp("NextLevel")) {
			GameManager.Instance.ResumeGame();
			LevelManager.Instance.NextLevel();
		}

		if (CrossPlatformInputManager.GetButtonUp("Revive")) {
			GameManager.Instance.ResumeGame ();
			bool canRevive = LevelManager.Instance.Revive();
			if (canRevive) {
				LevelManager.Instance.FreezeCharacters();
				GUIManager.Instance.SetCountDown(true);
			} else {
				LevelManager.Instance.RestartLevel();
			}
		}

		if (GameManager.Instance.Paused)
			return;	

		if (CrossPlatformInputManager.GetButtonUp("Pause")) {
			GUIManager.Instance.SetPause(true);
			GameManager.Instance.PauseGame();
		}

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

