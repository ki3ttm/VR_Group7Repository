using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimlateManager : MonoBehaviour {
	static SimlateManager instance = null;
	static public SimlateManager Instance {
		get {
			if (!instance) {
				instance = FindObjectOfType<SimlateManager>();
			}
			return instance;
		}
	}
	static public bool Enable {
		get {
			return (Instance.thisSt == State.Enable);
		}
	}
	static public State St {
		get {
			return Instance.thisSt;
		}
	}

	public enum State {
		Enable,     // 有効
		Disable,    // 無効
		Auto,       // SteamVRの状態によって切り替える
	}

	[Header("現在の状態"), SerializeField, Tooltip("使用状態")]
	State thisSt = State.Disable;
	State ThisSt {
		get {
			if (thisSt == State.Auto) {
				if (SteamVR.active) {
					ThisSt = State.Disable;
				} else {
					ThisSt = State.Enable;
				}
			}
			return thisSt;
		}
		set {
			if (thisSt == value) return;
			thisSt = value;

			// SimlateOnlyObjectのactiveを設定
			if (this == Instance) {
				SimlateOnlyObject.SetSimOnlyObjListActive(Enable);
			}
		}
	}
	State prevSt = State.Disable;

	[SerializeField, Tooltip("左手操作フラグ")]
	bool leftCtrlFlg = false;
	public bool LeftCtrlFlg {
		get {
			return leftCtrlFlg;
		}
	}
	[SerializeField, Tooltip("右手操作フラグ")]
	bool rightCtrlFlg = false;
	public bool RightCtrlFlg {
		get {
			return rightCtrlFlg;
		}
	}
	[SerializeField, Tooltip("使用しているカメラのインデックス"), ReadOnly]
	int useCamIdx = -1;
	int UseCamIdx {
		get {
			return useCamIdx;
		}
		set {
			useCamIdx = value;
		}
	}
	[SerializeField, Tooltip("カメラリスト")]
	List<Camera> otherCamList = null;
	Camera defCam = null;
	int prevCamIdx = -1;

	[Header("関係オブジェクトの参照"), SerializeField, Tooltip("頭部(SteamVR)のTransform")]
	Transform headTrans = null;
	[SerializeField, Tooltip("左手(ViveDevices)のTransform")]
	Transform leftCtrlTrans = null;
	[SerializeField, Tooltip("右手(ViveDevices)のTransform")]
	Transform rightCtrlTrans = null;
	[SerializeField, Tooltip("左腕(ViveDevices)のTransform")]
	Transform leftArmTrans = null;
	[SerializeField, Tooltip("右腕(ViveDevices)のTransform")]
	Transform rightArmTrans = null;
	[SerializeField, Tooltip("頭部のRigidbody"), ReadOnly]
	Rigidbody headRb = null;
	[SerializeField, Tooltip("左手のRigidbody"), ReadOnly]
	Rigidbody leftCtrlRb = null;
	[SerializeField, Tooltip("右手のRigidbody"), ReadOnly]
	Rigidbody rightCtrlRb = null;
	[SerializeField, Tooltip("左手のVirtualViveController"), ReadOnly]
	VirtualViveController leftCtrlVirtualViveCtrl = null;
	[SerializeField, Tooltip("右手のVirtualViveController"), ReadOnly]
	VirtualViveController rightCtrlVirtualViveCtrl = null;

	[Header("速度等"), SerializeField, Tooltip("カメラ移動速度")]
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
	[SerializeField, Tooltip("コントローラー回転操作速度")]
	float ctrlRotSpd = 5.0f;
	[SerializeField, Tooltip("コントローラー距離操作速度")]
	float ctrlDisMoveSpd = 0.03f;
	[SerializeField, Tooltip("コントローラー距離操作マウス感度")]
	float ctrlDisMoveMouseSence = 0.5f;
	[SerializeField, Tooltip("コントローラーを向ける正面方向への距離")]
	float midClickCursorLookDis = 5.0f;
	[SerializeField, Tooltip("コントローラーと腕部の最小距離")]
	float ctrlMinDis = 0.1f;

	#region KeyCodeList(キーコード)
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
	List<KeyCode> touchPadKeyCodeList = new List<KeyCode>() { KeyCode.Y, KeyCode.Mouse0 };
	[SerializeField, Tooltip("トリガー")]
	List<KeyCode> triggerKeyCodeList = new List<KeyCode>() { KeyCode.H, KeyCode.Mouse1 };
	[SerializeField, Tooltip("グリップ")]
	List<KeyCode> gripKeyCodeList = new List<KeyCode>() { KeyCode.N };
	[SerializeField, Tooltip("中クリック")]
	List<KeyCode> midClickKeyCodeList = new List<KeyCode>() { KeyCode.M, KeyCode.Mouse2 };
	[SerializeField, Tooltip("ホイール(上)")]
	List<KeyCode> wheelUpKeyCodeList = new List<KeyCode>() { KeyCode.P };
	[SerializeField, Tooltip("ホイール(下)")]
	List<KeyCode> wheelDownKeyCodeList = new List<KeyCode>() { KeyCode.Semicolon };
	[SerializeField, Tooltip("カメラ切り替え")]
	List<KeyCode> backCamKeyCodeList = new List<KeyCode>() { KeyCode.Tab };
	#endregion

	#region fixedInput(入力状態)
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
	[SerializeField, Tooltip("中クリック(ホイール押下)")]
	bool fixedMidClickInput;
	public bool FixedMidClickInput {
		get {
			return fixedMidClickInput;
		}
		set {
			fixedMidClickInput = value;
		}
	}
	[SerializeField, Tooltip("ホイール操作(上)")]
	bool fixedWheelUpInput;
	public bool FixedWheelUpInput {
		get {
			return fixedWheelUpInput;
		}
		set {
			fixedWheelUpInput = value;
		}
	}
	[SerializeField, Tooltip("ホイール操作(下)")]
	bool fixedWheelDownInput;
	public bool FixedWheelDownInput {
		get {
			return fixedWheelDownInput;
		}
		set {
			fixedWheelDownInput = value;
		}
	}
	[SerializeField, Tooltip("カメラ切り替え")]
	bool fixedSwitchCamInput;
	public bool FixedSwitchCamInput {
		get {
			return fixedSwitchCamInput;
		}
		set {
			fixedSwitchCamInput = value;
		}
	}
	#endregion

	#region prevFixedInput(前回入力状態)
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
	[SerializeField, Tooltip("中クリック")]
	bool prevFixedMidClickInput = false;
	[SerializeField, Tooltip("ホイール(上)")]
	bool prevFixedWheelUpInput = false;
	[SerializeField, Tooltip("ホイール(下)")]
	bool prevFixedWheelDownInput = false;
	[SerializeField, Tooltip("カメラ切り替え")]
	bool prevFixedSwitchCamInput = false;
	#endregion

	// FixedUpdate()の後でUpdate()を通るまではtrue、Update()でtrueなら入力を削除してfalseになる
	bool firstUpdateFlg = false;

	[Header("ダブルクリック"), SerializeField, Tooltip("ダブルクリックの受付時間")]
	float doubleClickTime = 0.3f;
	[SerializeField, Tooltip("左コントローラー操作のダブルクリック判定")]
	bool leftCtrlDoubleClick = false;
	[SerializeField, Tooltip("右コントローラー操作のダブルクリック判定")]
	bool rightCtrlDoubleClick = false;
	[SerializeField, Tooltip("中(ホイール)ダブルクリック判定")]
	bool midDoubleClick = false;
	[SerializeField, Tooltip("左コントローラー操作の最後に入力があった時間")]
	float lastLeftCtrlInputTime = float.MinValue;
	[SerializeField, Tooltip("右コントローラー操作の最後に入力があった時間")]
	float lastRightCtrlInputTime = float.MinValue;
	[SerializeField, Tooltip("中クリックの最後に入力があった時間")]
	float lastMidInputTime = float.MinValue;
	[SerializeField, Tooltip("左コントローラー操作の入力前回取得時の状態")]
	bool prevLeftCtrlInput = false;
	[SerializeField, Tooltip("右コントローラー操作の入力前回取得時の状態")]
	bool prevRightCtrlInput = false;
	[SerializeField, Tooltip("中クリックの入力前回取得時の状態")]
	bool prevMidInput = false;

	[Header("マウスカーソル"), SerializeField, Tooltip("マウスカーソル位置"), ReadOnly]
	Vector2 mouseScreenPos = Vector2.zero;
	[SerializeField, Tooltip("マウスカーソル仮想キー操作速度")]
	float mouseScreenMoveSpd = 20.0f;
	Vector3 prevMousePos = Vector3.zero;

	float defArmLen = 0.0f;

	VirtualViveController virtualLeftCtrl = null;
	public VirtualViveController VirtualLeftCtrl {
		get {
			if (!virtualLeftCtrl) {
				virtualLeftCtrl = leftCtrlTrans.GetComponent<VirtualViveController>();
			}
			return virtualLeftCtrl;
		}
	}
	VirtualViveController virtualRightCtrl = null;
	public VirtualViveController VirtualRightCtrl {
		get {
			if (!virtualRightCtrl) {
				virtualRightCtrl = rightCtrlTrans.GetComponent<VirtualViveController>();
			}
			return virtualRightCtrl;
		}
	}

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

	void Awake() {
		instance = this;
		prevSt = St;
	}

	void Start() {
		headRb = headTrans.GetComponent<Rigidbody>();
		leftCtrlRb = leftCtrlTrans.GetComponent<Rigidbody>();
		rightCtrlRb = rightCtrlTrans.GetComponent<Rigidbody>();
		leftCtrlVirtualViveCtrl = leftCtrlTrans.GetComponent<VirtualViveController>();
		rightCtrlVirtualViveCtrl = rightCtrlTrans.GetComponent<VirtualViveController>();

		defArmLen = Vector3.Distance(leftArmTrans.position, leftCtrlTrans.position);
		InitControllerTransform();
	}

	void Update() {
		// InspectorからStが変更された場合
		if (prevSt != ThisSt) {
			prevSt = ThisSt;
			// SimlateOnlyObjectのactiveを設定
			if (this == Instance) {
				SimlateOnlyObject.SetSimOnlyObjListActive(Enable);
			}
		}

		if (!SimlateManager.Enable) return;

		// 入力があれば保持
		UpdateInput();
	}
	void UpdateInput() {
		// 前回のFixedUpdate()から初回のUpdate()なら
		if (firstUpdateFlg) {
			firstUpdateFlg = false;

			// 前回の入力状態を削除
			FixedForwardInput		= false;
			FixedBackInput			= false;
			FixedLeftInput			= false;
			FixedRightInput			= false;
			FixedUpInput			= false;
			FixedDownInput			= false;
			FixedLeftCtrlInput		= false;
			FixedRightCtrlInput		= false;
			FixedRotUpInput			= false;
			FixedRotDownInput		= false;
			FixedRotLeftInput		= false;
			FixedRotRightInput		= false;
			FixedTouchPadInput		= false;
			FixedTriggerInput		= false;
			FixedGripInput			= false;
			FixedMidClickInput		= false;
			FixedWheelUpInput		= false;
			FixedWheelDownInput		= false;

			leftCtrlDoubleClick		= false;
			rightCtrlDoubleClick	= false;
			midDoubleClick			= false;
			FixedSwitchCamInput		= false;
		}

		if (!FixedForwardInput && GetInput(forwardKeyCodeList)) {
			FixedForwardInput = true;
		}
		if (!FixedBackInput && GetInput(backKeyCodeList)) {
			FixedBackInput = true;
		}
		if (!FixedLeftInput && GetInput(leftKeyCodeList)) {
			FixedLeftInput = true;
		}
		if (!FixedRightInput && GetInput(rightKeyCodeList)) {
			FixedRightInput = true;
		}
		if (!FixedUpInput && GetInput(upKeyCodeList)) {
			FixedUpInput = true;
		}
		if (!FixedDownInput && GetInput(downKeyCodeList)) {
			FixedDownInput = true;
		}
		if (!FixedRotUpInput && GetInput(upRotKeyCodeList)) {
			FixedRotUpInput = true;
		}
		if (!FixedRotDownInput && GetInput(downRotKeyCodeList)) {
			FixedRotDownInput = true;
		}
		if (!FixedRotLeftInput && GetInput(leftRotKeyCodeList)) {
			FixedRotLeftInput = true;
		}
		if (!FixedRotRightInput && GetInput(rightRotKeyCodeList)) {
			FixedRotRightInput = true;
		}

		// 左クリック
		if (GetInput(leftCtrlKeyCodeList)) {
			FixedLeftCtrlInput = true;
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

		// 右クリック
		if (GetInput(rightCtrlKeyCodeList)) {
			FixedRightCtrlInput = true;
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

		if (!FixedTouchPadInput && GetInput(touchPadKeyCodeList)) {
			FixedTouchPadInput = true;
		}
		if (!FixedTriggerInput && GetInput(triggerKeyCodeList)) {
			FixedTriggerInput = true;
		}
		if (!FixedGripInput && GetInput(gripKeyCodeList)) {
			FixedGripInput = true;
		}

		// 中クリック
		if (GetInput(midClickKeyCodeList)) {
			FixedMidClickInput = true;
			if (!prevMidInput) {
				if (Time.realtimeSinceStartup <= (lastMidInputTime + doubleClickTime)) {
					midDoubleClick = true;
					lastMidInputTime = float.MinValue;
				}
				else {
					lastMidInputTime = Time.realtimeSinceStartup;
				}
			}
			prevMidInput = true;
		}
		else {
			prevMidInput = false;
		}

		if (!FixedWheelUpInput && GetInput(wheelUpKeyCodeList)) {
			FixedWheelUpInput = true;
		}
		if (!FixedWheelDownInput && GetInput(wheelDownKeyCodeList)) {
			FixedWheelDownInput = true;
		}
		if (!FixedSwitchCamInput && GetInput(backCamKeyCodeList)) {
			FixedSwitchCamInput = true;
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
		prevFixedForwardInput	= FixedForwardInput;
		prevFixedBackInput		= FixedBackInput;
		prevFixedLeftInput		= FixedLeftInput;
		prevFixedRightInput		= FixedRightInput;
		prevFixedUpInput		= FixedUpInput;
		prevFixedDownInput		= FixedDownInput;
		prevFixedLeftCtrlInput	= FixedLeftCtrlInput;
		prevFixedRightCtrlInput	= FixedRightCtrlInput;
		prevFixedRotUpInput		= FixedRotUpInput;
		prevFixedRotDownInput	= FixedRotDownInput;
		prevFixedRotLeftInput	= FixedRotLeftInput;
		prevFixedRotRightInput	= FixedRotRightInput;
		prevFixedTouchPadInput	= FixedTouchPadInput;
		prevFixedTriggerInput	= FixedTriggerInput;
		prevFixedGripInput		= FixedGripInput;
		prevFixedMidClickInput	= FixedMidClickInput;
		prevFixedWheelUpInput	= FixedWheelUpInput;
		prevFixedWheelDownInput	= FixedWheelDownInput;
		prevFixedSwitchCamInput	= FixedSwitchCamInput;

		firstUpdateFlg = true;
	}

	void FixedUpdate() {
		if (!SimlateManager.Enable) return;

		// 中ダブルクリックでコントローラーの位置と向きをリセット
		if (midDoubleClick) {
			InitControllerTransform();
		}

		// 移動
		Move();

		// 入力状態を更新
		FixedUpdateInput();

		prevCamIdx = UseCamIdx;
	}

	void Move() {
		// 操作モード切替
		if (leftCtrlDoubleClick || rightCtrlDoubleClick) {
			if (leftCtrlDoubleClick) {
				leftCtrlFlg = !leftCtrlFlg;
			}
			if (rightCtrlDoubleClick) {
				rightCtrlFlg = !rightCtrlFlg;
			}
		}

		// カメラ変更
		if (FixedSwitchCamInput && !prevFixedSwitchCamInput) {
			ChangeCamera(UseCamIdx + 1);
		}
		// Inspectorから使用カメラのインデックスが変更された場合
		 else if (prevCamIdx != UseCamIdx) {
			ChangeCamera(UseCamIdx);
		}

		// カーソルを中央に固定し直す
		if ((Cursor.lockState != CursorLockMode.Locked) && !GetInput(midClickKeyCodeList)) {
			Cursor.lockState = CursorLockMode.Locked;
		}

		// 一括操作モード
		if(Input.GetKeyDown(KeyCode.Escape)) {
			leftCtrlFlg = rightCtrlFlg = false;
		}
		if (!leftCtrlFlg && !rightCtrlFlg) {
			MoveHeadAndHands(true);
		}
		// コントローラー操作モード
		else {
			MoveController();
			MoveHeadAndHands(false);
		}
	}

	void MoveHeadAndHands(bool _canRightCtrl) {
		// コントローラーの親オブジェクトに頭部を設定
		Transform befLeftCtrlParent		= leftCtrlTrans.parent;
		Transform befRightCtrlParent	= rightCtrlTrans.parent;
		leftCtrlTrans.parent	= headTrans;
		rightCtrlTrans.parent	= headTrans;

		// 移動前の頭部と両手の位置と向きを保持
		Vector3 befHeadPos			= headTrans.position;
		Vector3 befLeftCtrlPos		= leftCtrlTrans.position;
		Vector3 befRightCtrlPos		= rightCtrlTrans.position;
		Quaternion befHeadRot		= headTrans.rotation;
		Quaternion befLeftCtrlRot	= leftCtrlTrans.rotation;
		Quaternion befRightCtrlRot	= rightCtrlTrans.rotation;

		if (_canRightCtrl) {
			// 頭部を回転
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
		}

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

		// 現状の頭部の向きを保持
		Quaternion midHeadRot = headTrans.rotation;

		// 頭部の上下向きを排除
		Vector3 lookPoint = (headTrans.position + headTrans.forward);
		lookPoint.y = headTrans.position.y;
		headTrans.LookAt(lookPoint);

		// 頭部を移動
		headTrans.Translate(new Vector3(moveVec.x, 0.0f, moveVec.z), Space.Self);
		headTrans.Translate(new Vector3(0.0f, moveVec.y, 0.0f), Space.World);

		Vector3 headAftPos = headTrans.position;
		Quaternion headAftRot = headTrans.rotation;
		
		// 頭部の向きを戻す
		headTrans.rotation = midHeadRot;

		// コントローラーの親オブジェクトを戻す
		leftCtrlTrans.parent = befLeftCtrlParent;
		rightCtrlTrans.parent = befRightCtrlParent;

		// 移動後の頭部と両手の位置と向きを保持
		Vector3 aftHeadPos			= headTrans.position;
		Quaternion aftHeadRot		= headTrans.rotation;
		Vector3 aftLeftCtrlPos		= leftCtrlTrans.position;
		Quaternion aftLeftCtrlRot	= leftCtrlTrans.rotation;
		Vector3 aftRightCtrlPos		= rightCtrlTrans.position;
		Quaternion aftRightCtrlRot	= rightCtrlTrans.rotation;
		
		// 頭部と両手の位置を一度戻す
		headTrans.position		= befHeadPos;
		headTrans.rotation		= befHeadRot;
		leftCtrlTrans.position	= befLeftCtrlPos;
		leftCtrlTrans.rotation	= befLeftCtrlRot;
		rightCtrlTrans.position	= befRightCtrlPos;
		rightCtrlTrans.rotation	= befRightCtrlRot;

		// Rigidbodyを使って位置と向きを設定
		Vector3 headMovePos = (aftHeadPos - befHeadPos);
		Quaternion headMoveRot = (aftHeadRot * Quaternion.Inverse(befHeadRot));
		Vector3 leftCtrlMovePos = (aftLeftCtrlPos - befLeftCtrlPos);
		Quaternion leftCtrlMoveRot = (aftLeftCtrlRot * Quaternion.Inverse(befLeftCtrlRot));
		Vector3 rightCtrlMovePos = (aftRightCtrlPos - befRightCtrlPos);
		Quaternion rightCtrlMoveRot = (aftRightCtrlRot * Quaternion.Inverse(befRightCtrlRot));

		headRb.MovePosition(aftHeadPos);
		headRb.MoveRotation(aftHeadRot);
		leftCtrlRb.MovePosition(aftLeftCtrlPos);
		leftCtrlRb.MoveRotation(aftLeftCtrlRot);
		rightCtrlRb.MovePosition(aftRightCtrlPos);
		rightCtrlRb.MoveRotation(aftRightCtrlRot);
	}

	void MoveController() {
		if (leftCtrlFlg) {
			MoveControllerTransform(leftCtrlTrans, leftArmTrans);
		}
		if (rightCtrlFlg) {
			MoveControllerTransform(rightCtrlTrans, rightArmTrans);
		}
	}
	void MoveControllerTransform(Transform _trans, Transform _armTrans) {
		// コントローラーの親オブジェクトに腕部を設定
		Transform befParent = _trans.parent;
		_trans.parent = _armTrans;

		// コントローラーの腕部からの距離を変更
		float ctrlMoveVec = (Input.GetAxis("Mouse ScrollWheel") * ctrlDisMoveMouseSence);
		if (FixedWheelDownInput) {
			ctrlMoveVec -= ctrlDisMoveSpd;
		}
		if (FixedWheelUpInput) {
			ctrlMoveVec += ctrlDisMoveSpd;
		}
		ctrlMoveVec = Mathf.Clamp(ctrlMoveVec, -1.0f, 1.0f);
		if (ctrlMoveVec != 0.0f) {
			float dis = Vector3.Distance(_trans.position, _armTrans.position);
			Vector3 vec = (_trans.position - _armTrans.position).normalized;
			if ((dis + ctrlMoveVec) > ctrlMinDis) {
				dis += (ctrlMoveVec);
			} else {
				dis = ctrlMinDis;
			}
			_trans.position = (_armTrans.position + (dis * vec));
		}

		// 中クリックでカーソル方向に腕を向ける
		if (GetInput(midClickKeyCodeList)) {
			LookMouseCursor(_armTrans);
		} else {
			// マウスの動きのベクトルを取得
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

			// コントローラーを腕部を基準に回転移動
			_armTrans.Rotate((new Vector3(rotVec.x, 0.0f, 0.0f) * ctrlRotSpd), Space.Self);
			_armTrans.Rotate((new Vector3(0.0f, rotVec.y, 0.0f) * ctrlRotSpd), Space.World);
		}
		
		// コントローラーの親オブジェクトを戻す
		_trans.parent = befParent;

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

	void InitControllerTransform() {
		leftCtrlTrans.position = leftArmTrans.position + (leftArmTrans.forward * defArmLen);
		leftCtrlTrans.rotation = leftArmTrans.rotation;
		rightCtrlTrans.position = rightArmTrans.position + (rightArmTrans.forward * defArmLen);
		rightCtrlTrans.rotation = rightArmTrans.rotation;
	}

	void ChangeCamera(int _idx) {
		if ((_idx < -1) || (_idx >= otherCamList.Count)) {
			_idx = -1;
		}

		Camera befCam = Camera.current;
		if (_idx == -1) {
			if (befCam != defCam) {
				befCam.gameObject.SetActive(false);
			}
			Camera.SetupCurrent(defCam);
		}
		else {
			otherCamList[_idx].gameObject.SetActive(true);
			Camera.SetupCurrent(otherCamList[_idx]);
		}

		if (UseCamIdx >= 0) {
			befCam.gameObject.SetActive(false);
		}
		UseCamIdx = _idx;
	}

	void LookMouseCursor(Transform _trans) {
		// マウスの動きのベクトルを取得
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

		// カーソルの中央固定を解除
		if (Cursor.lockState == CursorLockMode.Locked) {
			Cursor.lockState = CursorLockMode.None;
			mouseScreenPos = Input.mousePosition;
			prevMousePos = mouseScreenPos;
		}

		// マウスが前回更新から動いた場合
		if (prevMousePos != Input.mousePosition) {
			mouseScreenPos = Input.mousePosition;
			prevMousePos = mouseScreenPos;
		}

		// キーによるマウスカーソル位置の移動
		mouseScreenPos += new Vector2(rotVec.y, -rotVec.x) * mouseScreenMoveSpd;

		// カーソルの先にコントローラーを向ける
		Vector3 lookPoint = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, midClickCursorLookDis));
		_trans.rotation = Quaternion.LookRotation(lookPoint - _trans.position);
	}
}
