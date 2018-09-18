using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotation : MonoBehaviour {
	[SerializeField]
	Transform target = null;

	void Update () {
		target.rotation = transform.rotation;
	}
}
