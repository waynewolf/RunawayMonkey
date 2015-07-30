using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	public Transform target;
	public bool followX = true;
	public bool followY = false;

	public float xOffset = 0f;
	public float yOffset = 0f;

	private float _savedCameraX;
	private float _savedCameraY;
	private float _savedCameraZ;

	void Start() {
		_savedCameraX = transform.position.x;
		_savedCameraY = transform.position.y;
		_savedCameraZ = transform.position.z;
	}

	void Update () {
		if (!target) {
			MonkeyBehaviour player = GameManager.Instance.Player;
			if (player) target = player.gameObject.transform;
		}

		if (!target) return;

		Vector3 position = target.position;
		if (followX)
			position.x += xOffset;
		else
			position.x = _savedCameraX;

		if (followY)
			position.y += yOffset;
		else
			position.y = _savedCameraY;

		position.z = _savedCameraZ;
		transform.position = position;
	}
}
