using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerGrabObject : MonoBehaviour {
	// 追跡するオブジェクトの参照を持つ変数。この場合はコントローラ
	//private SteamVR_TrackedObject trackedObj;

	// コントローラーと当たっているオブジェクト
	private GameObject collidingObject;
	// 手に持っているオブジェクト
	private GameObject objectInHand;

	//// 簡単にコントローラーへの情報へアクセスできるための取得関数
	//// indexの値を使って追跡されているオブジェクトを指定
	//private SteamVR_Controller.Device Controller {
	//	get { return SteamVR_Controller.Input((int)trackedObj.index); }
	//}

	VirtualViveController controller = null;
	VirtualViveController Controller {
		get {
			if (!controller) {
				controller = GetComponent<VirtualViveController>();
			}
			return controller;
		}
	}

	// ゲームを起動するとオブジェクトのindex番号が取得される。
	void Awake() {
		////SteamVR
		//trackedObj = GetComponent<SteamVR_TrackedObject>();

		//Simulater
		//if (trackedObj) {
		//	trackedObj = trackedComponentObj.GetComponent<SteamVR_TrackedObject>();
		//}
	}

	private void SetCollidingObject(Collider col) {
		// 常にプレイヤーが手にモノを持っているまたは
		// 当たっているオブジェクトはrigidbodyがない場合何もしない
		if (collidingObject || !col.GetComponent<Rigidbody>() || !col.GetComponent<GrabObject>()) {
			return;
		}
		// 掴むことが可能なオブジェクトとして取得する
		collidingObject = col.gameObject;
	}

	// コントローラがColliderが付いているオブジェクトと当たると
	// そのオブジェクトを掴めるようにする。
	public void OnTriggerEnter(Collider other) {
		SetCollidingObject(other);
	}

	// ↑と同じ。バグ予防
	public void OnTriggerStay(Collider other) {
		SetCollidingObject(other);
	}

	// オブジェクトと離れていて掴むことが可能なものの参照を消す
	public void OnTriggerExit(Collider other) {
		if (!collidingObject) {
			return;
		}
		collidingObject = null;
	}

	private void GrabObject() {
		// 掴むことが可能なオブジェクトを手に持っている変数にコピーして
		// collidingObejctの参照を消す
		objectInHand = collidingObject;
		collidingObject = null;
		// オブジェクトをコントローラーに付けるためにジョイントを作り
		// オブジェクトのRigidbodyをジョイントにコピーする
		var joint = AddFixedJoint();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
	}

	// ジョイントを生成する
	// 簡単に外せないようにbreakForceとbreakTorqueを高い値にする。
	private FixedJoint AddFixedJoint() {
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}

	private void ReleaseObject() {
		// ジョイントがあることを確認する
		if (GetComponent<FixedJoint>()) {
			// ジョイントについているRigidBodyをnullにして
			// ジョイントを削除する
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy(GetComponent<FixedJoint>());
			// コントローラーのスピードと回転スピードをオブジェクトに
			// コピーする

			//Simulater
			//if (Controller != null) {

			objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
			objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;

			//Simulater
			//} else {
			//	objectInHand.GetComponent<Rigidbody>().velocity = ControllerObj.GetComponent<Rigidbody>().velocity;
			//	objectInHand.GetComponent<Rigidbody>().angularVelocity = ControllerObj.GetComponent<Rigidbody>().angularVelocity;
			//}

		}
		// 手に持っているオブジェクトへの参照をnullにする
		objectInHand = null;
	}

	void Update() {
		// Triggerを押されたらコントローラと当たっているオブジェクト
		// (Rigidbody)があればオブジェクトを握る関数を呼び出す
		
		if (Controller.GetHairTriggerDown()) {
			if (collidingObject) {
				GrabObject();
			}
		}
		// Triggerを放して手にオブジェクトがあればオブジェクトが放される

		if (Controller.GetHairTriggerUp()) {
			if (objectInHand) {
				ReleaseObject();
			}
		}
	}
}
