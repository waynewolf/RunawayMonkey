using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager> {	
	public bool _musicOn = true;
	public bool _soundEffectOn = true;
	[Range(0, 1)]
	public float _musicVolume = 0.3f;
	[Range(0, 1)]
	public float _soundEffectVolume = 1f;
	
	private AudioSource _backgroundMusic;	

	public void PlayBackgroundMusic(AudioSource music) {
		if (!_musicOn)
			return;
		if (_backgroundMusic != null)
			_backgroundMusic.Stop();
		_backgroundMusic = music;
		_backgroundMusic.volume = _musicVolume;
		_backgroundMusic.loop = true;
		_backgroundMusic.Play();		
	}	

	public AudioSource PlaySound(AudioClip soundEffect, Vector3 position) {
		if (!_soundEffectOn)
			return null;
		GameObject temporaryAudioHost = new GameObject("TempAudio");
		temporaryAudioHost.transform.position = position;
		AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource; 
		audioSource.clip = soundEffect; 
		audioSource.volume = _soundEffectVolume;
		audioSource.Play(); 
		Destroy(temporaryAudioHost, soundEffect.length);
		// return the audiosource reference
		return audioSource;
	}
}

