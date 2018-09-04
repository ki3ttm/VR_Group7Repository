using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

	[SerializeField] private float fadeTime = 3.0f;	// フェードする時間
	float fadeTimeCount = 0.0f;                     // フェードする時間のカウント
	[SerializeField] private Image imageObj;		// フェードさせるために使う画像
	// Use this for initialization
	void Start () {
		fadeTimeCount = 0.0f;	// 一応初期化
	}

	/// <summary>
	/// フェードイン
	/// </summary>
	/// <returns></returns>
	 public IEnumerator FadeIn() {
		float alpha = 0.0f;
		for (fadeTimeCount = 0.0f; fadeTimeCount < fadeTime; fadeTimeCount += Time.deltaTime) {
			alpha = 1.0f - (fadeTimeCount / fadeTime);
			imageObj.color = new Color(1.0f, 1.0f, 1.0f, alpha);
			yield return null;
		}
		FadeInReset();
	}

	/// <summary>
	/// フェードアウト
	/// </summary>
	/// <returns></returns>
	public IEnumerator FadeOut() {
		float alpha = 0.0f;
		for (fadeTimeCount = 0.0f; fadeTimeCount < fadeTime; fadeTimeCount += Time.deltaTime) {
			alpha = fadeTimeCount / fadeTime;
			imageObj.color = new Color(1.0f, 1.0f, 1.0f, alpha);
			yield return null;
		}
	}

	/// <summary>
	/// フェードイン時にα値が残ってしまうので0にするための関数
	/// </summary>
	private void FadeInReset() {
		imageObj.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	}
}
