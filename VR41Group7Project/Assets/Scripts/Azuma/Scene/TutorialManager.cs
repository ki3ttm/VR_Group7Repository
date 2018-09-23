using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	private SceneController sceneController;    // シーンコントローラ
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
	public GameObject cameraEyeObj;
	/// <summary>
	/// シーンコントローラの取得
	/// </summary>
	private void Awake() {

	}


	private void Start() {
		sceneController = GameObject.Find("[SceneManager]").GetComponent<SceneController>();
		// 座標の修正
		Transform cameraHeadTransform;
		// 座標の修正
		if (cameraHeadObj.activeSelf) {
			cameraHeadTransform = GameObject.Find ("[SceneManager]").GetComponent<SceneController> ().CameraHeadObj.transform;    // ゲームメイン以外のところからカメラリグを取得
			cameraHeadObj.transform.position = cameraHeadTransform.position;
			cameraHeadObj.transform.rotation = cameraHeadTransform.rotation;
		} else if (cameraEyeObj.activeSelf) {
			cameraHeadTransform = GameObject.Find ("[SceneManager]").GetComponent<SceneController> ().CameraEyeObj.transform;    // ゲームメイン以外のところからカメラリグを取得
			cameraEyeObj.transform.position = cameraHeadTransform.position;
			cameraEyeObj.transform.rotation = cameraHeadTransform.rotation;
		}
		GameObject.Find("[SceneManager]").GetComponent<SceneController>().CameraChange(false);

	}
	// Update is called once per frame
	void Update() {
		// シーンコントローラが所得できてるかをチェック
		if (sceneController) {
			// ここでシーンの切り替える条件などを書いてください
			if (Input.GetKeyDown(KeyCode.Return)) {
				// sceneController.SceneChange( シーンの列挙子 )を呼び出すとシーンを変えることができます
				sceneController.SceneChange(SceneController.SceneState.GameMain);
			}

			if (LeftController) {
				// 左コントローラの操作
				if (LeftController.GetGripDown()) {
					sceneController.SceneChange(SceneController.SceneState.GameMain);
				}
				if (LeftController.GetHairTriggerDown()) {
					sceneController.SceneChange(SceneController.SceneState.GameMain);
				}
			}
			if (RightController) {
				// 右コントローラの操作
				if (RightController.GetGripDown()) {
					sceneController.SceneChange(SceneController.SceneState.GameMain);
				}
				if (RightController.GetHairTriggerDown()) {
					sceneController.SceneChange(SceneController.SceneState.GameMain);
				}
			}
		} else {
			// ここではシーンコントローラが所得できなかった場合の処理を書きます
			sceneController = GameObject.Find("[SceneManager]").GetComponent<SceneController>();
		}
	}
}
