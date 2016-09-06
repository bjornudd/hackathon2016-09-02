using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class force : MonoBehaviour {

	public float forceVal = 0f;
	public int raknaV = 0;
	public int raknaH = 0;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (forceVal, 0f));
	}

	public void SetForceVal(float newForce) {
		forceVal = newForce;
	}

	public float GetForceVal(){
		 return forceVal;
	}

	void OnCollisionEnter2D(Collision2D coll2d)
	{
		Debug.Log ("collision");
		if (coll2d.gameObject.tag == "paddel") {
			forceVal *= -1;
		}
		if (coll2d.gameObject.tag == "goalLeft") {
			raknaH++;
			forceVal *= -1;
		}
		if (coll2d.gameObject.tag == "goalRight") {
			raknaV++;
			forceVal *= -1;
		}
	}

	void OnGUI(){
		GUI.Label (new Rect (0, 0, Screen.width, Screen.height), raknaV.ToString());
		GUI.Label (new Rect (Screen.width-50, 0, Screen.width, Screen.height), raknaH.ToString());
	}

}
