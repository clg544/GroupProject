using UnityEngine;
using System.Collections;

public class PlayerCameraTargetScript : MonoBehaviour
{
    /* Magic Numbers */
    int NUM_PLAYERS = 2;
    
    /* Serialized Game Objects */
    [SerializeField]
    GameObject PlayerOne;
    [SerializeField]
    GameObject PlayerTwo;
    [SerializeField]
    GameObject PlayerCamera;
        
    /* Used Components */
    Rigidbody2D PlayerOneBody;
    Rigidbody2D PlayerTwoBody;
    Camera myCamera;
    
    /* Player target movement variables */
    [SerializeField]
    private float AccelerationSpeed;
    [SerializeField]
    private float MaxSpeed;
    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private float AccelRatio;

    /* Camera Restrictions */
    [SerializeField]
    private float minCameraSize;
    [SerializeField]
    private float maxCameraSize;
    
    /* Maximum target movement speeds, independant for screen ratios */
    [SerializeField]
    float maxX;
    [SerializeField]
    float maxY;
    
    Vector3 tetherCenter;
    
    // Use this for initialization
    void Start()
    {
        PlayerOneBody = PlayerOne.GetComponent<Rigidbody2D>();
        PlayerTwoBody = PlayerTwo.GetComponent<Rigidbody2D>();

        myCamera = PlayerCamera.GetComponent<Camera>();
    }
    
    // Update is called once per frame
    void Update()
    {
        /* Start in the Center */
        Vector3 playerDist = (PlayerOne.transform.localPosition + PlayerTwo.transform.localPosition) / NUM_PLAYERS;
        Vector3 targetPos = playerDist;

        /* Add player velocities */
        Vector3 playerVelocities = ((Vector3)PlayerOneBody.velocity + (Vector3)PlayerTwoBody.velocity) * AccelRatio;
        /* Limit to max X and Y */
        // X
        if (Mathf.Abs(playerVelocities.x) > maxX)
            playerVelocities.x = maxX * Mathf.Sign(playerVelocities.x);
        // Y
        if (Mathf.Abs(playerVelocities.y) > maxY)
            playerVelocities.y = maxY * Mathf.Sign(playerVelocities.y);
        
        targetPos += playerVelocities;

        /* if we changed direction, double moveToward zero to get ahead */
        if ((Mathf.Sign(playerVelocities.x) != Mathf.Sign(targetPos.x)) ||
            (Mathf.Sign(playerVelocities.y) != Mathf.Sign(targetPos.y)))
        {
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, targetPos, cameraSpeed);
        }
        
        /* Move to the calculated position */
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, targetPos, cameraSpeed);


        /* Adjust the camera size based on player distance */
        //Todo: this

    }
}
