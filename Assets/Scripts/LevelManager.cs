using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BackgroundSlot {
	public Transform _layer = null;
	public float _speedFactor = 0.5f;
	[HideInInspector]
	public Vector3 _initPosition;
};

public class LevelManager : MonoBehaviour {

	private const float OFFSET_TO_HUNTER = -7.5f;
	private const float INIT_HUNTER_Y_OFFSET = -0.7f;
	private const float HUNTER_HIT_BACKWARD_DISTANCE = 1.5f;
	private const float MAX_TIMEOUT_WHEN_HUNTER_OUT_OF_SCREEN = 2f;
	private const float BACKGROUND_SPEED_FACTOR = 0.5f;
	private const int BANANA_POINTS = 2;
	private const int STRAWBERRY_POINTS = 1;

	// LevelManager is singleton, but it cannot survive from scene switch
	public static LevelManager Instance { get; private set; }

	public MonkeyBehaviour _playerPrefab ;
	public HunterBehaviour _hunterPrefab;
	public GameObject _shadowPrefab;
	public GameObject _foreground;
	public List<BackgroundSlot> _backgroundSlot;
	public List<GameObject> _recreateObjects;
	public Transform _hudItemsPlaceHolder;
	public float _catchMonkeyTime = 2f;
	public int _backgroundLayerWidth;
	public float _hunterSpeed;
	public AudioClip _buttonClickedSoundEffect;

	[HideInInspector]
	public MonkeyBehaviour Player { get; private set; }
	[HideInInspector]
	public HunterBehaviour Hunter { get; private set; }
	[HideInInspector]
	public int BananaNumber { get; set; }
	[HideInInspector]
	public int StrawberryNumber { get; set; }

	private float _halfScreenWidthInUnit;
	private float _currentMonkeySpeed;
	private float _elapsedTimeWhenHunterOutOfScreen;
	private GameObject _shadow;
	private Vector3 _shadowScale = Vector3.one;
	private Vector3 _bgStartPos;
	private float _bgLayerWidthInUnit;
	private List<IPauseable> _pauseables;
	private bool _paused;

	void Awake() {
		BananaNumber = 0;
		StrawberryNumber = 0;
		Instance = this;
		Player = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity) as MonkeyBehaviour;
		GameManager.Instance.Player = Player;

		_currentMonkeySpeed = GameManager.Instance.NormalSpeed;
		Hunter = Instantiate(_hunterPrefab, new Vector3(OFFSET_TO_HUNTER,
					INIT_HUNTER_Y_OFFSET, 0), Quaternion.identity) as HunterBehaviour;
		Hunter.SetRelativeSpeed(_hunterSpeed - _currentMonkeySpeed);

		_shadow = Instantiate(_shadowPrefab, new Vector3(0, -3f, 5f), Quaternion.identity) as GameObject;

		_elapsedTimeWhenHunterOutOfScreen = 0f;
		_halfScreenWidthInUnit = 1.0f * Screen.width / Screen.height * Camera.main.orthographicSize;
		_bgLayerWidthInUnit = 1.0f * _backgroundLayerWidth / GameManager.Instance.PixelsPerUnit;
		foreach(BackgroundSlot backgroundSlot in _backgroundSlot) {
			if (backgroundSlot != null) {
				backgroundSlot._initPosition = backgroundSlot._layer.position;
			}
		}

		_pauseables = new List<IPauseable>();
		_pauseables.Add(Player);
		_pauseables.Add(Hunter);
	}

	void Start() {
		GUIManager.Instance.RefreshScore(GameManager.Instance.Score);

		// make sure everything is paused
		foreach(IPauseable pause in _pauseables) {
			pause.OnPause();
		}
		_paused = true;

		OnShowCountDownDialog();
	}

	void FixedUpdate() {
		if (_paused) return;

		RaycastHit2D hit = Physics2D.Raycast(Player.transform.position, Vector2.down, Mathf.Infinity,
		                                     1 << LayerMask.NameToLayer("Platform"));
		if (hit && hit.transform.gameObject.tag == "Platform") {
			_shadow.transform.position = new Vector3(hit.point.x, hit.point.y, _shadow.transform.position.z);
			_shadow.SetActive(true);
		} else {
			_shadow.SetActive(false);
		}
	}

	void Update() {
		if (_paused) return;

		if (_currentMonkeySpeed > 0.01f) {
			// move the foreground
			Vector3 position = _foreground.transform.position;
			position.x -= _currentMonkeySpeed * Time.deltaTime;
			_foreground.transform.position = position;

			// move the backgrounds with parallax effect. support maximum 4 backgroud layers.
			for (int i = 0; i < _backgroundSlot.Count; i++) {
				float newPosition = Mathf.Repeat(_currentMonkeySpeed * Time.time * _backgroundSlot[i]._speedFactor, _bgLayerWidthInUnit);
				_backgroundSlot[i]._layer.position = _backgroundSlot[i]._initPosition + Vector3.left * newPosition;
			}
		}

		Hunter.SetRelativeSpeed(_hunterSpeed - _currentMonkeySpeed);

		// calculate hunter out of screen time only when monkey in normal speed or blocked,
		// to make sure hunter will never reappear on screen when the monkey is flying with bird.
		if (MonkeyHunterDistance() > _halfScreenWidthInUnit
		    && _currentMonkeySpeed <= GameManager.Instance.NormalSpeed) {
			_elapsedTimeWhenHunterOutOfScreen += Time.deltaTime;
		}

		if (_elapsedTimeWhenHunterOutOfScreen > MAX_TIMEOUT_WHEN_HUNTER_OUT_OF_SCREEN) {
			_elapsedTimeWhenHunterOutOfScreen = 0f;
			Hunter.MoveToX(OFFSET_TO_HUNTER);
		}

		if (_shadow.activeInHierarchy) {
			_shadowScale.x = 1f - (Player.transform.position.y - (-3f)) / 10f;
			_shadowScale.y = 1f - (Player.transform.position.y - (-3f)) / 10f;
			_shadow.transform.localScale = _shadowScale;
		}
	}

	private float MonkeyHunterDistance() {
		return Player.transform.position.x - Hunter.transform.position.x;
	}

	// stop the scene scrolling
	public void StopSceneScrolling() {
		_currentMonkeySpeed = 0;
	}

	public void ResumeSceneScrolling() {
		_currentMonkeySpeed = GameManager.Instance.NormalSpeed;
	}

	public void FastSceneScrolling() {
		_currentMonkeySpeed = GameManager.Instance.FastSpeed;
	}

	public void EatBanana (Transform transform) {
		StartCoroutine(ItemSmoothMovement(transform, _hudItemsPlaceHolder));
		BananaNumber++;
		GUIManager.Instance.SetBananaNumber(BananaNumber);
		GameManager.Instance.AddScore(BANANA_POINTS);
		GUIManager.Instance.RefreshScore(GameManager.Instance.Score);
	}

	public void EatStrawberry (Transform transform) {
		StartCoroutine(ItemSmoothMovement(transform, _hudItemsPlaceHolder));
		StrawberryNumber++;
		GUIManager.Instance.SetStrawberryNumber(StrawberryNumber);
		GameManager.Instance.AddScore(STRAWBERRY_POINTS);
		GUIManager.Instance.RefreshScore(GameManager.Instance.Score);
	}

	public void AttackWithBananaPeel() {
		BananaNumber--;
		GUIManager.Instance.SetBananaNumber(BananaNumber);
	}

	private IEnumerator ItemSmoothMovement(Transform start, Transform end) {
		Vector2 initPos = start.position;
		Vector2 targetPos = end.position;
		targetPos.y += 2f;
		targetPos.x -= 1.5f;
		
		for (float t = 0f; t < 1f; t += 0.05f) {
			if (start == null)
				yield break;
			start.position = Vector2.Lerp (initPos, targetPos, t / 1f);
			yield return new WaitForSeconds(0.01f);
		}

		Destroy(start.gameObject);
		yield return null;
	}

	public void HunterHit() {
		if (MonkeyHunterDistance() < 1.1f * _halfScreenWidthInUnit) {
			Hunter.Backward(HUNTER_HIT_BACKWARD_DISTANCE);
		}
	}

	public float CalculateTime (float distance) {
		if (_currentMonkeySpeed > 0.01f) {
			return distance / _currentMonkeySpeed;
		} else {
			return Mathf.Infinity;
		}
	}

	public float CurrentSpeed () {
		return _currentMonkeySpeed;
	}

	public float ScrollSpeed() {
		return CurrentSpeed();
	}

	void RecreateObjects () {
		foreach (GameObject obj in _recreateObjects) {
			if (!obj.activeInHierarchy)
				obj.SetActive(true);
		}
	}

	#region event handling code from different sources

	public void OnShowPauseDialog () {
		SoundManager.Instance.PlaySound(_buttonClickedSoundEffect, transform.position);
		foreach(IPauseable pause in _pauseables) {
			pause.OnPause();
		}
		_paused = true;

		GUIManager.Instance.SetPause(true);
	}

	public void OnMonkeyCaught() {
		foreach(IPauseable pause in _pauseables) {
			pause.OnPause();
		}
		_paused = true;

		// In this exceptional case, hunter and monkey animation is enabled
		Player.ResumeAnimation();
		Player.MoveToHunter(Hunter.transform);
		Player.Caught();
		Hunter.ResumeAnimation();

		GUIManager.Instance.DisableButtons();
		GUIManager.Instance.SetRevive(true);
	}
	
	public void OnMonkeyGoingToDie () {
		foreach(IPauseable pause in _pauseables) {
			pause.OnPause();
		}
		_paused = true;
		
		Player.Dead ();

		GUIManager.Instance.DisableButtons();
		GUIManager.Instance.SetRevive(true);
	}

	public void OnShowLevelCompleteDialog () {
		foreach(IPauseable pause in _pauseables) {
			pause.OnPause();
		}
		_paused = true;

		GUIManager.Instance.DisableButtons();
		GUIManager.Instance.DisableHUD ();
		GUIManager.Instance.SetLevelComplete(true);
	}

	public void OnShowCountDownDialog() {
		// don't need to call OnPause on each IPauseable because
		// we make sure everything is paused before entering this method.
		Debug.Assert(_paused);

		GUIManager.Instance.SetCountDown(true);
	}

	public void OnMainScreenButtonClicked () {
		SoundManager.Instance.PlaySound(_buttonClickedSoundEffect, transform.position);

		// Only pause dialog and level complete dialog have main screen button,
		// So we disable them all, no matter whichever is currently shown on screen.
		GUIManager.Instance.SetPause(false);
		GUIManager.Instance.SetLevelComplete(false);

		// don't need to resume because we're going to restart the level
		//	foreach(IPauseable pause in _pauseables) {
		//		pause.OnResume();
		//	}
		//	_paused = false;

		LoadLevel(0);
	}
	
	public void OnRestartButtonClicked () {
		SoundManager.Instance.PlaySound(_buttonClickedSoundEffect, transform.position);

		// Disable all the possible dialoug that is currently shown on screen.
		GUIManager.Instance.SetPause(false);
		GUIManager.Instance.SetRevive(false);
		GUIManager.Instance.SetLevelComplete(false);

		RestartLevel();
	}
	
	public void OnResumeButtonClicked () {
		SoundManager.Instance.PlaySound(_buttonClickedSoundEffect, transform.position);

		// Don't need to call OnResume on the Pauseables,
		// OnResume will be called after Count Down Timer expiers
		GUIManager.Instance.SetPause(false);
		GUIManager.Instance.SetCountDown(true);
	}

	public void OnNextLevelButtonClicked () {
		SoundManager.Instance.PlaySound(_buttonClickedSoundEffect, transform.position);

		GUIManager.Instance.SetLevelComplete(false);
		NextLevel();
	}

	public void OnReviveButtonClicked () {
		SoundManager.Instance.PlaySound(_buttonClickedSoundEffect, transform.position);

		GUIManager.Instance.SetRevive(false);
		GUIManager.Instance.EnableButtons();

		if (GameManager.Instance.Score < 100) {
			RestartLevel();
			return;
		}

		GameManager.Instance.SubtractScore(100);
		GUIManager.Instance.RefreshScore(GameManager.Instance.Score);
		Hunter.MoveToX(OFFSET_TO_HUNTER);
		Hunter.MonkeyRunaway();
		RecreateObjects();

		// Find a safe place for monkey to stand
		Vector2 castOrigin = new Vector3(0f, 2.5f, 0f);
		bool foundAPlaceToStand = false;
		float playerYPos = 0;
		for (float x = 0; x > - 2f * _halfScreenWidthInUnit; x -= 0.5f) {
			castOrigin.x = x;
			RaycastHit2D hit = Physics2D.Raycast(castOrigin,
			                                     Vector2.down, 50f,
			                                     1 << LayerMask.NameToLayer("Platform"));
			if (hit && hit.transform.gameObject.tag == "Platform") {
				// rewind to the head of the hit platform
				float newXPos = hit.transform.position.x - 0.5f * hit.transform.GetComponent<SpriteRenderer>().bounds.size.x;
				Vector3 newPos = _foreground.transform.position;
				newPos.x += -newXPos;
				_foreground.transform.position = newPos;
				// FIXME: put the player just on the platform, we have to
				// minus a hardcoded offset, which is relevant to platform size
				playerYPos = hit.transform.position.y + 0.5f * Player.GetComponent<SpriteRenderer>().bounds.size.y
					+ 0.5f * hit.transform.GetComponent<SpriteRenderer>().bounds.size.y - 0.3f;
				foundAPlaceToStand = true;
				break;
			}
		}
		
		if (!foundAPlaceToStand) {
			Debug.LogError("No place to stand, need to redesign the level"); 
		} else {
			Player.transform.position = new Vector3(0, playerYPos, 0);
			Player.Revived();
			GUIManager.Instance.SetCountDown(true);
		}
	}
	
	public void OnCountDownExpired () {
		GUIManager.Instance.SetCountDown(false);

		// Finally resume here !
		foreach(IPauseable pauseable in _pauseables) {
			pauseable.OnResume();
		}
		_paused = false;
		ResumeSceneScrolling();
	}

	#endregion

	#region scene swithment stuff

	public static void LoadLevel (int level) {
		Application.LoadLevel(level);
	}

	public static void RestartLevel () {
		LevelManager.LoadLevel(Application.loadedLevel);
	}
	
	public static void NextLevel () {
		int nextLevel = (Application.loadedLevel + 1) % GameManager.Instance.SceneCount;
		if (nextLevel > GameManager.Instance.UnlockedLevel) {
			GameManager.Instance.IncUnlockedLevel();
			if (nextLevel != GameManager.Instance.UnlockedLevel) {
				Debug.LogWarning("UnlockedLevel logical error, shouldn't happen");
			}
		}
		LevelManager.LoadLevel (nextLevel);
	}

	#endregion
}
