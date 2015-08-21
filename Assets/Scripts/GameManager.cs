using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager manages points, time, persistence. Also a centralized place to
/// gain acccess to the Player.
/// </summary>
public class GameManager : Singleton<GameManager> {
	public float TimeScale { get; private set; }
	public bool Paused { get; set; } 
	public bool canMove = true;
	public MonkeyBehaviour Player { get; set; }
	public float NormalSpeed { get; private set; }
	public float FastSpeed { get; private set; }

	// storage
	private float _savedTimeScale;
	private int _score;
	protected override void Awake() {
		base.Awake();
		Instance.LoadValue();
	}
	
	public void LoadValue() {
		_score = PlayerPrefs.GetInt("score");
		TimeScale = 1f;
		Paused = false;
		canMove = true;
		NormalSpeed = 5f;
		FastSpeed = 10f;
		GUIManager.Instance.RefreshScore (_score);
	}

	public void AddPoints(int pointsToAdd) {
		_score += pointsToAdd;
		PlayerPrefs.SetInt("score", _score);
		GUIManager.Instance.RefreshScore (_score);
	}

	public void SetPoints(int points) {
		_score = points;
		PlayerPrefs.SetInt("score", _score);
		GUIManager.Instance.RefreshScore (_score);
	}
	
	public void SetTimeScale(float newTimeScale) {
		_savedTimeScale = Time.timeScale;
		Time.timeScale = newTimeScale;
	}

	public void ResetTimeScale() {
		Time.timeScale = _savedTimeScale;
	}
	
	public void Pause() {
		// if time is not already stopped		
		if (Time.timeScale > 0.0f) {
			Instance.SetTimeScale(0.0f);
			Instance.Paused = true;
		}
		else {
			Instance.ResetTimeScale();	
			Instance.Paused = false;
		}
	}

}
