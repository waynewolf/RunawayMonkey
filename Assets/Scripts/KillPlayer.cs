using UnityEngine;

public class KillPlayer : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			LevelManager.Instance.Player.Dead ();
			GUIManager.Instance.SetRevive(true);
			GameManager.Instance.PauseGame();
		}
	}

}