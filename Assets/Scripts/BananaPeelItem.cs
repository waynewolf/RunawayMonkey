using UnityEngine;
using System.Collections;

public class BananaPeelItem : MonoBehaviour {

	private Coroutine _throwCoroutine;

	public void Throw(GameObject target) {
		if (_throwCoroutine != null) {
			StopCoroutine(_throwCoroutine);
		}
		_throwCoroutine = StartCoroutine (DoThrow(gameObject, target));
	}

	private IEnumerator DoThrow(GameObject start, GameObject target) {
		Vector2 initPos = start.transform.position;

		for (float t = 0f; t < 0.5f; t += 0.05f) {
			Vector2 targetPos = target.transform.position;
			targetPos.y += 1f;
			start.transform.position = Vector2.Lerp (initPos, targetPos, t / 0.5f);
			yield return new WaitForSeconds(0.01f);
		}
		
		yield return null;
	}

	public void DestroyMe() {
		if (_throwCoroutine != null) {
			StopCoroutine(_throwCoroutine);
		}
		Destroy(gameObject);
	}
}
