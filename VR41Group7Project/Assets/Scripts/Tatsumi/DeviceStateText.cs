using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceStateText : MonoBehaviour {
	[SerializeField]
	TextMesh textMesh = null;
	TextMesh TextMesh {
		get {
			if (!textMesh) {
				textMesh = GetComponent<TextMesh>();
			}
			if (!textMesh) {
				enabled = false;
			}
			return TextMesh;
		}
	}

	void Start() { }

	void Update() {
		if (!textMesh) return;

		// テキストを構成
		textMesh.text =
		"ControllerInput\n" +
//		" Touchpad	L:" + .ViveSimMng.GetPress(ViveSimulationManager.ViveDeviceType.LeftHand, SteamVR_Controller.ButtonMask.Touchpad) + "	R:" + ViveSimulationManager.ViveSimMng.GetPress(ViveSimulationManager.ViveDeviceType.RightHand, SteamVR_Controller.ButtonMask.Touchpad) + "\n" +
//		" Trigger\t	L:" + ViveSimulationManager.ViveSimMng.GetPress(ViveSimulationManager.ViveDeviceType.LeftHand, SteamVR_Controller.ButtonMask.Trigger) + "	R:" + ViveSimulationManager.ViveSimMng.GetPress(ViveSimulationManager.ViveDeviceType.RightHand, SteamVR_Controller.ButtonMask.Trigger) + "\n" +
//		" Grip\t\t	L:" + ViveSimulationManager.ViveSimMng.GetPress(ViveSimulationManager.ViveDeviceType.LeftHand, SteamVR_Controller.ButtonMask.Grip) + "	R:" + ViveSimulationManager.ViveSimMng.GetPress(ViveSimulationManager.ViveDeviceType.RightHand, SteamVR_Controller.ButtonMask.Grip) + "\n" +
		"\n";
	}
}
