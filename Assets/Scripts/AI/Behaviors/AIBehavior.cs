using UnityEngine;
using System.Collections;

/// <summary>
/// AI behaviors are high-level components used to control actuators.
/// Brains decide which behavior to activate at any given time.
/// </summary>
public class AIBehavior : MonoBehaviour {
	public System.Action FinishedHandler;

	public void StopBehavior() {
		NotifyFinished ();
		Cleanup ();
	}

	protected void NotifyFinished() {
		if (FinishedHandler != null) {
			FinishedHandler ();
		}
	}

	protected virtual void Cleanup() {
	}
}
