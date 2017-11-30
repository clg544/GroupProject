using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedEnemyBehaviour : MonoBehaviour {
    AudioManager soundOut;
    public GameObject fighty;
    public GameObject shooty;

    public float speed;
    public float spread;
    public int burst;
    public float reloadTime;
    public float rof;
    public int damage;

    private Rigidbody2D rb;
    private LineRenderer lr;
    public GameObject shootPoint;

    public List<GameObject> navPoints;
    public Queue<GameObject> navQueue;
    public float navTriggerDistance;
    private GameObject currentNavPoint;

    private RaycastHit2D shootHit;
    private GameObject target;

    // Use this for initialization
    void Start()
    {
        GameObject[] managers = GameObject.FindGameObjectsWithTag("Manager");

        for (int i = 0; i < managers.Length; i++)
        {
            if (managers[i].name == "SoundManager")
            {
                soundOut = managers[i].GetComponent<AudioManager>();
            }
        }
        
        navQueue = new Queue<GameObject>();
        foreach (GameObject g in navPoints) {
            navQueue.Enqueue(g);
        }

        currentNavPoint = navQueue.Dequeue();
      
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponentInChildren<LineRenderer>();

        StartCoroutine(shoot());
    }

    // Update is called once per frame
    void FixedUpdate() {
        //look at closer of fighty or shooty
        if(fighty != null && shooty != null)
        {
            if (Vector2.Distance(transform.position, fighty.transform.position) < Vector2.Distance(transform.position, shooty.transform.position))
            {
                target = fighty;
            }
            else
            {
                target = shooty;
            }
        }
        else
        {
            return;
        }
        //look at closest player

        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        rb.velocity = (currentNavPoint.transform.position - transform.position).normalized * speed;
        
        //
        if (Vector2.Distance(transform.position, currentNavPoint.transform.position) < navTriggerDistance) {
            navQueue.Enqueue(currentNavPoint);
            currentNavPoint = navQueue.Dequeue();
        }

    }

    IEnumerator shoot() {
        yield return new WaitForSeconds(Random.Range(0, reloadTime));
       // Debug.Log("beginning shooting");
        while (true) {
            //  Debug.Log("shooting");
            for (int i = 0; i < burst; i++)
            {

                //  Debug.Log("shot");

                lr.enabled = true;
                lr.SetPosition(0, shootPoint.transform.position);

                Vector3 miss = Quaternion.AngleAxis(Random.Range(-spread, spread), Vector3.forward) * transform.up;

                shootHit = Physics2D.Raycast(shootPoint.transform.position, miss);
                lr.SetPosition(1, shootHit.point);
                //  Debug.Log(shootHit.transform.tag);

                // Play Shot sound
                soundOut.PlaySound(soundOut.SoundIndex.Ouch);

                if (shootHit.transform.CompareTag("Player") || shootHit.transform.CompareTag("TetherLink"))
                {
                    shootHit.transform.gameObject.SendMessage("ApplyDamage", damage);
                }
                yield return 10;
                lr.enabled = false;

               yield return new WaitForSeconds(rof);
            }
          //  Debug.Log("reloading");
            yield return new WaitForSeconds(reloadTime);
        }
    }
}
