using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespondToDamage : MonoBehaviour {
    
    public float toggleTime;            // How long each blink lasts
    public float blinkTime;                    // How long to toggle for
    
    float curBlinkTime;

    SpriteRenderer mySprite;
        
    public void StartBlink()
    {
        curBlinkTime = blinkTime;
    }

	// Use this for initialization
	void Start () {
        mySprite = gameObject.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (curBlinkTime > 0)
        {
            mySprite.enabled = (((int)(((curBlinkTime / toggleTime) % 2))) == 0) ? false : true;
            
            curBlinkTime -= Time.deltaTime;
        }
        else
        {
            mySprite.enabled = true;
        }
	}
}
