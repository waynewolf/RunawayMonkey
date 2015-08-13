using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	// LevelManager is singleton, but it cannot survive from scene switch
	public static LevelManager Instance { get; private set; }

	public MonkeyBehaviour playerPrefab ;
	public HunterBehaviour hunterPrefab;
	public GameObject background;
	public GameObject foreground;
	public float catchMonkeyTime = 2f;

	[HideInInspector]
	public MonkeyBehaviour Player { get; private set; }
	[HideInInspector]
	public HunterBehaviour Hunter { get; private set; }
	
	private const float DISTANCE_TO_HUNTER = -7.5f;
	private const float INIT_HUNTER_Y_OFFSET = -0.7f;
	private const float HUNTER_HIT_BACKWARD_DISTANCE = 1.5f;
	private const float MAX_TIMEOUT_WHEN_HUNTER_OUT_OF_SCREEN = 2f;

	private float _halfScreenWidthInUnit;
	private float _currentSpeed;
	private int _bananaNumber = 0;
	private float _currentDistanceToHunter;
	private float _elapsedTimeWhenHunterOutOfScreen;

	void Awake() {
		Instance = this;
		Player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as MonkeyBehaviour;
		Hunter = Instantiate(hunterPrefab, new Vector3(DISTANCE_TO_HUNTER,
					INIT_HUNTER_Y_OFFSET, 0), Quaternion.identity) as HunterBehaviour;
		GameManager.Instance.Player = Player;
		_currentSpeed = GameManager.Instance.NormalSpeed;
		_currentDistanceToHunter = -DISTANCE_TO_HUNTER;
		_elapsedTimeWhenHunterOutOfScreen = 0f;
		_halfScreenWidthInUnit = 1.0f * Screen.width / Screen.height * Camera.main.orthographicSize;
	}
	
	void Update() {
		if (_currentSpeed > 0.01f) {
			Vector3 position = foreground.transform.position;
			position.x -= _currentSpeed * Time.deltaTime;
			foreground.transform.position = position;
		} else if (Hunter.IsCatching()) {
			// The monkey is blocked, hunter will catch the monkey in catchMonkeyTime seconds
			float advanceDistance = Mathf.Abs (DISTANCE_TO_HUNTER) * Time.deltaTime / catchMonkeyTime;
			_currentDistanceToHunter -= advanceDistance;
			Hunter.Forward(advanceDistance);
		}

		if (_currentDistanceToHunter > _halfScreenWidthInUnit) {
			_elapsedTimeWhenHunterOutOfScreen += Time.deltaTime;
		}

		if (_elapsedTimeWhenHunterOutOfScreen > MAX_TIMEOUT_WHEN_HUNTER_OUT_OF_SCREEN) {
			_elapsedTimeWhenHunterOutOfScreen = 0f;
			_currentDistanceToHunter = -DISTANCE_TO_HUNTER;
			Hunter.MoveToX(DISTANCE_TO_HUNTER);
		}
	}

	public void StopMoving() {
		_currentSpeed = 0;
	}

	public void ResumeMoving() {
		_currentSpeed = GameManager.Instance.NormalSpeed;
	}

	public void LevelComplete () {
		GameManager.Instance.Pause();
		GUIManager.Instance.SetLevelComplete(true);
	}

	public void RestartLevel () {
		Application.LoadLevel(Application.loadedLevel);
	}

	public void NextLevel () {
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	public void EatBanana () {
		_bananaNumber++;
		GUIManager.Instance.SetBananaNumber(_bananaNumber);
	}

	public void HunterHit() {
		if (_currentDistanceToHunter < 1.1f * _halfScreenWidthInUnit) {
			_currentDistanceToHunter += HUNTER_HIT_BACKWARD_DISTANCE;
			Hunter.Backward(HUNTER_HIT_BACKWARD_DISTANCE);
		}
	}

	public void ReviveScreen() {
		GUIManager.Instance.DisableButtons();
		GUIManager.Instance.SetRevive(true);
	}

	public void Revive() {
		GUIManager.Instance.SetRevive(false);
		GUIManager.Instance.EnableButtons();
		Player.transform.position = Vector3.zero;
		Player.Revived();
		Hunter.MoveToX(DISTANCE_TO_HUNTER);
		Hunter.MonkeyRunaway();
		_currentDistanceToHunter = -DISTANCE_TO_HUNTER;
	}
	
}
