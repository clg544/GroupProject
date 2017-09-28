using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour {
    
    int LightShotCooldown;
    PlayerBehavior myPlayer;

    [SerializeField]
    GameObject Bullet;
    [SerializeField]
    GameObject Crosshair;
    GameObject curCrosshair;

    [SerializeField]
    float crosshairDist;    // How away the crasshair is from the player
    [SerializeField]
    int LightBulletVel;     // How fast light bullets travel
    
    /**
     * LightShot - Instantiate a bullet at the player with trajectory decided by angle 
     */
    public void LightShot(Vector3 angle)
    {
        if(LightShotCooldown <= 0)
        {
            Instantiate(Bullet);
        }


    }

    public void PlayerAim(Vector2 joyPos)
    {
        Destroy(curCrosshair);

        joyPos.Normalize();
        joyPos *= crosshairDist;

        curCrosshair = Instantiate(Crosshair);

        curCrosshair.transform.localPosition = joyPos;
    }


	// Use this for initialization
	void Start () {
        LightShotCooldown = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (LightShotCooldown > 0)
            LightShotCooldown--;

        
	}
}
