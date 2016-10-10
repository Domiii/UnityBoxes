using UnityEngine;
using System.Collections;

public abstract class Strategy : MonoBehaviour {
	public abstract Actions.Action PopAction ();
}
