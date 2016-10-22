using UnityEngine;
using System.Collections;

public class WASDClickToShootPlayerControl : PlayerControlBase {
	public float speed = 8;
	bool mouseDown;

	public bool IsOnNavMesh(out NavMeshHit hit) {
		return NavMesh.Raycast (transform.position, transform.position + Vector3.down * 100, out hit, NavMesh.AllAreas);
	}

	void Update() {
		CheckClick ();
	}

	void FixedUpdate() {
		Move ();
	}

	void Move() {
		MoveByInput();
		PlaceOnNavMesh ();
	}

	void MoveByInput() {
		var delta = new Vector3 ();
		delta.x = Input.GetAxis("Horizontal");
		delta.z = Input.GetAxis("Vertical");
		delta.Normalize ();

		transform.position = transform.position + delta * speed * Time.fixedDeltaTime;
	}

	void PlaceOnNavMesh() {
		NavMeshHit hit;
		if (!IsOnNavMesh(out hit)) {
			// place back on navmesh
			if (NavMesh.SamplePosition (transform.position, out hit, 100, NavMesh.AllAreas)) {
				// get bounds, and place bottom face of bounds at position
				var mesh = GetComponent<Collider>();
				var originHeight = -mesh.bounds.min.y;
				transform.position = hit.position + Vector3.up * originHeight;
			}
		}
	}

	void CheckClick() {
		if (Input.GetMouseButtonDown (0)) {
			HandleMouse (true);
			mouseDown = true;
		} else if (Input.GetMouseButton (0)) {
			HandleMouse (false);
		} else if (mouseDown) {
			// idle
			mouseDown = false;
			NextAction = Strategies.IdleAction.Default;
		} 
	}

	void HandleMouse(bool clicked) {
		Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast(screenRay, out hit))
		{
			//HuntOrMoveToDestination (hit, clicked);
			var target = hit.point;
			target.y = transform.position.y;
			NextAction = new Strategies.ShootInDirectionAction {
				destination = target
			};
		}
	}
}