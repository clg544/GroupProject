using UnityEngine;
using System.Collections;

public class PlayerCameraScript : MonoBehaviour
{

    private GameObject myCamera;
    [SerializeField]
    private GameObject myTarget;

    /* Vectors to do trivial adjustments */
    Vector3 ZAdjustVect;
    Vector3 PlayerYAdjustVect;
    Vector3 CameraSpeedVect;

    /* Subtract these to account for the Z Vector */
    [SerializeField]
    private float Z_ADJ;
    [SerializeField]
    private float PLAYER_Y_ADJ;
    /* Camera moves at 1 / CAMERA_SPEED */
    [SerializeField]
    private float cameraLag;

    public GameObject MyTarget
    {
        get
        {
            return myTarget;
        }

        set
        {
            myTarget = value;
        }
    }


    /**
     * Does a smooth movement towards the set target, based on speed variables.
     */
    private void CameraTracking()
    {
        /* Adjust the Camera value to the right Z-plane */
    Vector3 target = myTarget.transform.position;
        target.z = myCamera.transform.position.z;

        /* Camera speeds up as player leaves centre */
        float speed = Vector3.Distance(myCamera.transform.position, target) / cameraLag;

        /* Set the new camera position */
        myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, target, speed);

        return;
    }


    // Use this for initialization
    void Start()
    {
        myCamera = this.gameObject;

        ZAdjustVect = new Vector3(0, 0, Z_ADJ);
        PlayerYAdjustVect = new Vector3(0, PLAYER_Y_ADJ, 0);

        /* Initiate Camera */
        myCamera.transform.position = MyTarget.transform.position;
        myCamera.transform.position += ZAdjustVect;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CameraTracking();
        //myCamera.transform.position += ZAdjustVect;
        //myCamera.transform.position += PlayerYAdjustVect;
    }
}