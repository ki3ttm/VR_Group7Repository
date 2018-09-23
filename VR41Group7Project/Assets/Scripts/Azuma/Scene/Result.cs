using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームメイン
/// </summary>
public class Result : MonoBehaviour {

	SceneController sceneController;    // シーンコントローラ

	/// <summary>
	/// シーンコントローラの取得
	/// </summary>
	private void Awake() {
		sceneController = GameObject.Find("[SceneManager]").GetComponent<SceneController>();
	}

	// Update is called once per frame
	void Update() {
		// シーンコントローラが所得できてるかをチェック
		if (sceneController) {
			// ここでシーンの切り替える条件などを書いてください
			if (Input.GetKeyDown(KeyCode.A)) {
				// sceneController.SceneChange( シーンの列挙子 )を呼び出すとシーンを変えることができます
				sceneController.SceneChange(SceneController.SceneState.Title);
			}

			// 左コントローラの操作
			if (sceneController.LeftController.GetGripDown()) {
				sceneController.SceneChange(SceneController.SceneState.Title);
			}
			if (sceneController.LeftController.GetHairTriggerDown()) {
				sceneController.SceneChange(SceneController.SceneState.Title);
			}

			// 右コントローラの操作
			if (sceneController.RightController.GetGripDown()) {
				sceneController.SceneChange(SceneController.SceneState.Title);
			}
			if (sceneController.RightController.GetHairTriggerDown()) {
				sceneController.SceneChange(SceneController.SceneState.Title);
			}

		} else {
			// ここではシーンコントローラが所得できなかった場合の処理を書きます
			sceneController = GameObject.Find("[SceneManager]").GetComponent<SceneController>();
		}
	}
}
