﻿using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Shooter : MonoBehaviour {
	public bool alwaysShoot = false;
	public Transform shootTransform;
	public Transform rotationTransform;
	public WeaponConfig weapon = new WeaponConfig();
	public float turnSpeed = 1000.0f;

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
					NotifyStartAttack ();
				} else {
					NotifyStopAttack ();
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

	void NotifyStartAttack () {
		SendMessage ("OnStartAttack", SendMessageOptions.DontRequireReceiver);
	}

	void NotifyStopAttack () {
		SendMessage ("OnStopAttack", SendMessageOptions.DontRequireReceiver);
		//lastShotTime = Time.time;
	}

	void Reset() {
		var agent = GetComponent<NavMeshAgent> ();
		if (agent != null) {
			turnSpeed = agent.angularSpeed;
		}
	}

	// Use this for initialization
	void Awake () {
		if (weapon == null) {
			print("Shooter is missing Weapon");
			return;
		}
		if (weapon.bulletPrefab == null) {
			print("Shooter's Weapon is missing Bullet Prefab");
			return;
		}

		if (shootTransform == null) {
			if (rotationTransform == null) {
				shootTransform = transform;
			} else {
				shootTransform = rotationTransform;
			}
		}
		if (rotationTransform == null) {
			rotationTransform = shootTransform;
		}
		isAttacking = false;

		currentTarget = shootTransform.position + shootTransform.forward;
	}

	void Update() {
		if (isAttacking || alwaysShoot) {
			// some debug stuff
			var dir = currentTarget - shootTransform.position;
			Debug.DrawRay (shootTransform.position, dir);

			// rotate toward target
			rotationTransform.RotateTowardTarget(currentTarget, turnSpeed);

			// keep shooting
			var delay = Time.time - lastShotTime;
			if (delay < weapon.attackDelay || !rotationTransform.IsLookingAt(currentTarget)) {
				// still on cooldown or not looking at target
				return;
			}
			ShootAt (currentTarget);
		}
	}

	public void ShootAt(Transform t) {
		ShootAt (t.position);
	}

	public void ShootAt(Vector3 target) {
		if (weapon == null || weapon.bulletPrefab == null) {
			return;
		}

		//var dir = target - transform.position;
		var dir = shootTransform.forward;
		dir.y = 0;
		dir.Normalize ();
		if (weapon.bulletCount > 1) {
			// shoot N bullets in a cone from -coneAngle/2 to +coneAngle/2
			dir = Quaternion.Euler (0, -weapon.ConeAngle / 2, 0) * dir;

			var deltaAngle = weapon.ConeAngle / (weapon.bulletCount-1);

			for (var i = 0; i < weapon.bulletCount; ++i) {
				//dir.Normalize ();
				ShootBullet (dir);
				dir = Quaternion.Euler (0, deltaAngle, 0) * dir;

			}
		} else {
			// just one bullet straight toward the target
			ShootBullet (dir);
		}

		// reset shoot time
		lastShotTime = Time.time;
	}

	void ShootBullet(Vector3 dir) {
		// create a new bullet (make sure, it's on same height as target)
		var pos = shootTransform.position;
		pos.y = currentTarget.y;
		var bullet = (Bullet)Instantiate(weapon.bulletPrefab, pos, shootTransform.GetRotationFromDirection(dir));

		// set bullet faction
		FactionManager.SetFaction (bullet.gameObject, gameObject);

		//bullet.owner = gameObject;
		bullet.damageMin = weapon.damageMin;
		bullet.damageMax = weapon.damageMax;
		bullet.lifeTime = weapon.bulletLifeTime;

		// set velocity
		var rigidbody = bullet.GetComponent<Rigidbody> ();
		rigidbody.velocity = dir * weapon.bulletSpeed;

		// special treatment for Seekers
		var seeker = bullet.GetComponent<HeatSeeker> ();
		var attacker = GetComponent<Attacker>();
		if (attacker && seeker && attacker.CurrentTarget) {
			seeker.target = attacker.CurrentTarget;
		}
	}

	void OnDeath(DamageInfo damageInfo) {
		enabled = false;
	}
}
