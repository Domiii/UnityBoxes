using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralEnemy))]
public class ProceduralEnemyEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		if (GUILayout.Button ("Generate!")) {
			var t = (ProceduralEnemy)target;
			t.Generate ();
		}

		if (GUILayout.Button ("Clear")) {
			var t = (ProceduralEnemy)target;
			t.Clear ();
		}
	}
}
