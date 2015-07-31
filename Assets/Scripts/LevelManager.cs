using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	// LevelManager is singleton, but it cannot survive from scene switch
	public static LevelManager Instance { get; private set; }

	public MonkeyBehaviour PlayerPrefab ;
	public GameObject background;
	public GameObject foreground;

	private MonkeyBehaviour _player;
	private float _currentSpeed;
	private int _bananaNumber;

	void Awake() {
		Instance = this;
		_player = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as MonkeyBehaviour;
		GameManager.Instance.Player = _player;
		_currentSpeed = GameManager.Instance.NormalSpeed;
		_bananaNumber = 0;
	}

	void Update() {
		Vector3 position = foreground.transform.position;
		position.x -= _currentSpeed * Time.deltaTime;
		foreground.transform.position = position;
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
