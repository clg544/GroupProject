using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour {

	List<GameObject> obj;
	bool hasEnteredDoor;

	GameObject sDoor;
	GameObject nDoor;
	GameObject wDoor;
	GameObject eDoor;

	// Use this for initialization
	void Start () {
		hasEnteredDoor = false;
		sDoor = GameObject.FindGameObjectWithTag ("DoorS");
		nDoor = GameObject.FindGameObjectWithTag ("DoorN");
		wDoor = GameObject.FindGameObjectWithTag ("DoorW");
		eDoor = GameObject.FindGameObjectWithTag ("DoorE");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D coll){
		
		if (coll.gameObject.tag == "Player") {
			hasEnteredDoor = true;
		}
	}

	void OnTriggerExit2d(Collider2D coll){
		hasEnteredDoor = false;
	}

	public void goThroughDoor(){

		if (hasEnteredDoor == true) {
			if (this.gameObject.name == "north door") {
				
				GameObject players = GameObject.FindGameObjectWithTag ("Player");
				players.gameObject.transform.position = sDoor.gameObject.transform.position;
				hasEnteredDoor = false;
			}

			if (this.gameObject.name == "south door") {
	
				GameObject players = GameObject.FindGameObjectWithTag ("Player");
				players.gameObject.transform.position = nDoor.gameObject.transform.position;
				hasEnteredDoor = false;
			}

			if (this.gameObject.name == "east door") {
				
				GameObject players = GameObject.FindGameObjectWithTag ("Player");
				players.gameObject.transform.position = wDoor.gameObject.transform.position;
				hasEnteredDoor = false;
			}

			if (this.gameObject.name == "west door") {
				
				GameObject players = GameObject.FindGameObjectWithTag ("Player");
				players.gameObject.transform.position = eDoor.gameObject.transform.position;
				hasEnteredDoor = false;
			}
		}
	}
}