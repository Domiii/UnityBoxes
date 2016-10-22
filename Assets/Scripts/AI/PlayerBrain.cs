using UnityEngine;
using System.Collections;

public class PlayerBrain : BaseBrain {
	void Awake () {
		AddStrategy<Strategies.Idle> ();
		//AddStrategy<Strategies.HuntTarget> ();
		AddStrategy<Strategies.MoveToDestination> ();

		// AutoAttack is the default strategy
		SetDefaultStrategy<Strategies.Idle>();
	}
}
