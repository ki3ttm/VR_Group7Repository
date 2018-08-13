using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {

	private SceneManager manager;   // マネージャー
	private VRControllerGet contoller;

	[SerializeField] GameObject gameMainPlane;

	// Use this for initialization
	void Start() {
		manager = transform.parent.GetComponent<SceneManager>();
		contoller = transform.parent.GetComponent<VRControllerGet>();
	}

	// Update is called once per frame
	void Update() {
		// マウス入力
		if (Input.GetMouseButtonDown(0)) {
			manager.SceneChange(SceneManager.SceneState.GameResult);
		}

		// VRの入力
		if (contoller.LeftController.GetHairTrigger()) {
			manager.SceneChange(SceneManager.SceneState.GameResult);
		}
		if (contoller.RightController.GetHairTrigger()) {
			manager.SceneChange(SceneManager.SceneState.GameResult);
		}
	}

	/// <summary>
	/// シーンを切り替えた時に初期化したいものをここに書く
	/// </summary>
	private void OnEnable() {
		GameObject fixParents;  // 作成する場所を変えるために一時保管しておく変数
		fixParents = Instantiate(gameMainPlane);

		// これで親子構造の設定ができる
		fixParents.transform.parent = transform;
	}

}
