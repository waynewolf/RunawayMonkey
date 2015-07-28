using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	// LevelManager is singleton, but it cannot survive from scene switch
	public static LevelManager Instance { get; private set; }
	public MonkeyBehaviour PlayerPrefab ;

	private MonkeyBehaviour _player;

	public void Awake() {
		Instance = this;
		_player = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as MonkeyBehaviour;
		GameManager.Instance.Player = _player;
	}
}
