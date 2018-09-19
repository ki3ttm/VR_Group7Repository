using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSelect : MonoBehaviour {
	[SerializeField] private string sceneName;
	Color baseColor;
	Color changeColor;
	StageSelect stageSelect;

	[SerializeField] KeyCode a;
	// Use this for initialization
	void Start () {
		baseColor = GetComponent<MeshRenderer>().material.color;
		changeColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
		stageSelect = transform.parent.gameObject.GetComponent<StageSelect>();
	}

	private void Update() {
		GetComponent<MeshRenderer>().material.color = baseColor;
	}

	public void HitLazer() {
		GetComponent<MeshRenderer>().material.color = changeColor;
	}

	public void ChangeScene() {
		stageSelect.NextScene(sceneName);
	}
}
