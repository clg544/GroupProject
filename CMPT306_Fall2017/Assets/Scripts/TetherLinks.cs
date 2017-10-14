using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherLinks : MonoBehaviour {

    public PlayerTetherScript tetherManager;    // The parent script

    public GameObject myConnection;             // What this node is connected to
    private Rigidbody2D myBody;                 // My RigidBody
    private Rigidbody2D connectionBody;         // Connections RigidBody
    private LineRenderer tetherVisual;          // My lineRenderer

    public float freezeDistance;

    /* Set lineRenderer to draw from me to my connection */
    public void ConnectNodes()
    {
        tetherVisual.SetPosition(0, this.transform.position);
        tetherVisual.SetPosition(1, myConnection.transform.position);
    }

    public float getDistance()
    {
        return Vector3.Distance(gameObject.transform.position, myConnection.transform.position);
    }

    void Awake()
    {
        tetherManager = transform.parent.gameObject.GetComponent<PlayerTetherScript>();
    }

	// Use this for initialization
	void Start() {
        myBody = this.GetComponent<Rigidbody2D>();
        tetherVisual = this.GetComponent<LineRenderer>();
        connectionBody = myConnection.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        float curDistance = Vector3.Distance(gameObject.transform.position, myConnection.transform.position);

        if(curDistance > freezeDistance)
        {
            if (tetherManager != null)
                tetherManager.Freeze(gameObject);
        }
	}

    public void setConnection(GameObject gameOb)
    {
        myConnection = gameOb;
    }
}
