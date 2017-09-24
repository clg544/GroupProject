using UnityEngine;
using System.Collections;

public class PlayerCameraTargetScript : MonoBehaviour
{
    /* Player target movement variables */
    [SerializeField]
    float ACCEL;
    float MAX_SPEED;
    Vector3 accel_scale;

    /* Maximum target movement speeds, independant for screen ratios */
    [SerializeField]
    float MAX_X;
    [SerializeField]
    float MAX_Y;
    Vector3 maxX;
    Vector3 maxY;

    [SerializeField]
    GameObject PlayerOne;
    [SerializeField]
    GameObject PlayerTwo;

    Vector3 tetherCenter;
    
    // Use this for initialization
    void Start()
    {
        MAX_SPEED = gameObject.transform.parent.GetComponentInChildren<PlayerBehavior>().getMaxSpeed();

        accel_scale = new Vector3(ACCEL, ACCEL, ACCEL);
        maxX = new Vector3(MAX_X, 0, 0);
        maxY = new Vector3(0, MAX_Y, 0);
    }
    
    // Update is called once per frame
    void Update()
    {
        /* Simple tracking for now */
        /* todo: Camera should lead players, and zoom based on player movements */
        Vector3 targetPos = (PlayerOne.transform.position + PlayerTwo.transform.position) / 2;
        
        this.transform.position = targetPos;
    }
}
