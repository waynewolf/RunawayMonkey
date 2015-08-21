using UnityEngine;
using System.Collections;

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
	private const int MAX_BACKGROUND_SLOTS = 4;

	// LevelManager is singleton, but it cannot survive from scene switch
	public static LevelManager Instance { get; private set; }

	public MonkeyBehaviour _playerPrefab ;
	public HunterBehaviour _hunterPrefab;
	public GameObject _shadowPrefab;
	public GameObject _foreground;
	public BackgroundSlot[] _backgroundSlot = new BackgroundSlot[MAX_BACKGROUND_SLOTS];
	public Transform _hudItemsPlaceHolder;
	public float _catchMonkeyTime = 2f;
	public int _backgroundLayerWidth;

	[HideInInspector]
	public MonkeyBehaviour Player { get; private set; }
	[HideInInspector]
	public HunterBehaviour Hunter { get; private set; }
	[HideInInspector]
	public int BananaNumber { get; set; }
	[HideInInspector]
	public int StrawberryNumber { get; set; }

	private float _halfScreenWidthInUnit;
	private float _currentSpeed;

	private float _elapsedTimeWhenHunterOutOfScreen;
	private GameObject _shadow;
	private Vector3 _shadowScale = Vector3.one;
	private Vector3 _bgStartPos;
	private float _bgLayerWidthInUnit;

	void Awake() {
		Instance = this;
		Player = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity) as MonkeyBehaviour;
		Hunter = Instantiate(_hunterPrefab, new Vector3(OFFSET_TO_HUNTER,
					INIT_HUNTER_Y_OFFSET, 0), Quaternion.identity) as HunterBehaviour;
		_shadow = Instantiate(_shadowPrefab, new Vector3(0, -3f, 5f), Quaternion.identity) as GameObject;
		GameManager.Instance.Player = Player;
		_currentSpeed = GameManager.Instance.NormalSpeed;
		_elapsedTimeWhenHunterOutOfScreen = 0f;
		_halfScreenWidthInUnit = 1.0f * Screen.width / Screen.height * Camera.main.orthographicSize;
		_bgLayerWidthInUnit = 1.0f * _backgroundLayerWidth / GameManager.Instance.PixelsPerUnit;
		foreach(BackgroundSlot backgroundSlot in _backgroundSlot) {
			if (backgroundSlot != null) {
				backgroundSlot._initPosition = backgroundSlot._layer.position;
			}
		}
		BananaNumber = 0;
		StrawberryNumber = 0;
	}

	void FixedUpdate() {
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
		if (_currentSpeed > 0.01f) {
			// move the foreground
			Vector3 position = _foreground.transform.position;
			position.x -= _currentSpeed * Time.deltaTime;
			_foreground.transform.position = position;

			// move the backgrounds with parallax effect. support maximum 4 backgroud layers.
			for (int i = 0; i < _backgroundSlot.Length; i++) {
				if (_backgroundSlot[i] != null) {
					float newPosition = Mathf.Repeat(_currentSpeed * Time.time * _backgroundSlot[i]._speedFactor, _bgLayerWidthInUnit);
					_backgroundSlot[i]._layer.position = _backgroundSlot[i]._initPosition + Vector3.left * newPosition;
				}
			}
		} else if (Hunter.IsCatching()) {
			// The monkey is blocked, hunter will catch the monkey in catchMonkeyTime seconds
			float advanceDistance = Mathf.Abs (OFFSET_TO_HUNTER) * Time.deltaTime / _catchMonkeyTime;
			Hunter.Forward(advanceDistance);
		}

		if (MonkeyHunterDistance() > _halfScreenWidthInUnit) {
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

	public void StopMoving() {
		_currentSpeed = 0;
	}

	public void ResumeMoving() {
		_currentSpeed = GameManager.Instance.NormalSpeed;
	}

	public void LevelComplete () {
		GUIManager.Instance.SetLevelComplete(true);
		GameManager.Instance.PauseGame();
	}

	public void RestartLevel () {
		Application.LoadLevel(Application.loadedLevel);
	}

	public void NextLevel () {
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	public void EatBanana (Transform transform) {
		StartCoroutine(ItemSmoothMovement(transform, _hudItemsPlaceHolder));
		BananaNumber++;
		GUIManager.Instance.SetBananaNumber(BananaNumber);
		GameManager.Instance.AddPoints(BANANA_POINTS);
	}

	public void EatStrawberry (Transform transform) {
		StartCoroutine(ItemSmoothMovement(transform, _hudItemsPlaceHolder));
		StrawberryNumber++;
		GUIManager.Instance.SetStrawberryNumber(StrawberryNumber);
		GameManager.Instance.AddPoints(STRAWBERRY_POINTS);
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

	public void ReviveScreen() {
		GUIManager.Instance.DisableButtons();
		GUIManager.Instance.SetRevive(true);
	}

	public void Revive() {
		GUIManager.Instance.SetRevive(false);
		GUIManager.Instance.EnableButtons();
		Player.transform.position = Vector3.zero;
		Player.Revived();
		Hunter.MoveToX(OFFSET_TO_HUNTER);
		Hunter.MonkeyRunaway();

		// Find a safe place for monkey to stand
		Vector2 origin = new Vector2(0f, 10f);
		bool foundAPlaceToStand = false;
		for (float x = 0; x > -_halfScreenWidthInUnit; x -= 0.5f) {
			origin.x = x;
			RaycastHit2D hit = Physics2D.Raycast(origin,
			                                     Vector2.down, 50f,
			                                     1 << LayerMask.NameToLayer("Platform"));
			if (hit && hit.transform.gameObject.tag == "Platform") {
				// rewind to the center of the hit platform
				//float newXPos = x - hit.transform.GetComponent<SpriteRenderer>().bounds.size.x;
				float newXPos = hit.transform.position.x;
				Vector3 newPos = _foreground.transform.position;
				newPos.x += -newXPos;
				_foreground.transform.position = newPos;
				foundAPlaceToStand = true;
				break;
			}
		}

		if (!foundAPlaceToStand) {
			Debug.LogError("No place to stand, need to redesign the level"); 
		}

		ResumeMoving();
	}
}
