using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	#region Life, Health + Death
	public float MaxHealth = 100;
	public float Health = 100;

	public bool IsAlive {
		get { return Health > 0; }
	}

	public bool CanBeAttacked {
		get { return IsAlive; }
	}

	void Die(DamageInfo damageInfo) {
		Health = 0;

		SendMessage ("OnDeath", damageInfo, SendMessageOptions.DontRequireReceiver);

		Destroy (gameObject);
	}
	#endregion


	#region Attack
	public void Damage(DamageInfo damageInfo) {
		if (!IsAlive) {
			// don't do anything when dead
			return;
		}
		
		Health -= damageInfo.Value;
		
		if (!IsAlive) {
			// died from damage
			Die(damageInfo);
		}
	}
   	#endregion

}
