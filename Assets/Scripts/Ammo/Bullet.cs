using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ProjectileCollisionTrigger))]
public class Bullet : MonoBehaviour {
	public float DamageMin = 10;
	public float DamageMax = 20;
	public float Speed = 10;

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
				// damage the unit!
				//var damageInfo = ObjectManager.Instance.Obtain<DamageInfo> ();
				var damageInfo = new DamageInfo ();
				damageInfo.Value = Random.Range (DamageMin, DamageMax);
				damageInfo.SourceFactionType = FactionManager.GetFactionType (gameObject);
				target.Damage (damageInfo);
				Destroy (gameObject);
				isDestroyed = true;
			}
		}
	}
}
