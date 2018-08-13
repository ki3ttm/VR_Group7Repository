﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour {

	private StageManager manager;   // マネージャー
	private VRControllerGet contoller;

	[SerializeField] GameObject resultPlane;

	// Use this for initialization
	void Start() {
		manager = transform.parent.GetComponent<StageManager>();
		contoller = transform.parent.GetComponent<VRControllerGet>();
	}

	// Update is called once per frame
	void Update() {
		// マウス入力
		if (Input.GetMouseButtonDown(0)) {
			manager.SceneChange(StageManager.SceneState.Title);
		}

		// VRの入力
		if (contoller.LeftController.GetHairTrigger()) {
			manager.SceneChange(StageManager.SceneState.Title);
		}
		if (contoller.RightController.GetHairTrigger()) {
			manager.SceneChange(StageManager.SceneState.Title);
		}
	}

	/// <summary>
	/// シーンを切り替えた時に初期化したいものをここに書く
	/// </summary>
	private void OnEnable() {
		GameObject fixParents;  // 作成する場所を変えるために一時保管しておく変数
		fixParents = Instantiate(resultPlane);

		// これで親子構造の設定ができる
		fixParents.transform.parent = transform;
	}

}
