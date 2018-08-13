using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePoints : MonoBehaviour {
	[SerializeField, Tooltip("構成する点の数")]
	int pointNum = 10;
	[SerializeField, Tooltip("円の半径")]
	float radius = 1.0f;
	[SerializeField, Tooltip("指定した四角形のサイズを覆うように円の半径を設定する\n(0, 0)なら使用されない")]
	Vector2 quadSize = Vector2.zero;
	[SerializeField, Tooltip("求められた点のリスト"), ReadOnly]
	List<Transform> points = new List<Transform>();
	public List<Transform> Points {
		get {
			return points;
		}
	}
	[SerializeField, Tooltip("点に生成するオブジェクトのプレハブ\nnullなら空のオブジェクトが生成される")]
	GameObject pointPrefab = null;
	[SerializeField, Tooltip("点オブジェクトの親\n指定がなければ自身が親になる")]
	Transform pointParent = null;

	void Start() {
		if (points.Count != pointNum) {
			Setting();
		}
	}

	[ContextMenu("Setting")]
	void Setting() {
//		Debug.Log(name + "(CirclePoints) Setting");
		// 半径を計算
		if (quadSize != Vector2.zero) {
			radius = Vector2.Distance(Vector2.zero, (quadSize * 0.5f));
		}

		foreach (var oldPoint in points) {
			if (oldPoint) {
				DestroyImmediate(oldPoint.gameObject);
			}
		}
		points.Clear();
		for (int cnt = 0; cnt < pointNum; cnt++) {
			GameObject point = null;
			if (!pointPrefab) {
				point = new GameObject("CirclePoint (" + cnt + ")");
			}
			else {
				point = Instantiate(pointPrefab);
				point.name = (pointPrefab.name + " (" + cnt + ")");
			}
			if (pointParent) {
				point.transform.parent = pointParent;
			}
			else {
				point.transform.parent = transform;
			}
			point.transform.localPosition = (Quaternion.Euler(0.0f, (360.0f / pointNum * cnt), 0.0f) * (Vector3.forward * radius));
			points.Add(point.transform);
		}
	}

	public Transform GetNearPoint(Vector3 _pos) {
		Transform nearPoint = null;
		float nearDis = float.MaxValue;
		foreach (var point in Points) {
			float dis = Vector3.Distance(_pos, point.position);
			if (nearDis >= dis) {
				nearDis = dis;
				nearPoint = point;
			}
		}
		return nearPoint;
	}

	public List<Transform> GetRoute(Vector3 _startPos, Vector3 _endPos) {
		List<Transform> routePointList = new List<Transform>();

		// 各指定地から最も近い地点を取得
		Transform beginPoint = GetNearPoint(_startPos);
		Transform endPoint = GetNearPoint(_endPos);

		// 開始地点と終了地点が同じならその地点だけをリストに入れて返す
		if (beginPoint == endPoint) {
			routePointList.Add(beginPoint);
			return routePointList;
		}

		// 開始地点をリストに追加
		routePointList.Add(beginPoint);

		// 開始地点と終了地点のインデックスを取得
		int beginIdx = Points.IndexOf(beginPoint);
		int endIdx = points.IndexOf(endPoint);

		// 開始地点と終了地点まで巡る方向ごとの距離を求める
		int plusDis = (endIdx - beginIdx);
		if(plusDis < 0) {
			plusDis += Points.Count;
		}
		int minusDis = (beginIdx - endIdx);
		if (minusDis < 0) {
			minusDis += Points.Count;
		}

		// 開始地点から終了地点まで巡る方向を設定
		int listVec = 0;
		if (plusDis < minusDis) {
			listVec = 1;
		}
		else if (plusDis > minusDis) {
			listVec = -1;
		}
		else {
			// ランダム(-1 or +1)
			listVec = (Random.Range(0, 1) * 2 - 1);
		}

		// 開始地点から終了地点までの地点をリストに追加
		for (int idx = LoopCalc(beginIdx, 0, (points.Count - 1), listVec), safeCnt = 0; idx != endIdx; idx = LoopCalc(idx, 0, (Points.Count - 1), listVec), safeCnt++) {
			// リストに追加
			routePointList.Add(Points[idx]);

			if (safeCnt > Points.Count) {
				Debug.LogError("safeCntがPoints.Countを超えました。");
				break;
			}
		}

		// 終了地点をリストに追加
		routePointList.Add(endPoint);

		return routePointList;
	}

	int LoopCalc(int _bef, int _min, int _max, int _add) {
		int ret = (_bef + _add);

		if (ret > _max) {
			ret = _min;
		}
		else if (ret < _min) {
			ret = _max;
		}
		return ret;
	}
}
