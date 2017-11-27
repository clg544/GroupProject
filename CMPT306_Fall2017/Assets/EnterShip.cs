using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterShip : MonoBehaviour {

	ScoreManager sm;

	// Use this for initialization
	void Start () {
		sm = GameObject.FindObjectOfType<ScoreManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll){

		if (sm.curScore > 100) {
			if (coll.gameObject.tag == "Player") {
				SceneManager.LoadScene("Win");
				Debug.Log ("You Win");
			}
		}
	}
}
