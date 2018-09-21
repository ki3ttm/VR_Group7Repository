using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	// シーンの名前
	public enum SceneState {
		Title,
		StageSelect,
		Tutorial,
		GameMain,
		Result,
		MaxScene,
	}

	[SerializeField] VirtualViveController leftController = null;
	public VirtualViveController LeftController {
		get {
			if (!leftController) {
				leftController = GetComponent<VirtualViveController>();
			}
			return leftController;
		}
	}

	[SerializeField] VirtualViveController rightController = null;
	public VirtualViveController RightController {
		get {
			if (!rightController) {
				rightController = GetComponent<VirtualViveController>();
			}
			return rightController;
		}
	}

	string[] sceneName; // シーンの名前を格納
	bool[] sceneFlg;    // シーンを生成中のフラグ
	bool changeNowFlg;  // チェンジ中のフラグ
	Fade fade;
	[SerializeField] private GameObject sceneObj;
	public GameObject CameraHeadObj;
	public GameObject CameraEyeObj;
	SceneState sceneStateOld;
	bool fastFlg;
	[SerializeField] Canvas effectCanvasObj;
	private void Awake() {
		fade = GetComponent<Fade>();
		fastFlg = false;
	}

	// Use this for initialization
	void Start() {
		// シーンの名前を初期化
		sceneName = new string[SceneState.MaxScene.GetHashCode()];
		sceneName[0] = "Title";
		sceneName[1] = "StageSelect";
		sceneName[2] = "Tutorial";
		sceneName[3] = "GameMain";
		sceneName[4] = "Result";

		// シーンを生成中のフラグを初期化
		sceneFlg = new bool[SceneState.MaxScene.GetHashCode()];
		for (int sequence = 0; sequence < SceneState.MaxScene.GetHashCode(); sequence++) {
			sceneFlg[sequence] = false;
		}

		// チェンジ中かのフラグを初期化
		changeNowFlg = false;

		sceneStateOld = SceneState.Title;
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

		LaserPointer laserObj;	// レーザポインターを消したりつけたりするために使用

		// 2重に通らないようにフラグを立てておく
		changeNowFlg = true;

		// フェードアウトするまで先へは進めない
		yield return StartCoroutine(fade.FadeOut());

		// チュートリアルまたはゲームメインから変わるときにカメラの座標を引き継ぐためのスクリプト
		if (sceneStateOld == SceneState.Tutorial || sceneStateOld == SceneState.GameMain) {
			Transform otherCameraTransform;
			// 超危険な取り方をしております
			// エミュ上
			if (CameraHeadObj.activeSelf) {
				otherCameraTransform = GameObject.Find ("Camera (head)").transform;
				CameraHeadObj.transform.position = otherCameraTransform.position;
				CameraHeadObj.transform.rotation = otherCameraTransform.rotation;
			}

			// VR上
			else if (CameraEyeObj.activeSelf) {
				otherCameraTransform = GameObject.Find ("Camera (eye)").transform;
				CameraEyeObj.transform.position = otherCameraTransform.position;
				CameraEyeObj.transform.rotation = otherCameraTransform.rotation;
			}

		}

		// sceneに入っている無駄なものをすべて削除する
		foreach (GameObject g in GameObject.FindObjectsOfType<GameObject>()) {
			if (g.transform.root.gameObject.tag != "SceneManager") {
				Destroy(g.transform.root.gameObject);
			}
		}

		// もし右手にレーザーがついてたら削除する
		laserObj = GameObject.Find("RightController").GetComponent<LaserPointer>();
		if (laserObj) {
			laserObj.DestroyPrefab();
		}

		// もし左手にレーザーがついてたら削除する
		laserObj = GameObject.Find("LeftContorller").GetComponent<LaserPointer>();
		if (laserObj) {
			laserObj.DestroyPrefab();
		}

		CameraChange(true);

		// 不要になったシーンを削除する
		for (int sequence = 0; sequence < SceneState.MaxScene.GetHashCode(); sequence++) {
			if (sceneFlg[sequence] == true) {
				SceneManager.UnloadSceneAsync(sceneName[sequence]);
				sceneFlg[sequence] = false;
			}
		}

		yield return new WaitForEndOfFrame();
		// タイトル、リザルト、ステージセレクトへ遷移する場合はカメラを切り替えてレーザーの作成を行います
		if (name == SceneState.Title || name == SceneState.Result || name == SceneState.StageSelect) {
			// アクティブシーンの切り替え
			SceneManager.SetActiveScene(SceneManager.GetSceneByName("SceneManager"));
			// もし左手にレーザーがついてたら作成する
			laserObj = GameObject.Find("RightController").GetComponent<LaserPointer>();
			if (laserObj) {
				laserObj.CreatPrefab();
			}

			// もし左手にレーザーがついてたら作成する
			laserObj = GameObject.Find("LeftContorller").GetComponent<LaserPointer>();
			if (laserObj) {
				laserObj.CreatPrefab();
			}
		}

		// シーンの追加
		AsyncOperation async = SceneManager.LoadSceneAsync(sceneName[name.GetHashCode()], LoadSceneMode.Additive);

		while (true) {
			if (async.isDone == true) {
				break;
			}
			yield return null;
		}

		effectCanvasObj.worldCamera = GameObject.Find("Camera (eye)").GetComponent<Camera>();

		// アクティブシーンの切り替え
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName[name.GetHashCode()]));

		// フェードインするまで先へ進めないよ
		yield return StartCoroutine(fade.FadeIn());
		
		sceneFlg[name.GetHashCode()] = true;

		changeNowFlg = false;
		sceneStateOld = name;
	}

	public void CameraChange(bool flg) {
		sceneObj.SetActive(flg);
	}
}