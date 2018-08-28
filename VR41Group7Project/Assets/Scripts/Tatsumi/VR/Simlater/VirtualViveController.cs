using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualViveController : MonoBehaviour {
	public enum ViveControllerInput {
		TouchPadKeyDown,
		TouchPadKey,
		TouchPadKeyUp,
		HairTriggerKeyDown,
		HairTriggerKey,
		HairTriggerKeyUp,
		GripKeyKeyDown,
		GripKeyKey,
		GripKeyKeyUp,
		SystemKeyKeyDown,
		SystemKeyKey,
		SystemKeyKeyUp,
	}

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
	[SerializeField, Tooltip("受け付けられた入力状態"), ReadOnly]
	bool fixedVirtualTouchKey = false, fixedVirtualTriggerKey = false, fixedVirtualGripKey = false;
	// 前回更新持に使用された入力状態
	bool prevVirtualTouchKey = false, prevVirtualTriggerKey = false, prevVirtualGripKey = false;

	[SerializeField, Tooltip("移動時に追従するCameraRig")]
	Transform followCamRig = null;
	// 前回更新時のCameraRigの位置
	Vector3 prevFollowCamRigPos = Vector3.zero;

	SteamVR_Controller.Device SteamVRController {
		get {
			return SteamVR_Controller.Input((int)trackedObj.index);
		}
	}

	Vector3 simVelocity = Vector3.zero;
	Vector3 simAngularVelocity = Vector3.zero;
	Vector3 prevFixedPos = Vector3.zero;
	Vector3 prevFixedRot = Vector3.zero;

	void Start() {
		rb = GetComponent<Rigidbody>();
		if (followCamRig) {
			prevFollowCamRigPos = followCamRig.position;
		}

		prevFixedPos = transform.position;
		prevFixedRot = transform.rotation.eulerAngles;
	}
	
	void Update () {
		// SteamVRのコントローラーが有効ならその子になる
		if (trackedObj.gameObject.activeInHierarchy && (transform.parent != trackedObj.transform)) {
			transform.parent = trackedObj.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}

		// CameraRigの移動に合わせて位置を補正
		if (!trackedObj.gameObject.activeInHierarchy && followCamRig && (prevFollowCamRigPos != followCamRig.position)) {
			transform.position += (-prevFollowCamRigPos + followCamRig.position);
			prevFollowCamRigPos = followCamRig.position;
		}
//	}
//
//	void FixedUpdate() {
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

		simVelocity = (transform.position - prevFixedPos) / Time.deltaTime;
		simAngularVelocity = (transform.rotation.eulerAngles - prevFixedRot) / Time.deltaTime;
		prevFixedPos = transform.position;
		prevFixedRot = transform.rotation.eulerAngles;
	}

	public Vector3 velocity {
		get {
			// SteamVR
			if (trackedObj.enabled && (trackedObj.index != SteamVR_TrackedObject.EIndex.None)) {
				return SteamVRController.velocity;
			}
			// シミュレート
			 else if (rb) {
				return simVelocity;
			}
			return Vector3.zero;
		}
	}

	public Vector3 angularVelocity {
		get {
			// SteamVR
			if (trackedObj.enabled && (trackedObj.index != SteamVR_TrackedObject.EIndex.None)) {
				return SteamVRController.angularVelocity;
			}
			// シミュレート
			else if (rb) {
				return simAngularVelocity;
			}
			return Vector3.zero;
		}
	}

	public bool GetPress(ulong buttonMask) {
		if (trackedObj.enabled && (trackedObj.index != SteamVR_TrackedObject.EIndex.None)) {
			return SteamVRController.GetPress(buttonMask);
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
			return SteamVRController.GetPressUp(buttonMask);
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
			return SteamVRController.GetPressDown(buttonMask);
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
			return SteamVRController.GetAxis();
		}
		else {
			return Vector2.zero;
		}
	}

	public bool GetTouchPad() {
		return GetPress(SteamVR_Controller.ButtonMask.Touchpad);
	}
	public bool GetTouchPadDown() {
		return GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);
	}
	public bool GetTouchPadUp() {
		return GetPressUp(SteamVR_Controller.ButtonMask.Touchpad);
	}
	public bool GetHairTrigger() {
		return GetPress(SteamVR_Controller.ButtonMask.Trigger);
	}
	public bool GetHairTriggerDown() {
		return GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
	}
	public bool GetHairTriggerUp() {
		return GetPressUp(SteamVR_Controller.ButtonMask.Trigger);
	}
	public bool GetGrip() {
		return GetPress(SteamVR_Controller.ButtonMask.Grip);
	}
	public bool GetGripDown() {
		return GetPressDown(SteamVR_Controller.ButtonMask.Grip);
	}
	public bool GetGripUp() {
		return GetPressUp(SteamVR_Controller.ButtonMask.Grip);
	}
	public bool GetSystem() {
		return GetPress(SteamVR_Controller.ButtonMask.System);
	}
	public bool GetSystemDown() {
		return GetPressDown(SteamVR_Controller.ButtonMask.System);
	}
	public bool GetSystemUp() {
		return GetPressUp(SteamVR_Controller.ButtonMask.System);
	}

	static public bool GetInput(VirtualViveController _virtualCtrl, ViveControllerInput _input) {
		switch (_input) {
		case VirtualViveController.ViveControllerInput.TouchPadKeyDown:
			return _virtualCtrl.GetTouchPadDown();
		case VirtualViveController.ViveControllerInput.TouchPadKey:
			return _virtualCtrl.GetTouchPad();
		case VirtualViveController.ViveControllerInput.TouchPadKeyUp:
			return _virtualCtrl.GetTouchPadUp();
		case VirtualViveController.ViveControllerInput.HairTriggerKeyDown:
			return _virtualCtrl.GetHairTriggerDown();
		case VirtualViveController.ViveControllerInput.HairTriggerKey:
			return _virtualCtrl.GetHairTrigger();
		case VirtualViveController.ViveControllerInput.HairTriggerKeyUp:
			return _virtualCtrl.GetHairTriggerUp();
		case VirtualViveController.ViveControllerInput.GripKeyKeyDown:
			return _virtualCtrl.GetGripDown();
		case VirtualViveController.ViveControllerInput.GripKeyKey:
			return _virtualCtrl.GetGrip();
		case VirtualViveController.ViveControllerInput.GripKeyKeyUp:
			return _virtualCtrl.GetGripUp();
		case VirtualViveController.ViveControllerInput.SystemKeyKeyDown:
			return _virtualCtrl.GetSystemDown();
		case VirtualViveController.ViveControllerInput.SystemKeyKey:
			return _virtualCtrl.GetSystem();
		case VirtualViveController.ViveControllerInput.SystemKeyKeyUp:
			return _virtualCtrl.GetSystemUp();
		}
		return false;
	}
}
