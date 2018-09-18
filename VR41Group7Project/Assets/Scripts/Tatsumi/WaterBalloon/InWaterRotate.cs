using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InWaterRotate : MonoBehaviour {
	Rigidbody rb;
	[SerializeField]
	float vel = 1.0f;
	[SerializeField, ReadOnly]
	Vector3 angularVel = Vector3.zero;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	void Update() {
		Vector3 torque = transform.rotation.eulerAngles;
		if (torque.x > 180.0f) {
			torque.x -= 360.0f;
		}
		if (torque.z > 180.0f) {
			torque.z -= 360.0f;
		}
		torque.y = 0.0f;
		rb.AddTorque(-torque * vel, ForceMode.Force);

		angularVel = rb.angularVelocity;
	}
}
