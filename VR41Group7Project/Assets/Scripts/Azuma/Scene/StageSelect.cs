using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ステージセレクト
/// </summary>
public class StageSelect : MonoBehaviour {

	SceneController sceneController;    // シーンコントローラ
	SelectLazer controller;				// RightControllerのオブジェクトを持ってくる

	/// <summary>
	/// シーンコントローラの取得
	/// </summary>
	private void Awake() {
		sceneController = GameObject.Find("[SceneManager]").GetComponent<SceneController>();
		controller = GameObject.Find("RightController").GetComponent<SelectLazer>();	// 超絶危険な取り方をしています
		controller.enabled = true;	// レーザを使えるようにする
	}

	// Update is called once per frame
	void Update() {
		if (!sceneController) {
			// ここではシーンコントローラが所得できなかった場合の処理を書きます
			sceneController = GameObject.Find("[SceneManager]").GetComponent<SceneController>();
		}
		if (Input.GetKeyDown(KeyCode.A)) {
			controller.enabled = false;
			sceneController.SceneChange(SceneController.SceneState.Tutorial);
		}
		if (Input.GetKeyDown(KeyCode.D)) {
			controller.enabled = false;
			sceneController.SceneChange(SceneController.SceneState.GameMain);
		}
	}

	/// <summary>
	/// シーンチェンジ
	/// </summary>
	/// <param name="sceneName"></param>
	public void NextScene(string sceneName) {
		switch (sceneName) {
			case "Tutorial":
				controller.enabled = false;
				sceneController.SceneChange(SceneController.SceneState.Tutorial);
				break;
			case "GameMain":
				controller.enabled = false;
				sceneController.SceneChange(SceneController.SceneState.GameMain);
				break;
		}
	}
}