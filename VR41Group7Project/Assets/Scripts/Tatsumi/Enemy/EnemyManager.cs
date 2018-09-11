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

	[SerializeField, Tooltip("目標地点")]
	Transform targetPoint = null;

	[SerializeField, Tooltip("目標地点のプレハブ")]
	GameObject targetPointPrefab = null;

	[SerializeField, Tooltip("目標地点を定める時間")]
	float aimTime = 5.0f;

	[SerializeField, Tooltip("目標地点を定める開始位置")]
	Transform targetPointStartPoint = null;

	[SerializeField, Tooltip("行動前の待機時間")]
	float actionBeforeStanbdyTime = 5.0f;

	[SerializeField, Tooltip("行動後の待機時間")]
	float actionAfterStanbdyTime = 3.0f;

	[SerializeField, Tooltip("ダメージ時の怯み時間")]
	float damageStanTime = 2.0f;

	[SerializeField, Tooltip("行動を開始した時間"), ReadOnly]
	float actionBeginTime = 0.0f;

	[SerializeField, Tooltip("モーション管理コンポーネント")]
	HumanMotion motion = null;

	[SerializeField, Tooltip("投げるオブジェクトのプレハブ")]
	GameObject throwObjPrefab = null;

	[SerializeField, Tooltip("投げるオブジェクトの待機位置")]
	Transform throwObjPoint = null;

	[SerializeField, Tooltip("投げるオブジェクト"), ReadOnly]
	Transform throwObj = null;

	[SerializeField, Tooltip("投げるオブジェクトの投げた後の親オブジェクト")]
	Transform throwAfterParent = null;

	[SerializeField, Tooltip("投げるベクトル")]
	Vector3 throwVec = new Vector3(0.0f, 8.0f, 10.0f);

	void Start() {
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

				// 待機モーション開始
				motion.StartAnimation(HumanMotion.AnimaList.Wait);

				ThrowStandby();
			}

			break;

		case State.aim:
			// ターゲットに向く
			transform.LookAt(targetPoint);

			if ((Time.time - actionBeginTime) >= aimTime) {
				St = State.actionBefore;

				// 的の移動を終了
				Destroy(targetPoint.GetComponent<FollowTarget>());

				// 投げモーション開始
				motion.StartAnimation(HumanMotion.AnimaList.Throw);
			}
			break;

		case State.actionBefore:
			if ((Time.time - actionBeginTime) >= actionBeforeStanbdyTime) {
				// 投げる
				ThrowObject(throwVec);
				St = State.actionAfter;

				// 待機モーション開始
				motion.StartAnimation(HumanMotion.AnimaList.Wait);
			}
			break;

		case State.actionAfter:
			if ((Time.time - actionBeginTime) >= actionAfterStanbdyTime) {
				St = State.aim;

				// 古い的を削除
				FadeColor destroyFade = targetPoint.gameObject.AddComponent<FadeColor>();
				destroyFade.ColList.Add(new Color(0.0f, 0.0f, 0.0f, 0.0f));
				destroyFade.DestroyOnLastFadeEnd = true;
				destroyFade.LoopFade = false;

				ThrowStandby();
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

	void ThrowStandby() {
		// 新たな的を生成
		targetPoint = Instantiate(targetPointPrefab).transform;
		targetPointPrefab.GetComponent<FollowTarget>().Target = Camera.main.transform;
		targetPoint.position = targetPointStartPoint.position;

		// 投げるオブジェクトを生成
		throwObj = Instantiate(throwObjPrefab).transform;
		throwObj.parent = throwObjPoint;
		throwObj.localPosition = Vector3.zero;
		throwObj.GetComponent<Rigidbody>().isKinematic = true;

		// 投げるオブジェクトのプレイヤーが掴めるコンポーネントを無効化
		GrabbableCollider[] grabCols = throwObj.GetComponentsInChildren<GrabbableCollider>();
		foreach (var grabCol in grabCols) {
			grabCol.GetComponent<Collider>().enabled = false;
		}
	}

	void ThrowObject(Vector3 _localVec) {
		Rigidbody rb = throwObj.GetComponent<Rigidbody>();
		throwObj.transform.parent = throwAfterParent;
		throwObj.LookAt(targetPoint.position);
		rb.isKinematic = false;
		//Vector3 horizontalVec = (Quaternion.Euler(new Vector3(0.0f, transform.rotation.eulerAngles.y, 0.0f)) * new Vector3(_localVec.x, 0.0f, _localVec.y));
		//rb.velocity = new Vector3(horizontalVec.x, _localVec.y, horizontalVec.z);
		//Debug.Log(_localVec + " " + rb.velocity);
		rb.velocity = (throwObj.rotation * _localVec);

		//test
		//		UnityEditor.Selection.activeTransform = throwObj;
		//		UnityEditor.EditorApplication.isPaused = true;

		// 投げるオブジェクトのプレイヤーが掴めるコンポーネントを有効化
		GrabbableCollider[] grabCols = throwObj.GetComponentsInChildren<GrabbableCollider>();
		foreach (var grabCol in grabCols) {
			grabCol.GetComponent<Collider>().enabled = true;
		}

		throwObj = null;
	}
	void DropObject() {
		ThrowObject(Vector3.zero);
	}
}
