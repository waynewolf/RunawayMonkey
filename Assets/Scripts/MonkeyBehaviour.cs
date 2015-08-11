﻿using UnityEngine;
using System.Collections;

public class MonkeyBehaviour : MonoBehaviour {

	public enum State {
		Running,
		Floating,
		WantHang,
		Hanging,
		Falling,
	};

	public float jumpForce = 500f;
	public GameObject bananaPeelPrefab;

	private State _state;
	private Animator _animator;
	private Rigidbody2D _rigidbody2D;
	private BoxCollider2D _boxCollider2D;
	private Vector2 _jumpForce;
	private GameObject _hunter;

	void Start () {
		_hunter = LevelManager.Instance.Hunter.gameObject;
		_animator = GetComponent<Animator>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_boxCollider2D = GetComponent<BoxCollider2D>();
		_state = State.Falling;
		_jumpForce = new Vector2(0, jumpForce);
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
		else if (otherTag == "Banana") {
			Destroy(other.gameObject);
			LevelManager.Instance.EatBanana();
		}
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
			_rigidbody2D.AddForce(_jumpForce);
			_animator.SetBool("Ground", false);
			_state = State.Floating;
		}
	}

	public void LongJump() {
		if (_state == State.Floating) {
			_rigidbody2D.AddForce(_jumpForce);
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
		if (IsRunning()) {
			_animator.SetTrigger ("Attack");
			Vector3 bananaSpawnPos = transform.position;
			bananaSpawnPos.x -= 1;
			GameObject bananaPeel = Instantiate(bananaPeelPrefab, bananaSpawnPos, Quaternion.identity) as GameObject;
			bananaPeel.GetComponent<BananaPeelBehaviour>().Throw(_hunter);
		}
	}

	private void ResetPhysics() {
		_rigidbody2D.isKinematic = false;
		_boxCollider2D.enabled = true;
	}

	public void Block() {
		LevelManager.Instance.StopMoving();
	}

	public void UnBlock() {
		LevelManager.Instance.ResumeMoving();
	}
}
