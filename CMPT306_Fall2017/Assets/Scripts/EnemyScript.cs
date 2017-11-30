using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    AudioManager soundOut;

    public int currentHealth;
    public int maxHealth;

    /**
     * Damage this enemy
     */
    public void ApplyDamage(int dam)
    {
        currentHealth -= dam;

        if (currentHealth <= 0)
        {
            soundOut.PlaySound(soundOut.SoundIndex.Pop_2);
            Kill();
        }
        else
        {
            SendMessage("StartBlink");
        }
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

        GameObject[] managers = GameObject.FindGameObjectsWithTag("Manager");

        for (int i = 0; i < managers.Length; i++)
        {
            if (managers[i].name == "SoundManager")
            {
                soundOut = managers[i].GetComponent<AudioManager>();
            }
        }

        currentHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}