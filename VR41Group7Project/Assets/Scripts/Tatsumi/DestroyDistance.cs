using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDistance : MonoBehaviour {
	[SerializeField, Tooltip("指定距離")]
	float dis = 100.0f;
	[SerializeField, Tooltip("距離を測る位置")]
	Transform point = null;
	Vector3 Point {
		get {
			if (!point) {
				return Vector3.zero;
			}
			return point.position;
		}
	}
	[SerializeField, Tooltip("削除オブジェクト")]
	Transform destroyTarget = null;
	Transform DestroyTarget {
		get {
			if (!destroyTarget) {
				return transform;
			}
			return destroyTarget;
		}
	}


	void FixedUpdate () {
		bool overDis = (Vector3.Distance(Point, transform.position) > dis);
		if (overDis) {
			Destroy(DestroyTarget.gameObject);
		}
	}
}
