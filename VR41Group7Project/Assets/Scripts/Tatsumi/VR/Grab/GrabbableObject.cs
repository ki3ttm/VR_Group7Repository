using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour {
	public enum GrabCorrectType {
		NoCorrectGrab,											// 補正を行わずに掴んだ時の位置をそのまま掴む
		ColliderCenterPositionCorrectGrab,						// コライダーから位置を取得して補正する
		ColliderCenterPositionAndTransformRotationCorrectGrab,	// コライダーから位置を取得しTransformから向きを取得して補正する
	}

	[SerializeField, Tooltip("掴んだ時の位置や向きの補正タイプ")]
	GrabCorrectType correctType = GrabCorrectType.NoCorrectGrab;
	public GrabCorrectType CorrectType {
		get {
			return correctType;
		}
	}

	[SerializeField, Tooltip("掴む操作")]
	VirtualViveController.ViveControllerInput grabInput = VirtualViveController.ViveControllerInput.HairTriggerKeyDown;

	[SerializeField, Tooltip("離す操作")]
	VirtualViveController.ViveControllerInput releaseInput = VirtualViveController.ViveControllerInput.HairTriggerKeyUp;

	[SerializeField, Tooltip("掴まれているか")]
	bool isGrab = false;
	public bool IsGrab {
		get {
			return isGrab;
		}
		set {
			if (isGrab == value) return;
			isGrab = value;
			if (isGrab) {
				grabEv.Invoke();
			} else {
				releaseEv.Invoke();
			}
		}
	}

	[SerializeField, Tooltip("掴まれるコライダーのリスト"), ReadOnly]
	List<GrabbableCollider> grabbableColList = new List<GrabbableCollider>();
	public List<GrabbableCollider> GrabbableColList {
		get {
			return grabbableColList;
		}
	}

	[SerializeField, Tooltip("掴まれる前の親オブジェクト"), ReadOnly]
	Transform defParent = null;
	public Transform DefParent {
		get {
			return defParent;
		}
	}

	[SerializeField, Tooltip("掴まれる前のRigidbodyのIsKinematic"), ReadOnly]
	bool defIsKinematic = false;
	public bool DefIsKinematic {
		get {
			return defIsKinematic;
		}
	}

	[SerializeField, Tooltip("掴まれた時のイベント")]
	UnityEngine.Events.UnityEvent grabEv = new UnityEngine.Events.UnityEvent();
	[SerializeField, Tooltip("離された時のイベント")]
	UnityEngine.Events.UnityEvent releaseEv = new UnityEngine.Events.UnityEvent();

	void Start() {
		SetDefaultState();
	}

	public bool CheckGrabInput(VirtualViveController _virtualCtrl) {
		if (VirtualViveController.GetInput(_virtualCtrl, grabInput)) {
			return true;
		}
		return false;
	}

	public bool CheckReleaseInput(VirtualViveController _virtualCtrl) {
		if (VirtualViveController.GetInput(_virtualCtrl, releaseInput)) {
			return true;
		}
		return false;
	}

	public void SetDefaultState() {
		defParent = transform.parent;
		defIsKinematic = GetComponent<Rigidbody>().isKinematic;
	}
}
