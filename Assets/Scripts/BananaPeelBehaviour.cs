using UnityEngine;
using System.Collections;

public class BananaPeelBehaviour : MonoBehaviour {

	private Coroutine throwCo;

	public void Throw(GameObject target) {
		if (throwCo != null) {
			StopCoroutine(throwCo);
		}
		throwCo = StartCoroutine (DoThrow(gameObject, target));
	}

	private IEnumerator DoThrow(GameObject start, GameObject target) {
		Vector2 initPos = start.transform.position;
		Vector2 targetPos = target.transform.position;
		targetPos.y += 1f;
		
		for (float t = 0f; t < 0.5f; t += 0.05f) {
			start.transform.position = Vector2.Lerp (initPos, targetPos, t / 0.5f);
			yield return new WaitForSeconds(0.01f);
		}
		
		yield return null;
	}

	public void DestroyMe() {
		if (throwCo != null) {
			StopCoroutine(throwCo);
		}
		Destroy(gameObject);
	}
}
