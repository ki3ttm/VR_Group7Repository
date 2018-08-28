using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {
	[SerializeField, Tooltip("対象")]
	Transform target =null;
	public Transform Target {
		get {
			return target;
		}
		set {
			target = value;
		}
	}
	[SerializeField, Tooltip("近づく速度")]
	float spd = 0.1f;

	void FixedUpdate () {
		float dis = Vector3.Distance(transform.position, target.position);
		Vector3 vec = (target.position - transform.position);
		transform.position += (vec * Mathf.Min(dis, spd));
	}
}
