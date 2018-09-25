using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ImpactEvent : MonoBehaviour {
	[SerializeField, Tooltip("発生させるイベント")]
	UnityEngine.Events.UnityEvent events = new UnityEngine.Events.UnityEvent();
	[SerializeField, Tooltip("イベントを発生させる衝撃の最低値")]
	float border = 1.0f;
	[SerializeField, Tooltip("生成以降に受けた最大の衝撃"), ReadOnly]
	float prevMaxImpact = 0.0f;

	void OnCollisionEnter(Collision _col) {
		CheckImpact(_col);
	}
	void OnCollisionStay(Collision _col) {
		CheckImpact(_col);
	}

	void CheckImpact(Collision _col) {
		Vector3 vel = GetComponent<Rigidbody>().velocity;

		// 相手側にもRigidbodyがあればその勢いを計算に加える
		Rigidbody colRb = _col.collider.GetComponent<Rigidbody>();
		if (colRb) {
			vel -= colRb.velocity;
		}

		prevMaxImpact = Mathf.Max(prevMaxImpact, vel.magnitude);

		// 勢いが一定以下なら
		if (vel.magnitude <= border) return;

		// 一定以上の勢いで衝突したらイベント実行
		Debug.Log("ImpactEvent");
		events.Invoke();
	}

	public void SelfDestroy() {
		Destroy(gameObject);
	}
	public void InstantiateObject(GameObject _prefab) {
		GameObject obj = Instantiate(_prefab);
		obj.transform.position = transform.position;
	}
}
