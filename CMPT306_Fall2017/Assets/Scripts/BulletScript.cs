using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public float damage;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Enemy")
        {
            coll.gameObject.SendMessage("ApplyDamage", damage);
        }

        if(coll.gameObject.tag != "Untagged")
        {
            Destroy(gameObject);
        }


    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
