using UnityEngine;
using System.Collections;

public class Attacker : MonoBehaviour {
	public GameObject BulletPrefab;
	public float AttackDelay = 1;
	public float AttackRadius = 30.0f;

	Collider[] collidersInRange = new Collider[128];
	Unit currentTarget;
	//bool isAttacking;
	float lastShotTime;

	// Use this for initialization
	void Awake () {
		if (BulletPrefab == null) {
			Debug.LogError("Attacker is missing Bullet Prefab", this);
			return;
		}

		lastShotTime = Time.time;
		//isAttacking = false;
	}

	void Update() {
		// some debug stuff
		if (currentTarget != null) {
			var dir = (currentTarget.transform.position - transform.position).normalized * AttackRadius;
			Debug.DrawRay (transform.position, dir);
		}
	}

	void OnDeath(DamageInfo damageInfo) {
		enabled = false;
	}
	
	
	#region Public
	public Unit CurrentTarget {
		get {
			return currentTarget;
		}
		set {
			currentTarget = value;
		}
	}

	public bool IsCurrentValid {
		get {
			return currentTarget != null && IsValidTarget(currentTarget);
		}
	}

	public bool IsCurrentInRange {
		get {
			return currentTarget != null && IsInRange (currentTarget);
		}
	}

	public bool IsInRange(Unit target) {
		var dist = (target.transform.position - transform.position).sqrMagnitude;
		return dist <= AttackRadius * AttackRadius;
	}

	public bool IsValidTarget(Unit target) {
		return target.CanBeAttacked && FactionManager.AreHostile (gameObject, target.gameObject);
	}

	public bool CanAttack(Unit target) {
		return IsInRange (target) && IsValidTarget (target);
	}
	
	public bool UpdateCurrentTarget() {
		// find new target
		currentTarget = FindTarget();
		
		if (IsCurrentValid) {
			return true;
		}
		else {
			//ResetRotation();
		}
		return false;
	}

	public bool AutoAttack() {
		// keep attacking previous target
		// if currently has no target, look for new target to attack
		var canAttackCurrent = IsCurrentValid && IsCurrentInRange;
		if (!canAttackCurrent && !UpdateCurrentTarget ()) {
			// could not find a valid target
			return false;
		} else {
			// keep attacking previous target
			AttackCurrentTargetUnchecked ();
			return true;
		}
	}

	public bool AttackCurrentTarget() {
		if (IsCurrentInRange && IsCurrentValid) {
			AttackCurrentTargetUnchecked ();
			return true;
		}
		return false;
	}

	void AttackCurrentTargetUnchecked() {
		RotateTowardTarget();

		var delay = Time.time - lastShotTime;
		if (delay < AttackDelay) {
			// still on cooldown
			return;
		}

		ShootAt (currentTarget);
	}
	#endregion


	#region Highlighting
//	SpriteRenderer CreateHighlighterObject() {
//		var go = (GameObject)Instantiate(GameUIManager.Instance.AttackerHighlighterPrefab, transform.position, Quaternion.identity);
//		var highlighter = go.GetComponent<SpriteRenderer>();
//		if (highlighter == null) {
//			Debug.LogError("Attack has invalid Highlighter Prefab. Highlighter must have a SpriteRenderer.");
//			Destroy (go);
//			return null;
//		}
//
//		highlighter.sortingLayerName = "Highlight";
//		
//		// set world-space bounds
//		var max = highlighter.transform.InverseTransformPoint(highlighter.bounds.max);
//		var min = highlighter.transform.InverseTransformPoint(highlighter.bounds.min);
//
//		var diameter = 2 * AttackRadius;
//		var realDiameter = Mathf.Max (max.x - min.x, max.y - min.y);
//		var newScale = diameter / realDiameter;
//		//var yFactor = diameter / realDiameter;
//
//		//var scale = highlighter.transform.localScale;
//		//highlighter.transform.localScale = new Vector3(scale.x * xFactor, scale.y * yFactor, 1);
//		highlighter.transform.localScale = new Vector3(newScale, newScale, 1);
//
//		return highlighter;
//	}
//
//	SpriteRenderer HighlighterObject {
//		get {
//			if (_highlighter == null) {
//				if (GameUIManager.Instance.AttackerHighlighterPrefab != null) {
//					_highlighter = CreateHighlighterObject();
//				}
//			}
//			return _highlighter;
//		}
//	}
//
//	void OnSelect() {
//		var highlighterObject = HighlighterObject;
//		if (highlighterObject == null) {
//			return;
//		}
//		
//		highlighterObject.gameObject.SetActive (true);
//	}
//	
//	void OnUnselect() {
//		if (_highlighter == null) {
//			return;
//		}
//		_highlighter.gameObject.SetActive (false);
//	}
	#endregion

	Quaternion GetRotationToward(Transform targetTransform) {
		Vector3 dir = targetTransform.position - transform.position;
		var angle = Mathf.Atan2 (dir.x, dir.z) * Mathf.Rad2Deg;
		return Quaternion.AngleAxis(angle, Vector3.up);
	}
	
	void ResetRotation() {
		//transform.rotation = Quaternion.AngleAxis (0, Vector3.forward);
	}
	
	void RotateTowardTarget() {
		if (!IsCurrentValid) {
			return;
		}
		
		var rigidbody = GetComponent<Rigidbody> ();
		if (rigidbody != null && (rigidbody.constraints & RigidbodyConstraints.FreezeRotation) != 0) {
			// don't rotate if rotation has been constrained
			return;
		}
		
		//transform.LookAt ();
		var turnSpeed = 10.0f;
		var agent = GetComponent<NavMeshAgent> ();
		if (agent != null) {
			turnSpeed = agent.angularSpeed;
		}
		var targetRotation = GetRotationToward(currentTarget.transform);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
	}

	#region Finding + Attacking
	Unit FindTarget() {
		var nResults = Physics.OverlapSphereNonAlloc(transform.position, AttackRadius, collidersInRange);
		for (var i = 0; i < nResults; ++i) {
			var collider = collidersInRange[i];
			var unit = collider.GetComponent<Unit> ();
			if (unit != null && IsValidTarget(unit)) {
				return unit;
			}
		}

		// no valid target found
		return null;
	}
	
	void ShootAt(Unit target) {
		if (BulletPrefab == null) {
			return;
		}

		// create a new bullet
		var bulletObj = (GameObject)Instantiate(BulletPrefab, transform.position, GetRotationToward(currentTarget.transform));

		// set bullet faction
		FactionManager.SetFaction (bulletObj, gameObject);

		// set velocity
		var bullet = bulletObj.GetComponent<Bullet> ();
		var rigidbody = bulletObj.GetComponent<Rigidbody> ();
		var direction = target.transform.position - bulletObj.transform.position;
		direction.Normalize ();
		rigidbody.velocity = direction * bullet.speed;

		// reset shoot time
		lastShotTime = Time.time;
	}
	
//	void StartAttack(Actions.Attack action) {
//		isAttacking = true;
//		SendMessage ("OnAttackStart", SendMessageOptions.DontRequireReceiver);
//	}
//
//	void Attack(Actions.Attack action) {
//		if (action.target != null) {
//			currentTarget = action.target;
//		}
//	}
//	
//	void StopAttack(Actions.Attack action) {
//		isAttacking = false;
//		SendMessage ("OnAttackStop", SendMessageOptions.DontRequireReceiver);
//	}
	#endregion
}
