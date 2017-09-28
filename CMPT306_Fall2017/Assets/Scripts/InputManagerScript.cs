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

    private PlayerShooter myShooter;

	// Use this for initialization
	void Start () {
        playerOneBehavior = playerOne.GetComponent<PlayerBehavior>();
        playerTwoBehavior = playerTwo.GetComponent<PlayerBehavior>();

        myShooter = playerOne.GetComponent<PlayerShooter>();
    }


    private void KeyboardInput()
    {
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

    public void JoypadOneInput()
    {
        /* Movement based on left stick */
        float leftHorizontal = Input.GetAxis("Left Horizontal");
        float leftVertical = Input.GetAxis("Left Vertical");
        playerOneBehavior.MoveHorizontal(leftHorizontal);
        playerOneBehavior.MoveVertical(leftVertical);

        /* Aiming based on right stick 
        float rightHorizontal = Input.GetAxis("Right Horizontal");
        float rightVertical = Input.GetAxis("Right Vertical");*/

        Vector2 rightStickOrientation = new Vector2(Input.GetAxis("Right Horizontal"), Input.GetAxis("Right Vertical"));

        if (rightStickOrientation.magnitude > 0)
            myShooter.PlayerAim(rightStickOrientation);


        if (Input.GetButton("Xbox B"))
            playerOneBehavior.Brake();

    }

    // Update is called once per frame
    void Update () {
        KeyboardInput();

        JoypadOneInput();
    }
}
