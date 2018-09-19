using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDisplay : MonoBehaviour {
	[SerializeField] private GameObject textPrefab;
	// Use this for initialization
	void Start() {
		TextMesh[] texts = new TextMesh[transform.childCount];
		Vector3 pos;
		int count = 0;
		foreach (Transform child in transform) {
			pos = child.position;
			pos.x =0.4f;
			pos.y += 2.0f;
			texts[count] = Instantiate(textPrefab, pos, Quaternion.identity).GetComponent<TextMesh>();
			count++;
		}
		texts[0].text = TimeDisplay(GameCount.PlayTime);
		texts[1].text = ScorePoint(GameCount.CatchCount, 3) + "  回";
		texts[2].text = ScorePoint(GameCount.DefendCount, 3) + "  回";
		texts[3].text = ScorePoint(GameCount.EnemyDownCount, 3) + "  人";
	}

	/// <summary>
	/// スコアを表示する際、桁数に応じて0を書くための変数
	/// </summary>
	/// <param name="digit"></param>
	/// <returns></returns>
	string ScorePoint(int score, int digit) {
		int scoreMoji;
		string recvMoji = "";
		int[] str;
		str = new int[digit];
		// 指定された桁数までスコアを1桁ずつ取得していく
		for (int count = 0; count < digit; count++) {
			scoreMoji = score % 10;
			str[count] = scoreMoji;
			score /= 10;
		}

		// 取得した数字を下から文字列に直して格納していく
		for (int count = 0; count < digit; count++) {
			recvMoji += str[digit - 1 - count].ToString();
		}
		return recvMoji;
	}

	/// <summary>
	/// 時間を表示するための関数
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	string TimeDisplay(int time) {
		int minutes = 0;
		int seconds = 0;
		string recvminutes = "";
		string recvseconds = "";
		int[] secondsNum;
		int[] minutesNum;
		secondsNum = new int[2];
		minutesNum = new int[2];

		while (true) {
			if (time >= 60) {
				minutes++;
				time -= 60;
			} else {
				seconds = time;
				time = 0;
				break;
			}
		}

		for (int count = 0; count < 2; count++) {
			secondsNum[count] = seconds % 10;
			seconds /= 10;

			minutesNum[count] = minutes % 10;
			minutes /= 10;
		}

		for (int count = 0; count < 2; count++) {
			recvseconds += secondsNum[2 - 1 - count].ToString();
			recvminutes += minutesNum[2 - 1 - count].ToString();
		}
		return recvminutes + " : " + recvseconds;
	}
}