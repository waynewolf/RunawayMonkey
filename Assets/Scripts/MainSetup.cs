﻿using UnityEngine;
using System.Collections;

public class MainSetup : MonoBehaviour {
	public GameObject _monkey;
	public GameObject _hunter;
	public GameObject _levelItemContainer;

	void Start() {
		for(int i = 0; i < _levelItemContainer.transform.childCount; i++) {
			Transform childTransform = _levelItemContainer.transform.GetChild(i);
			GameObject lockImage = childTransform.Find("Mask/LockImage").gameObject;
			int currentLevel = i + 1;
			if (currentLevel <= GameManager.Instance.UnlockedLevel) {
				lockImage.SetActive(false);
			}
		}

		// We need this because now everything is paused by default
		_monkey.GetComponent<MonkeyBehaviour>().OnResume();
		_hunter.GetComponent<HunterBehaviour>().OnResume();
	}
	
}
