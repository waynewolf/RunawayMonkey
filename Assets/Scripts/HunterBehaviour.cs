﻿using UnityEngine;
using System.Collections;

public class HunterBehaviour : MonoBehaviour {
	public GameObject attackedEffect;

	private Animator _animator;

	void Start() {
		_animator = GetComponent<Animator>();
	}

	public void Advance (float advanceDistance) {
		Vector3 hunterPos = transform.position;
		hunterPos.x += advanceDistance;
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
		}
	}

	// called by animation event
	void ExitHitByBanana() {
		_animator.SetBool("Hit", false);
	}
}
