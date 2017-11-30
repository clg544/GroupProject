using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureScript : MonoBehaviour {

    SpriteRenderer mySprite;

    public int worth;
    public ScoreManager myScoreManager;
    
    public void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.transform.tag == "Player")
        {
            myScoreManager.AddToScore(worth);
            Destroy(gameObject);
        }
    }


    // Use this for initialization
    void Start()
    {
        GameObject[] managers = GameObject.FindGameObjectsWithTag("Manager");

        for (int i = 0; i < managers.Length; i++)
        {
            if (managers[i].name == "Managers")
            {
                myScoreManager = managers[i].GetComponent<ScoreManager>();
            }
        }

        mySprite = gameObject.GetComponent<SpriteRenderer>();

        int randCol = Random.Range(0, 13);

        /* Choose a color via rand + copypasta */
        switch (randCol)
        {
            case 0:
                mySprite.color = Color.red;
                break;
            case 1:
                mySprite.color = Color.black;
                break;
            case 2:
                mySprite.color = Color.blue;
                break;
            case 3:
                mySprite.color = Color.cyan;
                break;
            case 4:
                mySprite.color = Color.green;
                break;
            case 5:
                mySprite.color = Color.magenta;
                break;
            case 6:
                mySprite.color = Color.yellow;
                break;
            case 7:
                mySprite.color = Color.white;
                break;
        }

        worth = randCol;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
