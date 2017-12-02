using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour {
    
    public float enterDistance;

	public GameObject powerContainer;
	public Sprite needTwo;
	public Sprite needOne;
	public Sprite needNone;

	List<GameObject> obj;
    public bool fightyHasEnteredDoor;
    public bool shootyHasEnteredDoor;

	GameObject[] sDoor;
	GameObject[] nDoor;
	GameObject[] wDoor;
	GameObject[] eDoor;

	public GameObject players;
    GameObject fighty;
    GameObject shooty;

	public GameObject theDoor;

	InputManagerScript ims;
    PlayerTetherScript tetherManager;
    Behaviour myHalo;

	public int powerSupply;

	// Use this for initialization
	void Start () {

		ims = GameObject.FindObjectOfType<InputManagerScript>();
        tetherManager = GameObject.FindObjectOfType<PlayerTetherScript>();

        myHalo = (Behaviour)GetComponent("Halo");
        myHalo.enabled = false;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].name == "Fighty")
            {
                fighty = players[i];
            }
            if (players[i].name == "Shooty")
            {
                shooty = players[i];
            }
        }

        powerSupply = 0;
		fightyHasEnteredDoor = false;
        shootyHasEnteredDoor = false;

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
		
		if (coll.gameObject.name == "Fighty") {
			fightyHasEnteredDoor = true;
		}
        else if(coll.gameObject.name == "Shooty") {
            shootyHasEnteredDoor = true;
        }

		if (coll.gameObject.tag == "Power") {
			powerSupply += 1;
			this.GetComponentInParent<LevelGenerator> ().placeSingleItem (coll.gameObject);
			if (powerSupply == 1) {
				powerContainer.GetComponent<SpriteRenderer> ().sprite = needTwo;
			} else if (powerSupply == 2) {
				powerContainer.GetComponent<SpriteRenderer> ().sprite = needOne;
			} else if(powerSupply >= 3){
				powerContainer.GetComponent<SpriteRenderer> ().sprite = needNone;
                myHalo.enabled = true;
			}
		}
	}

	void OnTriggerExit2d(Collider2D coll)
    {
        if (coll.gameObject.name == "Fighty")
        {
            fightyHasEnteredDoor = false;
        }
        else if (coll.gameObject.name == "Shooty")
        {
            shootyHasEnteredDoor = false;
        }
    }

	//Determines what door to go through
	public void goThroughDoor(){
        powerSupply = 3;

        //If player has entered door zone, allow passage to another
		if (fightyHasEnteredDoor && shootyHasEnteredDoor && powerSupply >= 3) {
            fightyHasEnteredDoor = false;
            shootyHasEnteredDoor = false;

            switch (theDoor.name)
            {
                case ("north door"):
                    fighty.transform.position = theDoor.transform.position + new Vector3(-tetherManager.initialDistance / 2, 0, 0);
                    shooty.transform.position = theDoor.transform.position + new Vector3(tetherManager.initialDistance / 2, 0, 0);
                    tetherManager.distributeNodes();
                    break;

                case ("south door"):
                    fighty.transform.position = theDoor.transform.position + new Vector3(-tetherManager.initialDistance / 2, 0, 0);
                    shooty.transform.position = theDoor.transform.position + new Vector3(tetherManager.initialDistance / 2, 0, 0);
                    tetherManager.distributeNodes();
                    break;
                    
                case ("east door"):
                    fighty.transform.position = theDoor.transform.position + new Vector3(0, -tetherManager.initialDistance / 2, 0);
                    shooty.transform.position = theDoor.transform.position + new Vector3(0, tetherManager.initialDistance / 2, 0);
                    tetherManager.distributeNodes();
                    break;

                case ("west door"):
                    fighty.transform.position = theDoor.transform.position + new Vector3(0, -tetherManager.initialDistance / 2, 0);
                    shooty.transform.position = theDoor.transform.position + new Vector3(0, tetherManager.initialDistance / 2, 0);
                    tetherManager.distributeNodes();
                    break;

            }
		}
	}
}