using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGather : MonoBehaviour {

	int hits = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll){
		hits++;

		if (hits > 10) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		hits--;
	}
}
