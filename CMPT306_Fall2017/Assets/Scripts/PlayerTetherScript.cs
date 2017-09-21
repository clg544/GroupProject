using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTetherScript : MonoBehaviour {
    
    /* How far the players can go before the tether pulls them in */
    [SerializeField]
    private float engageDistance;

    /* Hooke's law labels this k, how much force the tether applies */
    [SerializeField]
    private float elasticity;

    /* The current distance between the two players */
    [SerializeField]
    private float curDistance;


    /* Our Players */
    [SerializeField]
    private GameObject playerOne;
    [SerializeField]
    private GameObject playerTwo;
    
    private Rigidbody2D playerOneBody;
    private Rigidbody2D playerTwoBody;


    /* Resource to represent the tether as a line */
    LineRenderer tetherVisual;

    /* Draw the tether as a line between the two players */
    public void DrawTetherAsLine()
    {
        tetherVisual.SetPosition(0, playerOne.transform.position);
        tetherVisual.SetPosition(1, playerTwo.transform.position);
    }

    
    // Use this for initialization
    void Start () {
        playerOneBody = playerOne.GetComponent<Rigidbody2D>();
        playerTwoBody = playerTwo.GetComponent<Rigidbody2D>();

        tetherVisual = this.GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {

        // Get current distance
        Vector3 posOne = playerOne.transform.position;
        Vector3 posTwo = playerTwo.transform.position;

        // Calculate vector of p1 to p2
        Vector3 difference = posOne - posTwo;
        // curdistance = Magnitide of difference
        curDistance = Vector3.Distance(posOne, posTwo);
        
        /* if The players are far enough apart... */
        if (curDistance > engageDistance)
        {
            /* Apply force of Distance, Scaled by elasticity, and divide among both players */
            float tension = (elasticity * (curDistance - engageDistance)) / 2;

            /* Vector difference gets magnitide = tension */
            difference = Vector3.ClampMagnitude(difference, tension);

            /* Apply said Vector to both players */
            playerOneBody.AddForce(-difference);
            playerTwoBody.AddForce(difference);
        }

        /* Draw the tether */
        DrawTetherAsLine();
	}

}
