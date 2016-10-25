using UnityEngine;
using System.Collections;

namespace Strategies {
	public class AutoAttackAction : AIAction {
		public static readonly AutoAttackAction Default = new AutoAttackAction();
	}
	
	/// <summary>
	/// Scan for attackable targets in range and attack when found.
	/// </summary>
	[RequireComponent(typeof(UnitAttacker))]
	public class AutoAttack : AIStrategy<AutoAttackAction> {
		UnitAttacker attacker;
		Collider[] collidersInRange = new Collider[128];

		void Awake () {
			attacker = GetComponent<UnitAttacker> ();
		}

		void Update () {
			// keep attacking previous target
			// if currently has no target, look for new target to attack
			if (!attacker.CanAttackCurrentTarget && !UpdateCurrentTarget ()) {
				// could not find a valid target -> Stop
				attacker.StopAttack();
			}
		}

		public bool UpdateCurrentTarget() {
			// find new target
			var target = FindTarget();

			if (target != null) {
				return attacker.StartAttack (target);
			}
			return false;
		}

		protected override void OnStop () {
			attacker.StopAttack ();
		}


		#region Finding Target
		Unit FindTarget() {
			var nResults = Physics.OverlapSphereNonAlloc(transform.position, attacker.AttackRadius, collidersInRange);
			for (var i = 0; i < nResults; ++i) {
				var collider = collidersInRange[i];
				var unit = collider.GetComponent<Unit> ();
				if (unit != null && attacker.IsValidTarget(unit)) {
					return unit;
				}
			}

			// no valid target found
			return null;
		}
		#endregion
	}
}