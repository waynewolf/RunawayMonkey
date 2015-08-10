using UnityEngine;
using System.Collections;

public class HunterBehaviour : MonoBehaviour {
	public GameObject attackedEffect;

	private Animator _animator;

	void Start() {
		_animator = GetComponent<Animator>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Banana") {
			Destroy(other.gameObject);
			Vector3 spawnPos = transform.position;
			spawnPos.y += 1f;
			spawnPos.z = 0;
			Instantiate(attackedEffect, spawnPos, Quaternion.identity);
			_animator.SetBool("Hit", true);
		}
	}

	// called by animation event
	void ExitHitByBanana() {
		_animator.SetBool("Hit", false);
	}
}
