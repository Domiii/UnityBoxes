using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AttackTargetFinder))]
public class HeatSeeker : MonoBehaviour {
	[Range(0, 1)]
	public float accuracy = .8f;

	public Living target;
	public float findTargetRadius = 10;

	Rigidbody body;
	AttackTargetFinder finder;


	void Start () {
		body = GetComponent<Rigidbody> ();
		finder = GetComponent<AttackTargetFinder> ();
	}

	void FixedUpdate () {
		if (!target) {
			target = finder.FindTarget (findTargetRadius);
		}
		if (target) {
			var v = body.velocity;
			var speed = v.magnitude;
			var vDir = v.normalized;
			var newDir = target.transform.position - transform.position;
			newDir.Normalize ();

			var ratio = accuracy * Time.fixedDeltaTime;

			vDir = (1-ratio) * vDir + ratio * newDir;
			vDir.Normalize ();

			body.velocity = vDir * speed;
			transform.forward = vDir;
		}
	}
}
