using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HermiteCurve))]
public class EnemyActionMarker : MonoBehaviour {
	[SerializeField, Tooltip("")]
	GameObject circlePrefab = null;
	[SerializeField, Tooltip(""), ReadOnly]
	Transform circle = null;
	public Transform Circle {
		get {
			return circle;
		}
	}
	[SerializeField, Tooltip("")]
	GameObject linePrefab = null;
	[SerializeField, Tooltip(""), ReadOnly]
	List<Transform> lineList = new List<Transform>();
	public List<Transform> LineList {
		get {
			return lineList;
		}
	}
	HermiteCurve hermite = null;

	[SerializeField]
	int lineNum = 10;
	[SerializeField, ReadOnly]
	float spanRatio = 0.0f;
	[SerializeField]
	float loopTime = 1.0f;
	[SerializeField]
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
