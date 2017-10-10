using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour {

	List<GameObject> obj;
	bool hasLeftDoor;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D coll){
		Debug.Log ("Here");
		if (coll.collider.tag == "Player") {
			coll.transform.position += new Vector3 (100, 0, 0);
		}
	}

}