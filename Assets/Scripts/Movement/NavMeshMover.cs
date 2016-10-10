using UnityEngine;
using System.Collections;


[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMover : Actuator {
	protected NavMeshAgent agent;

	bool isMoving;
	bool startedMovingFlag;	// used for internal mini hack

	#region Public
	public bool StopMovingAtDestination {
		get;
		set;
	}

	public bool HasArrived {
		get;
		private set;
	}

	public bool IsMoving {
		get { return !isMoving; }
	}

	public Vector3 CurrentDestination {
		get { return agent.destination; }
		set {
			if (!isMoving) {
				// start moving!
				StartMove();
			}

			// update position
			agent.SetDestination (value);

			//print (string.Join(", ", new []{ agent.pathStatus.ToString (), agent.remainingDistance.ToString () }));
		}
	}

	public void StopMove() { 
		if (isMoving) {
			HasArrived = true;
			isMoving = false;
			agent.Stop ();
			OnStopMove ();
		}
	}
	#endregion

	void StartMove() {
		if (!isMoving) {
			HasArrived = false;
			startedMovingFlag = false;
			isMoving = true;
			agent.Resume ();
			OnStartMove ();
		}
	}

	void Awake() {
		StopMovingAtDestination = true;
		agent = GetComponent<NavMeshAgent> ();
	}

	protected virtual void OnStartMove() {
	}

	protected virtual void OnStopMove () {
	}

	void LateUpdate() {
		if (isMoving) {
			if (!startedMovingFlag) {
				// hackfix: during the first update cycle after assigning a target, remainingDistance is still 0!
				startedMovingFlag = true;
			} else if (StopMovingAtDestination) {
				if (agent.remainingDistance <= agent.stoppingDistance) {
					// done!
					StopMove();
				}
			}
		}
	}
}
