using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    private Rigidbody2D myBody;
    public float damage;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Enemy")
        {
            coll.gameObject.SendMessage("ApplyDamage", damage);
            coll.gameObject.GetComponent<Rigidbody2D>().AddForce(myBody.velocity * myBody.mass);
        }

        if(coll.gameObject.tag != "Untagged")
        {
            Destroy(gameObject);
        }
        
        coll.gameObject.SendMessage("ApplyDamage", damage);
    }

	// Use this for initialization
	void Start () {
        myBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
