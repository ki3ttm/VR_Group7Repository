using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour {

	private StageManager manager;   // マネージャー
	private VRControllerGet contoller;

	[SerializeField] GameObject selectPlane;

	// Use this for initialization
	void Start() {
		manager = transform.parent.GetComponent<StageManager>();
		contoller = transform.parent.GetComponent<VRControllerGet>();
	}

	// Update is called once per frame
	void Update() {
		// マウス入力
		if (Input.GetMouseButtonDown(0)) {
			manager.SceneChange(StageManager.SceneState.GameMain);
		}

		// VRの入力
		if (contoller.LeftController.GetHairTrigger()) {
			manager.SceneChange(StageManager.SceneState.GameMain);
		}
		if (contoller.RightController.GetHairTrigger()) {
			manager.SceneChange(StageManager.SceneState.GameMain);
		}
	}

	/// <summary>
	/// シーンを切り替えた時に初期化したいものをここに書く
	/// </summary>
	private void OnEnable() {
		GameObject fixParents;  // 作成する場所を変えるために一時保管しておく変数
		fixParents = Instantiate(selectPlane);

		// これで親子構造の設定ができる
		fixParents.transform.parent = transform;
	}

}
