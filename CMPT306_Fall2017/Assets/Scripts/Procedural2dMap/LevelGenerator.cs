using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

	public int width;
	public int height;

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

	int partOfList = 0;

	void Start() {
		GenerateMap();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			GenerateMap();
		}
	}

	void GenerateMap() {
		map = new int[width,height];
		RandomFillMap();

		for (int i = 0; i < 5; i ++) {
			SmoothMap();
		}

		MeshCreator meshGen = GetComponent<MeshCreator>();
		meshGen.GenerateMesh(map, 1);
	}


	void RandomFillMap() {
		int randomSpawnPlacement = UnityEngine.Random.Range (0, 5);

		if (useRandomSeed) {
			seed = Time.time.ToString();
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (x < doorWidth2 && x > doorWidth1 && y < doorHeight1 && y > doorHeight2) {
					map [x, y] = 0;
				} else if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					map [x, y] = 1;
				} else {
					map [x, y] = (pseudoRandom.Next (0, 100) < randomFillPercent) ? 1 : 0;
					if (map [x, y] == 0 && partOfList < randomSpawnPlacement) {
						partOfList += 1;
						GameObject spawn = Instantiate (spawnObject);
						spawn.transform.SetParent (this.transform);
						spawn.transform.position = new Vector2 (x, y);
					}
				}
			}
		}
	}

	public void doorPicker(String direction){
		if (direction == "N") {
			doorWidth1 = (width/2) - 10;
			doorWidth2 = (width/2) + 10;
			doorHeight1 = height -1;
			doorHeight2 = height - 5;

			n += 1;
			GameObject nDoor = Instantiate(door);
			nDoor.transform.SetParent (this.transform);
			nDoor.transform.position = this.transform.position + new Vector3(0, height/3, 0);
			nDoor.tag = "DoorN";
			nDoor.name = "north door";
		}
		if (direction == "E") {
			doorWidth1 = width - 5;
			doorWidth2 = width - 1;
			doorHeight1 = (height / 2) + 10;
			doorHeight2 = (height / 2) - 10;

			e += 1;
			GameObject eDoor = Instantiate(door);
			eDoor.transform.SetParent (this.transform);
			eDoor.transform.position = this.transform.position + new Vector3(width/3, 0, 0);
			eDoor.tag = "DoorE";
			eDoor.name = "east door";
		}
		if (direction == "S") {
			doorWidth1 = (width/2) - 10;
			doorWidth2 = (width/2) + 10;
			doorHeight1 = 5;
			doorHeight2 = 0;

			s += 1;
			GameObject sDoor = Instantiate(door);
			sDoor.transform.SetParent (this.transform);
			sDoor.transform.position = this.transform.position - new Vector3 (0, height / 3, 0);
			sDoor.tag = "DoorS";
			sDoor.name = "south door";
		}
		if (direction == "W") {
			doorWidth1 = 0;
			doorWidth2 = 5;
			doorHeight1 = (height/2) + 10;
			doorHeight2 = (height / 2) - 10;

			w += 1;
			GameObject wDoor = Instantiate(door);
			wDoor.transform.SetParent (this.transform);
			wDoor.transform.position = this.transform.position - new Vector3(width/3, 0, 0);
			wDoor.tag = "DoorW";
			wDoor.name = "west door";
		}
	}

	void SmoothMap() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neighbourWallTiles = GetSurroundingWallCount(x,y);

				if (neighbourWallTiles > 4)
					map[x,y] = 1;
				else if (neighbourWallTiles < 4)
					map[x,y] = 0;

			}
		}
	}

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
}