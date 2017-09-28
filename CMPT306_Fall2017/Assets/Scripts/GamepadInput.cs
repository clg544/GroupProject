using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadInput : MonoBehaviour {

    [SerializeField]
    float Deadzone;
    float maxComparable;
    
    [SerializeField]
    PlayerBehavior myPlayer;

	// Use this for initialization
	void Start () {
        maxComparable = 1 - Deadzone;
	}
	
	// Update is called once per frame
	void Update () {
        float leftX = Input.GetAxis("Left Horizontal");
        float leftY = Input.GetAxis("Left Vertical");
        
        /* Ratio of joystick amount over the deadzone */
        myPlayer.MoveHorizontal(leftX);
        myPlayer.MoveVertical(leftY);

        if (Input.GetButton("Xbox B"))
            myPlayer.Brake();
    }
}
