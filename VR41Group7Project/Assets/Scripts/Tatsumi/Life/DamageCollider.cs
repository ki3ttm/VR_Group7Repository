using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour {
	[SerializeField, Tooltip("処理を行うライフ管理コンポーネント")]
	LifeManager lifeMng = null;

	void OnCollisionEnter(Collision _col) {
		//		Debug.Log(_col.collider.name);

		lifeMng.Hit(_col.collider);
	}

//	void OnTriggerEnter(Collider _col) {
//		lifeMng.Hit(_col);
//	}
}
