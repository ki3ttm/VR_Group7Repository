using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicNoticeLine : MonoBehaviour {
	[SerializeField, Tooltip("初速")]
	Vector3 firstVelocity = new Vector3();
	public Vector3 FirstVelocity {
		get {
			return firstVelocity;
		}
		set {
			firstVelocity = value;
		}
	}

	void Start () {
		
	}
	
	void FixedUpdate () {
		DrawLines();
	}

	public Vector3 CalcPosition(float _time) {
		Vector3 pos = ((firstVelocity * _time) + (Physics.gravity * (_time * _time)));
		return pos;
	}

	public float CalcLandingTime() {
		float time = 0.0f;

		// 初速方向が上向きなら
		if (FirstVelocity.y > 0.0f) {
			// 開始点から頂点に達するまでの時間を求める
			time += (FirstVelocity.y / Physics.gravity.y);

			// 頂点から指定の高さまでの自由落下に掛かる時間を求める
//			time += ();

//			spd == firstVelocity + Physics.gravity * time;
		}
		// 初速方向が下向きなら
		else {
			// 開始点から指定の高さまでの落下に掛かる時間を求める

		}

		return time;
	}

	public Vector3 CalcLandingPosition(out float _time, float _landHeight = 0.0f) {
		_time = 0.0f;

		// 落下点を返す
		return (CalcPosition(_time));
	}

	public Vector3 CalcLandingPosition(float _landHeight = 0.0f) {
		float dummyTime;
		return CalcLandingPosition(out dummyTime);
	}

	void DrawLines() {

	}

	void DrawLine(float _ratio) {
		
	}
}
