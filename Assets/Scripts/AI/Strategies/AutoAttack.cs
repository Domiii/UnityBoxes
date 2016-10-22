using UnityEngine;
using System.Collections;

namespace Strategies {
	public class AutoAttackAction : AIAction {
		public Unit target;
	}
	
	/// <summary>
	/// Stand still. Scan for attackable targets in range and attack when found.
	/// </summary>
	[RequireComponent(typeof(Attacker))]
	public class AutoAttack : AIStrategy<AutoAttackAction> {
		Attacker attacker;

		void Awake () {
			attacker = GetComponent<Attacker> ();
		}

		void Update () {
			attacker.AutoAttack ();
		}

		protected override void Cleanup () {
			attacker.CurrentTarget = null;
		}
	}
}