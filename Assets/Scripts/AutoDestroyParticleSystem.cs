using UnityEngine;
using System.Collections;
/// <summary>
/// Add this class to a ParticleSystem so it auto destroys once it has stopped emitting.
/// Make sure your ParticleSystem isn't looping, otherwise this script will be useless
/// </summary>
public class AutoDestroyParticleSystem : MonoBehaviour 
{
	public bool _destroyParent = false;
	
	private ParticleSystem _particleSystem;

	public void Start() {
		_particleSystem = GetComponent<ParticleSystem>();
	}

	public void Update() {	
		if (_particleSystem.isPlaying)
			return;
		
		if (transform.parent != null) {
			if(_destroyParent) {	
				Destroy(transform.parent.gameObject);	
			}
		}
				
		Destroy (gameObject);
	}

}
