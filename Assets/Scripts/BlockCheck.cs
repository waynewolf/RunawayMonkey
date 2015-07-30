using UnityEngine;
using System.Collections;

public class BlockCheck : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			MonkeyBehaviour player = other.gameObject.GetComponent<MonkeyBehaviour>();
			player.Block();
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			MonkeyBehaviour player = other.gameObject.GetComponent<MonkeyBehaviour>();
			player.UnBlock();
		}
	}
}
