using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTetherScript : MonoBehaviour {
    
    /* How far the players can go before the tether pulls them in */
    [SerializeField]
    private float engageDistance;       // Distance where players get pulled in
    [SerializeField]
    private float distanceMultiplier;   // Scaler to pull players in by

    /* Hooke's law labels this k, how much force the tether applies */
    [SerializeField]
    private float elasticity;           // Constant tether force, scaled

    /* The current distance between the two players */
    private float curDistance;
    
    /* Our Players */
    [SerializeField]
    private GameObject playerOne;       
    [SerializeField]
    private GameObject playerTwo;
    private Rigidbody2D playerOneBody;
    private Rigidbody2D playerTwoBody;

    /* Resource to represent the tether as a line */
    private GameObject[] tetherLinks;   // All the tether's child links
    private TetherLinks[] tetherScripts;// Scripts for the tethers
    public int tetherResolution;        // How many nodes to use
    [SerializeField]
    private GameObject linkNode;        // Link prefab to clone

    public float freezeDistance;        // Child's frozen distance
    private bool frozen;                // a pair of nodes passed a distance threshold
    private GameObject whoFroze;        // the node who froze the tether 

    /* Get the centre of the players */
    public Vector3 getCentre()
    {
        return (playerOne.transform.position + playerTwo.transform.position) / 2;
    }

    /* Distribute each node in tetherLinks[] in an equidistant distribution */
    public void distributeNodes()
    {
        Vector3 diff = (playerTwo.transform.position - playerOne.transform.position) / (tetherLinks.Length + 1);

        int i = 1;
        foreach (GameObject link in tetherLinks)
        {
            link.transform.position = playerOne.transform.position + (diff * i);
            i++;
        }
    }

    /* A node hit something pointy, so freeze offending node untin they figure it out */
    public void Freeze(GameObject freezer)
    {
        if (!(frozen))
        {
            Rigidbody2D offenderBody = freezer.GetComponent<Rigidbody2D>();

            frozen = true;
            whoFroze = freezer;

            offenderBody.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
    public void Unfreeze()
    {
        if (frozen)
        {
            whoFroze.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

            frozen = false;
            whoFroze = null;

        }
    }

    /* Set up via Awake to prepare for child nodes */
    void Awake () {
        /* Prepare players */
        playerOneBody = playerOne.GetComponent<Rigidbody2D>();
        playerTwoBody = playerTwo.GetComponent<Rigidbody2D>();

        /* Create & populate child link nodes */
        tetherLinks = new GameObject[tetherResolution];
        tetherScripts = new TetherLinks[tetherResolution];
        for(int i = 0; i < tetherResolution; i++)
        {
            tetherLinks[i] = Instantiate(linkNode, gameObject.transform);
            tetherScripts[i] = tetherLinks[i].GetComponent<TetherLinks>();
        }
        distributeNodes();

        /* Set the links targets to prev node, with special cases for the end links */
        tetherLinks[0].GetComponent<DistanceJoint2D>().connectedBody = playerOneBody;
        tetherLinks[0].GetComponent<TetherLinks>().setConnection(playerOne);
        for (int i = 1; i < tetherLinks.Length; i++)
        {
            tetherLinks[i].GetComponent<DistanceJoint2D>().connectedBody = tetherLinks[i - 1].GetComponent<Rigidbody2D>();
            tetherLinks[i].GetComponent<TetherLinks>().setConnection(tetherLinks[i-1]);
        }
        playerTwo.GetComponent<DistanceJoint2D>().connectedBody = tetherLinks[tetherLinks.Length - 1].GetComponent<Rigidbody2D>();
        playerTwo.GetComponent<TetherLinks>().setConnection(tetherLinks[tetherLinks.Length - 1]);
    }
	
	// Update is called once per frame
	void Update () {
        // Get current distance between players
        Vector3 posOne = playerOne.transform.position;
        Vector3 posTwo = playerTwo.transform.position;

        Vector3 difference;

        if (!(frozen))
        {
            // Calculate vector of p1 to p2
            difference = posOne - posTwo;

            // curdistance = Magnitide of difference
            curDistance = Vector3.Distance(posOne, posTwo);
        }
        else
        {
            Vector3 posFrz = whoFroze.transform.position;

            // Calculate vector of p1 to p2
            difference = posOne - posTwo - posFrz;

            // curdistance = Magnitide of difference
            curDistance = Vector3.Distance(posOne, posFrz) + Vector3.Distance(posTwo, posFrz);
        }

        /* if The players are too far apart... */
        if (curDistance > engageDistance)
        {
            float distance = curDistance - engageDistance;

            /* Apply force of Distance squared, Scaled by elasticity, and divide among both players */
            float tension = (elasticity * (distance * distanceMultiplier)) / 2;

            /* Vector difference gets magnitide = tension */
            difference = Vector3.ClampMagnitude(difference, tension);

            /* Apply said Vector to both players */
            playerOneBody.AddForce(-difference);
            playerTwoBody.AddForce(difference);
        }

        /* Tell all nodes to render a line */ 
        BroadcastMessage("ConnectNodes");
        playerTwo.SendMessage("ConnectNodes");

        int overDist = 0;
        if (frozen)
        {
            foreach(TetherLinks tl in tetherScripts)
            {
                if(tl.getDistance() > tl.freezeDistance)
                {
                    overDist++;
                }
            }

            if(overDist <= 2)
            {
                Unfreeze();
            }
        }
	}

}
