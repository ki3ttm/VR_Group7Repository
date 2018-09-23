using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// タイトル
/// </summary>
public class Title : MonoBehaviour {

	private SceneController sceneController;    // シーンコントローラ

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
			if (Input.GetKeyDown(KeyCode.Return)) {
				// sceneController.SceneChange( シーンの列挙子 )を呼び出すとシーンを変えることができます
				sceneController.SceneChange(SceneController.SceneState.StageSelect);
			}

			if (sceneController.LeftController) {
				// 左コントローラの操作
				if (sceneController.LeftController.GetGripDown()) {
					sceneController.SceneChange(SceneController.SceneState.StageSelect);
				}
				if (sceneController.LeftController.GetHairTriggerDown()) {
					sceneController.SceneChange(SceneController.SceneState.StageSelect);
				}
			}
			if (sceneController.RightController) {
				// 右コントローラの操作
				if (sceneController.RightController.GetGripDown()) {
					sceneController.SceneChange(SceneController.SceneState.StageSelect);
				}
				if (sceneController.RightController.GetHairTriggerDown()) {
					sceneController.SceneChange(SceneController.SceneState.StageSelect);
				}
			}
		} else {
			// ここではシーンコントローラが所得できなかった場合の処理を書きます
			sceneController = GameObject.Find("[SceneManager]").GetComponent<SceneController>();
		}
	}
}