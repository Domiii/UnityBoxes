using UnityEngine;
using System.Collections;

public class ClickToShootPlayerControl : PlayerControlBase {
	void Update()
	{
		CheckClick ();

		Move ();
	}

	void Move() {
		
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
			//HuntOrMoveToDestination (hit, clicked);

		}
	}
}