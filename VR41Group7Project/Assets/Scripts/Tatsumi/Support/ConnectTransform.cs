using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectTransform : MonoBehaviour {
	public enum Axis {
		X,
		Y,
		Z,
	}

	public enum Mode {
		StartUpdate,		// Start()で更新を行う
		AlwaysUpdate,		// Update()で常に更新を行う
		MoveTriggerUpdate,	// 接続地点が移動した時に更新を行う
	}
	
	[SerializeField, Tooltip("接続地点")]
	Transform fromTrans = null, toTrans = null;

	[SerializeField, Tooltip("接続されている座標"), ReadOnly]
	Vector3 fromPoint = Vector3.zero, toPoint = Vector3.zero;

	[SerializeField, Tooltip("更新モード")]
	Mode updateMode = Mode.MoveTriggerUpdate;
	
	[SerializeField, Tooltip("正面(接続)方向の軸")]
	Axis forwardAxis = Axis.Z;

	[SerializeField, Tooltip("正面軸以外の大きさ")]
	float width = 0.1f;

	[SerializeField, Tooltip("軸に沿った回転角")]
	float rotAngle = 0.0f;

	[SerializeField, Tooltip("軸の長さの拡縮率")]
	float lenRatio = 1.0f;

	void Start() {
		// 接続するようにTransformを設定
		Connecting();
	}

	void Update () {
		// Start()時しかTransformを更新しない場合
		if (updateMode == Mode.StartUpdate) return;

		// 接続地点の移動時にしかTransformを更新しない場合
		if ((updateMode == Mode.MoveTriggerUpdate) &&
			(fromPoint == fromTrans.position) && (toPoint == toTrans.position)) return;

		// 接続するようにTransformを設定
		Connecting();
	}

	[ContextMenu("Connectiong")]
	void Connecting() {
		if (!fromTrans || !toTrans) return;

		// 位置を取得
		fromPoint = fromTrans.position;
		toPoint = toTrans.position;

		// 中心位置に移動
		transform.position = ((fromPoint + toPoint) * 0.5f);

		// 指定の軸が正面に向くように回転
		transform.rotation = (Quaternion.LookRotation(toPoint - fromPoint) * GetRotQuaternion());

		// 正面軸の長さと他軸の太さを設定
		Vector3 scl = (width * Vector3.one);
		switch (forwardAxis) {
		case Axis.X:
			scl.x = (Vector3.Distance(fromPoint, toPoint) * lenRatio);
			break;
		case Axis.Y:
			scl.y = (Vector3.Distance(fromPoint, toPoint) * lenRatio);
			break;
		case Axis.Z:
			scl.z = (Vector3.Distance(fromPoint, toPoint) * lenRatio);
			break;
		}
		transform.localScale = scl;

		// 軸に沿った回転を加える
		if (rotAngle != 0.0f) {
			switch (forwardAxis) {
			case Axis.X:
				transform.Rotate((Vector3.left * rotAngle), Space.Self);
				break;
			case Axis.Y:
				transform.Rotate((Vector3.up * rotAngle), Space.Self);
				break;
			case Axis.Z:
				transform.Rotate((Vector3.back * rotAngle), Space.Self);
				break;
			}
		}
	}

	Quaternion GetRotQuaternion() {
		Quaternion ret = Quaternion.identity;
		if (forwardAxis == Axis.X) {
			ret = Quaternion.Euler(0.0f, -90.0f, 0.0f);
		} else if (forwardAxis == Axis.Y) {
			ret = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
		}
		return ret;
	}
}
