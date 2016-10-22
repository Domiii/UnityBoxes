using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager Instance {
		get;
		private set;
	}

	GameManager() {
		Instance = this;
	}
}
