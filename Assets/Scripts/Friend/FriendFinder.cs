using UnityEngine;
using System.Collections;

namespace Strategies {
	/// <summary>
	/// Follows
	/// </summary>
	public class FriendFinder : TargetFinder {
		public float radius = 10;
		public float followDistance = 5;
		public Living friend;
		public bool changeMaterial = true;
		public MonoBehaviour[] enableComponentOnFriend;

		public bool IsCurrentValid {
			get {
				return IsValidTarget (friend);
			}
		}

		public bool IsCloseToTarget {
			get {
				return IsInRange (friend, followDistance);
			}
		}

		public override bool IsValidTarget (Living target)
		{
			return target != null && target.gameObject != gameObject && FactionManager.AreAllied(gameObject, target.gameObject);
		}

		public bool IsInRange (Living target) {
			return IsInRange (target, radius);
		}


		void Update() {
			if (!friend) {
				friend = FindTarget (radius);
				if (friend) {
					OnNewFriend ();
				}
			}
		}

		void OnNewFriend() {
			// change material to friend's material
			if (changeMaterial && friend && friend.GetComponent<Renderer> ()) {
				var targetRenderer = friend.GetComponent<Renderer> ();
				transform.ForEachComponentInHierarchy<Renderer> (renderer => {
					renderer.material = targetRenderer.material;
				});
			}

			// enable some components
			if (enableComponentOnFriend != null) {
				enableComponentOnFriend.ForEach (c => c.enabled = true);
			}
		}
	}
}