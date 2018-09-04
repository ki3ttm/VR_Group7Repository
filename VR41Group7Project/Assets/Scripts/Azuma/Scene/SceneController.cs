using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	// シーンの名前
	public enum SceneState {
		Title,
		StageSelect,
		GameMain,
		Result,
		MaxScene,
	}

	string[] sceneName; // シーンの名前を格納
	bool []sceneFlg;	// シーンを生成中のフラグ
	bool changeNowFlg;  // チェンジ中のフラグ

	Fade fade;

	private void Awake() {
		fade = GetComponent<Fade>();
	
	}

	// Use this for initialization
	void Start() {
		// シーンの名前を初期化
		sceneName = new string[SceneState.MaxScene.GetHashCode()];
		sceneName[0] = "Title";
		sceneName[1] = "StageSelect";
		sceneName[2] = "GameMain";
		sceneName[3] = "Result";

		// シーンを生成中のフラグを初期化
		sceneFlg = new bool[SceneState.MaxScene.GetHashCode()];
		for (int sequence = 0; sequence < SceneState.MaxScene.GetHashCode(); sequence++) {
			sceneFlg[sequence] = false;
		}

		// チェンジ中かのフラグを初期化
		changeNowFlg = false;

		SceneChange(SceneState.Title);

	}

	/// <summary>
	/// シーンチェンジをするときに呼び出すもの
	/// </summary>
	/// <param name="name"></param>
	public void SceneChange(SceneState name) {
		// 2重に入らないようにするための仕掛け
		if (changeNowFlg) {
			return;
		}
		StartCoroutine(SceneActiveChange(name));
	}

	/// <summary>
	/// 遷移開始
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	IEnumerator SceneActiveChange(SceneState name) {
		// 2重に通らないようにフラグを立てておく
		changeNowFlg = true;

		yield return StartCoroutine(fade.FadeOut());

		// 不要なシーンを削除する
		for (int sequence = 0; sequence < SceneState.MaxScene.GetHashCode(); sequence++) {
			if (sceneFlg[sequence] == true) {
				SceneManager.UnloadSceneAsync(sceneName[sequence]);
				sceneFlg[sequence] = false;
			}
		}
		// シーンの追加
		SceneManager.LoadSceneAsync(sceneName[name.GetHashCode()], LoadSceneMode.Additive);

		yield return StartCoroutine(fade.FadeIn());

		sceneFlg[name.GetHashCode()] = true;
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName[name.GetHashCode()]));
	
		changeNowFlg = false;
	}

}
