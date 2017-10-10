using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teather_Child : MonoBehaviour {

    public GameObject left;
    public GameObject right;

	// Use this for initialization 
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = new Vector2(Vector2.Distance(left.transform.position, right.transform.position), transform.localScale.y);
	}
}
