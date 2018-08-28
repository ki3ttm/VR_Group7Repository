using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HermiteCurve))]
public class EnemyManager : MonoBehaviour {
	public enum State {
		standbyMove,
		aim,
		actionBefore,
		actionAfter,
		damage,
	}

	[SerializeField]
	State st = State.standbyMove;
	State St {
		get {
			return st;
		}
		set {
			st = value;
			actionBeginTime = Time.time;
		}
	}

	HermiteCurve hermite = null;
	HermiteCurve Hermite {
		get {
			if (!hermite) {
				hermite = GetComponent<HermiteCurve>();
			}
			return hermite;
		}
	}

	[SerializeField, Tooltip("目標地点までの移動に掛かる時間")]
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

	[SerializeField, Tooltip("")]
	Transform actionMarker = null;

	[SerializeField, Tooltip("")]
	float aimTime = 5.0f;

	[SerializeField, Tooltip("")]
	GameObject actionMarkerPrefab = null;

	[SerializeField, Tooltip("")]
	Transform actionMarkerStartPoint = null;

	[SerializeField, Tooltip("")]
	Transform targetPoint = null;

	[SerializeField, Tooltip("")]
	float actionBeforeStanbdyTime = 5.0f;

	[SerializeField, Tooltip("")]
	float actionAfterStanbdyTime = 3.0f;

	[SerializeField, Tooltip("")]
	float damageStanTime = 2.0f;

	[SerializeField, Tooltip("行動を開始した時間"), ReadOnly]
	float actionBeginTime = 0.0f;

	[SerializeField, Tooltip("")]
	HumanMotion motion = null;

	void Start() {
//		defTargetPointRelativePos = (targetPoint.GetComponent<EnemyActionMarker>().Circle.position - transform.position);
		actionBeginTime = Time.time;
		Routing();
		motion.StartAnimation(HumanMotion.AnimaList.Walk);
	}

	void Update() {
		switch (St) {
		case State.standbyMove:
			// 進行率を更新
			float befRatio = moveRatio;
			moveRatio = ((Time.time - actionBeginTime) / StandbyMoveTime);
			moveRatio = Mathf.Clamp(moveRatio, 0.0f, 1.0f);

			// 進行率から位置と向きを求める
			transform.position = Hermite.GetPoint(moveRatio);
			if (befRatio != moveRatio) {
				transform.rotation = Quaternion.LookRotation(Hermite.GetPoint(moveRatio) - Hermite.GetPoint(befRatio));
			}

			if (moveRatio >= 1.0f) {
				St = State.aim;
				motion.StartAnimation(HumanMotion.AnimaList.Wait);
				targetPoint = Instantiate(actionMarkerPrefab).transform;
				actionMarkerPrefab.GetComponent<FollowTarget>().Target = Camera.main.transform;
				targetPoint.position = actionMarkerStartPoint.position;
			}

			break;

		case State.aim:
			// ターゲットに向く
			transform.LookAt(targetPoint);

			if ((Time.time - actionBeginTime) >= aimTime) {
				St = State.actionBefore;

				// 的の移動を終了
				Destroy(targetPoint.GetComponent<FollowTarget>());

				// 投げるモーション開始
				motion.StartAnimation(HumanMotion.AnimaList.Throw);
			}
			break;

		case State.actionBefore:
			if ((Time.time - actionBeginTime) >= actionBeforeStanbdyTime) {
				Action();
				St = State.actionAfter;
			}
			break;

		case State.actionAfter:
			if ((Time.time - actionBeginTime) >= actionAfterStanbdyTime) {
				St = State.aim;
				motion.StartAnimation(HumanMotion.AnimaList.Wait);

				// 新たな的を生成し直す
				Destroy(targetPoint.gameObject);
				targetPoint = Instantiate(actionMarkerPrefab).transform;
				actionMarkerPrefab.GetComponent<FollowTarget>().Target = Camera.main.transform;
				targetPoint.position = actionMarkerStartPoint.position;
			}
			break;

		case State.damage:
			if ((Time.time - actionBeginTime) >= damageStanTime) {
				St = State.actionBefore;
			}
			break;
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

	void Action() {
		
	}
}
