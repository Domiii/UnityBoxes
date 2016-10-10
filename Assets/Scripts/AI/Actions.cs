using UnityEngine;
using System.Collections;


namespace Actions {
	public class Idle : Action {
		public static readonly Idle Default = new Idle();

		public override string Name { get { return "Idle"; } }
	}
	public class Attack : Action {
		public Unit target;

		public override string Name { get { return "Attack"; } }
	}
	public class Move : Action {
		public Vector3 destination;

		public override string Name { get { return "Move"; } }
	}

	public abstract class Action
	{
		public string PreviousActionName;

		public bool HasActionChanged {
			get {
				return PreviousActionName != Name;
			}
		}

		public abstract string Name { get; }
	}
}
