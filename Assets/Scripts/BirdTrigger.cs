using UnityEngine;
using System.Collections;

/// <summary>
/// Bird trigger. After collided with ScreenRightEdge, bird will be spawn.
/// This game object will arrive at 0 together with bird
/// </summary>
public class BirdTrigger : MonoBehaviour {
	public GameObject _birdPrefab;
	public float _birdSpawnXPosition;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "ScreenRightEdge") {
			// Calculate bird speed, make sure when BirdTrigger is x: 0,
			// bird also arives at position x: 0
			float time = LevelManager.Instance.CalculateTime(other.gameObject.transform.position.x);
			float speed = Mathf.Abs (_birdSpawnXPosition) / time;
			GameObject birdObj = Instantiate(_birdPrefab,
			                                 new Vector3(_birdSpawnXPosition, transform.position.y, transform.position.z),
			                                 Quaternion.identity) as GameObject;
			birdObj.GetComponent<Bird>().SetSpeed(speed);
		} else if (other.gameObject.tag == "Bird") {
			other.gameObject.GetComponent<Bird>().WaitMonkeyForAWhile(3);
		}
	}


}
