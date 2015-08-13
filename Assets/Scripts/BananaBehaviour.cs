﻿using UnityEngine;
using System.Collections;

public class BananaBehaviour : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			LevelManager.Instance.EatBanana(transform);
		}
	}
	
}
