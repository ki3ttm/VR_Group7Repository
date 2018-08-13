using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 起動時に指定のオブジェクトが有効なら自身をそのオブジェクトの子に設定
public class RelateParentActiveObject : MonoBehaviour {
	[SerializeField]
	GameObject parentTarget = null;			// 対象
	[SerializeField]
	bool useBeforeLocalPosition = false;	// 関連付け前のローカルでの位置をそのまま使用するか
	[SerializeField]
	bool useBeforeLocalRotation = false;	// 関連付け前のローカルでの向きをそのまま使用するか
	[SerializeField]
	bool useBeforeLocalScale = false;		// 関連付け前のローカルでの拡縮をそのまま使用するか

	void Start () {
		if (parentTarget && parentTarget.activeInHierarchy) {
			Vector3 localPos = Vector3.zero;
			Quaternion localRot = Quaternion.identity;
			Vector3 localScl = Vector3.one;

			if (useBeforeLocalPosition) {
				// 元のローカルでの位置を取得
				localPos = transform.localPosition;
			}
			if (useBeforeLocalRotation) {
				// 元のローカルでの向きを取得
				localRot = transform.localRotation;
			}
			if (useBeforeLocalScale) {
				// 元のローカルでの拡縮を取得
				localScl = transform.localScale;
			}

			// 親子関係に設定
			transform.parent = parentTarget.transform;

			// 元のローカルでの位置を設定
			transform.localPosition = localPos;
			
			// 元のローカルでの向きを設定
			transform.localRotation = localRot;

			// 元のローカルでの拡縮を設定
			transform.localScale = localScl;
		}
	}
}
