using UnityEngine;
using System.Collections;

public class MonkeyBehaviour : MonoBehaviour {

	public enum State {
		Running,
		Floating,
		WantHang,
		Hanging,
		Falling,
	};

	private Vector2 JumpForce = new Vector2(0f, 400f);

	private State _state;
	private Animator _animator;
	private Rigidbody2D _rigidbody2D;
	private BoxCollider2D _boxCollider2D;

	void Start () {
		_animator = GetComponent<Animator>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_boxCollider2D = GetComponent<BoxCollider2D>();
		_state = State.Falling;
	}
	
	void FixedUpdate () {
		_animator.SetFloat("VSpeed", _rigidbody2D.velocity.y);
	}

	void OnCollisionEnter2D(Collision2D other) {
		string otherTag = other.gameObject.tag;
		if (otherTag == "Platform")
			Ground();
		else if (otherTag == "Hook")
			Hang();
	}

	void OnCollisionExit2D(Collision2D other) {
		string otherTag = other.gameObject.tag;
		if (otherTag == "Platform")
			Fall();
		//else if (otherTag == "Hook")
		//	NoHang();
	}

	#region state queries
	public bool IsRunning() {
		return (_state == State.Running);
	}

	public bool IsFloating() {
		return (_state == State.Floating);
	}

	public bool IsWantHang() {
		return (_state == State.WantHang);
	}

	public bool IsHanging() {
		return (_state == State.Hanging);
	}

	public bool IsFalling() {
		return (_state == State.Falling);
	}
	
	#endregion

	#region state transitions
	public void ShortJump() {
		if (_state == State.Running) {
			_rigidbody2D.AddForce(JumpForce);
			_animator.SetBool("Ground", false);
			_state = State.Floating;
		}
	}

	public void LongJump() {
		if (_state == State.Floating) {
			_rigidbody2D.AddForce(JumpForce);
			_animator.SetTrigger("DoubleJump");
			_state = State.WantHang;
		}
	}
	
	private void Ground() {
		if (_state == State.Floating || _state == State.WantHang || _state == State.Falling) {
			_animator.SetBool("Ground", true);
			_state = State.Running;
		}
	}

	private void Hang() {
		// notify hang success only in WantHang state, to avoid 
		// continuously hang on the hook.
		if (_state == State.WantHang) {
			_animator.SetTrigger("HangSuccess");
			_rigidbody2D.isKinematic = true;
			_state = State.Hanging;
		}
	}

	public void JumpOff() {
		if (_state == State.Hanging) {
			ResetPhysics();
			_state = State.Falling;
		}
	}

	public void Fall() {
		if (_state == State.Running) {
			_animator.SetBool("Ground", false);
			_state = State.Falling;
		}
	}

	// called when nothing to hang in hanging state, OnCollision{Enter,Exit}2D notification
	// keeps coming when the rigidbody is moving, we cannot rely on this
//	public void NoHang() {
//		if (_state == State.Hanging) {
//			ResetPhysics();
//			_state = State.Falling;
//		}
//	}

	#endregion

	public void Attack() {
	}

	private void ResetPhysics() {
		_rigidbody2D.isKinematic = false;
		_boxCollider2D.enabled = true;
	}

}
