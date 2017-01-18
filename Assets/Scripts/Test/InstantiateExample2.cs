using UnityEngine;

public class InstantiateExample2 : MonoBehaviour {
	public GameObject bulletPrefab;
	public Transform target;
	public float shootDelay = 0.5f;
	public float bulletSpeed = 10;

	void Start () {
		InvokeRepeating ("Shoot", shootDelay, shootDelay);
	}

	void Shoot() {
		var bullet = (GameObject)Instantiate(bulletPrefab, target.position, target.rotation);
		bullet.GetComponent<Rigidbody>().velocity = target.forward * bulletSpeed;
	}
}
