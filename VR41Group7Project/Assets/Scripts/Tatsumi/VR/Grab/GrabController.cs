using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour {
	[SerializeField, Tooltip("持っているオブジェクト"), ReadOnly]
	GrabbableObject grabObj = null;
	public GrabbableObject GrabObj {
		get {
			return grabObj;
		}
		set {
			grabObj = value;
		}
	}

	public bool IsGrabbing {
		get {
			return (GrabObj != null);
		}
	}

	[SerializeField, Tooltip("コントローラーに当たっている掴めるコライダーのリスト"), ReadOnly]
	List<GrabbableCollider> grabColList = new List<GrabbableCollider>();

	[SerializeField, Tooltip("コントローラーに当たっている掴めるオブジェクトのリスト"), ReadOnly]
	List<GrabbableObject> grabObjList = new List<GrabbableObject>();

	VirtualViveController controller = null;
	VirtualViveController Controller {
		get {
			if (!controller) {
				controller = GetComponent<VirtualViveController>();
			}
			return controller;
		}
	}

	void OnTriggerEnter(Collider _col) {
		GrabbableCollider grabCol = _col.GetComponent<GrabbableCollider>();
		if (!grabCol) return;
		if (grabColList.Contains(grabCol)) return;

		// 掴む対象のリストに追加
		grabColList.Add(grabCol);

		// 同一オブジェクトのコライダーがリストになければオブジェクトをリストに追加
		GrabbableObject grabObj = grabCol.Obj;
		if (!grabObjList.Contains(grabObj)) {
			grabObjList.Add(grabObj);
		}
	}
	void OnTriggerExit(Collider _col) {
		GrabbableCollider grabCol = _col.GetComponent<GrabbableCollider>();
		if (!grabCol) return;
		if (!grabColList.Contains(grabCol)) return;
		GrabbableObject grabObj = grabCol.Obj;

		// 掴む対象のリストから削除
		grabColList.Remove(grabCol);

		// 同一オブジェクトのコライダーがリストになければオブジェクトをリストから削除
		foreach (var otherGrabCol in grabColList) {
			if (grabObj == otherGrabCol.Obj) {
				return;
			}
		}
		grabObjList.Remove(grabObj);
	}

	void Update() {
		// 何も掴んでいない場合
		if (!IsGrabbing) {
			// 掴み操作が有効だったオブジェクトを取得
			List<GrabbableObject> grabbingObjList = new List<GrabbableObject>();
			foreach (var grabObj in grabObjList) {
				if (grabObj.CheckGrabInput(Controller)) {
					grabbingObjList.Add(grabObj);
				}
			}
			// 掴み操作が有効なオブジェクトが存在すれば
			if (grabbingObjList.Count > 0) {
				// 最も位置の近いオブジェクトを掴む対象として取得
				GrabbableObject grabTargetObj = null;
				float nearDis = float.MaxValue;
				foreach (var grabbingObj in grabbingObjList) {
					float dis = Vector3.Distance(transform.position, grabbingObj.transform.position);
					if (nearDis > dis) {
						nearDis = dis;
						grabTargetObj = grabbingObj;
					}
				}

				// 掴むオブジェクトの掴めるコライダーを取得
				List<GrabbableCollider> grabbingColList = new List<GrabbableCollider>();
				foreach (var grabCol in grabColList) {
					if (grabCol.Obj == grabTargetObj) {
						grabbingColList.Add(grabCol);
					}
				}
				
				// 最も向きの近いTransformに付いているコライダーを掴むコライダーとして取得
				GrabbableCollider grabTargetCol = null;
				float nearAngle = float.MaxValue;
				foreach (var grabbingCol in grabbingColList) {
					float angle = Vector3.Dot(transform.forward, grabbingCol.transform.forward);
					if (nearAngle > angle) {
						nearAngle = angle;
						grabTargetCol = grabbingCol;
					}
				}

				// 掴む
				GrabObject(grabTargetCol);
			}
		}
		// 既に何か掴んでいる場合
		else {
			// 離し操作が有効であれば
			if (GrabObj.CheckReleaseInput(Controller)) {
				// 離す
				ReleaseObject();
			}
		}
	}

	void GrabObject(GrabbableCollider _grabCol) {
		// 掴み中オブジェクトに設定
		GrabObj = _grabCol.Obj;

		// 掴み中オブジェクトの掴まれる前の状態を保持
		GrabObj.SetDefaultState();

		// 掴み中オブジェクトの親を自身に設定
		GrabObj.transform.parent = transform;

		// 掴み中オブジェクトのRigidbodyのIsKinamaticをtrueに
		GrabObj.GetComponent<Rigidbody>().isKinematic = true;

		// 設定によって位置や向きの補正を行う
		switch(GrabObj.CorrectType) {
		case GrabbableObject.GrabCorrectType.ColliderCenterPositionCorrectGrab:
			transform.position = _grabCol.GetComponent<Collider>().bounds.center;
			break;
		case GrabbableObject.GrabCorrectType.ColliderCenterPositionAndTransformRotationCorrectGrab:
			transform.position = _grabCol.GetComponent<Collider>().bounds.center;
			transform.rotation = _grabCol.transform.rotation;
			break;
		}
	}

	void ReleaseObject() {
		// 掴み中オブジェクトの親を元に戻す
		GrabObj.transform.parent = GrabObj.DefParent;

		// 掴み中オブジェクトのRigidbodyのIsKinematicを元に戻す
		GrabObj.GetComponent<Rigidbody>().isKinematic = GrabObj.DefIsKinematic;

		// コントローラーの移動量と回転量をオブジェクトにコピー
		GrabObj.GetComponent<Rigidbody>().velocity = Controller.velocity;
		GrabObj.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;

		// 掴み中オブジェクトから解除
		GrabObj = null;
	}
}
