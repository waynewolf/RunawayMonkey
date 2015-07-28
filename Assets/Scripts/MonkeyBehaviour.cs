using UnityEngine;
using System.Collections;

public class MonkeyBehaviour : MonoBehaviour {

	public enum State {
		Running,
		Jumping,
		Hanging,
	};

	private State _state;

	void Start () {
		_state = State.Running;
	}
	
	void Update () {
	
	}

	public bool IsRunning() {
		return (_state == State.Running);
	}

	public bool IsJumping() {
		return (_state == State.Jumping);
	}

	public bool IsHaning() {
		return (_state == State.Hanging);
	}

}
