using UnityEngine;

public class AttackTargetFinder : TargetFinder {
	public override bool IsValidTarget (Living target) {
		//Debug.Log(gameObject+ " vs. " + target.gameObject + ": " + FactionManager.AreHostile (gameObject, target.gameObject));
		return target.CanBeAttacked && FactionManager.AreHostile (gameObject, target.gameObject);
	}
}
