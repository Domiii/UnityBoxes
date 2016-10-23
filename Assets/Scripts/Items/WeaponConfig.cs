using UnityEngine;
using System.Collections;


[System.Serializable]
public class WeaponConfig {
	public Bullet bulletPrefab;
	public float attackDelay = 0.2f;
	public float damageMin = 10;
	public float damageMax = 15;

	public int bulletCount = 3;
	public float ConeAngle = 30;
}
