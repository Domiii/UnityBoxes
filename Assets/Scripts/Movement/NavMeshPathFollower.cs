using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;


public class NavMeshPathFollower : NavMeshMover {
	public Path path;
	public Path.FollowDirection direction;
	public Path.RepeatMode mode;

	float maxDistanceToGoal;
	IEnumerator<Transform> pathIterator;

	#region Public
	public void SetPath(Path path, Path.FollowDirection pathDirection = Path.FollowDirection.Forward, Path.RepeatMode mode = Path.RepeatMode.Once) {
		Debug.Assert (path != null);

		this.path = path;
		this.direction = pathDirection;
		this.mode = mode;

		RestartPath ();
	}

	public void RestartPath() {
		if (path != null) {
			pathIterator = path.GetPathEnumerator (direction);
			pathIterator.MoveNext ();
		}
	}
	#endregion


	void Start () {
		RestartPath ();
	}

	void Update () {
		MoveAlongPath ();
	}

	void MoveAlongPath() {
		if (pathIterator == null || pathIterator.Current == null)
			return;
		
		// move towards current target
		CurrentDestination = pathIterator.Current.position;
	}

	protected override void OnStartMove ()
	{
	}

	protected override void OnStopMove ()
	{
		pathIterator.MoveNext();
		//print("Next Waypoint: " + pathIterator.Current);

		if (pathIterator.Current == null) {
			// finished path once
			switch (mode) {
			case Path.RepeatMode.Once:
				// done!
				break;
			case Path.RepeatMode.Mirror:
				// reverse direction and walk back
				direction = direction == Path.FollowDirection.Forward ? Path.FollowDirection.Backward : Path.FollowDirection.Forward;
				RestartPath ();
				MoveAlongPath ();
				break;
			case Path.RepeatMode.Repeat:
				// start from the beginning
				RestartPath ();
				MoveAlongPath ();
				break;
			}
		}
	}
}
