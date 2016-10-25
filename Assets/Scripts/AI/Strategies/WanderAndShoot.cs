using UnityEngine;
using System.Collections;

namespace Strategies {
	/// <summary>
	/// Just move around randomly
	/// </summary>
	[RequireComponent(typeof(Wander))]
	[RequireComponent(typeof(AutoAttack))]
	public class WanderAndShoot : AIStrategy {
	}
}