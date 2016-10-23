using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {
	public GameObject BulletPrefab;
	public float AttackDelay = 1;
	public float turnSpeed = 1000.0f;
	public Transform shootTransform;

	Vector3 currentTarget;
	float lastShotTime;
	bool isAttacking;

	public bool IsAttacking {
		get {
			return isAttacking;
		}
		set {
			if (isAttacking != value) {
				isAttacking = value;
				if (isAttacking) {
					OnStartAttack ();
				} else {
					OnStopAttack ();
				}
			}
		}
	}

	public void StartShootingAt(Vector3 target) {
		currentTarget = target;
		IsAttacking = true;
	}

	public void StopShooting() {
		IsAttacking = false;
	}

	void OnStartAttack () {
	}

	void OnStopAttack () {
		//lastShotTime = Time.time;
	}

	// Use this for initialization
	void Awake () {
		if (BulletPrefab == null) {
			Debug.LogError("Attacker is missing Bullet Prefab", this);
			return;
		}

		if (shootTransform == null) {
			shootTransform = transform;
		}
		isAttacking = false;
	}

	void Update() {
		if (isAttacking) {
			// some debug stuff
			var dir = currentTarget - transform.position;
			Debug.DrawRay (transform.position, dir);

			// rotate toward target
			RotateTowardTarget();

			// keep shooting
			var delay = Time.time - lastShotTime;
			if (delay < AttackDelay) {
				// still on cooldown
				return;
			}
			ShootAt (currentTarget);
		}
	}

	Quaternion GetRotationToward(Vector3 target) {
		Vector3 dir = target - shootTransform.position;
		var angle = Mathf.Atan2 (dir.x, dir.z) * Mathf.Rad2Deg;
		return Quaternion.AngleAxis(angle, Vector3.up);
	}

	void RotateTowardTarget() {
		var rigidbody = GetComponent<Rigidbody> ();
		if (rigidbody != null && (rigidbody.constraints & RigidbodyConstraints.FreezePositionY) != 0) {
			// don't rotate if rotation has been constrained
			return;
		}

		//transform.LookAt ();
		var agent = GetComponent<NavMeshAgent> ();
		if (agent != null) {
			turnSpeed = agent.angularSpeed;
		}
		var targetRotation = GetRotationToward(currentTarget);
		shootTransform.rotation = Quaternion.RotateTowards(shootTransform.rotation, targetRotation, Time.deltaTime * turnSpeed);
	}

	public void ShootAt(Transform t) {
		ShootAt (t.position);
	}

	public void ShootAt(Vector3 target) {
		if (BulletPrefab == null) {
			return;
		}

		// create a new bullet
		var bulletObj = (GameObject)Instantiate(BulletPrefab, transform.position, GetRotationToward(target));

		// set bullet faction
		FactionManager.SetFaction (bulletObj, gameObject);

		// set velocity
		var bullet = bulletObj.GetComponent<Bullet> ();
		var rigidbody = bulletObj.GetComponent<Rigidbody> ();
		var direction = target - bulletObj.transform.position;
		direction.Normalize ();
		rigidbody.velocity = direction * bullet.speed;

		// reset shoot time
		lastShotTime = Time.time;
	}

	void OnDeath(DamageInfo damageInfo) {
		enabled = false;
	}
}
