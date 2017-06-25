using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyGenerator))]
public class EnemyGeneratorEditor : Editor {
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		if (GUILayout.Button ("Generate!")) {
			var t = (EnemyGenerator)target;
			t.Generate ();
		}

		if (GUILayout.Button ("Clear")) {
			var t = (EnemyGenerator)target;
			t.Clear ();
		}
	}
}
