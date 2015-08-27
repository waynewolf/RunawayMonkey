using UnityEngine;
using System.Collections;

public class StrawberryItem : MonoBehaviour {
	public GameObject _starEffectPrefab;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			LevelManager.Instance.EatStrawberry(transform);
			Instantiate(_starEffectPrefab, transform.position, Quaternion.identity);
		}
	}
}
