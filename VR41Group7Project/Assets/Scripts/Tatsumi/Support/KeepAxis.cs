using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAxis : MonoBehaviour {
	[SerializeField, Tooltip("位置を固定する軸")]
	bool isKeepPosX = false, isKeepPosY = false, isKeepPosZ = false;
	bool prevIsKeepPosX, prevIsKeepPosY, prevIsKeepPosZ;
	public bool IsKeepPosX {
		get {
			return isKeepPosX;
		}
		set {
			isKeepPosX = value;
			if (value) {
				keepPos.x = transform.position.x;
			}
		}
	}
	public bool IsKeepPosY {
		get {
			return isKeepPosY;
		}
		set {
			isKeepPosY = value;
			if (value) {
				keepPos.y = transform.position.y;
			}
		}
	}
	public bool IsKeepPosZ {
		get {
			return isKeepPosZ;
		}
		set {
			isKeepPosZ = value;
			if (value) {
				keepPos.z = transform.position.z;
			}
		}
	}

	[SerializeField, Tooltip("向きを固定する軸")]
	bool isKeepRotX = false, isKeepRotY = false, isKeepRotZ = false;
	bool prevIsKeepRotX, prevIsKeepRotY, prevIsKeepRotZ;
	public bool IsKeepRotX {
		get {
			return isKeepRotX;
		}
		set {
			isKeepRotX = value;
			if (value) {
				keepRot.x = transform.rotation.x;
			}
		}
	}
	public bool IsKeepRotY {
		get {
			return isKeepRotY;
		}
		set {
			isKeepRotY = value;
			if (value) {
				keepRot.y = transform.rotation.y;
			}
		}
	}
	public bool IsKeepRotZ {
		get {
			return isKeepRotZ;
		}
		set {
			isKeepRotZ = value;
			if (value) {
				keepRot.z = transform.rotation.z;
			}
		}
	}

	[SerializeField, Tooltip("固定されている位置")]
	Vector3 keepPos = Vector3.zero;
	[SerializeField, Tooltip("固定されている向き")]
	Vector3 keepRot = Vector3.zero;

	void Start () {
		KeepPosition(transform.position, IsKeepPosX, IsKeepPosY, IsKeepPosZ);
		KeepRotation(transform.rotation.eulerAngles, IsKeepRotX, IsKeepRotY, IsKeepRotZ);
	}

	void Update() {
		// Inspectorからフラグが変更された場合の固定値を設定
		if (IsKeepPosX != prevIsKeepPosX) {
			prevIsKeepPosX = IsKeepPosX;
			keepPos.x = transform.position.x;
		}
		if (IsKeepPosY != prevIsKeepPosY) {
			prevIsKeepPosY = IsKeepPosY;
			keepPos.y = transform.position.y;
		}
		if (IsKeepPosZ != prevIsKeepPosZ) {
			prevIsKeepPosZ = IsKeepPosZ;
			keepPos.z = transform.position.z;
		}
		if (IsKeepRotX != prevIsKeepRotX) {
			prevIsKeepRotX = IsKeepRotX;
			keepRot.x = transform.rotation.x;
		}
		if (IsKeepRotY != prevIsKeepRotY) {
			prevIsKeepRotY = IsKeepRotY;
			keepRot.y = transform.rotation.y;
		}
		if (IsKeepRotZ != prevIsKeepRotZ) {
			prevIsKeepRotZ = IsKeepRotZ;
			keepRot.z = transform.rotation.z;
		}

		// 固定している位置を保持
		Vector3 pos = transform.position;
		if (IsKeepPosX) {
			pos.x = keepPos.x;
		}
		if (IsKeepPosY) {
			pos.y = keepPos.y;
		}
		if (IsKeepPosZ) {
			pos.z = keepPos.z;
		}
		transform.position = pos;

		// 固定している向きを保持
		Vector3 rot = transform.rotation.eulerAngles;
		if (IsKeepRotX) {
			rot.x = keepRot.x;
		}
		if (IsKeepRotY) {
			rot.y = keepRot.y;
		}
		if (IsKeepRotZ) {
			rot.z = keepRot.z;
		}
		transform.rotation = Quaternion.Euler(rot);
	}

	public void KeepPosition(Vector3 _pos, bool _isKeepPosX, bool _isKeepPosY, bool _isKeepPosZ) {
		IsKeepPosX = _isKeepPosX;
		if (_isKeepPosX) {
			keepPos.x = _pos.x;
		}
		IsKeepPosY = _isKeepPosY;
		if (_isKeepPosY) {
			keepPos.y = _pos.y;
		}
		IsKeepPosZ = _isKeepPosZ;
		if (_isKeepPosZ) {
			keepPos.z = _pos.z;
		}
	}

	public void KeepRotation(Vector3 _rot, bool _isKeepRotX, bool _isKeepRotY, bool _isKeepRotZ) {
		IsKeepRotX = _isKeepRotX;
		if (_isKeepRotX) {
			keepRot.x = _rot.x;
		}
		IsKeepRotY = _isKeepRotY;
		if (_isKeepRotY) {
			keepRot.y = _rot.y;
		}
		IsKeepRotZ = _isKeepRotZ;
		if (_isKeepRotZ) {
			keepRot.z = _rot.z;
		}
	}
}
