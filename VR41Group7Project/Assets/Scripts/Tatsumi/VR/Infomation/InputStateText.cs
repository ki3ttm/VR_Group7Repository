using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputStateText : MonoBehaviour {
	enum InputStateType {
		LeftTouchPad,
		LeftTouchPadDown,
		LeftTouchPadUp,
		LeftTrigger,
		LeftTriggerDown,
		LeftTriggerUp,
		LeftGrip,
		LeftGripDown,
		LeftGripUp,
		RightTouchPad,
		RightTouchPadDown,
		RightTouchPadUp,
		RightTrigger,
		RightTriggerDown,
		RightTriggerUp,
		RightGrip,
		RightGripDown,
		RightGripUp,
		Max,
	}

	[SerializeField, Tooltip("出力先のTextMesh")]
	TextMesh textMesh = null;
	TextMesh TextMesh {
		get {
			if (!textMesh) {
				textMesh = GetComponent<TextMesh>();
			}
			if (!textMesh) {
				enabled = false;
			}
			return textMesh;
		}
	}

	[SerializeField, Tooltip("有効時の文字色")]
	Color trueColor = Color.red;
	[SerializeField, Tooltip("無効時の文字色")]
	Color falseColor = Color.white;
	[SerializeField, Tooltip("無効化直後のフェード開始時の文字色")]
	Color afterColor = new Color(1.0f, 0.5f, 0.0f, 1.0f);
	[SerializeField, Tooltip("AfterColorからFalseColorにフェードする秒数")]
	float afterTime = 1.0f;

	// 最後に入力された時間
	float[] inputTimeArray = new float[(int)InputStateType.Max];

	void Start() {
		for (int idx = 0; idx < inputTimeArray.Length; idx++) {
			inputTimeArray[idx] = float.MinValue;
		}
	}

	void Update() {
		if (!TextMesh) return;

		// 入力を更新
		UpdateInputTime(0,	SimlateManager.Instance.VirtualLeftCtrl.GetTouchPad());
		UpdateInputTime(1,	SimlateManager.Instance.VirtualLeftCtrl.GetTouchPadDown());
		UpdateInputTime(2,	SimlateManager.Instance.VirtualLeftCtrl.GetTouchPadUp());
		UpdateInputTime(3,	SimlateManager.Instance.VirtualLeftCtrl.GetHairTrigger());
		UpdateInputTime(4,	SimlateManager.Instance.VirtualLeftCtrl.GetHairTriggerDown());
		UpdateInputTime(5,	SimlateManager.Instance.VirtualLeftCtrl.GetHairTriggerUp());
		UpdateInputTime(6,	SimlateManager.Instance.VirtualLeftCtrl.GetGrip());
		UpdateInputTime(7,	SimlateManager.Instance.VirtualLeftCtrl.GetGripDown());
		UpdateInputTime(8,	SimlateManager.Instance.VirtualLeftCtrl.GetGripUp());
		UpdateInputTime(9,	SimlateManager.Instance.VirtualRightCtrl.GetTouchPad());
		UpdateInputTime(10,	SimlateManager.Instance.VirtualRightCtrl.GetTouchPadDown());
		UpdateInputTime(11,	SimlateManager.Instance.VirtualRightCtrl.GetTouchPadUp());
		UpdateInputTime(12,	SimlateManager.Instance.VirtualRightCtrl.GetHairTrigger());
		UpdateInputTime(13,	SimlateManager.Instance.VirtualRightCtrl.GetHairTriggerDown());
		UpdateInputTime(14,	SimlateManager.Instance.VirtualRightCtrl.GetHairTriggerUp());
		UpdateInputTime(15,	SimlateManager.Instance.VirtualRightCtrl.GetGrip());
		UpdateInputTime(16,	SimlateManager.Instance.VirtualRightCtrl.GetGripDown());
		UpdateInputTime(17,	SimlateManager.Instance.VirtualRightCtrl.GetGripUp());

		// テキストを構成
		string text = "";
		text =
		"CtrlFlg(L/R):(" + (SimlateManager.Instance.LeftCtrlFlg ? "true" : "false") + "/" + (SimlateManager.Instance.RightCtrlFlg ? "true" : "false") + ")\n" +
		"InputState(press,trig,release)\n" +
		"\tTouchPad\t\t\t\t\tHairTrigger\t\t\t\t\tGrip \n";
		
		for (int idx = 0; idx < inputTimeArray.Length; idx++) {
			switch ((InputStateType)idx) {
			case InputStateType.LeftTouchPad:
				text += "L:\t(";
				break;
			case InputStateType.LeftTrigger:
				text += "\t(";
				break;
			case InputStateType.LeftGrip:
				text += "\t(";
				break;
			case InputStateType.RightTouchPad:
				text += "R:\t(";
				break;
			case InputStateType.RightTrigger:
				text += "\t(";
				break;
			case InputStateType.RightGrip:
				text += "\t(";
				break;
			case InputStateType.LeftTouchPadDown:
				text += ", ";
				break;
			case InputStateType.LeftTriggerDown:
				text += ", ";
				break;
			case InputStateType.LeftGripDown:
				text += ", ";
				break;
			case InputStateType.RightTouchPadDown:
				text += ", ";
				break;
			case InputStateType.RightTriggerDown:
				text += ", ";
				break;
			case InputStateType.RightGripDown:
				text += ", ";
				break;
			case InputStateType.LeftTouchPadUp:
				text += ", ";
				break;
			case InputStateType.LeftTriggerUp:
				text += ", ";
				break;
			case InputStateType.LeftGripUp:
				text += ", ";
				break;
			case InputStateType.RightTouchPadUp:
				text += ", ";
				break;
			case InputStateType.RightTriggerUp:
				text += ", ";
				break;
			case InputStateType.RightGripUp:
				text += ", ";
				break;
			}

			// 入力状態を取得
			bool flg = false;
			switch ((InputStateType)idx) {
			case InputStateType.LeftTouchPad:
				flg = SimlateManager.Instance.VirtualLeftCtrl.GetTouchPad();
				break;
			case InputStateType.LeftTouchPadDown:
				flg = SimlateManager.Instance.VirtualLeftCtrl.GetTouchPadDown();
				break;
			case InputStateType.LeftTouchPadUp:
				flg = SimlateManager.Instance.VirtualLeftCtrl.GetTouchPadUp();
				break;
			case InputStateType.LeftTrigger:
				flg = SimlateManager.Instance.VirtualLeftCtrl.GetHairTrigger();
				break;
			case InputStateType.LeftTriggerDown:
				flg = SimlateManager.Instance.VirtualLeftCtrl.GetHairTriggerDown();
				break;
			case InputStateType.LeftTriggerUp:
				flg = SimlateManager.Instance.VirtualLeftCtrl.GetHairTriggerUp();
				break;
			case InputStateType.LeftGrip:
				flg = SimlateManager.Instance.VirtualLeftCtrl.GetGrip();
				break;
			case InputStateType.LeftGripDown:
				flg = SimlateManager.Instance.VirtualLeftCtrl.GetGripDown();
				break;
			case InputStateType.LeftGripUp:
				flg = SimlateManager.Instance.VirtualLeftCtrl.GetGripUp();
				break;
			case InputStateType.RightTouchPad:
				flg = SimlateManager.Instance.VirtualRightCtrl.GetTouchPad();
				break;
			case InputStateType.RightTouchPadDown:
				flg = SimlateManager.Instance.VirtualRightCtrl.GetTouchPadDown();
				break;
			case InputStateType.RightTouchPadUp:
				flg = SimlateManager.Instance.VirtualRightCtrl.GetTouchPadUp();
				break;
			case InputStateType.RightTrigger:
				flg = SimlateManager.Instance.VirtualRightCtrl.GetHairTrigger();
				break;
			case InputStateType.RightTriggerDown:
				flg = SimlateManager.Instance.VirtualRightCtrl.GetHairTriggerDown();
				break;
			case InputStateType.RightTriggerUp:
				flg = SimlateManager.Instance.VirtualRightCtrl.GetHairTriggerUp();
				break;
			case InputStateType.RightGrip:
				flg = SimlateManager.Instance.VirtualRightCtrl.GetGrip();
				break;
			case InputStateType.RightGripDown:
				flg = SimlateManager.Instance.VirtualRightCtrl.GetGripDown();
				break;
			case InputStateType.RightGripUp:
				flg = SimlateManager.Instance.VirtualRightCtrl.GetGripUp();
				break;
			}

			float inputTime = inputTimeArray[idx];
			while(inputTime >= 100.0f) {
				inputTime -= 100.0f;
			}
			inputTimeArray[idx] = inputTime;
			if (inputTime < 0.0f) {
				inputTime = 0.0f;
			}
			if (flg) {
				text += "<color=" + ColorToString(trueColor) + ">" + inputTime.ToString("00.00") + "</color>";
			}
			else {
				float t = ((Time.time - inputTimeArray[idx]) / afterTime);
				t = Mathf.Clamp(t, 0.0f, 1.0f);
				Color col = Color.Lerp(afterColor, falseColor, t);
				text += "<color=" + ColorToString(col) + ">" + inputTime.ToString("00.00") + "</color>";
			}

			switch ((InputStateType)idx) {
			case InputStateType.LeftTouchPadUp:
				text += ")";
				break;
			case InputStateType.LeftTriggerUp:
				text += ")";
				break;
			case InputStateType.LeftGripUp:
				text += ")\n";
				break;
			case InputStateType.RightTouchPadUp:
				text += ")";
				break;
			case InputStateType.RightTriggerUp:
				text += ")";
				break;
			case InputStateType.RightGripUp:
				text += ")\n";
				break;
			}
			TextMesh.text = text;
		}
	}

	string ColorToString(Color _col) {
		return ("#" + ((int)(_col.r * 255)).ToString("x2") + ((int)(_col.g * 255)).ToString("x2") + ((int)(_col.b * 255)).ToString("x2") + ((int)(_col.a * 255)).ToString("x2"));
	}

	void UpdateInputTime(int _idx, bool _flg) {
		if (_flg) {
			inputTimeArray[_idx] = Time.time;
		}
	}
}
