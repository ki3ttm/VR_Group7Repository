using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	[System.Serializable]
	class SpawnRoute {
		[SerializeField]
		List<Transform> route = new List<Transform>();
		public List<Transform> Route {
			get {
				return route;
			}
		}
	}

	[SerializeField, Tooltip("出現する敵のプレハブ")]
	GameObject enemyPrefab = null;
	[SerializeField, Tooltip("出現した敵の親オブジェクト")]
	Transform enemyParent = null;
	[SerializeField, Tooltip("出現済みの敵のリスト"), ReadOnly]
	List<EnemyManager> spawnEnemyList = new List<EnemyManager>();

	[SerializeField, Tooltip("敵が出現してから次の敵が出現するまでの最短時間")]
	float spawnSpanTime = 1.0f;
	public float SpawnSpanTime {
		get {
			return spawnSpanTime;
		}
		set {
			spawnSpanTime = value;
		}
	}
	[SerializeField, Tooltip("敵が出現してから定位置に到着するまでの時間\nこの値が変化した場合、次に出現した敵から適用される。")]
	float standbyMoveTime = 10.0f;
	public float StandbyMoveTime {
		get {
			return standbyMoveTime;
		}
		set {
			standbyMoveTime = value;
		}
	}
	[SerializeField, Tooltip("定位置に到達した敵が始めに行動するまでの準備時間\nこの値が変化した場合、出現済みの敵にも即座に適用される。")]
	float actionStandbyTime = 2.0f;
	public float ActionStandbyTime {
		get {
			return actionStandbyTime;
		}
		set {
			actionStandbyTime = value;
		}
	}
	[SerializeField, Tooltip("行動した敵が再び準備時間に入るまでの待機時間\n敵の前回行動から次回行動までの総待機時間はこの値とActionStandbyTimeを足した時間になる。\nこの値が変化した場合、出現済みの敵にも即座に適用される。")]
	float actionCoolTime = 2.0f;
	public float ActionCoolTime {
		get {
			return actionCoolTime;
		}
		set {
			actionCoolTime = value;
		}
	}
	[SerializeField, Tooltip("敵の最大出現数\n但しEnemyPointNumがこの値よりも低い場合はその数までしか出現しない。")]
	int spawnMax = 10;
	public int SpawnMax {
		get {
			return spawnMax;
		}
		set {
			spawnMax = value;
		}
	}
	
	[SerializeField, Tooltip("敵の定位置を設定している円コンポーネント")]
	CirclePoints enemyPointCircle = null;
	[SerializeField, Tooltip("プレイエリア周辺の敵移動移動を設定している円コンポーネント")]
	CirclePoints enemyOutSideCircle = null;
	[SerializeField, Tooltip("敵の定位置数\nこの数より多くの敵は出現できない。"), ReadOnly]
	int enemyPointNum = 0;
	[SerializeField, Tooltip("前回の敵出現時間"), ReadOnly]
	float prevSpawnTime = float.MinValue;
	[SerializeField, Tooltip("プレイエリア周辺までの敵出現ルート\n複数のルートを指定すればそのルートが順番に使用される。")]
	List<SpawnRoute> spawnRouteList = new List<SpawnRoute>();
	[SerializeField, Tooltip("前回に出現した敵が使用した敵出現ルート"), ReadOnly]
	int prevSpawnRouteIdx = -1;
	[SerializeField, Tooltip("敵が定位置に到達したときに向く位置")]
	Transform endVecPoint = null;

	void Start () {
		enemyPointNum = enemyPointCircle.Points.Count;
	}

	void Update () {
		// 敵リストから削除済みの敵を除外
		for (int idx = (spawnEnemyList.Count - 1); idx >= 0; idx--) {
			if (!spawnEnemyList[idx]) {
				spawnEnemyList.RemoveAt(idx);
			}
		}

		// 前回の敵出現から一定時間以上経過していれば
		if ((prevSpawnTime + SpawnSpanTime) <= Time.time) {
			// 敵の最大出現数
			int max = Mathf.Min(SpawnMax, enemyPointNum);
			if (spawnEnemyList.Count < max) {
				// 敵の最終出現時間を更新
				if (prevSpawnTime < 0.0f) {
					prevSpawnTime = Time.time;
				}
				else {
					prevSpawnTime += SpawnSpanTime;
				}

				// 敵出現
				Spawn();
			}
		}
	}

	void Spawn() {
		// 敵を出現
		GameObject enemyObj = Instantiate(enemyPrefab, enemyParent);
		EnemyManager enemyMng = enemyObj.GetComponent<EnemyManager>();

		// 出現ルートを設定
		int spawnRouteIdx = (prevSpawnRouteIdx + 1);
		if (spawnRouteIdx > (spawnRouteList.Count - 1)) {
			spawnRouteIdx = 0;
		}
		enemyMng.RouteNodeList.AddRange(spawnRouteList[spawnRouteIdx].Route);
		prevSpawnRouteIdx = spawnRouteIdx;

		// 出現時の位置と向きを設定
		enemyObj.transform.position = enemyMng.RouteNodeList[0].position;
		enemyObj.transform.LookAt(enemyMng.RouteNodeList[1].position);

		// 出現必要時間を設定
		enemyMng.StandbyMoveTime = StandbyMoveTime;

		// 出現位置リストを設定
		enemyMng.EnemyPointsCircle = enemyPointCircle;

		// プレイエリア周辺ルート位置リストを設定
		enemyMng.OutSideCircle = enemyOutSideCircle;

		// 定位置に到着時に向く位置を設定
		enemyMng.GetComponent<HermiteCurve>().EndVecPoint = endVecPoint;

		// 出現させた敵をリストに追加
		spawnEnemyList.Add(enemyMng);
	}
}
