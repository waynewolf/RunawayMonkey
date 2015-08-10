using UnityEngine;
using System.Collections;

public class HunterBehaviour : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Banana") {
			Destroy(other.gameObject);
		}
	}
	
}
