using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #########################################################

// TODO: More random parameters
// Enemy count parameters
// Enemy positioning parameters
// Enemy grouping parameters

// TODO: More interesting sscene + game design elements
// Obstacles
// Pick-ups
// Levels
// Difficulty progression

// TODO: Line of sight (LoS) for shooting + movement AI

// #########################################################

public class EnemyGenerator : MonoBehaviour {
	[Range(1, 100)]
	public float minStrength = 1;
	[Range(1, 100)]
	public float maxStrength = 10;

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
		enemy.strength = Random.Range (minStrength, maxStrength);
		enemy.Generate ();
		PlaceEnemy (enemy);
		return enemy;
	}

	void PlaceEnemy(ProceduralEnemy enemy) {
		enemy.transform.parent = enemyContainer.transform;
		enemy.transform.localPosition = Vector3.zero;
		enemy.FixYPosition ();
	}

	public void Clear() {
		for (var i = enemyContainer.transform.childCount-1; i >= 0; --i)
		{
			var child = enemyContainer.transform.GetChild (i);
			DestroyImmediate(child.gameObject);
		}
	}
}
