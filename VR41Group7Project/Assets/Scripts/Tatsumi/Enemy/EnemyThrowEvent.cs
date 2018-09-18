using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowEvent : MonoBehaviour {
	[SerializeField]
	EnemyManager enemyMng = null;

	public void ThrowEvent() {
		enemyMng.ThrowAction();
	}
}
