using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyViewer : MonoBehaviour {
	[SerializeField]
	Vector3 vel = Vector3.zero;
	[SerializeField]
	Vector3 angularVelocity = Vector3.zero;

	Rigidbody rb;
	void Start() {
		rb = GetComponent<Rigidbody>();
	}
	void FixedUpdate() {
		vel = rb.velocity;
		angularVelocity = rb.angularVelocity;
	}
}
