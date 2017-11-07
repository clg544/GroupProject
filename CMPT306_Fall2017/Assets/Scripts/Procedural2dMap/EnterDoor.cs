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

	//Determines what door to go through
	public void goThroughDoor(){

		//If player has entered door zone, allow passage to another
		if (hasEnteredDoor == true) {
			if (this.gameObject.name == "north door") {
				
				GameObject players = GameObject.FindGameObjectWithTag ("Players");
				players.transform.localPosition = new Vector2 (sDoor.transform.position.x, sDoor.transform.position.y);
	
				hasEnteredDoor = false;
			}

			if (this.gameObject.name == "south door") {
	
				GameObject players = GameObject.FindGameObjectWithTag ("Players");
				players.transform.localPosition = new Vector2 (nDoor.transform.position.x, nDoor.transform.position.y);

				hasEnteredDoor = false;
			}

			if (this.gameObject.name == "east door") {
				
				GameObject players = GameObject.FindGameObjectWithTag ("Players");
				players.transform.localPosition = new Vector2 (wDoor.transform.position.x, wDoor.transform.position.y);
				
				hasEnteredDoor = false;
			}

			if (this.gameObject.name == "west door") {
				
				GameObject players = GameObject.FindGameObjectWithTag ("Players");
				players.transform.localPosition = new Vector2 (eDoor.transform.position.x, eDoor.transform.position.y);

			
				hasEnteredDoor = false;
			}
		}
	}
}