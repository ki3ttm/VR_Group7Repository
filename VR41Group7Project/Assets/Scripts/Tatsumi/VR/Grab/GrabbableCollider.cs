using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableCollider : MonoBehaviour {
	[SerializeField, Tooltip("掴まれるオブジェクト\n指定されていない場合、自身にGrabbableObjectがアタッチされていれば起動時に取得されます。")]
	GrabbableObject obj = null;
	public GrabbableObject Obj {
		get {
			return obj;
		}
	}

	void Awake() {
		Collider col = GetComponent<Collider>();
		if (!col) {
			Debug.LogWarning(name + "にコライダーが存在しません。\nGrabbableColliderが正常に機能していない可能性があります。");
		}
		if (!col.isTrigger) {
			Debug.LogWarning(name + "にIsTriggerがfalseのコライダーが存在します。\nGrabbableColliderが正常に機能していない可能性があります。");
		}

		if (!obj) {
			obj = GetComponent<GrabbableObject>();
		}
		if (obj) {
			obj.GrabbableColList.Add(this);
		}
		else {
			Debug.LogWarning(name + "のObjが設定されていません。\nGrabbableColliderが正常に機能していない可能性があります。");
		}
	}
}
