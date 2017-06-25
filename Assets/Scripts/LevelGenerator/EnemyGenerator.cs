using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {
	[Range(1, 100)]
	public float strength = 10;

	[Range(1, 100)]
	public int nMaxEnemies = 5;

	public GameObject enemyContainer;

	public bool IsGenerated {
		get {
			return enemyContainer.transform.childCount > 0;
		}
	}

	void Awake() {
		if (enemyContainer == null) {
			enemyContainer = gameObject;
		}
		if (!IsGenerated) {
			Generate ();
		}
	}

	public void Generate() {
		Clear ();

		var nEnemies = Random.Range (1, nMaxEnemies);
		for (var i = 0; i < nEnemies; ++i) {
			GenEnemy (i);
		}
	}

	ProceduralEnemy GenEnemy(int i) {
		var go = new GameObject ("Enemy" + (i+1), typeof(ProceduralEnemy));
		var enemy = go.GetComponent<ProceduralEnemy> ();
		enemy.Generate ();
		enemy.transform.parent = enemyContainer.transform;
		return enemy;
	}

	public void Clear() {
		for (var i = enemyContainer.transform.childCount-1; i >= 0; --i)
		{
			var child = enemyContainer.transform.GetChild (i);
			DestroyImmediate(child.gameObject);
		}
	}
}
