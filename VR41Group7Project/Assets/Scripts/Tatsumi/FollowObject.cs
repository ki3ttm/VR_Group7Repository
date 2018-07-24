using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {
	[SerializeField]
	GameObject followTarget = null;		// 追従対象
	[SerializeField]
	GameObject followAltTarget = null;  // 予備追従対象
	[SerializeField]
	Transform followAltParentTarget;	// 予備追従時に親となる対象

	void Start () {
		if (!followTarget) {
			transform.parent = followAltParentTarget;
		}
	}

	void Update() {
		if (followTarget && followTarget.activeInHierarchy) {
			transform.position = followTarget.transform.position;
			transform.rotation = followTarget.transform.rotation;
		}
		else if (followAltTarget && followAltTarget.activeInHierarchy) {
			transform.position = followAltTarget.transform.position;
			transform.rotation = followAltTarget.transform.rotation;
		}
	}
}
