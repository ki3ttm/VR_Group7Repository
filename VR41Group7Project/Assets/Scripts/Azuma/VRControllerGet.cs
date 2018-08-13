using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VRコントローラーの取得
/// </summary>
public class VRControllerGet : MonoBehaviour {
	private SteamVR_TrackedObject trackedObjLeft;
	private SteamVR_TrackedObject trackedObjRight;
	[SerializeField] private GameObject leftControllerObj;
	[SerializeField] private GameObject rightControllerObj;

	private void Awake() {
		trackedObjLeft = leftControllerObj.GetComponent<SteamVR_TrackedObject>();
		trackedObjRight = rightControllerObj.GetComponent<SteamVR_TrackedObject>();
	}

	public SteamVR_Controller.Device LeftController {
		get { return SteamVR_Controller.Input((int)trackedObjLeft.index); }
	}

	public SteamVR_Controller.Device RightController {
		get { return SteamVR_Controller.Input((int)trackedObjRight.index); }
	}

}
