using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherLinks : MonoBehaviour {

    public GameObject myConnection;
    private LineRenderer tetherVisual;

    public void ConnectNodes()
    {
        tetherVisual.SetPosition(0, this.transform.position);
        tetherVisual.SetPosition(1, myConnection.transform.position);
    }

	// Use this for initialization
	void Start () {
        tetherVisual = this.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
