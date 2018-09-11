using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDamageSource : MonoBehaviour {
	[SerializeField, Tooltip("ダメージ量")]
	int dmg = 1;
	public int Dmg {
		get {
			return dmg;
		}
	}

	[SerializeField, Tooltip("ダメージを発生させた時に発生するイベント")]
	UnityEngine.Events.UnityEvent events = new UnityEngine.Events.UnityEvent();
	public UnityEngine.Events.UnityEvent Events {
		get {
			return events;
		}
	}
}
