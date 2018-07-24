using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour {
	[SerializeField]
	Transform grabPoint = null;
	public Transform GrabPoint {
		get {
			if (!grabPoint) {
				grabPoint = transform;
			}
			return grabPoint;
		}
	}

	[SerializeField]
	bool isGrab = false;
	public bool IsGrab {
		get {
			return isGrab;
		}
		set {
			isGrab = value;
		}
	}
	
	void Start () {}
	
	void Update () {}
}
