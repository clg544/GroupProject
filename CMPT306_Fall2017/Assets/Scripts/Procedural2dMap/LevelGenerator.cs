using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

	public int width;
	public int height;

    public bool refreshOnClick = false;

	public string seed;
	public bool useRandomSeed;

	[Range(0,100)]
	public int randomFillPercent;

	public GameObject door;

	int[,] map;
	int doorWidth1;
	int doorHeight1;
	int doorWidth2;
	int doorHeight2;

	int n = 0;
	int e = 0;
	int s = 0;
	int w = 0;

	public GameObject spawnObject;
	public GameObject navPoint;
	public GameObject[] enemyPlayers;
	public GameObject playerSpawn;
	public GameObject itemPrefab;

	List<GameObject> enemySpawns;
	List<GameObject> navPoints;
	List<GameObject> allEnemies;
	List<GameObject> allItems;
	public List<String> directions;
	List<GameObject> doors;

	void Awake(){
		directions = new List<String> ();
		doors = new List<GameObject> ();
	}

	void Start() {
		enemySpawns = new List<GameObject> ();
		navPoints = new List<GameObject> ();
		allEnemies = new List<GameObject> ();
		allItems = new List<GameObject> ();

		int numOfNavs = 3;

		for (int i = 0; i < numOfNavs; i++) {
			GameObject nav = Instantiate (navPoint);
			navPoints.Add (nav);
			nav.transform.SetParent (this.transform);
		}

		GenerateMap();
	}

	void Update() {
        if (refreshOnClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GenerateMap();
            }
        }
	}

	//randomly creates the map
	void GenerateMap() {
		if (allEnemies != null) {
			foreach (GameObject enemy in allEnemies) {
				Destroy (enemy.gameObject);
			}
		}

		//Destroy all previous spawns
		if (enemySpawns != null) {
			foreach (GameObject spawn in enemySpawns) {
				Destroy (spawn.gameObject);
			}
		}

		if (allItems != null) {
			foreach (GameObject item in allItems) {
				Destroy (item.gameObject);
			}
		}

		foreach (String dir in directions) {
			doorPicker (dir);
		}

		map = new int[width,height];
		RandomFillMap();

		for (int i = 0; i < 5; i ++) {
			SmoothMap();
		}

		clearDoorSpace ();

		placeNavs ();
        placeSpawns();
        placeItems();

        MeshCreator meshGen = GetComponent<MeshCreator>();
		meshGen.GenerateMesh(map, 1);
	}


	void RandomFillMap() {

		if (useRandomSeed) {
			seed = Time.time.ToString();
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		//Go through all the coordinates, setting the borders to be 1
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				//Set an open space for doors
				 if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					map [x, y] = 1;
				} else {
					map [x, y] = (pseudoRandom.Next (0, 100) < randomFillPercent) ? 1 : 0;
				}
			}
		}	
	}

	//Picks which doors are available, north, south, east or west
	public void doorPicker(String direction){

		//If direction equals a certain letter, set up the door accordingly
		if (direction == "N") {
			n += 1;
			GameObject nDoor = Instantiate(door);
			nDoor.transform.SetParent (this.transform);
			nDoor.transform.position = this.transform.position + new Vector3(0, height/3, 0);
			nDoor.tag = "DoorN";
			nDoor.name = "north door";
			doors.Add (nDoor);
		}
		if (direction == "E") {

			e += 1;
			GameObject eDoor = Instantiate(door);
			eDoor.transform.SetParent (this.transform);
			eDoor.transform.position = this.transform.position + new Vector3(width/3, 0, 0);
			eDoor.tag = "DoorE";
			eDoor.name = "east door";
			doors.Add (eDoor);
		}
		if (direction == "S") {

			s += 1;
			GameObject sDoor = Instantiate(door);
			sDoor.transform.SetParent (this.transform);
			sDoor.transform.position = this.transform.position - new Vector3 (0, height / 3, 0);
			sDoor.tag = "DoorS";
			sDoor.name = "south door";
			doors.Add (sDoor);
		}
		if (direction == "W") {

			w += 1;
			GameObject wDoor = Instantiate(door);
			wDoor.transform.SetParent (this.transform);
			wDoor.transform.position = this.transform.position - new Vector3(width/3, 0, 0);
			wDoor.tag = "DoorW";
			wDoor.name = "west door";
			doors.Add (wDoor);
		}
	}

	//smooth out the rough edges
	void SmoothMap() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {

				//if number of wall tiles around the point is greater than four
				//turn that space into a wall, if else then it is not a wall
				int neighbourWallTiles = GetSurroundingWallCount(x,y);

				if (neighbourWallTiles > 4)
					map[x,y] = 1;
				else if (neighbourWallTiles < 4)
					map[x,y] = 0;

			}
		}
	}

	void clearDoorSpace(){
		foreach (GameObject door in doors) {
			if (door.tag == "DoorN") {
				for (int x = width / 2 - 10; x < width / 2 + 10; x++) {
					for (int y = height - 10; y < height - 1; y++) {
						map [x, y] = 0;
					}
				} 
			}

			if (door.tag == "DoorE") {
				for (int x = width - 10; x < width - 1; x++) {
					for (int y = height/2 - 10; y < height/2 + 10; y++) {
						map [x, y] = 0;
					}
				} 
			}

			if (door.tag == "DoorS") {
				for (int x = width / 2 - 10; x < width / 2 + 10; x++) {
					for (int y = 1; y < 10; y++) {
						map [x, y] = 0;
					}
				} 
			}

			if (door.tag == "DoorW") {
				for (int x = 1; x < 10; x++) {
					for (int y = height/2 - 10; y < height/2 + 10; y++) {
						map [x, y] = 0;
					}
				} 
			}
		}
	}

	//Places the spawns for enemies
	void placeSpawns(){
		//the number of spawns that have been placed
		int placedSpawns = 0;
		int numOfSpawns = UnityEngine.Random.Range (2, 6);

		//the current x and y location randomly selected
		int currentLocX = UnityEngine.Random.Range (0, width);
		int currentLocY = UnityEngine.Random.Range (0, height);

		//while the number of spawns placed is less than the max, place new spawns
		while (placedSpawns < numOfSpawns) {

			//if the current x and y are a wall, Instantiate a spawnObject and set its location
			//to the current x and y
			//if not, randomly pick new x and y
			if (map [currentLocX, currentLocY] == 0) {
				placedSpawns += 1;
				GameObject spawn = Instantiate (spawnObject);
				enemySpawns.Add (spawn);

				spawn.transform.SetParent (this.transform);
				spawn.transform.localPosition = new Vector2 (currentLocX - width / 2, currentLocY - height / 2);

				//Instantiate the different enemies randomly
				int enemySelection = UnityEngine.Random.Range (0, 2);
				GameObject newEnemy = Instantiate (enemyPlayers [enemySelection]);
				allEnemies.Add (newEnemy);
				newEnemy.gameObject.tag = "Enemy";
				newEnemy.transform.position = spawn.transform.position;

				if (enemySelection == 1) {
					foreach (GameObject nav in navPoints) {
						newEnemy.GetComponent<BasicMeleeEnemyBehaviour> ().navPoints.Add (nav);
					}
				}

				currentLocX = UnityEngine.Random.Range (0, width);
				currentLocY = UnityEngine.Random.Range (0, height);
			} else {
				currentLocX = UnityEngine.Random.Range (0, width);
				currentLocY = UnityEngine.Random.Range (0, height);
			}
		}

		while (map [currentLocX, currentLocY] != 0) {
			currentLocX = UnityEngine.Random.Range (0, width);
			currentLocY = UnityEngine.Random.Range (0, height);
			playerSpawn.transform.localPosition = new Vector2 (currentLocX - width / 2, currentLocY - height / 2);
		}
	}

	//Place the nav points for the AI
	void placeNavs(){

		//the current x and y location randomly selected
		int currentLocX = UnityEngine.Random.Range (0, width);
		int currentLocY = UnityEngine.Random.Range (0, height);

		foreach(GameObject nav in navPoints){
			if (map [currentLocX, currentLocY] == 0) {
				nav.transform.localPosition = new Vector2 (currentLocX - width / 2, currentLocY - height / 2);

				currentLocX = UnityEngine.Random.Range (0, width);
				currentLocY = UnityEngine.Random.Range (0, height);
			} else {
				currentLocX = UnityEngine.Random.Range (0, width);
				currentLocY = UnityEngine.Random.Range (0, height);	
			}
		}
	}

	void placeItems(){
		int placedItems = 0;
		int numOfItems = UnityEngine.Random.Range (3, 6);

		//the current x and y location randomly selected
		int currentLocX = UnityEngine.Random.Range (0, width);
		int currentLocY = UnityEngine.Random.Range (0, height);

		//while the number of spawns placed is less than the max, place new spawns
		while (placedItems < numOfItems) {

			//if the current x and y are a wall, Instantiate a spawnObject and set its location
			//to the current x and y
			//if not, randomly pick new x and y
			if (map [currentLocX, currentLocY] == 0) {
				placedItems += 1;
				GameObject item = Instantiate (itemPrefab);
				allItems.Add (item);

				item.transform.SetParent (this.transform);
				item.transform.localPosition = new Vector2 (currentLocX - width / 2, currentLocY - height / 2);

				currentLocX = UnityEngine.Random.Range (0, width);
				currentLocY = UnityEngine.Random.Range (0, height);
			} else {
				currentLocX = UnityEngine.Random.Range (0, width);
				currentLocY = UnityEngine.Random.Range (0, height);
			}
		}
	}

	//Gets the number of surrounding walls
	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map[neighbourX,neighbourY];
					}
				}
				else {
					wallCount ++;
				}
			}
		}

		return wallCount;
	}

    public void DisableScene()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        for(int i = 0; i < allEnemies.Length; i++)
        {
            Destroy(allEnemies[i]);
        }

        GameObject[] allItems = GameObject.FindGameObjectsWithTag("Power");

        for (int i = 0; i < allItems.Length; i++)
        {
            Destroy(allItems[i]);
        }
    }
    public void EnableScene()
    {
        placeSpawns ();
        placeItems ();
    }
}