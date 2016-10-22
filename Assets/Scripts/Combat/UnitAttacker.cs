using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Shooter))]
public class UnitAttacker : MonoBehaviour {
	public float AttackRadius = 10.0f;

	Unit currentTarget;
	Shooter shooter;

	void Awake() {
		shooter = GetComponent<Shooter> ();
	}

	void Update() {
		KeepAttackingCurrentTarget ();
	}

	void OnDeath(DamageInfo damageInfo) {
		enabled = false;
	}
	
	
	#region Public
	public Unit CurrentTarget {
		get {
			return currentTarget;
		}
	}

	public bool CanAttackCurrentTarget {
		get { return IsCurrentValid && IsCurrentInRange; }
	}

	public bool IsCurrentValid {
		get {
			return currentTarget != null && IsValidTarget(currentTarget);
		}
	}

	public bool IsCurrentInRange {
		get {
			return currentTarget != null && IsInRange (currentTarget);
		}
	}

	public bool IsInRange(Unit target) {
		var dist = (target.transform.position - transform.position).sqrMagnitude;
		return dist <= AttackRadius * AttackRadius;
	}

	public bool IsValidTarget(Unit target) {
		return target.CanBeAttacked && FactionManager.AreHostile (gameObject, target.gameObject);
	}

	public bool CanAttack(Unit target) {
		return IsInRange (target) && IsValidTarget (target);
	}

	bool KeepAttackingCurrentTarget() {
		if (CanAttackCurrentTarget) {
			shooter.StartShootingAt (currentTarget.transform.position);
			return true;
		}
		StopAttack ();
		return false;
	}

	public bool StartAttack(Unit target) {
		if (CanAttackCurrentTarget) {
			StopAttack ();
		}

		currentTarget = target;
		if (CanAttackCurrentTarget) {
			shooter.StartShootingAt (target.transform.position);
			return true;
		}
		return false;
	}

	public void StopAttack() {
		shooter.StopShooting ();
	}
	#endregion


	#region Highlighting
//	SpriteRenderer CreateHighlighterObject() {
//		var go = (GameObject)Instantiate(GameUIManager.Instance.AttackerHighlighterPrefab, transform.position, Quaternion.identity);
//		var highlighter = go.GetComponent<SpriteRenderer>();
//		if (highlighter == null) {
//			Debug.LogError("Attack has invalid Highlighter Prefab. Highlighter must have a SpriteRenderer.");
//			Destroy (go);
//			return null;
//		}
//
//		highlighter.sortingLayerName = "Highlight";
//		
//		// set world-space bounds
//		var max = highlighter.transform.InverseTransformPoint(highlighter.bounds.max);
//		var min = highlighter.transform.InverseTransformPoint(highlighter.bounds.min);
//
//		var diameter = 2 * AttackRadius;
//		var realDiameter = Mathf.Max (max.x - min.x, max.y - min.y);
//		var newScale = diameter / realDiameter;
//		//var yFactor = diameter / realDiameter;
//
//		//var scale = highlighter.transform.localScale;
//		//highlighter.transform.localScale = new Vector3(scale.x * xFactor, scale.y * yFactor, 1);
//		highlighter.transform.localScale = new Vector3(newScale, newScale, 1);
//
//		return highlighter;
//	}
//
//	SpriteRenderer HighlighterObject {
//		get {
//			if (_highlighter == null) {
//				if (GameUIManager.Instance.AttackerHighlighterPrefab != null) {
//					_highlighter = CreateHighlighterObject();
//				}
//			}
//			return _highlighter;
//		}
//	}
//
//	void OnSelect() {
//		var highlighterObject = HighlighterObject;
//		if (highlighterObject == null) {
//			return;
//		}
//		
//		highlighterObject.gameObject.SetActive (true);
//	}
//	
//	void OnUnselect() {
//		if (_highlighter == null) {
//			return;
//		}
//		_highlighter.gameObject.SetActive (false);
//	}
	#endregion

}
