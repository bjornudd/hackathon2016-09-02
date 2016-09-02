using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class EnemyMovement : MonoBehaviour {

	private Rigidbody2D m_rigidbody2D;
	private int dire = 1;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		Quaternion rot = transform.rotation;
		transform.rotation = Quaternion.Euler(rot.x, Mathf.Sign(dire) == 1 ? 0 : 180, rot.z);
		//move += 1;
		m_rigidbody2D.velocity = new Vector2(dire* 2f, m_rigidbody2D.velocity.y);
	
	}

	void Awake() {
		m_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "RightBoundry") {
			dire = -1;
		} else if (other.gameObject.tag == "JumpBoundry") {
			m_rigidbody2D.AddForce (Vector2.up * 500);
		} else if (other.gameObject.tag == "LeftBoundry") {
			dire = 1;
		}

	}

}