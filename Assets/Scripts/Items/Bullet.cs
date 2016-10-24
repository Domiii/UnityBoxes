using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ProjectileCollisionTrigger))]
public class Bullet : MonoBehaviour {
	public GameObject owner;
	public float speed = 10;
	public bool destroyOnCollision = false;

	[HideInInspector]
	public float damageMin;
	[HideInInspector]
	public float damageMax;

	bool isDestroyed = false;

	void Start () {
		Destroy(gameObject, 10);		// destroy after at most 10 seconds
	}

	void Update () {
	}
	
	void OnProjectileHit(Collider col) {
		var target = col.gameObject.GetComponent<Unit>();
		if (!isDestroyed && target != null) {
			// when colliding with Unit -> Check if we can attack the Unit
			if (target.CanBeAttacked && FactionManager.AreHostile (gameObject, target.gameObject)) {
				DamageTarget (target);
			}
		}
		//else if (col.gameObject != owner && col.GetComponent<Bullet>() == null && col.GetComponentInParent<Bullet>() == null && destroyOnCollision) {
		else if (destroyOnCollision && !FactionManager.AreAllied(gameObject, col.gameObject)) {
			// hit something that is not an enemy unit -> Destroy anyway
			DestroyThis ();
		}
	}

	void DamageTarget(Unit target) {
		// damage the unit!
		//var damageInfo = ObjectManager.Instance.Obtain<DamageInfo> ();
		var damageInfo = new DamageInfo ();
		damageInfo.Value = Random.Range (damageMin, damageMax);
		damageInfo.SourceFactionType = FactionManager.GetFactionType (gameObject);
		target.Damage (damageInfo);
		DestroyThis ();
	}

	void DestroyThis() {
		Destroy (gameObject);
		isDestroyed = true;
	}
}
