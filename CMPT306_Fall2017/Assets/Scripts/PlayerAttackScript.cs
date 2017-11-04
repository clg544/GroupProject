﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour {

    public float elapsedTime;
    public float lifetime;
    public float attackDamage;

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            coll.gameObject.SendMessage("ApplyDamage", attackDamage);
            Debug.Log("I Dealt Damage" + attackDamage);
        }
    }

	// Use this for initialization
	void Start () {
        elapsedTime = 0;
	}

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > lifetime)
            Destroy(gameObject);
    }

    void setLifetime(float seconds)
    {
        lifetime = seconds;
    }
}
