using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParentScript : MonoBehaviour {

    [SerializeField]
    GameObject PlayerOne;
    [SerializeField]
    GameObject PlayerTwo;

    Transform myTansform;
    Transform playerOneTransform;
    Transform playerTwoTransform;

    // Use this for initialization
    void Start () {
        playerOneTransform = PlayerOne.GetComponent<Transform>();
        playerTwoTransform = PlayerTwo.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        /* Move to Players centre */
        this.transform.position = (playerOneTransform.position + playerTwoTransform.position) / 2;
	}
}
