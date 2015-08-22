﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

	void OnEnable() {
		Debug.Log ("OnEnable");
		StartCoroutine(DoCountDown(GetComponent<Text>(), 3));
	}

	IEnumerator DoCountDown(Text text, int seconds) {
		if (text == null) {
			Debug.LogError("DoCountDown: null Text reference");
			yield return null;
		}

		for (int i = seconds; i >= 0; i--) {
			if ( i == 0) {
				text.text = "GO";
			}
			else {
				text.text = i.ToString();
			}
			yield return new WaitForSeconds(1f);
		}

		gameObject.SetActive(false);
		yield return null;
	}

	void OnDisable() {
		Debug.Log ("OnDisable");
	}
}
