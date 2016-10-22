using UnityEngine;
using System.Collections;

namespace Strategies {
	public class HuntTargetAction : AIAction {
		public Unit target;
	}

	/// <summary>
	/// Attack when close enough; else move and catch up
	/// </summary>
	[RequireComponent(typeof(UnitAttacker))]
	[RequireComponent(typeof(NavMeshMover))]
	public class HuntTarget : AIStrategy<HuntTargetAction> {
		UnitAttacker attacker;
		NavMeshMover mover;

		#region Public
		public Unit CurrentTarget {
			get {
				return attacker.CurrentTarget;
			}
		}

		public override void StartBehavior(HuntTargetAction action) {
			attacker.StartAttack(action.target);
			mover.StopMovingAtDestination = false;
		}
		#endregion

		void Awake () {
			attacker = GetComponent<UnitAttacker> ();
			mover = GetComponent<NavMeshMover> ();
		}

		void Update () {
			// current target out of range -> move to catch up
			if (attacker.IsCurrentValid) {
				if (attacker.IsCurrentInRange) {
					// keep attacking; also: make sure, we are not moving
					mover.StopMove();
				}
				else {
					// target out of range -> move toward target
					mover.CurrentDestination = attacker.CurrentTarget.transform.position;
					attacker.StopAttack ();
				}
			}
			else {
				// we have no more valid target (target might have died, disappeared, turned etc) -> done!
				StopStrategy();
			}
		}

		/// <summary>
		/// Called when finished hunting.
		/// </summary>
		protected override void OnStop() {
			attacker.StopAttack();
			mover.StopMove ();
		}
	}
}