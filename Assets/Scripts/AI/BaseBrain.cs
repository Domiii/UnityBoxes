using UnityEngine;
using System.Collections.Generic;

public class BaseBrain : MonoBehaviour {
	protected List<AIBehavior> behaviors = new List<AIBehavior>();
	protected AIBehavior currentBehavior;

	protected B RegisterBehavior<B>(System.Action finishedHandler = null)
		where B : AIBehavior
	{
		var b = GetComponent<B> ();
		behaviors.Add (b);
		b.FinishedHandler = finishedHandler;
		return b;
	}

	public AIBehavior CurrentBehavior {
		get {
			return currentBehavior;
		}
		protected set {
			if (currentBehavior != value) {
				//print ("Player behavior: " + currentBehavior.name);

				// behavior has changed: disable all behaviors and enable new behavior
				behaviors.ForEach (b => b.enabled = false);
				value.enabled = true;
				currentBehavior = value;
			}
		}
	}
}
