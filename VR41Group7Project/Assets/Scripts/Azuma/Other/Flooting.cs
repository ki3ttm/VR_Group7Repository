using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flooting : MonoBehaviour {
	Vector3 basePos;
	private float ukuCount;     // 浮き終わるまでのカウント

	// Use this for initialization
	void Start() {
		basePos = transform.position;
	}

	// Update is called once per frame
	void Update() {
		ukuCount += Time.deltaTime * 1.5f;
		transform.position = new Vector3(basePos.x, basePos.y + (Mathf.Sin(ukuCount) * 0.5f), basePos.z);
	}
}