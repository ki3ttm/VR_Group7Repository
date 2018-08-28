using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMotion : MonoBehaviour {
    public enum AnimaList { Wait,Walk,Throw};
    Animator HumanAnimaData;
	// Use this for initialization
	void Awake () {
        HumanAnimaData = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            StartAnimation(AnimaList.Wait);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            StartAnimation(AnimaList.Walk);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            StartAnimation(AnimaList.Throw);
        }
    }
    //各アニメーション再生関数
    public void StartAnimation(AnimaList AnimaSelect)
    {
        switch (AnimaSelect)
        {
            case AnimaList.Wait:
                AllEndAnimation();
                HumanAnimaData.SetBool("Wait", true);
                break;
            case AnimaList.Walk:
                AllEndAnimation();
                HumanAnimaData.SetBool("Walk", true);
                break;
            case AnimaList.Throw:
                AllEndAnimation();
                HumanAnimaData.SetBool("Throw", true);
                break;
        }
    }
    //各アニメーション停止関数
    public void EndAnimation(AnimaList AnimaSelect)
    {
        switch (AnimaSelect)
        {
            case AnimaList.Wait:
                HumanAnimaData.SetBool("Wait", false);
                break;
            case AnimaList.Walk:
                HumanAnimaData.SetBool("Walk", false);
                break;
            case AnimaList.Throw:
                HumanAnimaData.SetBool("Throw", false);
                break;
        }
    }
    //全アニメーション停止関数
    public void AllEndAnimation()
    {
        HumanAnimaData.SetBool("Wait", false);
        HumanAnimaData.SetBool("Walk", false);
        HumanAnimaData.SetBool("Throw", false);
    }
}
