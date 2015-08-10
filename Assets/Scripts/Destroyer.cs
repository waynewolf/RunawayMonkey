using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour
{
	public bool destroyOnAwake;
	public float awakeDestroyDelay;
	public bool findChild = false;
	public string namedChild;

	void Awake () {
		if(destroyOnAwake) {
			if(findChild){
				Destroy (transform.Find(namedChild).gameObject);
			} else {
				Destroy(gameObject, awakeDestroyDelay);
			}
		}
	}
	
	void DestroyChildGameObject () {
		if(transform.Find(namedChild).gameObject != null)
			Destroy (transform.Find(namedChild).gameObject);
	}
	
	void DisableChildGameObject () {
		if(transform.Find(namedChild).gameObject.activeSelf == true)
			transform.Find(namedChild).gameObject.SetActive(false);
	}

	// can be called by animation event
	void DestroyGameObject () {
		Destroy (gameObject);
	}
}
