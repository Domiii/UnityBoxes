using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public static class RotationUtil {
	public static Quaternion GetRotationToward(this Transform transform, Vector3 target) {
		Vector3 dir = target - transform.position;
		return transform.GetRotationFromDirection(dir);
	}

	public static Quaternion GetRotationFromDirection(this Transform transform, Vector3 dir) {
		var angle = Mathf.Atan2 (dir.x, dir.z) * Mathf.Rad2Deg;
		return Quaternion.AngleAxis(angle, transform.up);
	}

	public static void RotateTowardTarget(this Transform transform, Vector3 target, float turnSpeed = 180) {
		var rigidbody = transform.GetComponent<Rigidbody> ();
		if (rigidbody != null && (rigidbody.constraints & RigidbodyConstraints.FreezePositionY) != 0) {
			// don't rotate if rotation has been constrained
			return;
		}

		//transform.LookAt ();
		var agent = transform.GetComponent<NavMeshAgent> ();
		if (agent != null) {
			turnSpeed = agent.angularSpeed;
		}
		var targetRotation = transform.GetRotationToward(target);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
	}

	/// <summary>
	/// Values for cosTolerance (you can get these values by googling: "acos(1-cosTolerance) in degrees"):
	/// 
	/// 0 -> looking exactly at target
	/// 0.1 -> 26 deg (half-angle of cone)
	/// 0.2 -> 37 deg
	/// 0.3 -> 46 deg
	/// ...
	/// 1 (or more) -> 180 deg (always true)
	/// 
	/// </summary>
	public static bool IsLookingAt(this Transform transform, Vector3 target, float cosTolerance = 0.1f) {
		Vector3 dirFromAtoB = (target - transform.position).normalized;
		float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);

		return dotProd >= 1-cosTolerance;
	}
}