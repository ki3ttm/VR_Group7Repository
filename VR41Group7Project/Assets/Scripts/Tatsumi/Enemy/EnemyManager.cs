using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HermiteCurve))]
public class EnemyManager : MonoBehaviour {
	HermiteCurve hermite = null;
	HermiteCurve Hermite {
		get {
			if (!hermite) {
				hermite = GetComponent<HermiteCurve>();
			}
			return hermite;
		}
	}

	[SerializeField, Tooltip("出現した時間"), ReadOnly]
	float spawnBeginTime = 0.0f;

	[SerializeField, Tooltip("目標地点までの移動に掛かる時間"),]
	float standbyMoveTime = 5.0f;
	public float StandbyMoveTime {
		get {
			return standbyMoveTime;
		}
		set {
			standbyMoveTime = value;
		}
	}

	[SerializeField, Tooltip("移動完了率"), ReadOnly]
	float moveRatio = -1.0f;

	[SerializeField, Tooltip("プレイエリア周辺までの道")]
	List<Transform> routeNodeList = new List<Transform>();
	public List<Transform> RouteNodeList {
		get {
			return routeNodeList;
		}
	}

	[SerializeField, Tooltip("プレイエリア周辺の道となるCirclePoint")]
	CirclePoints outSideCircle = null;
	public CirclePoints OutSideCircle {
		get {
			return outSideCircle;
		}
		set {
			outSideCircle = value;
		}
	}

	[SerializeField, Tooltip("定位置となるCirclePoint")]
	CirclePoints enemyPointsCircle = null;
	public CirclePoints EnemyPointsCircle {
		get {
			return enemyPointsCircle;
		}
		set {
			enemyPointsCircle = value;
		}
	}

	void Start() {
		spawnBeginTime = Time.time;
		Routing();
	}

	void Update() {
		// 定位置までの移動中
		if (moveRatio < 1.0f) {
			// 進行率を更新
			float befRatio = moveRatio;
			moveRatio = ((Time.time - spawnBeginTime) / StandbyMoveTime);
			moveRatio = Mathf.Clamp(moveRatio, 0.0f, 1.0f);

			// 進行率から位置と向きを求める
			transform.position = Hermite.GetPoint(moveRatio);
			if (befRatio != moveRatio) {
				transform.rotation = Quaternion.LookRotation(Hermite.GetPoint(moveRatio) - Hermite.GetPoint(befRatio));
			}
		}
		// 定位置
		else {
			// 定位置に着いてからの経過時間を求める
			float elapsedTime = (Time.time - spawnBeginTime - StandbyMoveTime);


		}
	}

	public void Routing() {
		Hermite.SrcPointList.Clear();

		// 目的位置を設定
		Transform enemyPoint = null;
		List<Transform> enemyPointList = EnemyPointsCircle.Points;
		for (int idx = (enemyPointList.Count - 1); idx >= 0; idx--) {
			// 既に使用中の位置は除外
			if (enemyPointList[idx].GetComponent<EnemyPointNode>().IsUsed) {
				enemyPointList.RemoveAt(idx);
			}
		}
		if (enemyPointList.Count <= 0) {
			Debug.LogWarning("使用されていない敵配置位置が見つかりませんでした。");
			return;
		}
		// 使用されていない位置からランダムで設定
		enemyPoint = (enemyPointList[Random.Range(0, (enemyPointList.Count - 1))]);
		enemyPoint.GetComponent<EnemyPointNode>().User = transform;

		// 出現位置からプレイエリア周辺までの道を追加
		Hermite.SrcPointList.AddRange(routeNodeList);

		// プレイエリア周辺から目的位置周辺までの円状の道を追加
		Hermite.SrcPointList.AddRange(OutSideCircle.GetRoute(Hermite.SrcPointList[(Hermite.SrcPointList.Count - 1)].position, enemyPoint.position));

		// 目的位置を追加
		Hermite.SrcPointList.Add(enemyPoint);
	}
}
