using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public int curScore;
    public Text scoreText;

    public void AddToScore(int num)
    {
        curScore += num;
        scoreText.text = "$" + curScore;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
