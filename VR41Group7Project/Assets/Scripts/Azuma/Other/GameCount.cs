using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCount : MonoBehaviour {

	static private float playTime;
	static private int catchCount;
	static private int defendCount;
	static private int enemyDownCount;
	static private bool timeCountFlg;
	// Use this for initialization
	void Start() {
		timeCountFlg = false;
		CountReset();
	}

	static public int PlayTime {
		get { return (int)playTime; }
	}

	static public int CatchCount {
		get { return catchCount; }
	}

	static public int DefendCount {
		get { return defendCount; }
	}

	static public int EnemyDownCount {
		get { return enemyDownCount; }
	}

	/// <summary>
	/// カウントのリセット
	/// </summary>
	static public void CountReset() {
		playTime = 0.0f;
		catchCount = 0;
		defendCount = 0;
		enemyDownCount = 0;
		timeCountFlg = false;
	}

	// Update is called once per frame
	void Update() {
		if (timeCountFlg) {
			playTime += Time.deltaTime;
		}
	}

	/// <summary>
	/// 経過時間のカウントをするかどうか
	/// false 止める  true カウントする
	/// </summary>
	/// <param name="flg"></param>
	static public void TimeCountFlg(bool flg) {
		timeCountFlg = flg;
	}

	/// <summary>
	/// キャッチした回数を加算する
	/// </summary>
	static public void CacheCounter() {
		catchCount++;
	}

	/// <summary>
	/// 防御した回数を加算する
	/// </summary>
	static public void DefendCounter() {
		defendCount++;
	}

	/// <summary>
	/// 防御した回数を加算する
	/// </summary>
	static public void EnemyDownCounter() {
		enemyDownCount++;
	}

}
