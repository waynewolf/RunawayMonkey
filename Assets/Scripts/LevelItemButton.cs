using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelItemButton : MonoBehaviour {

	public void ItemClicked(int level) {
		if (level <= GameManager.Instance.UnlockedLevel) {
			LevelManager.LoadLevel(level);
		}
	}
}
