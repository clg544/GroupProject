using UnityEngine;
using System.Collections;

public class PlayerCameraTargetScript : MonoBehaviour
{
    int NUM_PLAYERS = 2;

    /* Player target movement variables */
    [SerializeField]
    private float AccelerationSpeed;
    [SerializeField]
    private float MaxSpeed;
    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private float AccelRatio;
    
    /* Maximum target movement speeds, independant for screen ratios */
    [SerializeField]
    float maxX;
    [SerializeField]
    float maxY;
    Vector3 MAX_X;
    Vector3 MAX_Y;

    [SerializeField]
    GameObject PlayerOne;
    [SerializeField]
    GameObject PlayerTwo;

    Rigidbody2D PlayerOneBody;
    Rigidbody2D PlayerTwoBody;

    Vector3 tetherCenter;
    
    // Use this for initialization
    void Start()
    {
        PlayerOneBody = PlayerOne.GetComponent<Rigidbody2D>();
        PlayerTwoBody = PlayerTwo.GetComponent<Rigidbody2D>();

        MAX_X = new Vector3(maxX, 0, 0);
        MAX_Y = new Vector3(0, maxY, 0);
    }
    
    // Update is called once per frame
    void Update()
    {
        /* Start in the Center */
        Vector3 targetPos = (PlayerOne.transform.localPosition + PlayerTwo.transform.localPosition) / NUM_PLAYERS;

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
        
        /* Add player velocities */
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, targetPos, cameraSpeed);
    }
}
