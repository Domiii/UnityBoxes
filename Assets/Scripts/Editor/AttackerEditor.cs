using UnityEngine;
using UnityEditor;

/// <summary>
/// Draws the attack radius of Attacker in editor (assuming y is up in world space)
/// </summary>
[CustomEditor( typeof( Attacker ) )]
public class AttackerEditor : Editor
{
	void OnSceneGUI( )
	{
		var t = (Attacker)target;
		var mesh = t.GetComponent<MeshRenderer>();

		var pos = t.transform.position;

		if (mesh != null) {
			pos.y = mesh.bounds.min.y;
		} else {
			pos.y = t.transform.position.y;
		}
		pos.y += 0.01f; 	// add a small epsilon to reduce chances of interferance between surfaces


		Handles.color = Color.red;
		Handles.DrawWireDisc(pos, Vector3.up, t.AttackRadius);
	}
}