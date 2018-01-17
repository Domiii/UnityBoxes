using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetFinder : MonoBehaviour {
	protected Collider[] collidersInRange;


	public virtual bool IsValidTarget (Living target) {
		//Debug.Log(gameObject+ " vs. " + target.gameObject + ": " + FactionManager.AreHostile (gameObject, target.gameObject));
		return target != null && target.gameObject != gameObject;
	}

	public Living FindTarget (float radius) {
		if (collidersInRange == null) {
			collidersInRange = new Collider[128];
		}
		var nResults = Physics.OverlapSphereNonAlloc (transform.position, radius, collidersInRange);
		for (var i = 0; i < nResults; ++i) {
			var collider = collidersInRange [i];
			var living = Living.GetLiving (collider);
			if (living && IsValidTarget (living)) {
				return living;
			}
		}

		// no valid target found
		return null;
	}


	public bool IsInRange (Living target, float radius) {
		var collider = target.GetComponent<Collider> ();
		if (!collider) {
			collider = target.GetComponentInChildren<Collider> ();
		}

		Vector3 targetPos;
		if (collider != null) {
			targetPos = collider.ClosestPointOnBounds (transform.position);
		} else {
			targetPos = target.transform.position;
		}

		var distSq = (targetPos - transform.position).sqrMagnitude;
		//print (Vector3.Distance(targetPos, transform.position));
		return distSq <= radius * radius;
	}
}
