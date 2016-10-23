using UnityEngine;
using System.Collections;

public class RandomWanderAction : AIAction {
	public static readonly RandomWanderAction Default = new RandomWanderAction();
}

/// <summary>
/// Just move around randomly
/// </summary>
[RequireComponent(typeof(NavMeshMover))]
[RequireComponent(typeof(NavMeshAgent))]
public class RandomWander : AIStrategy<RandomWanderAction> {
	float smoothness = 1;
	NavMeshMover mover;
	NavMeshAgent agent;

	void Start() {
		mover = GetComponent<NavMeshMover> ();
		agent = GetComponent<NavMeshAgent> ();
		MoveIntoRandomDirection ();
	}

	void Update() {
		if (mover.HasArrived) {
			MoveIntoRandomDirection ();
		}
	}

	public override void StartBehavior(RandomWanderAction action) {
		mover.StopMovingAtDestination = true;
		MoveIntoRandomDirection ();
	}

	void MoveIntoRandomDirection() {
		// determine new random point in space
		var dir = Random.insideUnitSphere;
		dir.y = 0;
		var ray = dir * smoothness * agent.speed;
		var newPos = transform.position + ray;


		// project point onto NavMesh
		NavMeshHit hit;
		if (NavMesh.SamplePosition (newPos, out hit, 1000, NavMesh.AllAreas)) {
			// Go!
			mover.CurrentDestination = hit.position;
		}
	}

	protected override void OnStop() {
		mover.StopMove ();
	}
}
