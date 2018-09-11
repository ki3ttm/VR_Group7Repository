﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour {

	SceneController sceneController;

	private void Awake() {
		sceneController = GameObject.Find("[SceneManager]").GetComponent<SceneController>();
	}

	// Update is called once per frame
	void Update() {
		if (sceneController) {
			if (Input.GetKeyDown(KeyCode.A)) {
				sceneController.SceneChange(SceneController.SceneState.StageSelect);
			}
		} else {
			sceneController = GameObject.Find("[SceneManager]").GetComponent<SceneController>();
		}
	}
}