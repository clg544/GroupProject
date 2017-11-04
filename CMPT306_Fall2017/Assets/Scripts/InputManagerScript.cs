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


	private GameObject doorEntererN;
	private GameObject doorEntererS;
	private GameObject doorEntererW;
	private GameObject doorEntererE;

    private bool isAiming;

	// Use this for initialization
	void Start () {
        playerOneBehavior = playerOne.GetComponent<PlayerBehavior>();
        playerTwoBehavior = playerTwo.GetComponent<PlayerBehavior>();

		doorEntererN = GameObject.FindGameObjectWithTag ("DoorN");
		doorEntererS = GameObject.FindGameObjectWithTag ("DoorS");
		doorEntererW = GameObject.FindGameObjectWithTag ("DoorW");
		doorEntererE = GameObject.FindGameObjectWithTag ("DoorE"); 
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

		if (Input.GetKeyDown (KeyCode.F)) {
			if (doorEntererN != null) {
				doorEntererN.GetComponent<EnterDoor> ().goThroughDoor ();
			}
			if (doorEntererS != null) {
				doorEntererS.GetComponent<EnterDoor> ().goThroughDoor ();
			}
			if (doorEntererW != null) {
				doorEntererW.GetComponent<EnterDoor> ().goThroughDoor ();
			}
			if (doorEntererE != null) {
				doorEntererE.GetComponent<EnterDoor> ().goThroughDoor ();
			}
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
        //float leftHorizontal = Input.GetAxis("Left Horizontal");
       // float leftVertical = Input.GetAxis("Left Vertical");
       // playerOneBehavior.MoveHorizontal(leftHorizontal);
       // playerOneBehavior.MoveVertical(leftVertical);

        /* Aim based on the right stick */
        Vector2 rightStickOrientation = new Vector2(Input.GetAxis("Right Horizontal"), Input.GetAxis("Right Vertical"));

        if (rightStickOrientation.magnitude > 0)
        {
            if (!isAiming)
            {
                isAiming = true;
                //myShooter.PlayerAimStart(rightStickOrientation);
            }
            else
            {
                //myShooter.PlayerAimContinue(rightStickOrientation);
            }
        }
        else if((rightStickOrientation.magnitude <= 0) && (isAiming))
        {
            isAiming = false;
            //myShooter.PlayerAimEnd();
        }


        if (!isAiming)
        {
            if (rightStickOrientation.magnitude > 0)
            {
                isAiming = true;
                //myShooter.PlayerAimStart(rightStickOrientation);
            }
        }
        else
        {

        }


        /* Brake based on the B Button */
        if (Input.GetButton("Xbox B"))
            playerOneBehavior.Brake();
        
        /* Shoot based on Right Trigger */
        if (Input.GetAxis("Right Trigger") < -.3)
        {
           // myShooter.Shoot();
        }

    }

    // Update is called once per frame
    void Update () {
        KeyboardInput();
        JoypadOneInput();
    }
}
