﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadControl : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey("s")) {
			//Debug.Log ("down");
			this.transform.Translate (new Vector3 (0f, -0.1f, 0f));
		}	
		if (Input.GetKey("w")) {
			//Debug.Log ("up");
			this.transform.Translate (new Vector3 (0f, 0.1f, 0f));
		}	
		//Debug.Log ("update");

	}

}
