using UnityEngine;
using System.Collections;

public class MonkeyBehaviour : MonoBehaviour {

	public enum State {
		Running,
		Jumping,
		Hanging,
		Crouching,
	};

	private State _state;

	void Start () {
		_state = State.Running;
	}
	
	void Update () {
	
	}

	#region state queries
	public bool IsRunning() {
		return (_state == State.Running);
	}

	public bool IsJumping() {
		return (_state == State.Jumping);
	}

	public bool IsHaning() {
		return (_state == State.Hanging);
	}

	public bool IsCrouching() {
		return (_state == State.Hanging);
	}
	#endregion

	#region state transitions
	public void JumpOn() {
		if (_state == State.Running)
			_state = State.Jumping;
	}

	private void Grounded() {
		if (_state == State.Jumping)
			_state = State.Running;
	}

	public void Hang() {
		if (_state == State.Jumping)
			_state = State.Hanging;
	}

	public void JumpOff() {
		if (_state == State.Hanging)
			_state = State.Running;
	}

	public void Crouch() {
		if (_state == State.Running)
			_state = State.Crouching;
	}

	public void Stand() {
		if (_state == State.Crouching)
			_state = State.Running;
	}
	#endregion

	public void Attack() {
	}

}
