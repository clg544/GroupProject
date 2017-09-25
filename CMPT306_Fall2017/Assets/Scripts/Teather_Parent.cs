using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teather_Parent : MonoBehaviour {

    public int health { get; private set; }
    public int startingHealth;

	// Use this for initialization 
	void Start () {
        health = startingHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   /// <summary>
   /// handles recieved damage message and applies damage to the teather
   /// </summary>
   /// <param name="damage"> the amount of damage to apply </param>
    void damage(int damage)
    {
        health -= damage;
    }
}
