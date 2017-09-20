using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    [SerializeField]
    private float Acceleration;
    /* Movement to apply this frame */
    private Vector3 Movement;


    public GameObject player;
    private Rigidbody2D playerBody;



    public void MoveLeft()
    {
        Movement.x -= Acceleration;
    }

    public void MoveRight()
    {
        Movement.x += Acceleration;
    }

    public void MoveUp()
    {
        Movement.y += Acceleration;
    }

    public void MoveDown()
    {
        Movement.y -= Acceleration;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        playerBody.AddForce(Vector3.ClampMagnitude(Movement, Acceleration));
	}
}
