using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ゲームメイン
/// </summary>
public class GameMain : MonoBehaviour {

	SceneController sceneController;    // シーンコントローラ
	[SerializeField] VirtualViveController leftController = null;
	public VirtualViveController LeftController {
		get {
			if (!leftController) {
				leftController = GetComponent<VirtualViveController>();
			}
			return leftController;
		}
	}

	[SerializeField] VirtualViveController rightController = null;
	public VirtualViveController RightController {
		get {
			if (!rightController) {
				rightController = GetComponent<VirtualViveController>();
			}
			return rightController;
		}
	}
	[SerializeField] private GameObject cameraHeadObj;
	/// <summary>
	/// シーンコントローラの取得
	/// </summary>
	private void Awake() {

	}

	private void Start() {
		sceneController = GameObject.Find("[SceneManager]").GetComponent<SceneController>();
		// 座標の修正
		Transform cameraHeadTransform = GameObject.Find("[SceneManager]").GetComponent<SceneController>().CameraHeadObj.transform;    // ゲームメイン以外のところからカメラリグを取得
		cameraHeadObj.transform.position = cameraHeadTransform.position;
		cameraHeadObj.transform.rotation = cameraHeadTransform.rotation;
		GameObject.Find("[SceneManager]").GetComponent<SceneController>().CameraChange(false);
	}

	// Update is called once per frame
	void Update() {
		// シーンコントローラが所得できてるかをチェック
		if (sceneController) {
			// ここでシーンの切り替える条件などを書いてください
			if (Input.GetKeyDown(KeyCode.Return)) {
				// sceneController.SceneChange( シーンの列挙子 )を呼び出すとシーンを変えることができます
				sceneController.SceneChange(SceneController.SceneState.Result);

				// 左コントローラの操作
				if (sceneController.LeftController.GetGripDown()) {
					sceneController.SceneChange(SceneController.SceneState.Result);
				}
				if (sceneController.LeftController.GetHairTriggerDown()) {
					sceneController.SceneChange(SceneController.SceneState.Result);
				}

				// 右コントローラの操作
				if (sceneController.RightController.GetGripDown()) {
					sceneController.SceneChange(SceneController.SceneState.Result);
				}
				if (sceneController.RightController.GetHairTriggerDown()) {
					sceneController.SceneChange(SceneController.SceneState.Result);
				}

			}
		} else {
			// ここではシーンコントローラが所得できなかった場合の処理を書きます
			sceneController = GameObject.Find("[SceneManager]").GetComponent<SceneController>();
		}
	}
}
