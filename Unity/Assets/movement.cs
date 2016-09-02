using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
	private Rigidbody2D rb2D;  
	public bool direction = false;
	private int speed = 0;
	// Use this for initialization
	void Start () {
		rb2D = GetComponent <Rigidbody2D> ();
		if (direction == true) {
			speed = -5;
		} else if (direction == false) {
			speed = 5;
		}
	}
	
	// Update is called once per frame
	void Update () {
		rb2D.AddForce(new Vector3(speed,0,0));
	}

	void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log ("Collision!!!!");
		if (collision.gameObject.tag != "Player") {


			Destroy (this.gameObject);
		}
	}
}
