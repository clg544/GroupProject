using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	private float inputDirection;
	private float inputHeight;
	private float verticalVelocity;
	private bool doubleJump = false;

	private float speed = 5.0f;
	private float gravity= 0.5f;

	private Vector3 moveVector;
	private CharacterController controller;

	void Start () {
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		inputDirection = Input.GetAxis ("Horizontal") * speed;

		if (controller.isGrounded) {

			verticalVelocity = 0;

			if (Input.GetKeyDown (KeyCode.Space)) {
				verticalVelocity = 10;
				doubleJump = true;
			}
		} else {
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (doubleJump) {
					verticalVelocity = 10;
					doubleJump = false;
				}
			}
			verticalVelocity -= gravity;
		}
		moveVector = new Vector3 (inputDirection, verticalVelocity, 0);
		controller.Move (moveVector * Time.deltaTime);
	}
}

