using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {

	private StageManager manager;   // マネージャー

	// Use this for initialization
	void Start () {
		manager = transform.parent.GetComponent<StageManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			manager.SceneChange(StageManager.SceneState.GameResult);
		}
	}
}
