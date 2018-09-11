using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyPosition : MonoBehaviour {
	[SerializeField, Tooltip("頭部")]
	Transform headPoint = null;
	[SerializeField, Tooltip("首")]
	Transform neckPoint = null;
	[SerializeField, Tooltip("頭部と首の距離")]
	float neckLen = 0.1f;

	void Update() {
		transform.position = (headPoint.position + (headPoint.rotation * (Vector3.down * neckLen)));
		transform.rotation = Quaternion.Euler(0.0f, headPoint.rotation.eulerAngles.y, 0.0f);
	}
}

