using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTetherScript : MonoBehaviour {

    public MenuManager curMenuManager;

    /* How far the players can go before the tether pulls them in */
    public float engageDistance;       // Distance where players get pulled in
    public float distanceMultiplier;   // Scaler to pull players in by

    /* Hooke's law labels this k, how much force the tether applies */
    public float elasticity;           // Constant tether force, scaled

    /* The current distance between the two players */
    public float initialDistance;
    private float curDistance;


    /* Our Players */
    public GameObject playerOne;
    public GameObject playerTwo;
    private Rigidbody2D playerOneBody;
    private Rigidbody2D playerTwoBody;

    /* Resource to represent the tether as a line */
    private GameObject[] tetherLinks;   // All the tether's child links
    private TetherLinks[] tetherScripts;// Scripts for the tethers
    public int tetherResolution;        // How many nodes to use
    public GameObject linkNode;        // Link prefab to clone
    
    /* Collision tracking */
    public int numColls;
    public Vector3 curColl;

    /* Health Management */
    public float curHealth;
    public float maxHealth;

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

    public void ApplyDamageFromTether(float dam, int tetherID)
    {
        curHealth -= dam;

        if (curHealth < 0)
        {
            // Snap the tether
            if(tetherID != 0)
            {
                tetherLinks[tetherID].SetActive(false);

                tetherScripts[tetherID - 1].tetherVisual.enabled = false;

                if(tetherID <= tetherScripts.Length)
                    tetherScripts[tetherID + 1].tetherVisual.enabled = false;
            }

            // kill the players
            Kill();
        }
        
        return;
    }

    public void Kill()
    {
        curMenuManager.ShowDeathPanel();
    }

    public float getDamageRatio()
    {
        return curHealth / maxHealth;
    }

    /* Set up via Awake to prepare for child nodes */
    void Awake () {
        /* tracking variables */
        numColls = 0;

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
            tetherScripts[i].myID = i;
        }
        distributeNodes();

        /* Set the links targets to prev node, with special cases for the players */
        playerOne.GetComponent<TetherLinks>().setConnection(tetherLinks[0]);
        playerOne.GetComponent<TetherLinks>().tetherManager = this;
        tetherLinks[0].GetComponent<DistanceJoint2D>().connectedBody = playerOneBody;
        tetherLinks[0].GetComponent<TetherLinks>().setConnection(playerOne);

        for (int i = 1; i < tetherLinks.Length; i++)
        {
            tetherLinks[i].GetComponent<DistanceJoint2D>().connectedBody = tetherLinks[i - 1].GetComponent<Rigidbody2D>();
            tetherLinks[i].GetComponent<TetherLinks>().setConnection(tetherLinks[i-1]);
        }

        playerTwo.GetComponent<DistanceJoint2D>().connectedBody = tetherLinks[tetherLinks.Length - 1].GetComponent<Rigidbody2D>();
        playerTwo.GetComponent<TetherLinks>().setConnection(tetherLinks[tetherLinks.Length - 1]);
        playerTwo.GetComponent<TetherLinks>().setConnection(tetherLinks[tetherLinks.Length - 1]);
        playerTwo.GetComponent<TetherLinks>().tetherManager = this;
    }


    void Start(){
        curHealth = maxHealth;

        initialDistance = Vector3.Distance(playerOne.transform.position, playerTwo.transform.position);
    }
	
	// Update is called once per frame
	void Update () {
        // Get current distance between players
        Vector3 posOne = playerOne.transform.position;
        Vector3 posTwo = playerTwo.transform.position;
        Vector3 difference;
        
        /* Calculate distance based on taughtness */
        if (numColls == 0)
        {
            // Calculate vector of p1 to p2
            difference = posOne - posTwo;

            // curdistance = Magnitide of difference
            curDistance = Vector3.Distance(posOne, posTwo);
        }
        else
        {
            // Calculate vector of p1 to p2
            difference = posOne - posTwo - curColl;

            // curdistance = Magnitide of difference
            curDistance = Vector3.Distance(posOne, curColl) + Vector3.Distance(posTwo, curColl);
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
            playerOneBody.AddForce(-difference * Time.deltaTime);
            playerTwoBody.AddForce(difference * Time.deltaTime);
        }

        /* Tell all nodes to render a line */ 
        BroadcastMessage("ConnectNodes");
        playerTwo.SendMessage("ConnectNodes");
    }

}
