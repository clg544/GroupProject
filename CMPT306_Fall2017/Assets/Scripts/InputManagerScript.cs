/**
 *  Unity InputManager.asset file pulled from https://forum.unity.com/
 *      under "Attribution-ShareAlike 3.0 Unported (CC BY-SA 3.0)"
 *      
 *      Share — copy and redistribute the material in any medium or format
 *      Adapt — remix, transform, and build upon the material
 *          for any purpose, even commercially.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript : MonoBehaviour {

    public string LeftHorizontal;
    public string LeftVertical;
    public string RightHorizontal;
    public string RightVertical;

    public string ButtonX;
    public string ButtonA;
    public string ButtonB;
    public string ButtonY;

    public string LeftBumper;
    public string LeftTrigger;
    public string RightBumper;
    public string RightTrigger;

    /* Names chosen with Xbox controller in mind */
    public struct InputNames
    {
        public string playerSuffix;

        public string LeftHorizontal;
        public string LeftVertical;
        public string RightHorizontal;
        public string RightVertical;

        public string ButtonX;
        public string ButtonA;
        public string ButtonB;
        public string ButtonY;

        public string LeftBumper;
        public string LeftTrigger;
        public string RightBumper;
        public string RightTrigger;
    }


    public GameObject playerOne;
    public GameObject playerTwo;

    private PlayerBehavior playerOneBehavior;
    private PlayerBehavior playerTwoBehavior;

    private PlayerCombat playerOneCombat;
    private PlayerCombat playerTwoCombat;

    public InputNames P1Input;
    public InputNames P2Input;

    private bool P1isAiming;
    private bool P2isAiming;

    private GameObject doorEntererN;
    private GameObject doorEntererS;
    private GameObject doorEntererE;
    private GameObject doorEntererW;



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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (doorEntererN != null) {
                doorEntererN.GetComponent<EnterDoor>().goThroughDoor();
            }
            if (doorEntererS != null) {
                doorEntererS.GetComponent<EnterDoor>().goThroughDoor();
            }
            if (doorEntererW != null) {
                doorEntererW.GetComponent<EnterDoor>().goThroughDoor();
            }
            if (doorEntererE != null) {
                doorEntererE.GetComponent<EnterDoor>().goThroughDoor();
            }
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
        if (Input.GetKeyDown(KeyCode.Keypad9))
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

    public void JoypadInput(InputNames myInput, PlayerBehavior myBehavior, PlayerCombat myCombat)
    {
        /* Movement based on left stick */
        float leftHorizontal = Input.GetAxis(myInput.LeftHorizontal);
        float leftVertical = Input.GetAxis(myInput.LeftVertical);
        myBehavior.MoveHorizontal(leftHorizontal);
        myBehavior.MoveVertical(leftVertical);

        /* Aim based on the right stick */
        Vector2 rightStickOrientation = new Vector2(Input.GetAxis(myInput.RightHorizontal), Input.GetAxis(myInput.RightVertical));
        
        switch (myInput.playerSuffix)
        {
            case "_1":
                if (rightStickOrientation.magnitude > 0)
                {
                    if (rightStickOrientation.magnitude > 0)
                    {
                        if (!P1isAiming)
                        {
                            P1isAiming = true;
                            playerOneCombat.PlayerAimStart(rightStickOrientation);
                        }
                        else
                        {
                            playerOneCombat.PlayerAimContinue(rightStickOrientation);
                        }
                    }
                    else if ((rightStickOrientation.magnitude <= 0) && (P1isAiming))
                    {
                        P1isAiming = false;
                        playerOneCombat.PlayerAimEnd();
                    }
                }
                break;

            case "_2":
                if (rightStickOrientation.magnitude > 0)
                {
                    if (rightStickOrientation.magnitude > 0)
                    {
                        if (!P2isAiming)
                        {
                            P2isAiming = true;
                            playerTwoCombat.PlayerAimStart(rightStickOrientation);
                        }
                        else
                        {
                            playerTwoCombat.PlayerAimContinue(rightStickOrientation);
                        }
                    }
                    else if ((rightStickOrientation.magnitude <= 0) && (P2isAiming))
                    {
                        P2isAiming = false;
                        playerTwoCombat.PlayerAimEnd();
                    }
                }
                break;
        }
        
        if (Input.GetButton(myInput.LeftBumper))
        {
            myBehavior.Brake();
        }
        if (Input.GetButton(myInput.ButtonY))
        {
            myCombat.SwitchWeapon();
        }
        if (Input.GetButton(myInput.RightBumper))
        {
            myCombat.Shoot();
        }
    }

    private void PrintInputNames(InputNames i)
    {
        print(i.playerSuffix);

        print(i.LeftHorizontal);
        print(i.LeftVertical);
        print(i.RightHorizontal);
        print(i.RightVertical);

        print(i.ButtonX);
        print(i.ButtonA);
        print(i.ButtonB);
        print(i.ButtonY);

        print(i.LeftBumper);
        print(i.LeftTrigger);
        print(i.RightBumper);
        print(i.RightTrigger);
    }

    public InputNames SetUpInputNames(string suffix)
    {
        InputNames input = new InputNames();

        input.playerSuffix = suffix;

        input.LeftHorizontal = "" + this.LeftHorizontal + suffix;
        input.LeftVertical = "" + this.LeftVertical + suffix;
        input.RightHorizontal = "" + this.RightHorizontal + suffix;
        input.RightVertical = "" + this.RightVertical + suffix;
        input.ButtonX = "" + this.ButtonX + suffix;
        input.ButtonA = "" + this.ButtonA + suffix;
        input.ButtonB = "" + this.ButtonB + suffix;
        input.ButtonY = "" + this.ButtonY + suffix;
        input.LeftBumper = "" + this.LeftBumper + suffix;
        input.LeftTrigger = "" + this.LeftTrigger + suffix;
        input.RightBumper= "" + this.RightBumper + suffix;
        input.RightTrigger = "" + this.RightTrigger + suffix;

        return input;
}

    // Use this for initialization
    void Start()
    {
        playerOneBehavior = playerOne.GetComponent<PlayerBehavior>();
        playerTwoBehavior = playerTwo.GetComponent<PlayerBehavior>();

        playerOneCombat = playerOne.GetComponent<PlayerCombat>();
        playerTwoCombat = playerTwo.GetComponent<PlayerCombat>();

        P1Input = SetUpInputNames("_1");
        P2Input = SetUpInputNames("_2");

        doorEntererN = GameObject.FindGameObjectWithTag("DoorN");
        doorEntererS = GameObject.FindGameObjectWithTag("DoorS");
        doorEntererE = GameObject.FindGameObjectWithTag("DoorE");
        doorEntererW = GameObject.FindGameObjectWithTag("DoorW");   
    }


    // Update is called once per frame
    void Update () {
        KeyboardInput();
        
        JoypadInput(P1Input, playerOneBehavior, playerOneCombat);
        JoypadInput(P2Input, playerTwoBehavior, playerTwoCombat);
    }
}
