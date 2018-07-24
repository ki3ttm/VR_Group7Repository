using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualMove : MonoBehaviour {
	public enum State {
		Enable,     // 有効
		Disable,    // 無効
		Auto,       // SteamVRの状態によって切り替える
	}

	[Header("現在の状態"), SerializeField, Tooltip("使用状態")]
	State st = State.Disable;
	State St {
		get {
			return st;
		}
		set {
			if (value == State.Auto) {
				if (SteamVR.active) {
					st = State.Enable;
				} else {
					st = State.Disable;
				}
			} else {
				st = value;
			}
		}
	}
	[SerializeField, Tooltip("左手操作フラグ")]
	bool leftCtrlFlg = false;
	[SerializeField, Tooltip("右手操作フラグ")]
	bool rightCtrlFlg = false;

	[Header("関係オブジェクトの参照"), SerializeField, Tooltip("頭部(SteamVR)のTransform")]
	Transform headTrans = null;
	[SerializeField, Tooltip("左手(VirtualViveController)のTransform")]
	Transform leftCtrlTrans = null;
	[SerializeField, Tooltip("右手(VirtualViveController)のTransform")]
	Transform rightCtrlTrans = null;
	[SerializeField, Tooltip("左手のRigidbody"), ReadOnly]
	Rigidbody leftCtrlRb = null;
	[SerializeField, Tooltip("右手のRigidbody"), ReadOnly]
	Rigidbody rightCtrlRb = null;
	[SerializeField, Tooltip("左手のVirtualViveController"), ReadOnly]
	VirtualViveController leftCtrlVirtualViveCtrl = null;
	[SerializeField, Tooltip("右手のVirtualViveController"), ReadOnly]
	VirtualViveController rightCtrlVirtualViveCtrl = null;

	[Header("速度"), SerializeField, Tooltip("カメラ移動速度")]
	Vector3 camMoveSpd = (Vector3.one * 0.1f);
	[SerializeField, Tooltip("カメラ上昇速度")]
	float camUpSpd = 0.1f;
	[SerializeField, Tooltip("カメラ下降速度")]
	float camDownSpd = 0.1f;
	[SerializeField, Tooltip("カメラ回転マウス感度(縦)")]
	float camRotMouseSenceVertical = 1.0f;
	[SerializeField, Tooltip("カメラ回転マウス感度(横)")]
	float camRotMouseSenceHorizontal = 1.0f;
	[SerializeField, Tooltip("カメラ回転キー感度(縦)")]
	float camRotKeySenceVertical = 1.0f;
	[SerializeField, Tooltip("カメラ回転キー感度(横)")]
	float camRotKeySenceHorizontal = 1.0f;

	[Header("ダブルクリック"), SerializeField, Tooltip("ダブルクリックの受付時間")]
	float doubleClickTime = 0.2f;
	[SerializeField, Tooltip("左クリックが最後に入力された時間"), ReadOnly]
	float leftLastClickTime = float.MinValue;
	[SerializeField, Tooltip("右クリックが最後に入力された時間"), ReadOnly]
	float rightLastClickTime = float.MinValue;
	[SerializeField, Tooltip("中クリックが最後に入力された時間"), ReadOnly]
	float midLastClickTime = float.MinValue;

	[Header("キーコード"), SerializeField, Tooltip("前進")]
	List<KeyCode> forwardKeyCodeList = new List<KeyCode>() { KeyCode.W };
	[SerializeField, Tooltip("後退")]
	List<KeyCode> backKeyCodeList = new List<KeyCode>() { KeyCode.S };
	[SerializeField, Tooltip("左平行移動")]
	List<KeyCode> leftKeyCodeList = new List<KeyCode>() { KeyCode.A };
	[SerializeField, Tooltip("右平行移動")]
	List<KeyCode> rightKeyCodeList = new List<KeyCode>() { KeyCode.D };
	[SerializeField, Tooltip("上昇")]
	List<KeyCode> upKeyCodeList = new List<KeyCode>() { KeyCode.E, KeyCode.Space };
	[SerializeField, Tooltip("下降")]
	List<KeyCode> downKeyCodeList = new List<KeyCode>() { KeyCode.Q, KeyCode.LeftShift };
	[SerializeField, Tooltip("上回転")]
	List<KeyCode> upRotKeyCodeList = new List<KeyCode>() { KeyCode.I };
	[SerializeField, Tooltip("下回転")]
	List<KeyCode> downRotKeyCodeList = new List<KeyCode>() { KeyCode.K };
	[SerializeField, Tooltip("左回転")]
	List<KeyCode> leftRotKeyCodeList = new List<KeyCode>() { KeyCode.J };
	[SerializeField, Tooltip("右回転")]
	List<KeyCode> rightRotKeyCodeList = new List<KeyCode>() { KeyCode.L };
	[SerializeField, Tooltip("左コントローラー操作")]
	List<KeyCode> leftCtrlKeyCodeList = new List<KeyCode>() { KeyCode.Mouse0, KeyCode.U };
	[SerializeField, Tooltip("右コントローラー操作")]
	List<KeyCode> rightCtrlKeyCodeList = new List<KeyCode>() { KeyCode.Mouse1, KeyCode.O };
	[SerializeField, Tooltip("タッチパッド")]
	List<KeyCode> touchPadKeyCodeList = new List<KeyCode>() { KeyCode.Y };
	[SerializeField, Tooltip("トリガー")]
	List<KeyCode> triggerKeyCodeList = new List<KeyCode>() { KeyCode.H };
	[SerializeField, Tooltip("グリップ")]
	List<KeyCode> gripKeyCodeList = new List<KeyCode>() { KeyCode.N };

	[Header("入力状態"), SerializeField, Tooltip("前進")]
	bool fixedForwardInput = false;
	public bool FixedForwardInput {
		get {
			return fixedForwardInput;
		}
		set {
			fixedForwardInput = value;
		}
	}
	[SerializeField, Tooltip("後退")]
	bool fixedBackInput = false;
	public bool FixedBackInput {
		get {
			return fixedBackInput;
		}
		set {
			fixedBackInput = value;
		}
	}
	[SerializeField, Tooltip("左平行移動")]
	bool fixedLeftInput = false;
	public bool FixedLeftInput {
		get {
			return fixedLeftInput;
		}
		set {
			fixedLeftInput = value;
		}
	}
	[SerializeField, Tooltip("右平行移動")]
	bool fixedRightInput = false;
	public bool FixedRightInput {
		get {
			return fixedRightInput;
		}
		set {
			fixedRightInput = value;
		}
	}
	[SerializeField, Tooltip("上昇")]
	bool fixedUpInput = false;
	public bool FixedUpInput {
		get {
			return fixedUpInput;
		}
		set {
			fixedUpInput = value;
		}
	}
	[SerializeField, Tooltip("下降")]
	bool fixedDownInput = false;
	public bool FixedDownInput {
		get {
			return fixedDownInput;
		}
		set {
			fixedDownInput = value;
		}
	}
	[SerializeField, Tooltip("上回転")]
	bool fixedRotUpInput = false;
	public bool FixedRotUpInput {
		get {
			return fixedRotUpInput;
		}
		set {
			fixedRotUpInput = value;
		}
	}
	[SerializeField, Tooltip("下回転")]
	bool fixedRotDownInput = false;
	public bool FixedRotDownInput {
		get {
			return fixedRotDownInput;
		}
		set {
			fixedRotDownInput = value;
		}
	}
	[SerializeField, Tooltip("左回転")]
	bool fixedRotLeftInput = false;
	public bool FixedRotLeftInput {
		get {
			return fixedRotLeftInput;
		}
		set {
			fixedRotLeftInput = value;
		}
	}
	[SerializeField, Tooltip("右回転")]
	bool fixedRotRightInput = false;
	public bool FixedRotRightInput {
		get {
			return fixedRotRightInput;
		}
		set {
			fixedRotRightInput = value;
		}
	}
	[SerializeField, Tooltip("左コントローラー操作")]
	bool fixedLeftCtrlInput = false;
	public bool FixedLeftCtrlInput {
		get {
			return fixedLeftCtrlInput;
		}
		set {
			fixedLeftCtrlInput = value;
		}
	}
	[SerializeField, Tooltip("右コントローラー操作")]
	bool fixedRightCtrlInput = false;
	public bool FixedRightCtrlInput {
		get {
			return fixedRightCtrlInput;
		}
		set {
			fixedRightCtrlInput = value;
		}
	}
	[SerializeField, Tooltip("タッチパッド")]
	bool fixedTouchPadInput = false;
	public bool FixedTouchPadInput {
		get {
			return fixedTouchPadInput;
		}
		set {
			fixedTouchPadInput = value;
		}
	}
	[SerializeField, Tooltip("トリガー")]
	bool fixedTriggerInput = false;
	public bool FixedTriggerInput {
		get {
			return fixedTriggerInput;
		}
		set {
			fixedTriggerInput = value;
		}
	}
	[SerializeField, Tooltip("グリップ")]
	bool fixedGripInput = false;
	public bool FixedGripInput {
		get {
			return fixedGripInput;
		}
		set {
			fixedGripInput = value;
		}
	}

	[Header("前回入力状態"), SerializeField, Tooltip("前進")]
	bool prevFixedForwardInput = false;
	[SerializeField, Tooltip("後退")]
	bool prevFixedBackInput = false;
	[SerializeField, Tooltip("左平行移動")]
	bool prevFixedLeftInput = false;
	[SerializeField, Tooltip("右平行移動")]
	bool prevFixedRightInput = false;
	[SerializeField, Tooltip("上昇")]
	bool prevFixedUpInput = false;
	[SerializeField, Tooltip("下降")]
	bool prevFixedDownInput = false;
	[SerializeField, Tooltip("上回転")]
	bool prevFixedRotUpInput = false;
	[SerializeField, Tooltip("下回転")]
	bool prevFixedRotDownInput = false;
	[SerializeField, Tooltip("左回転")]
	bool prevFixedRotLeftInput = false;
	[SerializeField, Tooltip("右回転")]
	bool prevFixedRotRightInput = false;
	[SerializeField, Tooltip("左コントローラー操作")]
	bool prevFixedLeftCtrlInput = false;
	[SerializeField, Tooltip("右コントローラー操作")]
	bool prevFixedRightCtrlInput = false;
	[SerializeField, Tooltip("タッチパッド")]
	bool prevFixedTouchPadInput = false;
	[SerializeField, Tooltip("トリガー")]
	bool prevFixedTriggerInput = false;
	[SerializeField, Tooltip("グリップ")]
	bool prevFixedGripInput = false;

	// FixedUpdate()の後でUpdate()を通るまではtrue、Update()でtrueなら入力を削除してfalseになる
	bool firstUpdateFlg = false;

	[Header("コントローラー操作のダブルクリック"), SerializeField, Tooltip("左コントローラー操作のダブルクリック判定")]
	bool leftCtrlDoubleClick = false;
	[SerializeField, Tooltip("右コントローラー操作のダブルクリック判定")]
	bool rightCtrlDoubleClick = false;
	[SerializeField, Tooltip("左コントローラー操作の最後に入力があった時間")]
	float lastLeftCtrlInputTime = float.MinValue;
	[SerializeField, Tooltip("右コントローラー操作の最後に入力があった時間")]
	float lastRightCtrlInputTime = float.MinValue;
	[SerializeField, Tooltip("左コントローラー操作の入力前回取得時の状態")]
	bool prevLeftCtrlInput = false;
	[SerializeField, Tooltip("右コントローラー操作の入力前回取得時の状態")]
	bool prevRightCtrlInput = false;

	// コンポーネントへの参照
	VirtualViveController virtualViveCtrl = null;
	VirtualViveController VirtualViveCtrl {
		get {
			if (!virtualViveCtrl) {
				virtualViveCtrl = GetComponent<VirtualViveController>();
			}
			return virtualViveCtrl;
		}
	}

	void Start() {
		leftCtrlRb = leftCtrlTrans.GetComponent<Rigidbody>();
		rightCtrlRb = rightCtrlTrans.GetComponent<Rigidbody>();
		leftCtrlVirtualViveCtrl = leftCtrlTrans.GetComponent<VirtualViveController>();
		rightCtrlVirtualViveCtrl = rightCtrlTrans.GetComponent<VirtualViveController>();
	}

	void Update() {
		// 入力があれば保持
		UpdateInput();
	}
	void UpdateInput() {
		// 前回のFixedUpdate()から初回のUpdate()なら
		if (firstUpdateFlg) {
			firstUpdateFlg = false;

			// 前回の入力状態を削除
			FixedForwardInput	= false;
			FixedBackInput		= false;
			FixedLeftInput		= false;
			FixedRightInput		= false;
			FixedUpInput		= false;
			FixedDownInput		= false;
			FixedLeftCtrlInput	= false;
			FixedRightCtrlInput	= false;
			FixedRotUpInput		= false;
			FixedRotDownInput	= false;
			FixedRotLeftInput	= false;
			FixedRotRightInput	= false;
			FixedTouchPadInput	= false;
			FixedTriggerInput	= false;
			FixedGripInput		= false;

			leftCtrlDoubleClick		= false;
			rightCtrlDoubleClick	= false;
		}

		if (!fixedForwardInput && GetInput(forwardKeyCodeList)) {
			fixedForwardInput = true;
		}
		if (!fixedBackInput && GetInput(backKeyCodeList)) {
			fixedBackInput = true;
		}
		if (!fixedLeftInput && GetInput(leftKeyCodeList)) {
			fixedLeftInput = true;
		}
		if (!fixedRightInput && GetInput(rightKeyCodeList)) {
			fixedRightInput = true;
		}
		if (!fixedUpInput && GetInput(upKeyCodeList)) {
			fixedUpInput = true;
		}
		if (!fixedDownInput && GetInput(downKeyCodeList)) {
			fixedDownInput = true;
		}
		if (!fixedRotUpInput && GetInput(upRotKeyCodeList)) {
			fixedRotUpInput = true;
		}
		if (!fixedRotDownInput && GetInput(downRotKeyCodeList)) {
			fixedRotDownInput = true;
		}
		if (!fixedRotLeftInput && GetInput(leftRotKeyCodeList)) {
			fixedRotLeftInput = true;
		}
		if (!fixedRotRightInput && GetInput(rightRotKeyCodeList)) {
			fixedRotRightInput = true;
		}

		if (GetInput(leftCtrlKeyCodeList)) {
			fixedLeftCtrlInput = true;
			if (!prevLeftCtrlInput) {
				if (Time.realtimeSinceStartup <= (lastLeftCtrlInputTime + doubleClickTime)) {
					leftCtrlDoubleClick = true;
					lastLeftCtrlInputTime = float.MinValue;
				}
				else {
					lastLeftCtrlInputTime = Time.realtimeSinceStartup;
				}
			}
			prevLeftCtrlInput = true;
		}
		else {
			prevLeftCtrlInput = false;
		}

		if (GetInput(rightCtrlKeyCodeList)) {
			fixedRightCtrlInput = true;
			if (!prevRightCtrlInput) {
				if (Time.realtimeSinceStartup <= (lastRightCtrlInputTime + doubleClickTime)) {
					rightCtrlDoubleClick = true;
					lastRightCtrlInputTime = float.MinValue;
				}
				else {
					lastRightCtrlInputTime = Time.realtimeSinceStartup;
				}
			}
			prevRightCtrlInput = true;
		}
		else {
			prevRightCtrlInput = false;
		}

		if (!fixedTouchPadInput && GetInput(touchPadKeyCodeList)) {
			fixedTouchPadInput = true;
		}
		if (!fixedTriggerInput && GetInput(triggerKeyCodeList)) {
			fixedTriggerInput = true;
		}
		if (!fixedGripInput && GetInput(gripKeyCodeList)) {
			fixedGripInput = true;
		}

	}
	bool GetInput(List<KeyCode> _keyCodeList) {
		if (_keyCodeList == null) return false;
		foreach (var keyCode in _keyCodeList) {
			if (Input.GetKey(keyCode)) {
				return true;
			}
		}
		return false;
	}
	void FixedUpdateInput() {
		// 今回の入力状態を前回入力として保持
		prevFixedForwardInput	= fixedForwardInput;
		prevFixedBackInput		= fixedBackInput;
		prevFixedLeftInput		= fixedLeftInput;
		prevFixedRightInput		= fixedRightInput;
		prevFixedUpInput		= fixedUpInput;
		prevFixedDownInput		= fixedDownInput;
		prevFixedLeftCtrlInput	= fixedLeftCtrlInput;
		prevFixedRightCtrlInput	= fixedRightCtrlInput;
		prevFixedRotUpInput		= fixedRotUpInput;
		prevFixedRotDownInput	= fixedRotDownInput;
		prevFixedRotLeftInput	= fixedRotLeftInput;
		prevFixedRotRightInput	= fixedRotRightInput;
		prevFixedTouchPadInput	= FixedTouchPadInput;
		prevFixedTriggerInput	= FixedTriggerInput;
		prevFixedGripInput		= FixedGripInput;

		firstUpdateFlg = true;
	}

	void FixedUpdate() {
		// 移動
		Move();

		Debug.Log("Trig:" + FixedTriggerInput + " prev:" + prevFixedTriggerInput);

		// 入力状態を更新
		FixedUpdateInput();
	}

	void Move() {
		// 操作モード切替
		if (leftCtrlDoubleClick) {
			leftCtrlFlg = !leftCtrlFlg; 
		}
		if (rightCtrlDoubleClick) {
			rightCtrlFlg = !rightCtrlFlg;
		}

		// 一括操作モード
		if (!leftCtrlFlg && !rightCtrlFlg) {
			Cursor.lockState = CursorLockMode.Locked;
			MoveAll();
		}
		// コントローラー操作モード
		else {
			Cursor.lockState = CursorLockMode.None;
			MoveController();
		}
	}

	void MoveAll() {
		// コントローラーの親オブジェクトに頭部を設定
		Transform befLeftCtrlParent		= leftCtrlTrans.parent;
		Transform befRightCtrlParent	= rightCtrlTrans.parent;
		leftCtrlTrans.parent	= headTrans;
		rightCtrlTrans.parent	= headTrans;

		// 変化前の頭部と両手の移動前の位置と向きを保持
		Vector3 befLeftCtrlPos = leftCtrlTrans.position;
		Vector3 befRightCtrlPos = rightCtrlTrans.position;
		Quaternion befLeftCtrlRot = leftCtrlTrans.rotation;
		Quaternion befRightCtrlRot = rightCtrlTrans.rotation;

		// 頭部を回転
		Vector3 rotVec = new Vector3((-Input.GetAxis("Mouse Y") * camRotMouseSenceVertical),( Input.GetAxis("Mouse X") * camRotMouseSenceHorizontal), 0.0f);
		if (FixedRotUpInput) {
			rotVec = new Vector3((-1.0f * camRotKeySenceVertical), rotVec.y, rotVec.z);
		}
		if (FixedRotDownInput) {
			rotVec = new Vector3((1.0f * camRotKeySenceVertical), rotVec.y, rotVec.z);
		}
		if (FixedRotLeftInput) {
			rotVec = new Vector3(rotVec.x, (-1.0f * camRotKeySenceHorizontal), rotVec.z);
		}
		if (FixedRotRightInput) {
			rotVec = new Vector3(rotVec.x, (1.0f * camRotKeySenceHorizontal), rotVec.z);
		}
		headTrans.Rotate(new Vector3(rotVec.x, 0.0f, 0.0f), Space.Self);
		headTrans.Rotate(new Vector3(0.0f, rotVec.y, 0.0f), Space.World);

		// 頭部を移動
		Vector3 moveVec = Vector3.zero;
		if (FixedForwardInput) {
			moveVec.z += camMoveSpd.z;
		}
		if (FixedBackInput) {
			moveVec.z -= camMoveSpd.z;
		}
		if (FixedLeftInput) {
			moveVec.x -= camMoveSpd.x;
		}
		if (FixedRightInput) {
			moveVec.x += camMoveSpd.x;
		}
		if (FixedUpInput) {
			moveVec.y += camMoveSpd.y;
		}
		if (FixedDownInput) {
			moveVec.y -= camMoveSpd.y;
		}

		// 現在の頭部の向きを保持
		Quaternion befHeadRot = headTrans.rotation;

		// 頭部の上下向きを排除
		Vector3 lookPoint = (headTrans.position + headTrans.forward);
		lookPoint.y = headTrans.position.y;
		headTrans.LookAt(lookPoint);

		// 頭部を移動
		headTrans.Translate(new Vector3(moveVec.x, 0.0f, moveVec.z), Space.Self);
		headTrans.Translate(new Vector3(0.0f, moveVec.y, 0.0f), Space.World);

		// 頭部の向きを戻す
		headTrans.rotation = befHeadRot;

		// 両手のRigidbodyに力を加える
		leftCtrlRb.velocity = (leftCtrlTrans.position - befLeftCtrlPos);
		rightCtrlRb.velocity = (rightCtrlTrans.position - befRightCtrlPos);
		leftCtrlRb.angularVelocity = (leftCtrlTrans.rotation.eulerAngles - befLeftCtrlRot.eulerAngles);
		rightCtrlRb.angularVelocity = (rightCtrlTrans.rotation.eulerAngles - befRightCtrlRot.eulerAngles);

		// コントローラーの親オブジェクトを戻す
		leftCtrlTrans.parent	= befLeftCtrlParent;
		rightCtrlTrans.parent	= befRightCtrlParent;
	}

	void MoveController() {
		if (leftCtrlFlg) {
			MoveControllerTransform(leftCtrlTrans);
		}
		if (rightCtrlFlg) {
			MoveControllerTransform(rightCtrlTrans);
		}
	}
	void MoveControllerTransform(Transform _trans) {
		// コントローラーの親オブジェクトに頭部を設定
		Transform befParent = _trans.parent;
		_trans.parent = headTrans;

		// コントローラーを頭部を基準に回転移動
		Quaternion befHeadRot = headTrans.rotation;
		Vector3 rotVec = new Vector3((-Input.GetAxis("Mouse Y") * camRotMouseSenceVertical), (Input.GetAxis("Mouse X") * camRotMouseSenceHorizontal), 0.0f);
		if (FixedRotUpInput) {
			rotVec = new Vector3((-1.0f * camRotKeySenceVertical), rotVec.y, rotVec.z);
		}
		if (FixedRotDownInput) {
			rotVec = new Vector3((1.0f * camRotKeySenceVertical), rotVec.y, rotVec.z);
		}
		if (FixedRotLeftInput) {
			rotVec = new Vector3(rotVec.x, (-1.0f * camRotKeySenceHorizontal), rotVec.z);
		}
		if (FixedRotRightInput) {
			rotVec = new Vector3(rotVec.x, (1.0f * camRotKeySenceHorizontal), rotVec.z);
		}
		headTrans.Rotate(new Vector3(rotVec.x, 0.0f, 0.0f), Space.Self);
		headTrans.Rotate(new Vector3(0.0f, rotVec.y, 0.0f), Space.World);

		// コントローラーを移動
		Vector3 moveVec = Vector3.zero;
		if (FixedForwardInput) {
			moveVec.z += camMoveSpd.z;
		}
		if (FixedBackInput) {
			moveVec.z -= camMoveSpd.z;
		}
		if (FixedLeftInput) {
			moveVec.x -= camMoveSpd.x;
		}
		if (FixedRightInput) {
			moveVec.x += camMoveSpd.x;
		}
		if (FixedUpInput) {
			moveVec.y += camMoveSpd.y;
		}
		if (FixedDownInput) {
			moveVec.y -= camMoveSpd.y;
		}
		_trans.localPosition += moveVec;

		// コントローラーの親オブジェクトを戻す
		_trans.parent = befParent;

		// 頭部の向きを元に戻す
		headTrans.rotation = befHeadRot;

		// 左手
		VirtualViveController virtualViveCtrl;
		if (_trans == leftCtrlTrans) {
			virtualViveCtrl = leftCtrlVirtualViveCtrl;
		}
		// 右手
		else {
			virtualViveCtrl = rightCtrlVirtualViveCtrl;
		}

		// タッチパッド
		virtualViveCtrl.VirtualTouchKey = FixedTouchPadInput;
		// トリガー
		virtualViveCtrl.VirtualTriggerKey = FixedTriggerInput;
		// グリップ
		virtualViveCtrl.VirtualGripKey = FixedGripInput;
	}

	/*
	[Header("移動"), SerializeField, Tooltip("移動速度")]
	Vector3 moveSpd = new Vector3(3.0f, 1.5f, 3.0f);
	[SerializeField, Tooltip("前方向キー")]
	List<KeyCode> forwardKeyList = new List<KeyCode> { KeyCode.W, KeyCode.UpArrow };
	[SerializeField, Tooltip("後方向キー")]
	List<KeyCode> backKeyList = new List<KeyCode> { KeyCode.S, KeyCode.DownArrow };
	[SerializeField, Tooltip("左方向キー")]
	List<KeyCode> leftKeyList = new List<KeyCode> { KeyCode.A, KeyCode.LeftArrow };
	[SerializeField, Tooltip("右方向キー")]
	List<KeyCode> rightKeyList = new List<KeyCode> { KeyCode.D, KeyCode.RightArrow };
	[SerializeField, Tooltip("上方向キー")]
	List<KeyCode> upKeyList = new List<KeyCode> { KeyCode.Space, KeyCode.Keypad0 };
	[SerializeField, Tooltip("下方向キー")]
	List<KeyCode> downKeyList = new List<KeyCode> { KeyCode.LeftShift, KeyCode.RightShift };

	[SerializeField, Tooltip("左手操作キー")]
	KeyCode leftCtrlKey = KeyCode.Q;
	[SerializeField, Tooltip("右手操作キー")]
	KeyCode rightCtrlKey = KeyCode.E;

	[Header("回転（マウス）"), SerializeField, Tooltip("マウス操作感度")]
	Vector2 mouseSence = new Vector2(100.0f, 100.0f);
	[SerializeField, Tooltip("マウス代用キー使用フラグ")]
	bool useMouseAltKey = true;
	[SerializeField, Tooltip("マウス代用キー感度")]
	Vector2 mouseAltSence = new Vector2(0.7f, 0.45f);
	[SerializeField, Tooltip("マウス上方向代用キー")]
	KeyCode mouseAltUpKey = KeyCode.I;
	[SerializeField, Tooltip("マウス下方向代用キー")]
	KeyCode mouseAltDownKey = KeyCode.K;
	[SerializeField, Tooltip("マウス左方向代用キー")]
	KeyCode mouseAltLeftKey = KeyCode.J;
	[SerializeField, Tooltip("マウス右方向代用キー")]
	KeyCode mouseAltRightKey = KeyCode.L;
	[SerializeField, Tooltip("マウス左クリック代用キー")]
	KeyCode mouseAltLeftButtonKey = KeyCode.U;
	[SerializeField, Tooltip("マウス右クリック代用キー")]
	KeyCode mouseAltRightButtonKey = KeyCode.O;
	[SerializeField, Tooltip("マウス中(ホイール)クリック代用キー")]
	KeyCode mouseAltWheelButtonKey = KeyCode.N;
	[SerializeField, Tooltip("マウスホイール上方向代用キー")]
	KeyCode mouseAltWheelUpKey = KeyCode.Y;
	[SerializeField, Tooltip("マウスホイール下方向代用キー")]
	KeyCode mouseAltWheelDownKey = KeyCode.H;

	[Header("コントローラー操作"), SerializeField, Tooltip("カメラからのレイの有効距離")]
	float rayDis = 10.0f;
	[SerializeField, Tooltip("カメラからのレイの判定を行うレイヤー")]
	LayerMask rayMask;
	[SerializeField, Tooltip("コントローラー操作時のホイールでの移動速度")]
	float moveCtrlWheelSpd = 1.5f;
	[SerializeField, Tooltip("コントローラー操作時のキーでの移動速度")]
	Vector3 moveCtrlKeySpd = new Vector3(1.5f, 1.5f, 1.5f);
	[SerializeField, Tooltip("コントローラー操作時の回転速度")]
	Vector2 ctrlRotSpd = new Vector2(0.05f, 0.05f);

	[Header("入力"), SerializeField, Tooltip("キー入力されたベクトル(x, y)")]
	Vector3 inputVec;
	[SerializeField, Tooltip("マウス又はマウス代用キーで入力された移動量(x, y)")]
	Vector2 mouseMove;
	[SerializeField, Tooltip("コントローラー操作時のキー入力での向かせる方向")]
	Vector2 lookRot = Vector2.zero;
	[SerializeField, Tooltip("前回更新時のマウス位置")]
	Vector3 prevMousePos = Vector3.zero;
	[SerializeField, Tooltip("コントローラー操作時のコントローラー注視位置")]
	Vector3 lookPoint = Vector3.zero;

		void Update() {
			UpdateInputVec();
			UpdateMouseMove();
		}

		void FixedUpdate() {
			bool leftCtrlFlg = Input.GetKey(leftCtrlKey);
			bool rightCtrlFlg = Input.GetKey(rightCtrlKey);

			if (!leftCtrlFlg && !rightCtrlFlg) {
				// マウスカーソルを画面中央に固定
				if (Cursor.lockState == CursorLockMode.None) {
					Cursor.lockState = CursorLockMode.Locked;
				}
				// 頭部を操作し、両手を相対位置を保ったまま動かす

				// マウスカーソルを画面中央に固定
				if (Cursor.lockState == CursorLockMode.None) {
					Cursor.lockState = CursorLockMode.Locked;
				}

				// 一時的に頭部と両手を親子関係付ける
				Transform befLeftHandParent = leftCtrlTrans.parent, befRightHandParent = rightCtrlTrans.parent;
				leftCtrlTrans.parent = headTrans;
				rightCtrlTrans.parent = headTrans;

				// 頭部を移動
				Translate(headTrans);

				// 頭部を回転
				Rotate(headTrans);

				// 頭部と両手の親子関係を解除
				leftCtrlTrans.parent = befLeftHandParent;
				rightCtrlTrans.parent = befRightHandParent;
			} else {
				// マウスカーソルの中央への固定を解除
				if (Cursor.lockState == CursorLockMode.Locked) {
					Cursor.lockState = CursorLockMode.None;
					prevMousePos = Input.mousePosition;
					lookPoint = Camera.main.transform.position + (Camera.main.transform.forward * rayDis);
				}

				if (leftCtrlFlg) {
					// 頭部を動かさずに左手を操作
					MoveController(leftCtrlTrans);
				}
				if (rightCtrlFlg) {
					// 頭部を動かさずに右手を操作
					MoveController(rightCtrlTrans);
				}
			}
		}

		// キー入力による移動量を更新
		void UpdateInputVec() {
			inputVec = Vector3.zero;
			foreach (var forwardKey in forwardKeyList) {
				if (Input.GetKey(forwardKey)) {
					inputVec.z += 1.0f;
					break;
				}
			}
			foreach (var backKey in backKeyList) {
				if (Input.GetKey(backKey)) {
					inputVec.z -= 1.0f;
					break;
				}
			}
			foreach (var leftKey in leftKeyList) {
				if (Input.GetKey(leftKey)) {
					inputVec.x -= 1.0f;
					break;
				}
			}
			foreach (var rightKey in rightKeyList) {
				if (Input.GetKey(rightKey)) {
					inputVec.x += 1.0f;
					break;
				}
			}
			foreach (var upKey in upKeyList) {
				if (Input.GetKey(upKey)) {
					inputVec.y += 1.0f;
					break;
				}
			}
			foreach (var downKey in downKeyList) {
				if (Input.GetKey(downKey)) {
					inputVec.y -= 1.0f;
					break;
				}
			}
		}

		// マウス又はマウス代用キーによる移動量を返す
		void UpdateMouseMove() {
			mouseMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

			// マウス代用キー入力が有効な場合
			if (useMouseAltKey) {
				// マウス移動があれば処理しない
				if (mouseMove == Vector2.zero) {
					// 代用キー入力
					Vector2 altVec = Vector2.zero;
					if (Input.GetKey(mouseAltUpKey)) {
						altVec.y += 1.0f;
					}
					if (Input.GetKey(mouseAltDownKey)) {
						altVec.y -= 1.0f;
					}
					if (Input.GetKey(mouseAltLeftKey)) {
						altVec.x -= 1.0f;
					}
					if (Input.GetKey(mouseAltRightKey)) {
						altVec.x += 1.0f;
					}
					// マウス代用キー感度を各軸に掛ける
					mouseMove = new Vector2((altVec.x * mouseAltSence.x), (altVec.y * mouseAltSence.y));
				}
			}

			// y軸回転方向を反転
			mouseMove.y *= -1.0f;

			// マウス感度を各軸に掛ける
			mouseMove = new Vector2((mouseMove.x * mouseSence.x), (mouseMove.y * mouseSence.y));
		}

		void MoveController(Transform _ctrlTrans) {
			// マウスカーソルが移動した場合
			if (prevMousePos != Input.mousePosition) {
				// マウスカーソル方向を注視点として設定
				lookPoint = (Camera.main.transform.position + (Camera.main.ScreenToWorldPoint(Input.mousePosition)));
			}
			// キー入力が行われた場合
			else if (Input.GetKey(mouseAltUpKey) || Input.GetKey(mouseAltDownKey) || Input.GetKey(mouseAltLeftKey) || Input.GetKey(mouseAltRightKey)) {
				// 注視点を移動
				lookPoint += headTrans.localPosition;
			}

			// 注視地点に手を向ける
			_ctrlTrans.rotation = Quaternion.LookRotation(lookPoint - _ctrlTrans.position);

			// 手の移動
			_ctrlTrans.Translate(new Vector3((inputVec.x * moveCtrlKeySpd.x), (inputVec.y * moveCtrlKeySpd.y), (inputVec.z * moveCtrlKeySpd.z)) * Time.deltaTime);

			// 今回更新のマウス位置を保持
			prevMousePos = Input.mousePosition;
		}

		void Translate(Transform _trans) {
			// 上下以外の入力でy軸を変えないようにする為、ワールド座標系に変換
			Vector3 move = (_trans.rotation * new Vector3(inputVec.x, 0.0f, inputVec.z));
			// 長さを保ったままワールド座標系y軸平面の移動のみに変換
			move = new Vector3(move.x, 0.0f, move.z).normalized * move.magnitude;
			// 上下入力の移動も含めてワールド座標系で移動
			_trans.Translate(new Vector3((move.x * moveSpd.x), (inputVec.y * moveSpd.y), (move.z * moveSpd.z)) * Time.deltaTime, Space.World);
		}

		void Rotate(Transform _trans) {
			// 横方向の回転はローカル座標系
			_trans.Rotate(new Vector3(0.0f, mouseMove.x, 0.0f) * Time.deltaTime, Space.World);
			// 縦方向の回転はワールド座標系
			_trans.Rotate(new Vector3(mouseMove.y, 0.0f, 0.0f) * Time.deltaTime, Space.Self);
		}*/
}
