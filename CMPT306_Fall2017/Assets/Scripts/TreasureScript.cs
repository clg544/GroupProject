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

    void Awake()
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

        int randCol = Random.Range(1, 6);

        /* Choose a color via rand + copypasta */
        switch (randCol)
        {
            case 1:
                mySprite.color = Color.red;
                break;
            case 2:
                mySprite.color = Color.black;
                break;
            case 3:
                mySprite.color = Color.magenta;
                break;
            case 4:
                mySprite.color = Color.yellow;
                break;
            case 5:
                mySprite.color = Color.white;
                break;
        }

        worth = randCol * 3;
    }
    
	// Update is called once per frame
	void Update () {
		
	}
}
