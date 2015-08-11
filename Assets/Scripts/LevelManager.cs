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

	private float _currentSpeed;
	private int _bananaNumber = 0;
	private const float _distanceToHunter = -7.5f;
	private const float _initHunterYOffset = -0.7f;

	void Awake() {
		Instance = this;
		Player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as MonkeyBehaviour;
		Hunter = Instantiate(hunterPrefab, new Vector3(_distanceToHunter,
					_initHunterYOffset, 0), Quaternion.identity) as HunterBehaviour;
		GameManager.Instance.Player = Player;
		_currentSpeed = GameManager.Instance.NormalSpeed;
	}

	void Update() {
		if (_currentSpeed > 0.01f) {
			Vector3 position = foreground.transform.position;
			position.x -= _currentSpeed * Time.deltaTime;
			foreground.transform.position = position;
		} else {
			// The monkey is blocked, hunter will catch the monkey in catchMonkeyTime seconds
			float advanceDistance = Mathf.Abs (_distanceToHunter) * Time.deltaTime / catchMonkeyTime;
			Hunter.Advance(advanceDistance);
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
}
