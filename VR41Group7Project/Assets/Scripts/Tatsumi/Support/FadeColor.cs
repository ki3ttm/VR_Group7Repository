using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeColor : MonoBehaviour {
	[System.Serializable]
	class RendererColorList {
		[SerializeField, ReadOnly]
		List<Color> colList = new List<Color>();
		public List<Color> ColList {
			get {
				return colList;
			}
		}
	}

	[Header("色リスト"), SerializeField, Tooltip("色リスト")]
	List<Color> colList = new List<Color>();
	public List<Color> ColList {
		get {
			return colList;
		}
	}
	[SerializeField, Tooltip("現在の色のインデックス")]
	int idx = 0;

	[Header("状態"), SerializeField, Tooltip("変化前の色のリスト"), ReadOnly]
	List<RendererColorList> befColList = new List<RendererColorList>();
	[SerializeField, Tooltip("変化後の色")]
	Color aftCol = Color.black;
	[SerializeField, Tooltip("現在の色")]
	Color nowCol = Color.black;
	[SerializeField, Tooltip("現在の変化率")]
	float ratio = 0.0f;

	[Header("設定項目"), SerializeField, Tooltip("所要時間")]
	float requiredTime = 1.0f;
	[SerializeField, Tooltip("使用するレンダラーのリスト\n設定されていなければStart()時に自身と子のRendererを取得する")]
	List<Renderer> rendererList = new List<Renderer>();
	[SerializeField, Tooltip("フェードを行う")]
	bool isFade = true;
	public bool IsFade {
		get {
			return isFade;
		}
		set {
			isFade = value;
		}
	}
	bool prevIsFade = true;
	[SerializeField, Tooltip("開始時にBefColにColList[0]、AftColにColList[1]を設定する")]
	bool beginColListTop = true;
	[SerializeField, Tooltip("連続フェードを行う")]
	bool autoNextFade = true;
	public bool AutoNextFade {
		get {
			return autoNextFade;
		}
		set {
			autoNextFade = value;
		}
	}
	[SerializeField, Tooltip("フェードのループを行うか")]
	bool loopFade = true;
	public bool LoopFade {
		get {
			return loopFade;
		}
		set {
			loopFade = value;
		}
	}
	[SerializeField, Tooltip("一連のフェード終了時にGameObjectごと削除するか\nLoopFadeがtrueなら無視される")]
	bool destroyOnLastFadeEnd = false;
	public bool DestroyOnLastFadeEnd {
		get {
			return destroyOnLastFadeEnd;
		}
		set {
			destroyOnLastFadeEnd = value;
		}
	}

	void Start() {
		if (rendererList.Count == 0) {
			rendererList.Add(GetComponentInChildren<Renderer>());
		}
		prevIsFade = IsFade;

		if (beginColListTop) {
			if (colList.Count > 0) {
				// 開始時の色を設定
				for (int rendererIdx = 0; rendererIdx < rendererList.Count; rendererIdx++) {
					for (int matIdx = 0; matIdx < rendererList[rendererIdx].materials.Length; matIdx++) {
						rendererList[rendererIdx].materials[matIdx].color = colList[0];
					}
				}

				if (colList.Count > 1) {
					idx = 1;
					// 変化後色を設定
					ChangeColor(idx);
				}
			}
		}

		InitFade();
	}

	void FixedUpdate() {
		if (!IsFade) return;

		if (!prevIsFade) {
			InitFade();
		}

		// 変化率を更新
		ratio += (Time.fixedDeltaTime / requiredTime);
		ratio = Mathf.Clamp(ratio, 0.0f, 1.0f);

		// 変化率から色を変更
		for (int rendererIdx = 0; rendererIdx < rendererList.Count; rendererIdx++) {
			for (int matIdx = 0; matIdx < rendererList[rendererIdx].materials.Length; matIdx++) {
				rendererList[rendererIdx].materials[matIdx].color = ((aftCol * ratio) + (befColList[rendererIdx].ColList[matIdx] * (1.0f - ratio)));
			}
		}

		// フェード完了
		if (ratio == 1.0f) {
			// 連続フェードしない場合
			if (!autoNextFade) {
				// フェード終了
				IsFade = false;
			}
			// 連続フェードする場合
			else {
				// 最後の指定色の場合
				if (idx == (colList.Count - 1)) {
					// ループしない場合
					if (!LoopFade) {
						// フェード終了
						IsFade = false;
						// 自動的に消滅する場合
						if (DestroyOnLastFadeEnd) {
							Destroy(gameObject);
						}
						return;
					}
					// ループする場合
					else {
						idx = 0;
					}
				}
				else {
					// 次の色
					idx++;
				}
				// 変化開始
				ChangeColor(idx);
			}
		}
		prevIsFade = IsFade;
	}

	public void ChangeColor(Color _aftCol) {
		// 変化後の色を設定
		aftCol = _aftCol;

		// フェード中のフェード開始なら
		if (IsFade) {
			// フェード開始処理をし直す
			InitFade();
		}
		else {
			// フェード開始
			IsFade = true;
		}
	}

	public void ChangeColor(int _idx) {
		ChangeColor(colList[_idx]);
	}
	public void ChangeColor() {
		if ((idx + 1) < colList.Count) {
			idx++;
		}
		else {
			idx = 0;
		}
		ChangeColor(idx);
	}

	void InitFade() {
		// 現在の色を保持
		befColList.Clear();
		foreach (var renderer in rendererList) {
			RendererColorList colList = new RendererColorList();
			befColList.Add(colList);
			foreach (var mat in renderer.materials) {
				colList.ColList.Add(mat.color);
			}
		}

		// 変化率を初期化
		ratio = 0.0f;
	}
}
