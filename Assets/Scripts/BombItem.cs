using UnityEngine;
using System.Collections;

public class BombItem : MonoBehaviour {
	public GameObject _bombExplosionEffectPrefab;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			gameObject.SetActive(false);
			Instantiate(_bombExplosionEffectPrefab, transform.position, Quaternion.identity);
			LevelManager.Instance.OnMonkeyGoingToDie();
		}
	}

}
