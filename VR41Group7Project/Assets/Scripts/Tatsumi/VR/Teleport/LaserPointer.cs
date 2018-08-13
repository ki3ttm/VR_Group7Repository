using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {
	// 追跡するオブジェクトの参照を持つ変数。この場合はコントローラ
	//private SteamVR_TrackedObject trackedObj;

	// 簡単にコントローラーへの情報へアクセスできるための取得関数
	// indexの値を使って追跡されているオブジェクトを指定
	//	private SteamVR_Controller.Device Controller {
	//		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	//	}

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
		//trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	//  レーザーのプレハブへの参照
	public GameObject laserPrefab;
	// レーザーのインスタンス
	private GameObject laser;
	// レーザーのTransform情報
	private Transform laserTransform;
	// レーザーが当たる点のベクトル
	private Vector3 hitPoint;

	// [CameraRig]のTransformコンポーネント
	public Transform cameraRigTransform;
	// 的のプレハブへの参照
	public GameObject teleportReticlePrefab;
	// 的のインスタンス
	private GameObject reticle;
	// 的のTransformコンポーネント
	private Transform teleportReticleTransform;
	// プレイヤーの頭（カメラ）のTransformコンポーネント
	public Transform headTransform;
	// 的と床が重ならないようにOffsetを設定
	public Vector3 teleportReticleOffset;
	// テレポート可能なエリアを区別するためのマスク
	public LayerMask teleportMask;
	// テレポート先がテレポート可能かの判断用
	private bool shouldTeleport;

	private void ShowLaser(RaycastHit hit) {
		// レーザーとオブジェクトを表示する
		laser.SetActive(true);
		// レーザと当たっているオブジェクトの位置とコントローラーの
		// 位置の中心点を求めて、レーザーオブジェクトの位置にする。
		
		laserTransform.position = Vector3.Lerp(/*trackedObj.*/Controller.transform.position, hitPoint, 0.5f);

		// レーザーオブジェクトを当たっているオブジェクトに向かわせる
		laserTransform.LookAt(hitPoint);
		// レーザーのZ軸の長さをコントローラーから当たるオブジェクトまでの
		// 距離にする
		laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
	}

	void Update() {
//		Debug.Log("Update");

		// Touchpadを押されている間、レーザーを表示する
		if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) {
			RaycastHit hit;

//			Debug.Log("isPress");

			// コントローラーから光線を飛ばす
			// 100m以内にオブジェクトと当たったレーザーを表示する

			if (Physics.Raycast(/*trackedObj.*/Controller.transform.position, Controller.transform.forward, out hit, 100, teleportMask)) {

				//				Debug.Log("isDrawLaser");

				hitPoint = hit.point;
				ShowLaser(hit);

				// 的を表示する
				reticle.SetActive(true);
				// Offsetを当たっている位置に加える
				teleportReticleTransform.position = hitPoint + teleportReticleOffset;
				// テレポートを可能にする
				shouldTeleport = true;
			}

		} else {    // Touchpadが放されたら、レーザーを非表示にする

			laser.SetActive(false);

			// 的を非表示にする
			reticle.SetActive(false);
		}
		//Debug.Log("UpdateEnd");

		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shouldTeleport) {
			Teleport();
		}
	}

	void Start() {
		// レーザーのオブジェクトをプレハブから生成する
		laser = Instantiate(laserPrefab);

		// Transformのコンポネントを最初から取得する
		// (アクセスしやすくするため)
		laserTransform = laser.transform;

		// 的のプレハブからの的のオブジェクトを生成する
		reticle = Instantiate(teleportReticlePrefab);
		// 的のTransformコンポネントを取得する
		teleportReticleTransform = reticle.transform;
	}

	private void Teleport() {
		// テレポート中に再テレポートできないようにする
		shouldTeleport = false;
		// 的を消す
		reticle.SetActive(false);
		// CameraRigの位置とプレイヤーの頭の位置の差を求める
		Vector3 difference = cameraRigTransform.position - headTransform.position;

		// 高さの差を消す
		difference.y = 0;
		// テレポート先の位置に差を加える
		// （これがないと微妙に違う位置にテレポートする事がある）
		cameraRigTransform.position = hitPoint + difference;
	}
}
