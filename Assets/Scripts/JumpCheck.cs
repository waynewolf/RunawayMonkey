using UnityEngine;
using System.Collections;

public class JumpCheck : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Hunter") {
			HunterBehaviour hunter = other.gameObject.GetComponent<HunterBehaviour>();
			hunter.Jump();
		}
	}

}
