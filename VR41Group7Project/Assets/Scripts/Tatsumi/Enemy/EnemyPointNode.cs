using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointNode : MonoBehaviour {
	[SerializeField, Tooltip("使用中のTransform"), ReadOnly]
	Transform user = null;
	public Transform User {
		get {
			return user;
		}
		set {
			user = value;
		}
	}

	public bool IsUsed {
		get {
			return (User != null);
		}
	}
}
