using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour {
    AudioManager soundOut;

    public float coolDownTime;
    public float windUpTime;
    public int damage;
    public bool winding = false;

    private LinkedList<GameObject> inCombatWith;

    void Awake()
    {
        GameObject[] managers = GameObject.FindGameObjectsWithTag("Manager");

        for (int i = 0; i < managers.Length; i++)
        {
            if (managers[i].name == "SoundManager")
            {
                soundOut = managers[i].GetComponent<AudioManager>();
            }
        }
    }



    // Use this for initialization
    void Start() {
        inCombatWith = new LinkedList<GameObject>();
        StartCoroutine(swing());
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player" && !inCombatWith.Contains(collision.gameObject)) {
            inCombatWith.AddFirst(collision.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player" && inCombatWith.Contains(collision.gameObject)) {
            inCombatWith.Remove(collision.gameObject);
        }
    }

    IEnumerator swing() {
        while (true) {
            if (inCombatWith.Count == 0) {
                yield return 5; //wait 5 frames and then check again
            }
            else {
                winding = true;
                //send windup animation
                yield return new WaitForSeconds(windUpTime);
                foreach (GameObject g in inCombatWith) {
                    soundOut.PlaySound(soundOut.SoundIndex.Brrrr);
                    g.SendMessage("ApplyDamage", damage);
                }
                winding = false;
                yield return new WaitForSeconds(coolDownTime);
            }
        }
    }
}
