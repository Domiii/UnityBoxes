using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
	public float radius = 10.0F;
	public float pushPower = 100.0F;
	public float minDamage = 50, maxDamage = 100;
	public float upwardModifier = 10;
	public GameObject explosionPrefab;

	void Start () {
	}

	void OnCollisionEnter (Collision other) {
		var isBomb = !!other.gameObject.GetComponent<Bomb> ();
		var isAllied = FactionManager.AreAllied (other.gameObject, gameObject);
		if (!isBomb && !isAllied) {
			Explode ();
			Destroy (gameObject);
		}
	}

	void Explode () {
		Explode (gameObject, explosionPrefab, transform.position, radius, minDamage, maxDamage, pushPower, upwardModifier);
	}

	static void AdjustParticleSpeedSettings(ParticleSystem particles, float radius) {
		// modify lifetime, so that given the particle's intial speed, it can cover the entire radius
		var main = particles.main;
		var explSpeedSettings = main.startSpeed;
		explSpeedSettings = radius / main.startLifetime.constantMax;

		main.startSpeed = explSpeedSettings;
	}

	public static void Explode(GameObject owner, GameObject explosionPrefab, Vector3 pos, float radius, float minDamage, float maxDamage, float pushPower, float upwardModifier) {
		if (explosionPrefab) {
			// spawn explosion object
			var go = (GameObject)Instantiate (explosionPrefab);
			go.transform.position = pos;
			var particles = go.GetComponent<ParticleSystem> ();
			if (particles) { 
				//AdjustParticleSpeedSettings(particles, radius);
			}
			Destroy (go, 5);
		}

		Collider[] colliders = Physics.OverlapSphere (pos, radius);
		foreach (Collider hit in colliders) {
			// add damage if object is Living
			var target = Living.GetLiving(hit);
			if (target && FactionManager.AreHostile(target.gameObject, owner)) {
				// get random damage, then scale with distance
				var dmg = Random.Range (minDamage, maxDamage);
				var closestPoint = hit.ClosestPointOnBounds (pos);
				var dist = Vector3.Distance(closestPoint, pos);
				dmg = dmg * (1 - Mathf.Clamp01(dist / radius));

				// apply damage
				var damageInfo = new DamageInfo() {
					Value = dmg,
					SourceFactionType = FactionManager.GetFactionType (owner)
				};
				target.Damage (damageInfo);
			}

			// make object fly!
			var rb = hit.GetComponent<Rigidbody> ();
			if (rb && !rb.GetComponent<Bomb> ()) {
				rb.AddExplosionForce (pushPower, pos, radius, upwardModifier);
			}
		}
	}
}