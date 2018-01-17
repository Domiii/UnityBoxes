using UnityEngine;
using System.Collections;

namespace Strategies {
	public class FollowFriendAction : AIAction {
		public Living target;
	}

	/// <summary>
	/// Attack given target when close enough; else move and catch up
	/// </summary>
	[RequireComponent(typeof(FriendFinder))]
	[RequireComponent(typeof(NavMeshMover))]
	public class FollowFriend : AIStrategy<FollowFriendAction> {
		FriendFinder finder;
		NavMeshMover mover;
		bool isMoving = false;
		Living target;

		#region Public
		public Living CurrentTarget {
			get {
				return finder.friend;
			}
		}

		public override void StartBehavior(FollowFriendAction action) {
			target = action.target;
			mover.StopMovingAtDestination = false;
		}
		#endregion

		void Awake () {
			finder = GetComponent<FriendFinder> ();
			mover = GetComponent<NavMeshMover> ();
		}

		protected override void UpdateStrategy() {
			if (finder.IsCurrentValid && !finder.IsCloseToTarget) {
				// target out of range -> move toward target
				if (!isMoving) {
					isMoving = true;
					StopOtherStrategies ();
				}
				mover.CurrentDestination = finder.friend.transform.position;
			}
			else {
				isMoving = false;
				mover.StopMove();
				StartOtherStrategies();
			}
		}

		/// <summary>
		/// Called when finished hunting.
		/// </summary>
		protected override void OnStop() {
			mover.StopMove ();
		}
	}
}