using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    /* Movement to apply this frame */
    private Vector3 frameMovement;
    
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float brakeFraction;

    [SerializeField]
    private float maxSpeed;

    public GameObject player;
    private Rigidbody2D playerBody;



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
        playerBody.AddForce(Vector3.ClampMagnitude(frameMovement, acceleration));

        if(playerBody.velocity.magnitude > maxSpeed)
        {
            playerBody.velocity = (playerBody.velocity.normalized * maxSpeed);
        }

        frameMovement = Vector3.zero;
	}
}
