using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	public Transform _target;
	public bool _followX = true;
	public bool _followY = false;

	public float _xOffset = 0f;
	public float _yOffset = 0f;

	private float _savedCameraX;
	private float _savedCameraY;
	private float _savedCameraZ;

	void Start() {
		_savedCameraX = transform.position.x;
		_savedCameraY = transform.position.y;
		_savedCameraZ = transform.position.z;
	}

	void Update () {
		if (!_target) {
			MonkeyBehaviour player = GameManager.Instance.Player;
			if (player) _target = player.gameObject.transform;
		}

		if (!_target) return;

		Vector3 position = _target.position;
		if (_followX)
			position.x += _xOffset;
		else
			position.x = _savedCameraX;

		if (_followY)
			position.y += _yOffset;
		else
			position.y = _savedCameraY;

		position.z = _savedCameraZ;
		transform.position = position;
	}
}
