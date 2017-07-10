using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WASDClickToShootPlayerControl : PlayerControlBase {
	public float speed = 8;
	bool mouseDown;
	Plane testPlane;

	public bool IsOnNavMesh(out NavMeshHit hit) {
		return NavMesh.Raycast (transform.position, transform.position + Vector3.down * 100, out hit, NavMesh.AllAreas);
	}

	void Start() {
		// create a plane at 0,0,0 whose normal points to +Y
		testPlane = new Plane ();
	}

	void Update() {
		CheckClick ();
	}

	void FixedUpdate() {
		Move ();
	}

	void Move() {
		MoveByInput();
		//PlaceOnNavMesh ();
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
		// see: http://answers.unity3d.com/questions/269760/ray-finding-out-x-and-z-coordinates-where-it-inter.html
		// cast ray onto plane that goes through our current position and normal points upward
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		testPlane.SetNormalAndPosition (Vector3.up, transform.position);
		float distance; 
		if (testPlane.Raycast(ray, out distance)){
			var target = ray.GetPoint(distance);
			NextAction = new Strategies.ShootInDirectionAction {
				destination = target
			};
		}
	}
}