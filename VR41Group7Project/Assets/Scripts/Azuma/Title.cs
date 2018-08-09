using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour {

	private StageManager manager;   // マネージャー

	// Use this for initialization
	void Start () {
		manager = transform.parent.GetComponent<StageManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			manager.SceneChange(StageManager.SceneState.StageSelect);
		}
	}

	/// <summary>
	/// シーンを切り替えた時に初期化したいものをここに書く
	/// </summary>
	private void OnEnable() {

	}


}
