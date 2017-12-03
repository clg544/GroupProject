using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldCameraManager : MonoBehaviour {

    public int maxFov;  // Max FOV to add
    public int maxAllowableDistance;

    public Camera myCamera;
    public GameObject players;
    public GameObject ship;

    Vector3 playerPos;
    Vector3 shipPos;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        playerPos = players.transform.position;
        shipPos = ship.transform.position;

        gameObject.transform.position = (playerPos - shipPos) / 2;
        gameObject.transform.position += new Vector3(0, 0, -150);
        myCamera.fieldOfView = 60 + ((Vector3.Distance(playerPos, shipPos) / maxAllowableDistance) * maxFov);
	}
}
