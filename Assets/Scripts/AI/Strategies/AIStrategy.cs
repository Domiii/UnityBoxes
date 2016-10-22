using UnityEngine;
using System.Collections;

/// <summary>
/// AI strategies are high-level components used to control actuators.
/// Brains decide which strategy to activate at any given time.
/// </summary>
public abstract class AIStrategy<A> : AIStrategy 
	where A : AIAction
{
	public virtual void StartBehavior (A action) {
	}

	public override void StartStrategy(AIAction action) {
		StartBehavior ((A)action);
	}
}


public class AIStrategy : MonoBehaviour {
	public System.Action<AIStrategy> FinishedHandler;


	public virtual void StartStrategy(AIAction action) {
	}

	public void StopStrategy() {
		NotifyFinished ();
		OnStop ();
	}

	protected void NotifyFinished() {
		if (FinishedHandler != null) {
			FinishedHandler (this);
		}
	}

	protected virtual void OnStop() {
	}
}