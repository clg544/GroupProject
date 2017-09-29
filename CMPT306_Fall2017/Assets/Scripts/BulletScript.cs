using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag != "Untagged")
        {
            Destroy(gameObject);
        }
        Debug.Log(coll.tag);

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
