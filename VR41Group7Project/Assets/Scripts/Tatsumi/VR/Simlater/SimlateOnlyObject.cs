using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimlateOnlyObject : MonoBehaviour {
	static List<SimlateOnlyObject> simOnlyObjList = new List<SimlateOnlyObject>();
	public static void SetSimOnlyObjListActive(bool _isActive) {
		if ((lastIsActive == null) || ((bool)lastIsActive != _isActive)) {
			lastIsActive = _isActive;
			foreach (var simOnlyObj in simOnlyObjList) {
				if (simOnlyObj) {
					simOnlyObj.gameObject.SetActive(_isActive);
				}
			}
		}
	}
	static bool? lastIsActive = null;

	void Start() {
		if (lastIsActive == null) {
			lastIsActive = SimlateManager.Enable;
		}

		simOnlyObjList.Add(this);
		gameObject.SetActive((bool)lastIsActive);
	}
}
