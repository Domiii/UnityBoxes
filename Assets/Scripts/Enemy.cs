using UnityEngine;
using System.Collections;

public class Enemy : FactionMember {
	void Reset() {
		FactionType = FactionType.Enemy;
	}

	void OnDeath(DamageInfo info) {
		EnemyManager.Instance.FinishedEnemy ();
	}
}
