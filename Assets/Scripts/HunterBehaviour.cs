using UnityEngine;
using System.Collections;

public class HunterBehaviour : MonoBehaviour {
	public GameObject attackedEffect;
	public float jumpForce = 800f;

	private Animator _animator;
	private bool _catching = true;
	private Vector2 _jumpForce;

	void Awake() {
		_animator = GetComponent<Animator>();
		_jumpForce = new Vector2(0, jumpForce);
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
			Instantiate(attackedEffect, spawnPos, Quaternion.identity);
			_animator.SetBool("Hit", true);
			LevelManager.Instance.HunterHit();
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			_catching = false;
			LevelManager.Instance.ReviveScreen();
			_animator.SetBool("Catch", true);
			MonkeyBehaviour monkey = other.gameObject.GetComponent<MonkeyBehaviour>();
			monkey.MoveToHunter(transform);
			monkey.Caught();
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
		GetComponent<Rigidbody2D>().AddForce(_jumpForce);
	}

	public void MonkeyRunaway () {
		_animator.SetBool("Catch", false);
		_animator.SetTrigger("Runaway");
		_catching = true;
	}

}
