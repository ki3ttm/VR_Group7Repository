using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HermiteCurve))]
public class EnemyActionMarker : MonoBehaviour {
	[SerializeField, Tooltip("着弾地点オブジェクトのプレハブ")]
	GameObject circlePrefab = null;
	[SerializeField, Tooltip("着弾地点オブジェクト"), ReadOnly]
	Transform circle = null;
	public Transform Circle {
		get {
			return circle;
		}
	}
	[SerializeField, Tooltip("線のプレハブ")]
	GameObject linePrefab = null;
	[SerializeField, Tooltip("線(破線を構成する短い線)のリスト"), ReadOnly]
	List<Transform> lineList = new List<Transform>();
	public List<Transform> LineList {
		get {
			return lineList;
		}
	}
	HermiteCurve hermite = null;

	[SerializeField, Tooltip("線の本数")]
	int lineNum = 10;
	[SerializeField, Tooltip("線の本数から求める一本が全体に占める比率"), ReadOnly]
	float spanRatio = 0.0f;
	[SerializeField, Tooltip("線の動きがループするまでにかかる時間")]
	float loopTime = 1.0f;
	[SerializeField, Tooltip("生成された時間")]
	float startTime = 0.0f;

	void Awake() {
		hermite = GetComponent<HermiteCurve>();
		circle = Instantiate(circlePrefab).transform;
		hermite.EndVecPoint = circle;
		circle.GetComponent<FollowTarget>().Target = Camera.main.transform;
		for (int cnt = 0; cnt < lineNum; cnt++) {
			lineList.Add(Instantiate(linePrefab, transform).transform);
		}
		startTime = Time.time;
		spanRatio = (1.0f / lineNum);
	}

	void Update() {
		float loopRatio = ((Time.time - startTime) / loopTime);
		for (int idx = 0; idx < lineList.Count; idx++) {
			lineList[idx].transform.position = hermite.GetPoint((spanRatio * idx) + (loopRatio * spanRatio));
			lineList[idx].transform.LookAt(hermite.GetPoint((spanRatio * idx) + (loopRatio * spanRatio) + 0.1f));
		}
	}
}
