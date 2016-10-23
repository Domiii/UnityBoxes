using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	bool isPaused;

	public static GameManager Instance {
		get;
		private set;
	}

	GameManager() {
		Instance = this;
	}

	void Awake() {
		Reset ();
	}

	void Reset() {
		IsPaused = false;
		Time.timeScale = 1;
	}

	public bool IsPaused {
		get {
			return isPaused;
		}
		set {
			if (isPaused != value) {
				if (value) {
					// pause
					Time.timeScale = 0;
				} else {
					// unpause!
					Time.timeScale = 1;
				}
				isPaused = value;
			}
		}
	}
}
