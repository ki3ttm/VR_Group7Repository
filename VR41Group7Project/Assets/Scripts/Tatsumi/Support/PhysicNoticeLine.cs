using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicNoticeLine : MonoBehaviour {
	[Header("Physics"), SerializeField, Tooltip("開始位置")]
	Vector3 startPoint = Vector3.zero;

	[SerializeField, Tooltip("着地位置")]
	Vector3 endPoint = Vector3.forward;

	[SerializeField, Tooltip("角度")]
	float angle = 45.0f;
	float rad = 0.0f;

	[SerializeField, Tooltip("初速度"), ReadOnly]
	float firstVel = 1.0f;

	[SerializeField, Tooltip("最大初速度")]
	float firstVelMax = 10.0f;

	[SerializeField, Tooltip("最大初速度以上の初速が必要な距離であるか"), ReadOnly]
	bool firstVelLimmit = false;

	[Header("Line"), SerializeField, Tooltip("線のプレハブ")]
	GameObject linePrefab = null;

	[SerializeField, Tooltip("線のリスト"), ReadOnly]
	List<Transform> lineList = new List<Transform>();

	[SerializeField, Tooltip("線の本数")]
	int lineNum = 10;

	[SerializeField, Tooltip("全長に占める線一本の長さの割合")]
	float lineLenRatio = 0.1f;

	[SerializeField, Tooltip("線の流れている現在の割合"), ReadOnly]
	float loopRatio = 0.0f;

	[SerializeField, Tooltip("線の流れる速度")]
	float loopRatioSpd = 1.0f;

	void Start() {
		for (int idx = 0; idx < lineNum; idx++) {
			Transform line = Instantiate(linePrefab, transform).transform;
			line.localPosition = Vector3.zero;
			lineList.Add(line);
		}
	}

	void Update() {
		// 進行
		loopRatio += (loopRatioSpd * Time.deltaTime);
		while (loopRatio > 1.0f) {
			loopRatio -= 1.0f;
		}

		Calc();

		for (int idx = 0; idx < lineNum; idx++) {
			Vector3 startPoint = GetPoint(((float)idx / lineNum) + (loopRatio * (1.0f / lineNum)));
			Vector3 endPoint = GetPoint(((float)idx / lineNum) + (loopRatio * (1.0f / lineNum)) + lineLenRatio);
			Transform line = lineList[idx];
			line.transform.position = startPoint;
			line.transform.localScale = (Vector3.forward * Vector3.Distance(startPoint, endPoint));
			line.transform.LookAt(endPoint);
		}
	}

	void Calc() {
		rad = (angle * Mathf.PI / 180.0f);
	}

	public Vector3 GetPoint(float _ratio) {
		//Vector3 ret = ((startPoint + endPoint) * 0.5f);
		Vector3 ret = (Vector3.forward * _ratio * 20.0f) + Vector3.up * 100.0f;
		return ret;
	}

	public Vector3 GetEndPoint() {
		return GetPoint(1.0f);
	}
}
