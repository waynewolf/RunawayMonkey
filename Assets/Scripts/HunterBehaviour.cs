using UnityEngine;
using System.Collections;

public class HunterBehaviour : MonoBehaviour {
	public GameObject attackedEffect;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Banana") {
			Destroy(other.gameObject);
			Vector3 spawnPos = transform.position;
			spawnPos.y += 1f;
			spawnPos.z = 0;
			Instantiate(attackedEffect, spawnPos, Quaternion.identity);
		}
	}
	
}
