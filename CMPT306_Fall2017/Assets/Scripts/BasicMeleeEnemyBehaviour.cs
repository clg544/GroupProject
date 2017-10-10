using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeEnemyBehaviour : MonoBehaviour {

    public GameObject fighty;
    public GameObject shooty;

    public int maxHealth;
    public float speed;
    public List<GameObject> navPoints;
    public Queue<GameObject> navQueue;
    public float navTriggerDistance;

    private GameObject currentNavPoint;
    private int curentHealth;
    private Rigidbody2D rb;
    private int inCombat = 0;

	// Use this for initialization
	void Start () {
        navQueue = new Queue<GameObject>();
        foreach (GameObject g in navPoints) {
            navQueue.Enqueue(g);
        }
        rb = GetComponent<Rigidbody2D>();
        currentNavPoint = navQueue.Dequeue();
       
    }
	void FixedUpdate () {
		if (inCombat < 0){ // this should never happen
            Debug.LogError("how the hell did we get here?"); 
        }
        if (inCombat == 0) { //when not in combat, patrol the nav points. 
            Vector3 dir = currentNavPoint.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
          

            if (Vector2.Distance(transform.position, currentNavPoint.transform.position) < navTriggerDistance) {
                rb.velocity = new Vector2(0,0);
                navQueue.Enqueue(currentNavPoint);
                currentNavPoint = navQueue.Dequeue();
            }
        }
        if (inCombat > 0){ // in combat

        }
        rb.AddRelativeForce(new Vector2(0, speed));
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);

    }
    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            inCombat++;
        }
    }
    void OnTriggerExit2D(Collider2D col) {
        if (col.tag == "Player"){
            inCombat--;
        }
    }
}
