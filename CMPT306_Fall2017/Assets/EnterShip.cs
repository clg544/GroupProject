using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterShip : MonoBehaviour {

    bool canWin;
	ScoreManager sm;
    Text HudInteract;
    InputManagerScript ims;

    // Use this for initialization
    void Start () {
		sm = GameObject.FindObjectOfType<ScoreManager> ();
        canWin = false;
        
        ims = GameObject.FindObjectOfType<InputManagerScript>();

        GameObject[] hud = GameObject.FindGameObjectsWithTag("HUD");
        for (int i = 0; i < hud.Length; i++)
        {
            if (hud[i].name == "InteractPrompt")
            {
                HudInteract = hud[i].GetComponent<Text>();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {


        if (ims.enteringShip && canWin)
        {
            SceneManager.LoadScene("Win");
            Debug.Log("You Win");
        }
    }

	void OnTriggerEnter2D(Collider2D coll){

        if (coll.gameObject.tag == "Player")
        {
            if (sm.curScore > 100)
            {
                canWin = true;
                HudInteract.enabled = true;
                HudInteract.text = "A: Take Off!";
            }
        }
	}

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            canWin = false;
            HudInteract.enabled = false;
        }
    }
}
