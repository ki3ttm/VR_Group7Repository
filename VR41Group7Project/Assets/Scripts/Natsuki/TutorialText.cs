using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour {
    // 看板のゲームオブジェクトの取得
    public GameObject SignBoard;

    // 表示する文字の変数
    private TextMeshPro tutoText;
    // 成功の文字を出す変数
    public GameObject collectText;
    // 次のチュートリアルに進むかどうかの変数
    private bool next = false;
    // α値設定
    private float alpha;
    // 成功文字が消えたかどうか
    private bool collectFlg = false;

    // 今どのチュートリアルを表示しているかの列挙型変数
    enum TUTO_TRANS{
        HOLD_SHIELD,        // 盾持つ
        SWAP_HAND,          // 持ち替え
        ONLY_ONE_HAND,      // 片手にしか持てない
        THROW_BALOON,       // 水風船を投げる
        PREVENT_BALOON,     // 水風船を防ぐ
        //DURABILITY,         // 耐久度
        //THROW_IN,           // 盾投げこみ
    //    SWAT_ITEM,          // アイテムの持ち替え（アイテムを捨てる？）
        HOLD_BAT,           // バット持つ
        HIT_BACK,           // 打ち返す
        WAHT_DIE,           // 何で死ぬか
        END                 // チュートリアル終了
    }

    // チュートリアルの順番
    private TUTO_TRANS tutoTrans = TUTO_TRANS.HOLD_SHIELD;

    // 右のコントローラーに持ってるもの
    private GrabController rightGrabController;
    // 左のコントローラーに持ってるもの
    private GrabController leftGrabController;

    // 右のコントローラー
    private VirtualViveController rightController;
    // 左のコントローラー
    private VirtualViveController leftController;

    // 左手に何か持ってるか
    private bool leftHandObj;


    // 敵挙動スクリプト
    private EnemySpawner enemy;

    Collider coll;

    // Use this for initialization
    void Start () {
        SignBoard.SetActive(true);
        tutoText = SignBoard.transform.GetComponentInChildren<TextMeshPro>();
        rightGrabController = GameObject.Find("RightController").GetComponent<GrabController>();
        leftGrabController = GameObject.Find("LeftContorller").GetComponent<GrabController>();
        rightController = GameObject.Find("RightController").GetComponent<VirtualViveController>();
        leftController = GameObject.Find("LeftContorller").GetComponent<VirtualViveController>();
        enemy = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        alpha = 1.0f;
    }
	
	// Update is called once per frame
	void Update () {

        TutoTrans();
        ChangeText();

        if (Input.GetKeyDown(KeyCode.V))
        {
            enemy.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            enemy.enabled = false;
        }

        if(leftGrabController.GrabObj != null)
        {
            Debug.Log(leftGrabController.GrabObj.GetComponent<GetCollisionObject>().hitObj.name);
        }
    }

    
    void ChangeText()
    {

        if (next && tutoTrans < TUTO_TRANS.END)
        {
            tutoTrans += 1;
            next = false;
        }

        Collect();

        switch (tutoTrans)
        {
            case TUTO_TRANS.HOLD_SHIELD:
                tutoText.text = "盾を持ってみよう";
                break;

            case TUTO_TRANS.SWAP_HAND:
                tutoText.text = "持ち手を変えてみよう";
                break;

            case TUTO_TRANS.ONLY_ONE_HAND:
                tutoText.text = "アイテムは\n片手にしか持てない\nトリガーで次へ";
                break;

            case TUTO_TRANS.THROW_BALOON:
                tutoText.text = "相手は水風船を\n投げてくるよ\nトリガーで次へ";
                break;

            case TUTO_TRANS.PREVENT_BALOON:
                tutoText.text = "投げてくる水風船を\n弾き返そう";
                break;

            /*case TUTO_TRANS.DURABILITY:
                tutoText.text = "盾には耐久度があるよ\nトリガーで次へ";
                break;

            case TUTO_TRANS.THROW_IN:
                tutoText.text = "盾が壊れると\nたまに盾が投げ込まれるよ\nトリガーで次へ";
                break;

            case TUTO_TRANS.SWAT_ITEM:
                tutoText.text = "盾からバットへ\n持ち替えよう";
                break;*/

            case TUTO_TRANS.HOLD_BAT:
                tutoText.text = "バットを持ってみよう";
                break;

            case TUTO_TRANS.HIT_BACK:
                tutoText.text = "打ち返してみよう";
                break;

            case TUTO_TRANS.WAHT_DIE:
                tutoText.text = "３回水風船に当たると\nゲームオーバーです\nトリガーで次へ";
                break;

            case TUTO_TRANS.END:
                tutoText.text = "チュートリアルは終わり\nゲームを楽しんでね\nトリガーでゲームスタート";
                break;

            default:
                break;
        }
    }


    void TutoTrans()
    {
        // デバッグ用エンターキーで次の説明へ
        if (Input.GetKeyDown(KeyCode.Return))
        {
            next = true;
        }
        // デバッグ用エンターキーで次の説明へ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tutoTrans -= 1;
        }

        // チュートリアルをこなしたかの処理
        switch (tutoTrans)
        {
            case TUTO_TRANS.HOLD_SHIELD:    // 盾を持ったか否か
                if ((rightGrabController.GrabObj != null && rightGrabController.GrabObj.name == "Shield") ||
                    (leftGrabController.GrabObj != null && leftGrabController.GrabObj.name == "Shield"))
                {
                    if(leftGrabController.GrabObj != null)
                    {
                        leftHandObj = true;
                    }
                    else
                    {
                        leftHandObj = false;
                    }
                    // 次のチュートリアルへ
                    next = true;
                    collectFlg = true;
                    alpha = 1.0f;
                }
                break;

            case TUTO_TRANS.SWAP_HAND:      // 持ち手を変えたかどうか
                if (leftHandObj) {
                    if(leftGrabController.GrabObj == null && rightGrabController.GrabObj != null && rightGrabController.GrabObj.name == "Shield")
                    {
                        // 次のチュートリアルへ
                        next = true;
                        collectFlg = true;
                        alpha = 1.0f;
                    }
                }
                else
                {
                    if (rightGrabController.GrabObj == null && leftGrabController.GrabObj != null && leftGrabController.GrabObj.name == "Shield")
                    {
                        // 次のチュートリアルへ
                        next = true;
                        collectFlg = true;
                        alpha = 1.0f;
                    }
                }
                break;

            case TUTO_TRANS.ONLY_ONE_HAND:  // 片手しか持てないことを文章で説明
                if(!collectText.activeSelf)
                {
                    if (rightController.GetPress(SteamVR_Controller.ButtonMask.Trigger) || leftController.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                    {
                        // 次のチュートリアルへ
                        next = true;
                        collectFlg = true;
                        alpha = 1.0f;
                        collectText.GetComponent<TextMeshProUGUI>().text = "Next";
                    }
                }
                break;

            case TUTO_TRANS.THROW_BALOON:  // 敵が水風船を投げてくることを説明
                if(!enemy.enabled)
                    enemy.enabled = true;
                if (!collectText.activeSelf)
                {
                    if (rightController.GetPress(SteamVR_Controller.ButtonMask.Trigger) || leftController.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
                        // 次のチュートリアルへ
                        next = true;
                        collectFlg = true;
                        alpha = 1.0f;
                    }
                }
                
                break;

            case TUTO_TRANS.PREVENT_BALOON:
                if (leftGrabController.GrabObj != null &&
                    leftGrabController.GrabObj.GetComponent<GetCollisionObject>().hitObj.name == "WaterBalloon" ||
                    (rightGrabController.GrabObj != null &&
                    rightGrabController.GrabObj.GetComponent<GetCollisionObject>().hitObj.name == "WaterBalloon"))
                {
                    // 次のチュートリアルへ
                    next = true;
                    collectFlg = true;
                    alpha = 1.0f;
                    collectText.GetComponent<TextMeshProUGUI>().text = "Collect";
                }
                break;

           /* case TUTO_TRANS.DURABILITY:
                if (!collectText.activeSelf)
                {
                    if (rightController.GetPress(SteamVR_Controller.ButtonMask.Trigger) || leftController.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
                        // 次のチュートリアルへ
                        next = true;
                        collectFlg = true;
                        alpha = 1.0f;
                        collectText.GetComponent<TextMeshProUGUI>().text = "Next";
                    }
                }
                break;

            case TUTO_TRANS.THROW_IN:
                if (!collectText.activeSelf)
                {
                    if (rightController.GetPress(SteamVR_Controller.ButtonMask.Trigger) || leftController.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                    {
                        // 次のチュートリアルへ
                        next = true;
                        collectFlg = true;
                        alpha = 1.0f;
                    }
                }
                break;*/

            case TUTO_TRANS.HOLD_BAT:
                if ((rightGrabController.GrabObj != null && rightGrabController.GrabObj.name == "But") ||
                    (leftGrabController.GrabObj != null && leftGrabController.GrabObj.name == "But"))
                {
                    if(leftGrabController.GrabObj != null)
                    {
                        leftHandObj = true;
                    }
                    else
                    {
                        leftHandObj = false;
                    }
                    // 次のチュートリアルへ
                    next = true;
                    collectFlg = true;
                    alpha = 1.0f;
                    collectText.GetComponent<TextMeshProUGUI>().text = "Collect";
                }
                break;

            case TUTO_TRANS.HIT_BACK:
                if ((leftGrabController.GrabObj != null && 
                    leftGrabController.GrabObj.GetComponent<GetCollisionObject>().hitObj.name == "WaterBalloon") ||
                    (rightGrabController.GrabObj != null &&
                    rightGrabController.GrabObj.GetComponent<GetCollisionObject>().hitObj.name == "WaterBalloon"))
                {
                    // 次のチュートリアルへ
                    next = true;
                    collectFlg = true;
                    alpha = 1.0f;
                }
                break;

            case TUTO_TRANS.WAHT_DIE:
                if(!collectText.activeSelf)
                {
                    if (rightController.GetPress(SteamVR_Controller.ButtonMask.Trigger) || leftController.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                    {
                        // 次のチュートリアルへ
                        next = true;
                        collectFlg = true;
                        alpha = 1.0f;
                        collectText.GetComponent<TextMeshProUGUI>().text = "Next";
                    }
                }
                break;

            case TUTO_TRANS.END:
                if (!collectText.activeSelf)
                {
                    if (rightController.GetPress(SteamVR_Controller.ButtonMask.Trigger) || leftController.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                    {
                        // Tutorialを終了して次の処理へ
                    }
                }
                break;

            default:
                break;
        }
    }

    void Collect()
    {
        if (collectFlg)
        {
            collectText.SetActive(true);

            collectText.GetComponent<TextMeshProUGUI>().color = new Color(collectText.GetComponent<TextMeshProUGUI>().color.r, collectText.GetComponent<TextMeshProUGUI>().color.g, collectText.GetComponent<TextMeshProUGUI>().color.b, alpha);
            alpha -= 0.01f;

            if (alpha <= 0)
            {
                collectText.SetActive(false);
                collectFlg = false;
            }
        }
    }
}
