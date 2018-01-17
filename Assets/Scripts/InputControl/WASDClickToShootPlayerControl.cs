using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WASDClickToShootPlayerControl : PlayerControlBase {
	public float moveSpeed = 8;
	public float turnSpeed = 180;

	bool mouseDown;
	Plane testPlane;


	public bool IsOnNavMesh(out NavMeshHit hit) {
		return NavMesh.Raycast (transform.position, transform.position + Vector3.down * 100, out hit, NavMesh.AllAreas);
	}

	void Start() {
		// create a plane at 0,0,0 whose normal points to +Y
		testPlane = new Plane ();
		var shooter = GetComponent<Shooter> ();
		if (shooter) {
			turnSpeed = shooter.turnSpeed;
		}
	}

	void Update () {
		// get input in Update()
		CheckClick ();
	}

	void FixedUpdate () {
		// update physics in FixedUpdate()

		Move ();
		UpdateDirection ();
	}

	Transform GetTransform() {
		if (GetComponent<Shooter> ()) {
			return GetComponent<Shooter> ().rotationTransform;
		}
		return transform;
	}

	Quaternion GetRotationToward(Vector3 target) {
		Vector3 dir = target - GetTransform().position;
		return GetRotationFromDirection(dir);
	}

	Quaternion GetRotationFromDirection(Vector3 dir) {
		var angle = Mathf.Atan2 (dir.x, dir.z) * Mathf.Rad2Deg;
		return Quaternion.AngleAxis(angle, Vector3.up);
	}


	void UpdateDirection () {
		// rotate
		Vector3 target;
		if (GetMouseWorldPos (out target)) {
			var targetRotation = GetRotationToward (target);
			var t = GetTransform ();
			t.rotation = Quaternion.RotateTowards (t.rotation, targetRotation, Time.deltaTime * turnSpeed);
		}
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

		transform.position = transform.position + delta * moveSpeed * Time.fixedDeltaTime;
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
			// mouse click -> start something
			// 點滑鼠 -> 開始做事
			HandleMouse (true);
			mouseDown = true;
		} else if (Input.GetMouseButton (0)) {
			// mouse button is pressed all the time
			// 繼續點滑鼠
			HandleMouse (false);
		} else if (mouseDown) {
			// mouse released -> stop it
			// 滑鼠被放解
			mouseDown = false;
			NextAction = Strategies.IdleAction.Default;
		} 
	}

	bool GetMouseWorldPos(out Vector3 v) {
		// see: http://answers.unity3d.com/questions/269760/ray-finding-out-x-and-z-coordinates-where-it-inter.html
		// cast ray onto plane that goes through our current position and normal points upward
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		testPlane.SetNormalAndPosition (Vector3.up, transform.position);
		float distance; 
		if (testPlane.Raycast (ray, out distance)) {
			v = ray.GetPoint (distance);
			return true;
		}
		v = Vector3.zero;
		return false;
	}

	void HandleMouse(bool clicked) {
		Vector3 target;
		if (GetMouseWorldPos(out target)) {
			NextAction = new Strategies.ShootInDirectionAction {
				destination = target
			};
		}
	}
}