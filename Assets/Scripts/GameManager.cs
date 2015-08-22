using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager manages points, time, persistence, settings. Also a centralized place to
/// gain acccess to the Player.
/// </summary>
public class GameManager : Singleton<GameManager> {
	public float TimeScale { get; private set; }
	public bool Paused { get; set; } 
	public MonkeyBehaviour Player { get; set; }
	public float NormalSpeed { get; private set; }
	public float FastSpeed { get; private set; }
	public int PixelsPerUnit { get; private set; }
	public int Score { get; private set; }
	// 1 <= level <= UnlockedLevel are unlocked, 0 is reserved for Main scene
	public int UnlockedLevel { get; private set; }

	// storage
	private float _savedTimeScale;

	protected override void Awake() {
		base.Awake();
		Instance.LoadValue();
	}
	
	public void LoadValue() {
		Score = PlayerPrefs.GetInt("score");
		UnlockedLevel = PlayerPrefs.GetInt ("unlockedLevel");
		if (UnlockedLevel == 0) {
			UnlockedLevel = 1;
			PlayerPrefs.SetInt("unlockedLevel", UnlockedLevel);
		}
		TimeScale = 1f;
		Paused = false;
		NormalSpeed = 5f;
		FastSpeed = 10f;
		PixelsPerUnit = 100;
	}

	public void AddPoints(int pointsToAdd) {
		Score += pointsToAdd;
		PlayerPrefs.SetInt("score", Score);
	}

	public void SetPoints(int points) {
		Score = points;
		PlayerPrefs.SetInt("score", Score);
	}
	
	public void SetTimeScale(float newTimeScale) {
		_savedTimeScale = Time.timeScale;
		Time.timeScale = newTimeScale;
	}

	public void ResetTimeScale() {
		Time.timeScale = _savedTimeScale;
	}
	
	public void PauseGame() {
		// if time is not already stopped		
		if (!Paused) {
			SetTimeScale(0.0f);
			Paused = true;
		}
	}

	public void ResumeGame() {
		if (Paused) {
			ResetTimeScale();	
			Paused = false;
		}
	}

	public void LoadLevel (int level) {
		Application.LoadLevel (level);
	}
}
