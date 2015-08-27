using UnityEngine;
using System.Collections;

public class BananaItem : MonoBehaviour {
	public GameObject _starEffectPrefab;
	public AudioClip _itemCollectedSoundEffect;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			LevelManager.Instance.EatBanana(transform);
			Instantiate(_starEffectPrefab, transform.position, Quaternion.identity);
			SoundManager.Instance.PlaySound(_itemCollectedSoundEffect, transform.position);
		}
	}
	
}
