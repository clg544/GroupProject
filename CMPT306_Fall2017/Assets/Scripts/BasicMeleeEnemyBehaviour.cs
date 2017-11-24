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
    private Rigidbody2D rb;
    private int inCombat = 0;
    public MeleeDamage md;
    private GameObject tartget;

    private RaycastHit2D hit;
    // Use this for initialization
    void Start () {
        navQueue = new Queue<GameObject>();
        foreach (GameObject g in navPoints) {
            navQueue.Enqueue(g);
        }

        rb = GetComponent<Rigidbody2D>();
        currentNavPoint = navQueue.Dequeue();
        //md = GetComponentInChildren<MeleeDamage>();
    }

	void FixedUpdate () {
		if (inCombat < 0){ // this should never happen
            Debug.LogError("how the hell did we get here?"); 
        }
        else if (inCombat == 0) { //when not in combat, patrol the nav points. 
            Vector3 dir = currentNavPoint.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Debug.DrawRay(transform.position, (currentNavPoint.transform.position - transform.position ).normalized);
            hit = Physics2D.Raycast(transform.position, ( currentNavPoint.transform.position - transform.position).normalized);
            Debug.Log(hit.transform.gameObject.name);
            if (Vector2.Distance(transform.position, currentNavPoint.transform.position) < navTriggerDistance || hit.transform.gameObject != currentNavPoint) {
                rb.velocity = new Vector2(0,0);
                navQueue.Enqueue(currentNavPoint);
                currentNavPoint = navQueue.Dequeue();
            }
        }
        else if (inCombat > 0){ // in combat

            //Debug.Log("here");
            if (Vector2.Distance(transform.position, fighty.transform.position) < Vector2.Distance(transform.position, shooty.transform.position)){
                tartget = fighty;
            }
            else {
                tartget = shooty;
            }
            Vector3 dir = tartget.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
          //  Debug.Log(tartget.transform.position + ", "+ dir + ", " + angle);
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (!md.winding) {
           rb.AddRelativeForce(new Vector2(0, speed));
           Vector2.ClampMagnitude(rb.velocity, speed);
        }
        else {
            rb.velocity = new Vector2(0,0);
        }
    }
    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player" && inCombat < 2) {
            inCombat++;
        }
    }
    void OnTriggerExit2D(Collider2D col) {
        if (col.tag == "Player" && inCombat > 0){
            inCombat--;
        }
    }
}
