using UnityEngine;
using System.Collections;

public class MonkeyBehaviour : MonoBehaviour, IPauseable {

	public enum State {
		Running,
		Floating,
		WantHang,
		Hanging,
		Falling,
		Caught,
		Dead,
	};

	public float _jumpForce = 575f;
	public GameObject _bananaPeelPrefab;
	public GameObject _longJumpEffectPrefab;

	private State _state;
	private Animator _animator;
	private Rigidbody2D _rigidbody2D;
	private BoxCollider2D _boxCollider2D;
	private Vector2 _jumpForceVector;
	private bool _paused;
	private bool _stuckInSwamp;
	private GameObject _bridgeCurrentlyStandOn;

	void Awake () {
		_animator = GetComponent<Animator>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_boxCollider2D = GetComponent<BoxCollider2D>();
		_state = State.Falling;
		_jumpForceVector = new Vector2(0, _jumpForce);
		_paused = true;
		_stuckInSwamp = false;
	}

	void Start() {
		OnPause ();
	}

	void FixedUpdate () {
		_animator.SetFloat("VSpeed", _rigidbody2D.velocity.y);
	}

	void OnCollisionEnter2D(Collision2D other) {
		string otherTag = other.gameObject.tag;
		if (otherTag == "Platform") {
			Ground();
		}
		else if (otherTag == "TreeBranch")
			Hang(other.gameObject.transform);
		else if (otherTag == "Bird")
			Hang(other.gameObject.transform, true);
		else if (otherTag == "Swamp") {
			Ground ();
			Block ();
			_stuckInSwamp = true;
		} else if (otherTag == "Bridge") {
			Ground ();
			_bridgeCurrentlyStandOn = other.gameObject;
		}
	}

	void OnCollisionExit2D(Collision2D other) {
		string otherTag = other.gameObject.tag;
		// FIXME: If exit platform and the monkey is not standing on a bridge, then Fall(). 
		// This is an ugly hack that an extra Fall animation will be played if the platform
		// and a bridge stitched together. Also, bridge collider must be put on top and 
		// overlapped with platform colliders.
		if (otherTag == "Platform" && !_bridgeCurrentlyStandOn) {
			Fall();
		}
		else if (otherTag == "Bridge") {
			_bridgeCurrentlyStandOn = null;
		}
	}

	#region state queries
	public bool IsRunning() {
		return (_state == State.Running);
	}

	public bool IsFloating() {
		return (_state == State.Floating);
	}

	public bool IsWantHang() {
		return (_state == State.WantHang);
	}

	public bool IsHanging() {
		return (_state == State.Hanging);
	}

	public bool IsFalling() {
		return (_state == State.Falling);
	}

	public bool IsCaught() {
		return (_state == State.Caught);
	}

	public bool IsDead() {
		return (_state == State.Dead);
	}
	
	#endregion

	#region state transitions
	public void ShortJump() {
		if (_state == State.Running && !_stuckInSwamp) {
			_rigidbody2D.AddForce(_jumpForceVector);
			_animator.SetBool("Ground", false);
			_animator.SetTrigger ("Jump");
			_state = State.Floating;

			// FIXME: If we jump from bridge, bridge will be broken.
			// This is an ugly hack, need to rearchitect the monkey state machine.
			if (_bridgeCurrentlyStandOn != null) {
				_bridgeCurrentlyStandOn.SetActive(false);
				_bridgeCurrentlyStandOn = null;
			}
		}
	}

	public void LongJump() {
		if (_state == State.Floating) {
			_rigidbody2D.AddForce(0.7f * _jumpForceVector);
			_animator.SetTrigger("DoubleJump");
			_state = State.WantHang;
			Vector3 position = transform.position;
			position.y -= 1f;
			Instantiate(_longJumpEffectPrefab, position, Quaternion.identity);
		}
	}
	
	private void Ground() {
		if (_state == State.Floating || _state == State.WantHang || _state == State.Falling) {
			_animator.SetBool("Ground", true);
			_state = State.Running;
		}
	}

	private void Hang(Transform hangeOn, bool flyWithBird = false) {
		// notify hang success only in WantHang state, to avoid 
		// continuously hang on the hook.
		if (_state == State.WantHang) {
			_animator.SetTrigger("HangSuccess");
			_rigidbody2D.isKinematic = true;
			Vector3 newPos = hangeOn.position;
			newPos.y -= 0.5f * GetComponent<SpriteRenderer>().bounds.size.y;
			newPos.x = transform.position.x;
			newPos.z = transform.position.z;
			transform.position = newPos;
			_state = State.Hanging;
			if (flyWithBird) {
				LevelManager.Instance.FastSceneScrolling();
			}
		}
	}

	public void JumpOff() {
		if (_state == State.Hanging) {
			LevelManager.Instance.ResumeSceneScrolling();
			EnablePhysics();
			_state = State.Falling;
		}
	}

	public void Fall() {
		if (_state == State.Running) {
			_animator.SetBool("Ground", false);
			_state = State.Falling;
		}
	}

	// called when nothing to hang in hanging state, OnCollision{Enter,Exit}2D notification
	// keeps coming when the rigidbody is moving, we cannot rely on this,
	// now this function is called from trigger in BranchTipCheck
	public void NoHang() {
		if (_state == State.Hanging) {
			LevelManager.Instance.ResumeSceneScrolling();
			EnablePhysics();
			_state = State.Falling;
		}
	}

	#endregion

	public void Attack() {
		if (IsRunning()) {
			// we animate no matter have or have not banana while running
			_animator.SetBool ("Attack", true);
			if (LevelManager.Instance.BananaNumber > 0) {
				Vector3 bananaSpawnPos = transform.position;
				bananaSpawnPos.x -= 1;
				GameObject bananaPeel = Instantiate(_bananaPeelPrefab, bananaSpawnPos, Quaternion.identity) as GameObject;
				bananaPeel.GetComponent<BananaPeelItem>().Throw(LevelManager.Instance.Hunter.gameObject);
				LevelManager.Instance.AttackWithBananaPeel();
			}
		}
	}

	// called by animation event
	void ExitAttack() {
		_animator.SetBool("Attack", false);
	}

	public void EnablePhysics() {
		_rigidbody2D.isKinematic = false;
		_boxCollider2D.enabled = true;
	}

	public void DisablePhysics() {
		_rigidbody2D.isKinematic = true;
		_boxCollider2D.enabled = false;
	}

	public void PauseAnimation () {
		_animator.enabled =  false;
	}

	public void ResumeAnimation() {
		_animator.enabled = true;
	}

	public void Block() {
		LevelManager.Instance.StopSceneScrolling();
	}

	public void UnBlock() {
		LevelManager.Instance.ResumeSceneScrolling();
	}

	public void MoveToHunter (Transform target) {
		DisablePhysics();
		StartCoroutine(SmoothMovement(transform, target));
	}

	private IEnumerator SmoothMovement(Transform start, Transform end) {
		Vector2 initPos = start.position;
		Vector2 targetPos = end.position;
		targetPos.y += 2f;
		targetPos.x -= 1.5f;
		
		for (float t = 0f; t < 0.1f; t += 0.05f) {
			start.position = Vector2.Lerp (initPos, targetPos, t / 0.1f);
			yield return new WaitForSeconds(0.01f);
		}
		
		yield return null;
	}

	public void Caught () {
		_state = State.Caught;
		_animator.SetBool("Caught", true);
	}

	public void Dead() {
		_state = State.Dead;
	}

	public void Revived () {
		if (_state == State.Caught) {
			_animator.SetBool("Caught", false);
			_animator.SetTrigger("Revived");
			StartCoroutine(DelayedResumeFunctioning(4));
			_state = State.Falling;
		} else if (_state == State.Dead) {
			_state = State.Falling;
			_animator.Play ("FallOnly");
		}
	}

	private IEnumerator DelayedResumeFunctioning(float delaySecond) {
		yield return new WaitForSeconds(delaySecond);
		EnablePhysics();
		yield return null;
	}

	public void OnPause() {
		DisablePhysics();
		PauseAnimation();
		_paused = true;
	}

	public void OnResume() {
		_stuckInSwamp = false;
		ResumeAnimation();
		EnablePhysics();
		_paused = false;
	}
}
