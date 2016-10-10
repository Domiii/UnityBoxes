using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Strategy))]
[RequireComponent(typeof(Behaviors.AutoAttack), typeof(Behaviors.HuntTarget), typeof(Behaviors.MoveToPosition))]
public class PlayerBrain : BaseBrain {
	Strategy strategy;

	Behaviors.AutoAttack autoAttack;
	Behaviors.HuntTarget huntTarget;
	Behaviors.MoveToPosition moveToPosition;

	void Awake () {
		strategy = GetComponent<Strategy> ();

		autoAttack = RegisterBehavior<Behaviors.AutoAttack> ();
		huntTarget = RegisterBehavior<Behaviors.HuntTarget> (OnFinishedHuntTarget);
		moveToPosition = RegisterBehavior<Behaviors.MoveToPosition> (OnFinishedMove);

		// AutoAttack is the default behavior
		CurrentBehavior = autoAttack;
	}

	void Update () {
		var nextAction = strategy.PopAction ();
		if (nextAction != null) {
			// change action
			Decide (nextAction);
		} else {
			// keep doing what we are already doing
		}
	}

	void OnFinishedHuntTarget() {
		// go back to AutoAttack
		CurrentBehavior = autoAttack;
	}

	void OnFinishedMove() {
		// go back to AutoAttack
		CurrentBehavior = autoAttack;
	}

	void Decide(Actions.Action action) {
		// determine what player wants to do next
		if (action is Actions.Attack) {
			// hunt the given target
			CurrentBehavior = huntTarget;
			huntTarget.StartBehavior(((Actions.Attack)action).target);
		} else if (action is Actions.Move) {
			// move
			CurrentBehavior = moveToPosition;
			moveToPosition.StartBehavior(((Actions.Move)action).destination);
		}
		else {
			// auto attack when idle
			CurrentBehavior = autoAttack;
		}
	}
}
