using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public int currentHealth;
    public int maxHealth;

    /**
     * Damage this enemy
     */
    public void ApplyDamage(int dam)
    {
        currentHealth -= dam;

        if (currentHealth < 0)
            Kill();
    }

    /** 
     * Kill this enemy
     */
    public void Kill()
    {
        Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        currentHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
