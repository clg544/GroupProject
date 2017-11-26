using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour {

	public GameObject powerContainer;
	public Sprite needTwo;
	public Sprite needOne;
	public Sprite needNone;

	List<GameObject> obj;
	public bool hasEnteredDoor;

	GameObject[] sDoor;
	GameObject[] nDoor;
	GameObject[] wDoor;
	GameObject[] eDoor;

	public GameObject theDoor;

	InputManagerScript ims;

	int powerSupply;

	// Use this for initialization
	void Start () {

		ims = GameObject.FindObjectOfType<InputManagerScript>();

		powerSupply = 0;
		hasEnteredDoor = false;

		if (this.gameObject.name == "north door") {
			sDoor = GameObject.FindGameObjectsWithTag ("DoorS");
			GameObject closestDoor = null;
			foreach (GameObject obj in sDoor)
			{
				if(closestDoor == null)
				{
					closestDoor = obj;
				}
				//compares distances
				if(Vector3.Distance(transform.position, obj.transform.position) <= Vector3.Distance(transform.position, closestDoor.transform.position))
				{
					closestDoor = obj;
				}
			}
			theDoor = closestDoor;
		}

		if (this.gameObject.name == "south door") {
			nDoor = GameObject.FindGameObjectsWithTag ("DoorN");
			GameObject closestDoor = null;
			foreach (GameObject obj in nDoor)
			{
				if(closestDoor == null)
				{
					closestDoor = obj;
				}
				//compares distances
				if(Vector3.Distance(transform.position, obj.transform.position) <= Vector3.Distance(transform.position, closestDoor.transform.position))
				{
					closestDoor = obj;
				}
			}
			theDoor = closestDoor;
		}

		if (this.gameObject.name == "east door") {
			wDoor = GameObject.FindGameObjectsWithTag ("DoorW");
			GameObject closestDoor = null;
			foreach (GameObject obj in wDoor)
			{
				if(closestDoor == null)
				{
					closestDoor = obj;
				}
				//compares distances
				if(Vector3.Distance(transform.position, obj.transform.position) <= Vector3.Distance(transform.position, closestDoor.transform.position))
				{
					closestDoor = obj;
				}
			}
			theDoor = closestDoor;
		}

		if (this.gameObject.name == "west door") {
			eDoor = GameObject.FindGameObjectsWithTag ("DoorE");
			GameObject closestDoor = null;
			foreach (GameObject obj in eDoor)
			{
				if(closestDoor == null)
				{
					closestDoor = obj;
				}
				//compares distances
				if(Vector3.Distance(transform.position, obj.transform.position) <= Vector3.Distance(transform.position, closestDoor.transform.position))
				{
					closestDoor = obj;
				}
			}

			theDoor = closestDoor;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (ims.openingDoor == true) {
			goThroughDoor ();
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		
		if (coll.gameObject.tag == "Player") {
			hasEnteredDoor = true;
		}

		if (coll.gameObject.tag == "Power") {
			Debug.Log ("Ran into item");
			powerSupply += 1;
			this.GetComponentInParent<LevelGenerator> ().placeSingleItem (coll.gameObject);
			if (powerSupply == 1) {
				powerContainer.GetComponent<SpriteRenderer> ().sprite = needTwo;
			} else if (powerSupply == 2) {
				powerContainer.GetComponent<SpriteRenderer> ().sprite = needOne;
			} else if(powerSupply >= 3){
				powerContainer.GetComponent<SpriteRenderer> ().sprite = needNone;
			}
		}
	}

	void OnTriggerExit2d(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            hasEnteredDoor = false;
        }
	}

	//Determines what door to go through
	public void goThroughDoor(){
		//If player has entered door zone, allow passage to another
		if (hasEnteredDoor == true) {

			GameObject players = GameObject.FindGameObjectWithTag ("Players");
			players.transform.position = new Vector2 (theDoor.transform.position.x, theDoor.transform.position.y);

			players.GetComponentInChildren<Transform>().position = new Vector2 (theDoor.transform.position.x, theDoor.transform.position.y);

			Debug.Log (theDoor.name);
	
			hasEnteredDoor = false;

		}
	}
}