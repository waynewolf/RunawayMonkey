using UnityEngine;
using System.Collections;

public class HunterBehaviour : MonoBehaviour {
	public GameObject attackedEffect;

	private Animator _animator;
	private bool _catching = true;

	void Start() {
		_animator = GetComponent<Animator>();
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
			other.gameObject.GetComponent<BananaPeelBehaviour>().DestroyMe();
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
}
