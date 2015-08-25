using UnityEngine;
using System.Collections;

public class AutoJumpOffCheck : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			MonkeyBehaviour monkey = other.gameObject.GetComponent<MonkeyBehaviour>();
			monkey.NoHang();
		}
	}

}
