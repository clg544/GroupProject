﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    /* Movement to apply this frame */
    private Vector3 frameMovement;
    
    /* How much force is applied by player movement */
    [SerializeField]
    private float acceleration;
    
    /* How much the velocity is scaled down on brake */
    [SerializeField]
    private float brakeFraction;

    /* The maximum speed a player is allowed to go */
    [SerializeField]
    private float maxSpeed;

    /* Holds our plaayer and player body */
    public GameObject player;
    private Rigidbody2D playerBody;


    /**
     *  Collection of basic movement functions that add to a movement vector, which is 
     *      reset every frame.
     */
    public void MoveLeft()
    {
        frameMovement.x -= acceleration;
    }

    public void MoveRight()
    {
        frameMovement.x += acceleration;
    }

    public void MoveUp()
    {
        frameMovement.y += acceleration;
    }

    public void MoveDown()
    {
        frameMovement.y -= acceleration;
    }

    /**
     * Scale the player's velocity down, if brakeFraction is < zero 
     */
    public void Brake()
    {
        playerBody.velocity *= brakeFraction;
    }


    // Use this for initialization
    void Start () {
        playerBody = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        /* Accelerate the player with a movement vector based on this frames input */
        playerBody.AddForce(frameMovement.normalized * acceleration);

        /* Cap player speed with a velocity check */
        if (playerBody.velocity.magnitude > maxSpeed)
            playerBody.velocity = (playerBody.velocity.normalized * maxSpeed);
        
        /* Apply some simple friction */
        playerBody.velocity *= .99f;

        /* Reset any persistant variables */
        frameMovement = Vector3.zero;

		if(Input.GetKey(KeyCode.A)){
			MoveLeft ();
		}

		if (Input.GetKey (KeyCode.D)) {
			MoveRight();
		}

		if (Input.GetKey (KeyCode.W)) {
			MoveUp ();
		}

		if (Input.GetKey (KeyCode.S)) {
			MoveDown ();
		}
	}
}
