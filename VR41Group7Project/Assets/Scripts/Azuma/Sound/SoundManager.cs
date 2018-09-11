using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	[SerializeField, HeaderAttribute("BGMのリソース")]
	private AudioClip[] bgm;
	[SerializeField, HeaderAttribute("SEのリソース")]
	private AudioClip[] se;

	static private AudioSource audioSource;
	static private AudioClip[] audioBgm;
	static private AudioClip[] audioSe;

	static private Vector3 soundPos;
	public enum BgmTitle {
		Title,
		StageSelect,
		GameMain,
		Result,
	}

	public enum SeTitle {
		Catch,
		Equioment,
		BodyHit,
		Select,
		Throw,
		WeaponHit,
	}

	// Use this for initialization
	void Awake () {
		audioSource = GetComponent<AudioSource>();
		// BGMのAudioSourceの生成と曲の登録
		audioBgm = new AudioClip[bgm.Length];
		for (int count = 0; count < bgm.Length; count++) {
			audioBgm[count] = bgm[count];
		}

		// SEのAudioSourceの生成と曲の登録
			audioSe = new AudioClip[se.Length];
		for (int count = 0; count < se.Length; count++) {
			audioSe[count] = se[count];
		}
		soundPos = transform.position;
		audioSource.clip = audioBgm[0];
	}

	/// <summary>
	/// BGMの再生
	/// </summary>
	/// <param name="title"> </param>
	static public void BGMStart(BgmTitle title) {
		audioSource.clip = audioBgm[title.GetHashCode()];
		audioSource.Play();
	}

	static public void BGMStop(bool stopFlg) {
		if (stopFlg) {
			audioSource.Stop();
		} else {
			audioSource.Play();
		}
	}

	/// <summary>
	/// SEの再生
	/// </summary>
	/// <param name="title"></param>
	/// <param name="pos"></param>
	static public void SEStart(SeTitle title , Vector3 pos) {
		AudioSource.PlayClipAtPoint(audioSe[title.GetHashCode()], pos);
	}
}
