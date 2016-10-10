using UnityEngine;
using System.Collections;

namespace Behaviors {
	/// <summary>
	/// Attack when close enough; else move and catch up
	/// </summary>
	[RequireComponent(typeof(Attacker))]
	[RequireComponent(typeof(NavMeshMover))]
	public class HuntTarget : AIBehavior {
		Attacker attacker;
		NavMeshMover mover;

		#region Public
		public Unit CurrentTarget {
			get {
				return attacker.CurrentTarget;
			}
		}

		public void StartBehavior(Unit target) {
			attacker.CurrentTarget = target;
			mover.StopMovingAtDestination = false;
		}
		#endregion

		void Awake () {
			attacker = GetComponent<Attacker> ();
			mover = GetComponent<NavMeshMover> ();
		}

		void Update () {
			// current target out of range -> move to catch up
			if (attacker.IsCurrentValid) {
				if (attacker.IsCurrentInRange) {
					// keep attacking
					mover.StopMove();
					attacker.AttackCurrentTarget();
				}
				else {
					// target out of range -> move toward target
					mover.CurrentDestination = attacker.CurrentTarget.transform.position;
				}
			}
			else {
				// we have no more valid target (target might have died, disappeared, turned etc) -> done!
				StopBehavior();
			}
		}

		/// <summary>
		/// Called when finished hunting.
		/// </summary>
		protected override void Cleanup() {
			attacker.CurrentTarget = null;
			mover.StopMove ();
		}
	}
}