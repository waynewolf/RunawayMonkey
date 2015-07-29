﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager manages points, time, persistence. Also a centralized place to
/// gain acccess to the Player.
/// </summary>
public class GameManager : Singleton<GameManager> {
	/// the current number of game points
	public int Points { get; private set; }
	/// the current time scale
	public float TimeScale { get; private set; }
	/// true if the game is currently paused
	public bool Paused { get; set; } 
	/// true if the player is not allowed to move (in a dialogue for example)
	public bool CanMove = true;
	/// the current player
	public MonkeyBehaviour Player { get; set; }
	
	// storage
	private float _savedTimeScale;
	
	/// <summary>
	/// this method resets the whole game manager
	/// </summary>
	public void Reset() {
		Points = 0;
		TimeScale = 1f;
		Paused = false;
		CanMove = false;
		//GUIManager.Instance.RefreshPoints ();
	}	
	
	/// <summary>
	/// Adds the points in parameters to the current game points.
	/// </summary>
	/// <param name="pointsToAdd">Points to add.</param>
	public void AddPoints(int pointsToAdd) {
		Points += pointsToAdd;
		//GUIManager.Instance.RefreshPoints ();
	}
	
	/// <summary>
	/// use this to set the current points to the one you pass as a parameter
	/// </summary>
	/// <param name="points">Points.</param>
	public void SetPoints(int points) {
		Points = points;
		//GUIManager.Instance.RefreshPoints ();
	}
	
	/// <summary>
	/// sets the timescale to the one in parameters
	/// </summary>
	/// <param name="newTimeScale">New time scale.</param>
	public void SetTimeScale(float newTimeScale) {
		_savedTimeScale = Time.timeScale;
		Time.timeScale = newTimeScale;
	}
	
	/// <summary>
	/// Resets the time scale to the last saved time scale.
	/// </summary>
	public void ResetTimeScale() {
		Time.timeScale = _savedTimeScale;
	}
	
	/// <summary>
	/// Pauses the game
	/// </summary>
	public void Pause() {
		// if time is not already stopped		
		if (Time.timeScale > 0.0f) {
			Instance.SetTimeScale(0.0f);
			Instance.Paused = true;
			GUIManager.Instance.SetPause(true);
		}
		else {
			Instance.ResetTimeScale();	
			Instance.Paused = false;
			GUIManager.Instance.SetPause(false);
		}
	}
	
	/// <summary>
	/// Freezes the character.
	/// </summary>
	public void FreezeCharacter() {
		//Player.SetHorizontalMove(0);
		//Player.SetVerticalMove(0);
		//Instance.CanMove = false;
	}

}
