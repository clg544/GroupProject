using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTetherScript : MonoBehaviour {
    
    /* How far the players can go before the tether pulls them in */
    [SerializeField]
    private float engageDistance;
    [SerializeField]
    private float distanceMultiplier;
    private bool taught;

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
    private GameObject[] tetherLinks;
    public int tetherResolution;
    private LineRenderer tetherVisual;
    [SerializeField]
    private float linkSpeed;
    [SerializeField]
    private GameObject linkNode;

    /* Draw the tether as a line between the two players */
    public void DrawTetherAsLine()
    {
        tetherVisual.SetPosition(0, playerOne.transform.position);
        tetherVisual.SetPosition(1, playerTwo.transform.position);
    }

    public Vector3 getCentre()
    {
        return (playerOne.transform.position + playerTwo.transform.position) / 2;
    }

    public void distributeNodes()
    {
        Vector3 diff = (playerTwo.transform.position - playerOne.transform.position) / (tetherLinks.Length + 1);

        int i = 1;
        foreach (GameObject link in tetherLinks)
        {
            link.transform.position = playerOne.transform.position + (diff * i);
            //link.GetComponent<Rigidbody2D>().velocity = Vector3.MoveTowards(link.transform.position, playerOne.transform.position + (diff * i), linkSpeed);
            i++;
        }
    }

    // Use this for initialization
    void Awake () {
        playerOneBody = playerOne.GetComponent<Rigidbody2D>();
        playerTwoBody = playerTwo.GetComponent<Rigidbody2D>();

        tetherLinks = new GameObject[tetherResolution];

        for(int i = 0; i < tetherResolution; i++)
        {
            tetherLinks[i] = Instantiate(linkNode, gameObject.transform);
        }
        distributeNodes();

        /* Set the links targets, with special cases for the end links */
        tetherLinks[0].GetComponent<DistanceJoint2D>().connectedBody = playerOneBody;
        tetherLinks[0].GetComponent<TetherLinks>().setConnection(playerOne);
        for (int i = 1; i < tetherLinks.Length; i++)
        {
            tetherLinks[i].GetComponent<DistanceJoint2D>().connectedBody = tetherLinks[i - 1].GetComponent<Rigidbody2D>();
            tetherLinks[i].GetComponent<TetherLinks>().setConnection(tetherLinks[i-1]);
        }
        playerTwo.GetComponent<DistanceJoint2D>().connectedBody = tetherLinks[tetherLinks.Length - 1].GetComponent<Rigidbody2D>();
        playerTwo.GetComponent<TetherLinks>().setConnection(tetherLinks[tetherLinks.Length - 1]);
        
        tetherVisual = this.GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        bool curTaught = (curDistance > engageDistance);
        
        // Get current distance
        Vector3 posOne = playerOne.transform.position;
        Vector3 posTwo = playerTwo.transform.position;

        // Calculate vector of p1 to p2
        Vector3 difference = posOne - posTwo;
        // curdistance = Magnitide of difference
        curDistance = Vector3.Distance(posOne, posTwo);

        /* if The players are far enough apart... */
        if (curTaught)
        {
            float distance = curDistance - engageDistance;
            /* Apply force of Distance squared, Scaled by elasticity, and divide among both players 
             *
             * I used distance squared to make the tether more responsive, though less realistic. - Clint
             */
            float tension = (elasticity * (distance * distanceMultiplier)) / 2;

            /* Vector difference gets magnitide = tension */
            difference = Vector3.ClampMagnitude(difference, tension);

            /* Apply said Vector to both players */
            playerOneBody.AddForce(-difference);
            playerTwoBody.AddForce(difference);
        }

        BroadcastMessage("ConnectNodes");
        playerTwo.SendMessage("ConnectNodes");

        taught = curTaught;
	}

}
