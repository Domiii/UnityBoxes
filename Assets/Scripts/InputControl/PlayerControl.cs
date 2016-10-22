using UnityEngine;
using System.Collections;


/// <summary>
/// 用 Input 來控制 Player 的 行為
/// </summary>
public class PlayerControl : BehaviorController {
	bool isMoving;

	public AIAction NextAction {
		get;
		private set;
	}

	/// <summary>
	/// Get player's intended action and resets NextAction, indicating that the action has been handled.
	/// </summary>
	public override AIAction PopAction() {
		if (NextAction != null) {
			var action = NextAction;
			NextAction = null;
			return action;
		}
		return null;
	}

	void Update()
	{
		CheckClick ();
	}

	void CheckClick() {
		if (Input.GetMouseButtonDown (0))
		{
			HandleMouse (true);
		}
		else if (Input.GetMouseButton(0))
		{
			HandleMouse (false);
		}
	}

	void HandleMouse(bool clicked) {
		Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast(screenRay, out hit))
		{
			HandleCursorRay (hit, clicked);
		}
	}

	void HandleCursorRay (RaycastHit hit, bool clicked)
	{
		NextAction = null;
		isMoving = false;
		if (clicked) {
			// clicking -> attack or move
			var go = hit.collider.gameObject;
			var unit = go.GetComponent<Unit> ();

			if (unit != null) {
				// clicked a unit -> attack if enemy
				// TODO: Move to Unit, then start attacking
				NextAction = new Strategies.HuntTargetAction {
					target = unit
				};
			} else {
				// did not click on a unit -> start moving
				NextAction = new Strategies.MoveToDestinationAction {
					destination = hit.point
				};
				isMoving = true;
			}
		}
		else {
			// dragging -> keep moving, if already moving
			if (isMoving) {
				NextAction = new Strategies.MoveToDestinationAction {
					destination = hit.point
				};
			}
		}
	}
}