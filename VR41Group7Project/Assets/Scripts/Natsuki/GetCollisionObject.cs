using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCollisionObject : MonoBehaviour {
    public GameObject hitObj;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        hitObj = col.gameObject;
    }
}
