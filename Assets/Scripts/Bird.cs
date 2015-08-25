using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {

	private float _speed;
	private bool _waitMonkey = false;

	void Update() {
		if (!_waitMonkey) {
			Vector3 position = transform.position;
			position.x += _speed * Time.deltaTime;
			transform.position = position;
		}
	}

	public void SetSpeed(float speed) {
		_speed = speed;
	}

	public void WaitMonkeyForAWhile(float howLong) {
		_waitMonkey = true;
		StartCoroutine(DontWaitAnyMore(howLong));
	}

	private IEnumerator DontWaitAnyMore(float time) {
		yield return new WaitForSeconds(time);
		_waitMonkey = false;
		yield return null;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "ScreenRightEdge") {
			Destroy(gameObject);
		}
	}
}
