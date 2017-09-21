using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript : MonoBehaviour {

    [SerializeField]
    private GameObject playerOne;
    [SerializeField]
    private GameObject playerTwo;

    private PlayerBehavior playerOneBehavior;
    private PlayerBehavior playerTwoBehavior;

	// Use this for initialization
	void Start () {
        playerOneBehavior = playerOne.GetComponent<PlayerBehavior>();
        playerTwoBehavior = playerTwo.GetComponent<PlayerBehavior>();
    }
	
	// Update is called once per frame
	void Update () {

        /**
         * Player One Movement Controls 
         */
        if (Input.GetKey(KeyCode.W))
        {
            playerOneBehavior.MoveUp();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerOneBehavior.MoveDown();
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerOneBehavior.MoveLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerOneBehavior.MoveRight();
        }

        if (Input.GetKey(KeyCode.E))
        {
            playerOneBehavior.Brake();
        }


        /**
         * Player Two Movement Controls 
         */
        if (Input.GetKey(KeyCode.UpArrow))
        {
            playerTwoBehavior.MoveUp();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            playerTwoBehavior.MoveDown();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerTwoBehavior.MoveLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            playerTwoBehavior.MoveRight();
        }
        if (Input.GetKey(KeyCode.RightControl))
        {
            playerTwoBehavior.Brake();
        }
    }
}
