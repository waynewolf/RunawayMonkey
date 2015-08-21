using UnityEngine;
using System.Collections;

public class StrawberryItem : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			LevelManager.Instance.EatStrawberry(transform);
		}
	}
}
