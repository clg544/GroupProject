using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript : MonoBehaviour {
    
    public GameObject playerOne;
    public GameObject playerTwo;

    private PlayerBehavior playerOneBehavior;
    private PlayerBehavior playerTwoBehavior;

    private PlayerCombat playerOneCombat;
    private PlayerCombat playerTwoCombat;

    private bool P1isAiming;
    private bool P2isAiming;

    // Use this for initialization
    void Start () {
        playerOneBehavior = playerOne.GetComponent<PlayerBehavior>();
        playerTwoBehavior = playerTwo.GetComponent<PlayerBehavior>();

        playerOneCombat = playerOne.GetComponent<PlayerCombat>();
        playerTwoCombat = playerTwo.GetComponent<PlayerCombat>();
    }
    
    private void KeyboardInput()
    {
        float tempHor;
        float tempVer;

        /**
         * Player One Movement Controls 
         * 
         * WASD - Move
         * E    - Brake
         * TFGH - Aim
         * Space- Shoot
         * 
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

        /* Aim based on the tfgh */
        tempHor = (Input.GetKey(KeyCode.H) ? 1 : 0) - (Input.GetKey(KeyCode.F) ? 1 : 0);
        tempVer = (Input.GetKey(KeyCode.T) ? 1 : 0) - (Input.GetKey(KeyCode.G) ? 1 : 0);
        Vector2 P1AimAxis = new Vector2(tempHor, tempVer);
        if (P1AimAxis.magnitude > 0)
        {
            if (!P1isAiming)
            {
                P1isAiming = true;
                playerOneCombat.PlayerAimStart(P1AimAxis);
            }
            else
            {
                playerOneCombat.PlayerAimContinue(P1AimAxis);
            }
        }
        else if ((P1AimAxis.magnitude <= 0) && (P1isAiming))
        {
            P1isAiming = false;
            playerOneCombat.PlayerAimEnd();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            playerOneCombat.FighterAttack();
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
        if (Input.GetKey(KeyCode.Keypad9))
        {
            playerTwoCombat.SwitchWeapon();
        }

        /* Aim based on the 8456 */
        tempHor = (Input.GetKey(KeyCode.Keypad6) ? 1 : 0) - (Input.GetKey(KeyCode.Keypad4) ? 1 : 0);
        tempVer = (Input.GetKey(KeyCode.Keypad8) ? 1 : 0) - (Input.GetKey(KeyCode.Keypad5) ? 1 : 0);
        Vector2 P2AimAxis = new Vector2(tempHor, tempVer);
        if (P2AimAxis.magnitude > 0)
        {
            if (!P2isAiming)
            {
                P2isAiming = true;
                playerTwoCombat.PlayerAimStart(P2AimAxis);
            }
            else
            {
                playerTwoCombat.PlayerAimContinue(P2AimAxis);
            }
        }
        else if ((P2AimAxis.magnitude <= 0) && (P2isAiming))
        {
            P2isAiming = false;
            playerTwoCombat.PlayerAimEnd();
        }

        if (Input.GetKey(KeyCode.Keypad0))
        {
            playerTwoCombat.Shoot();
        }
    }

    public void JoypadOneInput()
    {
        /* Movement based on left stick */
        float leftHorizontal = Input.GetAxis("P1 Left Stick H");
        float leftVertical = Input.GetAxis("P1 Left Stick V");
        playerOneBehavior.MoveHorizontal(leftHorizontal);
        playerOneBehavior.MoveVertical(leftVertical);

        /* Aim based on the right stick */
        Vector2 rightStickOrientation = new Vector2(Input.GetAxis("P1 Right Stick H"), Input.GetAxis("P1 Right Stick V"));

        if (rightStickOrientation.magnitude > 0)
        {
            if (!P1isAiming)
            {
                P1isAiming = true;
                playerTwoCombat.PlayerAimStart(rightStickOrientation);
            }
            else
            {
                playerTwoCombat.PlayerAimContinue(rightStickOrientation);
            }
        }
        else if((rightStickOrientation.magnitude <= 0) && (P1isAiming))
        {
            P1isAiming = false;
            playerTwoCombat.PlayerAimEnd();
        }


        if (!P1isAiming)
        {
            if (rightStickOrientation.magnitude > 0)
            {
                P1isAiming = true;
                playerTwoCombat.PlayerAimStart(rightStickOrientation);
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
            playerOneCombat.Shoot();
        }

    }

    // Update is called once per frame
    void Update () {
        KeyboardInput();
        //JoypadOneInput();
    }
}
