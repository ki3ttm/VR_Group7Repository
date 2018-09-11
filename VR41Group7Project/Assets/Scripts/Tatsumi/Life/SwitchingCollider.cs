using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingCollider : MonoBehaviour {
	[SerializeField, Tooltip("無効化するオブジェクト")]
	Transform disableObj = null;
	[SerializeField, Tooltip("有効化するオブジェクト")]
	Transform enableObj = null;

	void OnCollisionEnter(Collision _col) {
		enableObj.gameObject.SetActive(true);
		disableObj.gameObject.SetActive(false);
	}
}
