using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashInput : MonoBehaviour {

	public string easyScene;
	public string AButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.T)) {
			SceneManager.LoadScene(easyScene);
		}
	
		if (Input.GetKeyDown("joystick button 0")) {
			SceneManager.LoadScene(easyScene);
		}
	}
}
