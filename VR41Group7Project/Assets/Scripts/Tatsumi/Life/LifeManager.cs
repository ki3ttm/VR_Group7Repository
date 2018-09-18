using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager: MonoBehaviour {
	[SerializeField, Tooltip("残りライフ")]
	int life = 3;
	[SerializeField, Tooltip("ダメージ後の無敵時間")]
	float dmgInvincibleTime = 1.0f;
	[SerializeField, Tooltip("ダメージ判定に使用する当たり判定")]
	Collider dmgCol = null;
	[SerializeField, Tooltip("ダメージ時に発生させるイベント")]
	UnityEngine.Events.UnityEvent damageEvents = new UnityEngine.Events.UnityEvent();
	public UnityEngine.Events.UnityEvent DamageEvents {
		get {
			return damageEvents;
		}
	}
	[SerializeField, Tooltip("死亡時に発生させるイベント")]
	UnityEngine.Events.UnityEvent deadEvents = new UnityEngine.Events.UnityEvent();
	public UnityEngine.Events.UnityEvent DeadEvents {
		get {
			return deadEvents;
		}
	}
	[SerializeField, Tooltip("直前のダメージ源オブジェクト"), ReadOnly]
	GameObject prevDmgObj = null;

	public void LifeDamage() {
		CustomDamageSource customDmg = prevDmgObj.GetComponent<CustomDamageSource>();
		if (!customDmg) {
			life--;
		} else {
			life -= customDmg.Dmg;
		}
		if (life <= 0) {
			DeadEvents.Invoke();
		}
	}

	public void DisableCollider() {
		if (dmgCol) {
			dmgCol.enabled = false;
		}
	}

	public void Hit(Collider _col) {
		prevDmgObj = _col.gameObject;
		DamageEvents.Invoke();
	}

	public void SelfDestroy() {
		Destroy(gameObject);
	}
}
