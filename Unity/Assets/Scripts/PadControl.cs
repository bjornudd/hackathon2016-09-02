using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadControl2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("down")) {
			Debug.Log ("down");
			this.transform.Translate (new Vector3 (0f, -0.1f, 0f));
		}	
		if (Input.GetKey("up")) {
			Debug.Log ("up");
			this.transform.Translate (new Vector3 (0f, 0.1f, 0f));
		}	

	}


}
