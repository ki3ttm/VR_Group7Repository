using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualViveController : MonoBehaviour {
	// 追跡するVRオブジェクト
	[SerializeField]
	SteamVR_TrackedObject trackedObj = null;
	Rigidbody rb;

	[SerializeField, Tooltip("入力状態\n外部から設定する")]
	bool virtualTouchKey, virtualTriggerKey, virtualGripKey;
	public bool VirtualTouchKey {
		get {
			return virtualTouchKey;
		}
		set {
			virtualTouchKey = value;
		}
	}
	public bool VirtualTriggerKey {
		get {
			return virtualTriggerKey;
		}
		set {
			virtualTriggerKey = value;
		}
	}
	public bool VirtualGripKey {
		get {
			return virtualGripKey;
		}
		set {
			virtualGripKey = value;
		}
	}
	// 今回更新時に使用された入力状態
	bool fixedVirtualTouchKey = false, fixedVirtualTriggerKey = false, fixedVirtualGripKey = false;
	// 前回更新持に使用された入力状態
	bool prevVirtualTouchKey = false, prevVirtualTriggerKey = false, prevVirtualGripKey = false;

	[SerializeField, Tooltip("移動時に追従するCameraRig")]
	Transform followCamRig = null;
	// 前回更新時のCameraRigの位置
	Vector3 prevFollowCamRigPos = Vector3.zero;

	SteamVR_Controller.Device Controller {
		get {
			return SteamVR_Controller.Input((int)trackedObj.index);
		}
	}

	void Start() {
		rb = GetComponent<Rigidbody>();
		if (followCamRig) {
			prevFollowCamRigPos = followCamRig.position;
		}

		// SteamVRのコントローラーが有効ならその子になる
		if (trackedObj.gameObject.activeInHierarchy) {
			transform.parent = trackedObj.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}
	}
	
	void Update () {
		// CameraRigの移動に合わせて位置を補正
		if (!trackedObj.gameObject.activeInHierarchy && followCamRig && (prevFollowCamRigPos != followCamRig.position)) {
			transform.position += (-prevFollowCamRigPos + followCamRig.position);
			prevFollowCamRigPos = followCamRig.position;
		}
	}

	void FixedUpdate() {
		// 前回更新時の入力状態を保持
		prevVirtualTouchKey		= fixedVirtualTouchKey;
		prevVirtualTriggerKey	= fixedVirtualTriggerKey;
		prevVirtualGripKey		= fixedVirtualGripKey;

		// 前回更新から今回更新まで間の入力状態を保持
		fixedVirtualTouchKey	= VirtualTouchKey;
		fixedVirtualTriggerKey	= VirtualTriggerKey;
		fixedVirtualGripKey		= VirtualGripKey;

		// 現在の入力状態を削除
		VirtualTouchKey		= false;
		VirtualTriggerKey	= false;
		VirtualGripKey		= false;
	}

	public Vector3 velocity {
		get {
			if (trackedObj.enabled && (trackedObj.index != SteamVR_TrackedObject.EIndex.None)) {
				return Controller.velocity;
			} else if (rb) {
				return rb.velocity;
			}
			return Vector3.zero;
		}
	}

	public Vector3 angularVelocity {
		get {
			if (trackedObj.enabled && (trackedObj.index != SteamVR_TrackedObject.EIndex.None)) {
				return Controller.angularVelocity;
			} else if (rb) {
				return rb.angularVelocity;
			}
			return Vector3.zero;
		}
	}

	public bool GetPress(ulong buttonMask) {
		if (trackedObj.enabled && (trackedObj.index != SteamVR_TrackedObject.EIndex.None)) {
			return Controller.GetPress(buttonMask);
		} else {
			bool ret = false;
			if ((buttonMask & SteamVR_Controller.ButtonMask.Touchpad) != 0.0f) {
				if (fixedVirtualTouchKey) {
					ret = true;
				} else {
					return false;
				}
			}
			if ((buttonMask & SteamVR_Controller.ButtonMask.Trigger) != 0.0f) {
				if (fixedVirtualTriggerKey) {
					ret = true;
				} else {
					return false;
				}
			}
			if ((buttonMask & SteamVR_Controller.ButtonMask.Grip) != 0.0f) {
				if (fixedVirtualGripKey) {
					ret = true;
				} else {
					return false;
				}
			}
			return ret;
		}
	}
	public bool GetPressUp(ulong buttonMask) {
		if (trackedObj.enabled && (trackedObj.index != SteamVR_TrackedObject.EIndex.None)) {
			return Controller.GetPressUp(buttonMask);
		} else {
			bool ret = false;
			if ((buttonMask & SteamVR_Controller.ButtonMask.Touchpad) != 0.0f) {
				if (!fixedVirtualTouchKey && prevVirtualTouchKey) {
					ret = true;
				} else {
					return false;
				}
			}
			if ((buttonMask & SteamVR_Controller.ButtonMask.Trigger) != 0.0f) {
				if (!fixedVirtualTriggerKey && prevVirtualTriggerKey) {
					ret = true;
				} else {
					return false;
				}
			}
			if ((buttonMask & SteamVR_Controller.ButtonMask.Grip) != 0.0f) {
				if (!fixedVirtualGripKey && prevVirtualGripKey) {
					ret = true;
				} else {
					return false;
				}
			}
			return ret;
		}
	}
	public bool GetPressDown(ulong buttonMask) {
		if (trackedObj.enabled && (trackedObj.index != SteamVR_TrackedObject.EIndex.None)) {
			return Controller.GetPressDown(buttonMask);
		} else {
			bool ret = false;
			if ((buttonMask & SteamVR_Controller.ButtonMask.Touchpad) != 0.0f) {
				if (fixedVirtualTouchKey && !prevVirtualTouchKey) {
					ret = true;
				} else {
					return false;
				}
			}
			if ((buttonMask & SteamVR_Controller.ButtonMask.Trigger) != 0.0f) {
				if (fixedVirtualTriggerKey && !prevVirtualTriggerKey) {
					ret = true;
				} else {
					return false;
				}
			}
			if ((buttonMask & SteamVR_Controller.ButtonMask.Grip) != 0.0f) {
				if (fixedVirtualGripKey && !prevVirtualGripKey) {
					ret = true;
				} else {
					return false;
				}
			}
			return ret;
		}
	}

	public Vector2 GetAxis() {
		if (trackedObj.index != SteamVR_TrackedObject.EIndex.None) {
			return Controller.GetAxis();
		}
		else {
			return Vector2.zero;
		}
	}

	public bool GetHairTouch() {
		return GetPress(SteamVR_Controller.ButtonMask.Touchpad);
	}
	public bool GetHairTouthUp() {
		return GetPressUp(SteamVR_Controller.ButtonMask.Touchpad);
	}
	public bool GetHairTouthDown() {
		return GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);
	}
	public bool GetHairTrigger() {
		return GetPress(SteamVR_Controller.ButtonMask.Trigger);
	}
	public bool GetHairTriggerUp() {
		return GetPressUp(SteamVR_Controller.ButtonMask.Trigger);
	}
	public bool GetHairTriggerDown() {
		return GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
	}
}
