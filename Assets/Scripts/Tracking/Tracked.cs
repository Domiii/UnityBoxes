using UnityEngine;
using System.Collections;

public class Tracked : MonoBehaviour {

	public bool IsOnCamera {
		get;
		private set;
	}

	public UnityEngine.UI.Image ArrowImage {
		get;
		set;
	}

	public Vector3 ViewportPoint {
		get;
		private set;
	}

	void Start() {
		UpdateTrackStatus (true);
	}

	void Update() {
		UpdateTrackStatus ();
	}

	void UpdateTrackStatus(bool force = false) {
		ViewportPoint = Camera.main.WorldToViewportPoint (transform.position);
		var wasOncamera = IsOnCamera;
		IsOnCamera = Mathf.Clamp (ViewportPoint.x, 0, 1) == ViewportPoint.x && Mathf.Clamp (ViewportPoint.y, 0, 1) == ViewportPoint.y && ViewportPoint.z >= 0;
		if (force || IsOnCamera != wasOncamera) {
			ObjectTracker.Instance.UpdateTrackStatus (this);
		}
	}

	void OnDeath(DamageInfo damageInfo) {
		ObjectTracker.Instance.StopTracking(this);
	}

}
