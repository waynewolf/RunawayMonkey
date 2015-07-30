using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	// LevelManager is singleton, but it cannot survive from scene switch
	public static LevelManager Instance { get; private set; }

	public MonkeyBehaviour PlayerPrefab ;
	public GameObject background;
	public GameObject platforms;
	
	private MonkeyBehaviour _player;
	private float _currentSpeed;

	void Awake() {
		Instance = this;
		_player = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as MonkeyBehaviour;
		GameManager.Instance.Player = _player;
		_currentSpeed = GameManager.Instance.NormalSpeed;
	}

	void Update() {
		Vector3 position = platforms.transform.position;
		position.x -= _currentSpeed * Time.deltaTime;
		platforms.transform.position = position;
	}
}
