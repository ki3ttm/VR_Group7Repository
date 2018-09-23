using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLazer : MonoBehaviour {

	VirtualViveController controller = null;
	VirtualViveController Controller {
		get {
			if (!controller) {
				controller = GetComponent<VirtualViveController>();
			}
			return controller;
		}
	}

	//  レーザーのプレハブへの参照
	public GameObject laserPrefab;
	// レーザーのインスタンス
	private GameObject laser;
	// レーザーのTransform情報
	private Transform laserTransform;
	// レーザーが当たる点のベクトル
	private Vector3 hitPoint;

	[SerializeField]LayerMask sceneMask;

	SceneSelect sceneSelect;

	bool selectOk=false;
	bool selectFlg =false;
	private void ShowLaser(RaycastHit hit) {
		// レーザーとオブジェクトを表示する
		laser.SetActive(true);
		// レーザと当たっているオブジェクトの位置とコントローラーの
		// 位置の中心点を求めて、レーザーオブジェクトの位置にする。

		laserTransform.position = Vector3.Lerp(/*trackedObj.*/Controller.transform.parent.transform.position, hitPoint, 0.5f);

		// レーザーオブジェクトを当たっているオブジェクトに向かわせる
		laserTransform.LookAt(hitPoint);
		// レーザーのZ軸の長さをコントローラーから当たるオブジェクトまでの
		// 距離にする
		laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
	}

	void OnEnable(){
	selectOk=false;
	selectFlg =false;
	}

	void Update() {
		if (selectFlg) {
			return;
		}
		// Touchpadを押されている間、レーザーを表示する
		if (Controller.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {
			RaycastHit hit;

			// コントローラーから光線を飛ばす
			// 100m以内にオブジェクトと当たったレーザーを表示する

			if (Physics.Raycast (/*trackedObj.*/Controller.transform.position, Controller.transform.forward, out hit, 100, sceneMask)) {
				hitPoint = hit.point;
				ShowLaser (hit);
				sceneSelect = hit.transform.gameObject.GetComponent<SceneSelect> ();
				if (sceneSelect) {
					sceneSelect.HitLazer ();
					selectOk = true;
				}
			}
		}
		else {    // Touchpadが放されたら、レーザーを非表示にする
			laser.SetActive (false);
		}

		if (Controller.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad) && selectOk) {
			selectFlg = true;
			sceneSelect.ChangeScene ();
			laser.SetActive (false);
		}
	}

	void Start() {
		// レーザーのオブジェクトをプレハブから生成する
		laser = Instantiate(laserPrefab);

		// Transformのコンポネントを最初から取得する
		// (アクセスしやすくするため)
		laserTransform = laser.transform;
	}
}