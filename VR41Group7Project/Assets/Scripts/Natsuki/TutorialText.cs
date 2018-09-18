using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutrorialText : MonoBehaviour {
    // 看板のゲームオブジェクトの取得
    public GameObject SignBoard;
    // 表示する文字の変数
    private TextMeshPro tutoText;
    // 次のチュートリアルに進むかどうかの変数
    private bool next = false;

    // 今どのチュートリアルを表示しているかの列挙型変数
    enum TUTO_TRANS{
        HOLD_SHIELD,        // 盾持つ
        SWAP_HAND,          // 持ち替え
        ONLY_ONE_HAND,      // 片手にしか持てない
        THROW_BALOON,       // 水風船を投げる
        PREVENT_BALOON,     // 水風船を防ぐ
        DURABILITY,         // 耐久度
        THROW_IN,           // 盾投げこみ
    //    SWAT_ITEM,          // アイテムの持ち替え（アイテムを捨てる？）
        HOLD_BAT,           // バット持つ
        HIT_BACK,           // 打ち返す
        WAHT_DIE,           // 何で死ぬか
        END                 // チュートリアル終了
    }

    // チュートリアルの順番
    private TUTO_TRANS tutoTrans = TUTO_TRANS.HOLD_SHIELD;
    // 右側コントローラー
   // public ViveControllerGrabObject rightController;
    // 左側コントローラー
   // public ViveControllerGrabObject leftController;

    // Use this for initialization
    void Start () {
        SignBoard.SetActive(true);
        tutoText = SignBoard.transform.GetComponentInChildren<TextMeshPro>();
	}
	
	// Update is called once per frame
	void Update () {
        TutoTrans();
        ChangeText();
	}

    
    void ChangeText()
    {
        if (next && tutoTrans < TUTO_TRANS.WAHT_DIE)
        {
            tutoTrans += 1;
            next = false;
        }

        switch (tutoTrans)
        {
            case TUTO_TRANS.HOLD_SHIELD:
                tutoText.text = "盾を持ってみよう";
                break;

            case TUTO_TRANS.SWAP_HAND:
                tutoText.text = "持ち手は変える事が\nできる";
                break;

            case TUTO_TRANS.ONLY_ONE_HAND:
                tutoText.text = "アイテムは\n片手にしか持てない";
                break;

            case TUTO_TRANS.THROW_BALOON:
                tutoText.text = "相手は水風船を\n投げてくるよ";
                break;

            case TUTO_TRANS.PREVENT_BALOON:
                tutoText.text = "投げてくる水風船を\n弾き返そう";
                break;

            case TUTO_TRANS.DURABILITY:
                tutoText.text = "盾には耐久度があるよ";
                break;

            case TUTO_TRANS.THROW_IN:
                tutoText.text = "盾が壊れると\nたまに盾が投げ込まれるよ";
                break;

            /*case TUTO_TRANS.SWAT_ITEM:
                tutoText.text = "盾からバットへ\n持ち替えよう";
                break;*/

            case TUTO_TRANS.HOLD_BAT:
                tutoText.text = "バットを持ってみよう";
                break;

            case TUTO_TRANS.HIT_BACK:
                tutoText.text = "打ち返してみよう";
                break;

            case TUTO_TRANS.WAHT_DIE:
                tutoText.text = "３回水風船に当たると\nゲームオーバー\n視界が見えなくなるよ";
                break;

            default:
                break;
        }
    }


    void TutoTrans()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            next = true;
        }
        /*switch (tutoTrans)
        {
            case TUTO_TRANS.HOLD_SHIELD:
                if (どっちかの手に盾持ってる)
                {
                    if (次へを押す) {
                        次のチュートリアルへ
                    }
                }
                break;

            case TUTO_TRANS.SWAP_HAND:
                if (持ち手を変える) {
                    if (次へを押す) {
                        次のチュートリアルへ
                    }
                }
                break;

            case TUTO_TRANS.ONLY_ONE_HAND:
                if (次へを押す) {
                    次のチュートリアルへ
                }
                break;

            case TUTO_TRANS.THROW_BALOON:
                if (次へを押す) {
                    次のチュートリアルへ
                }
                break;

            case TUTO_TRANS.PREVENT_BALOON:
                if (一個でも弾き返す) {
                    if (次へを押す) {
                        次のチュートリアルへ
                    }
                }
                break;

            case TUTO_TRANS.DURABILITY:
                if (次へを押す) {
                    次のチュートリアルへ
                }
                break;

            case TUTO_TRANS.THROW_IN:
                if (次へを押す) {
                    次のチュートリアルへ
                }
                break;

            case TUTO_TRANS.SWAT_ITEM:
                if (次へを押す) {
                    次のチュートリアルへ
                }
                break;

            case TUTO_TRANS.HOLD_BAT:
                if (どっちかの手にバット持ってる)
                {
                    if (次へを押す) {
                        次のチュートリアルへ
                    }
                }
                break;

            case TUTO_TRANS.HIT_BACK:
                if (一発でも打ち返す)
                {
                    if (次へを押す) {
                        次のチュートリアルへ
                    }
                }
                break;

            case TUTO_TRANS.WAHT_DIE:
                if (視界が見えなくなる)
                {
                    if (次へを押す) {
                        チュートリアル終了
                    }
                }
                break;

            default:
                break;
        }*/
    }
}
