using UnityEngine;

public class KillPlayer : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			GameManager.Instance.Pause();
			GUIManager.Instance.SetGameOver(true);
		}
	}

}