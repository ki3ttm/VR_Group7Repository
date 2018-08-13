using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermiteCurve : MonoBehaviour {
	[SerializeField, Tooltip("曲線の点となるオブジェクトのリスト")]
	List<Transform> srcPointList = new List<Transform>();
	public List<Transform> SrcPointList {
		get {
			return srcPointList;
		}
	}
	[SerializeField, Tooltip("各点を繋ぐ線が持つ重みのリスト\n線[0]は点[0]と[1]を繋ぐ。\n指定されていない要素があれば実行時に自動的に設定される。")]
	List<float> srcLineWeightList = new List<float>();
	List<float> SrcLineWeightList {
		get {
			CorrectSrcLineWeightList();
			return srcLineWeightList;
		}
	}
	[SerializeField, Tooltip("線の重さが指定されていない場合に使用される値")]
	float defLineWeight = 1.0f;

	[SerializeField, Tooltip("最後の点からのベクトルを示す位置\n指定がなければ直前の地点からの方向ベクトルが使用される")]
	Transform endVecPoint = null;
	public Transform EndVecPoint {
		get {
			return endVecPoint;
		}
		set {
			endVecPoint = value;
		}
	}

	float TotalWeight {
		get {
			float ret = 0.0f;
			foreach (var srcWeight in srcLineWeightList) {
				ret += srcWeight;
			}
			return ret;
		}
	}

	void Start() {
		CorrectSrcLineWeightList();
	}

	[ContextMenu("CorrectSrcLineWeightList")]
	public void CorrectSrcLineWeightList() {
		// 線の重みリストが存在しなければ作成
		if (srcLineWeightList == null) {
			srcLineWeightList = new List<float>();
		}

		// 線の数
		int lineNum = 0;
		if ((SrcPointList != null) && (SrcPointList.Count > 0)) {
			lineNum = (SrcPointList.Count - 1);
		}

		// 線の重みリストの要素数が多ければ削除
		while (srcLineWeightList.Count > lineNum) {
			srcLineWeightList.RemoveAt(srcLineWeightList.Count - 1);
		}

		// 線の重みリストの要素数が少なければ追加
		while (srcLineWeightList.Count < lineNum) {
			srcLineWeightList.Add(defLineWeight);
		}
	}

	public Vector3 GetPoint(float _totalRatio) {
		// 全体の割合がどの区間かと、その区間内での割合を求める
		float weight = (TotalWeight * _totalRatio);
		float lineWeight = weight;
		int lineIdx = 0;
		for (int idx = 0; idx < SrcLineWeightList.Count; idx++) {
			weight -= SrcLineWeightList[idx];
			if (weight <= 0.0f) {
				lineIdx = idx;
				break;
			}
			lineWeight -= SrcLineWeightList[idx];
		}
		float lineRatio = (lineWeight / SrcLineWeightList[lineIdx]);
		
		// 区間と区間内の割合から位置を求める
		Vector3 midPoint = ((SrcPointList[lineIdx].position + SrcPointList[lineIdx + 1].position) * 0.5f);

		Vector3 startVec = Vector3.zero;
		//startVec = (midPoint - SrcPointList[lineIdx].position);
		if (lineIdx <= 0) {
			startVec = (SrcPointList[lineIdx + 1].position - SrcPointList[lineIdx].position);
		}
		else {
			Vector3 centerVec = ((SrcPointList[lineIdx - 1].position - SrcPointList[lineIdx].position) + (SrcPointList[lineIdx + 1].position - SrcPointList[lineIdx].position));
			Vector3 rightVec = Quaternion.Euler(0.0f, 90.0f, 0.0f) * centerVec;
			Vector3 leftVec = Quaternion.Euler(0.0f, -90.0f, 0.0f) * centerVec;
			Vector3 nextVec = (SrcPointList[lineIdx + 1].position - SrcPointList[lineIdx].position);
			if(Vector3.Dot(nextVec, rightVec) > Vector3.Dot(nextVec, leftVec)) {
				startVec = rightVec;
			}else {
				startVec = leftVec;
			}
		}

		Vector3 endVec = Vector3.zero;
		if (lineIdx < (SrcPointList.Count - 2)) {
			Vector3 centerVec = ((SrcPointList[lineIdx].position - SrcPointList[lineIdx + 1].position) + (SrcPointList[lineIdx + 2].position - SrcPointList[lineIdx + 1].position));
			Vector3 rightVec = Quaternion.Euler(0.0f, 90.0f, 0.0f) * centerVec;
			Vector3 leftVec = Quaternion.Euler(0.0f, -90.0f, 0.0f) * centerVec;
			Vector3 nextVec = (SrcPointList[lineIdx + 2].position - SrcPointList[lineIdx + 1].position);
			if (Vector3.Dot(nextVec, rightVec) > Vector3.Dot(nextVec, leftVec)) {
				endVec = rightVec;
			}
			else {
				endVec = leftVec;
			}
		}
		else {
			if (endVecPoint && ((lineIdx + 1) == (SrcLineWeightList.Count))) {
				endVec = (endVecPoint.position - SrcPointList[lineIdx + 1].position);
			}
			else {
				endVec = (SrcPointList[lineIdx + 1].position - midPoint);
			}
		}
		return GetHermiteCurvePoint(lineRatio, SrcPointList[lineIdx].position, startVec, SrcPointList[lineIdx + 1].position, endVec);
	}

	Vector3 GetHermiteCurvePoint(float _ratio, Vector3 _startPoint, Vector3 _startVec, Vector3 _endPoint, Vector3 _endVec) {
		Vector3 ret;
		float u1 = _ratio;
		float u2 = u1 * u1;
		float u3 = u1 * u1 * u1;
		float p0 = 2.0f * u3 - 3.0f * u2 + 1.0f;
		float v0 = u3 - 2.0f * u2 + u1;
		float p1 = -2.0f * u3 + 3.0f * u2;
		float v1 = u3 - u2;
		ret = new Vector3(
			_startPoint.x * p0 + _startVec.x * v0 + _endPoint.x * p1 + _endVec.x * v1,
			_startPoint.y * p0 + _startVec.y * v0 + _endPoint.y * p1 + _endVec.y * v1,
			_startPoint.z * p0 + _startVec.z * v0 + _endPoint.z * p1 + _endVec.z * v1);
		return ret;
	}
}
