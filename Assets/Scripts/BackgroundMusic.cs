using UnityEngine;
using System.Collections;

public class BackgroundMusic : Singleton<BackgroundMusic> {
	public AudioClip _soundClip ;
	private AudioSource _audioSource;

	void Start () {
		_audioSource = gameObject.AddComponent<AudioSource>() as AudioSource;	
		_audioSource.playOnAwake = false;
		_audioSource.spatialBlend = 0;
		_audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
		_audioSource.loop = true;	
		
		_audioSource.clip = _soundClip;
		
		SoundManager.Instance.PlayBackgroundMusic(_audioSource);
	}
}
