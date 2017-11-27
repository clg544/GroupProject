using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherLinks : MonoBehaviour {

    public PlayerTetherScript tetherManager;    // The parent script
    public int myID;

    public GameObject myConnection;             // What this node is connected to
    private Rigidbody2D myBody;                 // My RigidBody
    private Rigidbody2D connectionBody;         // Connections RigidBody
    public LineRenderer tetherVisual;           // My lineRenderer

    private Vector3 collisionPos;
    public float pullDistance;
    public float tension;

    /* Set lineRenderer to draw from me to my connection */
    public void ConnectNodes()
    {
        if (gameObject.name == "Player One")
            return;

        tetherVisual.SetPosition(0, this.transform.position);
        tetherVisual.SetPosition(1, myConnection.transform.position);
    }

    public float getDistance()
    {
        return Vector3.Distance(gameObject.transform.position, myConnection.transform.position);
    }

    public void UnfreezeNode()
    {
        myBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Awake()
    {
        tetherManager = transform.parent.gameObject.GetComponent<PlayerTetherScript>();
    }
    
    void ApplyDamage(float dam)
    {
        tetherManager.ApplyDamageFromTether(dam, myID);
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        tetherManager.numColls++;

        if (coll.gameObject.tag == "Barrier" || 
            coll.gameObject.tag == "tetherLink")
        {
            return;
        }

        collisionPos = (coll.contacts)[0].point;        // If we have a circle collider, this is safe
        tetherManager.curColl = collisionPos;
    }

    public void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Barrier" || coll.gameObject.tag == "tetherLink")
            return;

        float curDistance = Vector3.Distance(gameObject.transform.position, myConnection.transform.position);
        
        Vector3 collDir = this.transform.position - collisionPos;
            
        myBody.AddForce(collDir.normalized * curDistance * Time.deltaTime);
        connectionBody.AddForce(((-1) * collDir.normalized) * curDistance * tension * Time.deltaTime);
    }

    public void OnCollisionExit2D(Collision2D coll)
    {
        tetherManager.numColls--;
    }  
    
    // Use this for initialization
    void Start() {
        myBody = this.GetComponent<Rigidbody2D>();
        tetherVisual = this.GetComponent<LineRenderer>();
        connectionBody = myConnection.GetComponent<Rigidbody2D>();
        tetherManager = GetComponentInParent<PlayerTetherScript>();
	}

    public void setConnection(GameObject gameOb)
    {
        myConnection = gameOb;
    }
}
