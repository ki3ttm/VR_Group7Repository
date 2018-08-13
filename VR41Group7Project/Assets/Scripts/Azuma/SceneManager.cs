using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
	/// <summary>
	/// 現在のシーンの列挙子
	/// </summary>
	public enum SceneState {
		Title,
		StageSelect,
		GameMain,
		GameResult,
		SceneMax,
	}

	private GameObject[] sceneObj;           // シーンのオブジェクトを格納を切り替えるために
	private Fade fade;                       // フェードさせるためのオブジェクト
	private bool fadeNow = false;

	[SerializeField] private SceneState state = SceneState.Title;   // 現在のシーン

	// Use this for initialization
	void Start() {
		int childCount = 0;             // 子の数をカウントするためのもの

		state = SceneState.Title;       // シーン状態の初期化
		sceneObj = new GameObject[transform.childCount];

		// [GameManager]の中に入ったシーンを取得するところ
		// 必ず、読み込みたい順番で入れること
		foreach (Transform child in transform) {
			sceneObj[childCount] = child.gameObject;
			sceneObj[childCount].SetActive(false);
			childCount++;
		}

		fadeNow = false;

		// フェードスクリプトの取得
		fade = GetComponent<Fade>();

		// シーンの切り替え
		SceneChange(SceneState.Title);
	}

	/// <summary>
	/// シーンの切り替え
	/// </summary>
	/// <param name="setSceneState"> 次に遷移したい列挙子 </param>
	public void SceneChange(SceneState setSceneState) {
		if (fadeNow == true) {
			return;
		}
		StartCoroutine(SceneChangeNow(setSceneState));
	}

	public IEnumerator SceneChangeNow(SceneState setSceneState) {
		fadeNow = true;
		yield return StartCoroutine(fade.FadeOut());
		foreach (Transform sceneChild in sceneObj[state.GetHashCode()].transform) {
			Destroy(sceneChild.gameObject);
		}
		sceneObj[state.GetHashCode()].SetActive(false);
		state = setSceneState;
		sceneObj[state.GetHashCode()].SetActive(true);
		yield return StartCoroutine(fade.FadeIn());
		fadeNow = false;
	}
}
