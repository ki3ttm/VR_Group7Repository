using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOverFade : MonoBehaviour {
	[SerializeField, Tooltip("エリアを示すコライダー")]
	Collider areaCol = null;
	[SerializeField, Tooltip("自身がエリア内に存在しているか"), ReadOnly]
	bool isInArea = false;
	bool IsInArea {
		get {
			return isInArea;
		}
		set {
			if (isInArea == value) return;
			isInArea = value;
			if (isInArea) {
				fadeScreen.ChangeColor(inFadeIdx);
			}
			else {
				fadeScreen.ChangeColor(outFadeIdx);
			}
		}
	}
	[SerializeField, Tooltip("フェードを行うスクリーン")]
	FadeColor fadeScreen = null;
	[SerializeField, Tooltip("エリア内にいる時にフェードするインデックス")]
	int inFadeIdx = 0;
	[SerializeField, Tooltip("エリア外にいる時にフェードするインデックス")]
	int outFadeIdx = 1;

	void OnTriggerEnter(Collider _col) {
		if (_col != areaCol) return;
		IsInArea = true;
	}
	void OnTriggerStay(Collider _col) {
		if (_col != areaCol) return;
		IsInArea = true;
	}
	void OnTriggerExit(Collider _col) {
		if (_col != areaCol) return;
		IsInArea = false;
	}
}
