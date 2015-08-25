using UnityEngine;
using System.Collections;

public class HunterBehaviour : MonoBehaviour, IPauseable {
	public GameObject _attackedEffect;
	public float _jumpForce = 800f;

	private Animator _animator;
	private bool _catching = true;
	private Vector2 _jumpForceVector;
	private Rigidbody2D _rigidbody2D;
	private BoxCollider2D _boxCollider2D;
	private float _relativeSpeed;
	private bool _paused;

	void Awake() {
		_animator = GetComponent<Animator>();
		_jumpForceVector = new Vector2(0, _jumpForce);
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_boxCollider2D = GetComponent<BoxCollider2D>();
		_paused = true;
	}

	void Start() {
		OnPause ();
	}
	
	void Update() {
		if (!_paused) {
			Vector3 position = transform.position;
			position.x += _relativeSpeed * Time.deltaTime;
			transform.position = position;
		}
	}

	public void SetRelativeSpeed(float speed) {
		_relativeSpeed = speed;
	}

	public void Forward (float distance) {
		Vector3 hunterPos = transform.position;
		hunterPos.x += distance;
		transform.position = hunterPos;
	}

	public void Backward(float distance) {
		Forward (-distance);
	}

	public void MoveToX(float xpos) {
		Vector3 hunterPos = transform.position;
		hunterPos.x = xpos;
		transform.position = hunterPos;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "BananaPeel") {
			other.gameObject.GetComponent<BananaPeelItem>().DestroyMe();
			Vector3 spawnPos = transform.position;
			spawnPos.y += 1f;
			spawnPos.z = 0;
			Instantiate(_attackedEffect, spawnPos, Quaternion.identity);
			_animator.SetBool("Hit", true);
			LevelManager.Instance.HunterHit();
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			_catching = false;
			_animator.SetBool("Catch", true);
			LevelManager.Instance.OnMonkeyCaught();
		}
	}

	// called by animation event
	void ExitHitByBanana() {
		_animator.SetBool("Hit", false);
	}

	public bool IsCatching() {
		return _catching;
	}

	public void Jump () {
		GetComponent<Rigidbody2D>().AddForce(_jumpForceVector);
	}

	public void MonkeyRunaway () {
		_animator.SetBool("Catch", false);
		_animator.SetTrigger("Runaway");
		_catching = true;
	}

	public void StopCatching () {
		_catching = false;
	}

	public void ResumeCatching() {
		_catching = true;
	}

	public void EnablePhysics() {
		_rigidbody2D.isKinematic = false;
		_boxCollider2D.enabled = true;
	}
	
	public void DisablePhysics() {
		_rigidbody2D.isKinematic = true;
		_boxCollider2D.enabled = false;
	}

	public void PauseAnimation () {
		_animator.enabled =  false;
	}
	
	public void ResumeAnimation() {
		_animator.enabled = true;
	}

	public void OnPause() {
		DisablePhysics();
		PauseAnimation();
		StopCatching();
		_paused = true;
	}
	
	public void OnResume() {
		ResumeAnimation();
		EnablePhysics();
		ResumeCatching();
		_paused = false;
	}
}
