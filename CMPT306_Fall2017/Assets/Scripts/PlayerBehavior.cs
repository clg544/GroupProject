﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    /* Movement to apply this frame */
    private Vector3 frameMovement;

	public GameObject spawnPoint;

    /* How much force is applied by player movement */
    public float acceleration;

    /* How much the velocity is scaled down on brake */
    public float brakeFraction;
    public float constantFraction;

    /* The maximum speed a player is allowed to go */
    public float maxSpeed;
    
    /* Holds our plaayer and player body */
    public GameObject player;
    private Rigidbody2D playerBody;
	public Animator anim;
    
    /* Getters and Setters */
    public float getMaxSpeed()
    {
        return maxSpeed;
    }

    /**
     *  Collection of basic movement functions that add to a movement vector, which is 
     *      reset every frame.
     */
    public void MoveLeft()
    {
		anim.SetBool ("MoveLeft", true);
        frameMovement.x -= acceleration;
    }
    public void MoveRight()
    {
		anim.SetBool ("MoveRight", true);
        frameMovement.x += acceleration;
    }
    public void MoveUp()
    {
		anim.SetBool ("MoveUp", true);
        frameMovement.y += acceleration;
    }
    public void MoveDown()
    {
		anim.SetBool ("MoveDown", true);
        frameMovement.y -= acceleration;
    }

    public void MoveHorizontal(float f)
    {
        if(Mathf.Abs(f) <= 1)
            frameMovement.x += f;

		if (frameMovement.x < 0) {
			anim.SetBool ("MoveLeft", true);
			anim.SetBool ("MoveRight", false);
		} else if(frameMovement.x > 0){
			anim.SetBool ("MoveRight", true);
			anim.SetBool ("MoveLeft", false);
		}
    }
    public void MoveVertical(float f)
    {
        if (Mathf.Abs(f) <= 1)
            frameMovement.y += f;
    }

	public void MoveIdel(){
		anim.SetBool ("MoveUp", false);
		anim.SetBool ("MoveDown", false);
		anim.SetBool ("MoveLeft", false);
		anim.SetBool ("MoveRight", false);
	}
    
    /**
     * Scale the player's velocity down, if brakeFraction is < zero 
     */
    public void Brake()
    {
        playerBody.velocity *= (brakeFraction * Time.deltaTime);
    }


    // Use this for initialization
    void Start () {
        playerBody = this.GetComponent<Rigidbody2D>();
		anim = this.GetComponent<Animator> ();
    }


	// Update is called once per frame
	void FixedUpdate () {
        /* Accelerate the player with a movement vector based on this frames input */
        playerBody.AddForce(frameMovement.normalized * acceleration);

        /* Cap player speed with a velocity check */
        if (playerBody.velocity.magnitude > maxSpeed)
            playerBody.velocity = (playerBody.velocity.normalized * maxSpeed);
        
        /* Apply some simple friction */
        playerBody.velocity *= constantFraction;

        /* Reset any persistant variables */
        frameMovement = Vector3.zero;
	}
}
