using UnityEngine;
using System.Collections;

public class BombItem : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			gameObject.SetActive(false);
			// TODO: explosion animation
			LevelManager.Instance.OnMonkeyGoingToDie();
		}
	}

}
