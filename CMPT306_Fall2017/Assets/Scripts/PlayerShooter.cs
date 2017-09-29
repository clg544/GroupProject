using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour {
    
    int ShotCooldown;

    [SerializeField]
    PlayerBehavior myPlayer;
    Rigidbody2D myBody;

    [SerializeField]
    GameObject Bullet;
    [SerializeField]
    GameObject Crosshair;
    GameObject curCrosshair;

    [SerializeField]
    float bulletSpawnDist;  // How far to spawn the bullet
    [SerializeField]
    float crosshairDist;    // How away the crasshair is from the player
    [SerializeField]
    int LightBulletVel;     // How fast light bullets travel
    [SerializeField]
    int LightBulletCooldown;    // How many frames before we can shoot again
    [SerializeField]
    int HeavyBulletVel;     // How fast light bullets travel
    [SerializeField]
    int HeavyBulletCooldown;    // How many frames before we can shoot again


    public enum Weapon { LIGHT_GUN, HEAVY_GUN };
    private Weapon curWeapon;

    /**
     * Shoot - Shoot the current weapon 
     */
    public void Shoot()
    {
        if (curCrosshair == null)
            return;

        if (ShotCooldown > 0)
            return; 

        switch (curWeapon)
        {
            case Weapon.LIGHT_GUN:
                LightShot();
                break;

        }
    }
    /**
     * LightShot - Instantiate a bullet at the player with trajectory decided by curCrosshair 
     */
    private void LightShot()
    {
        GameObject newBullet;

        if (ShotCooldown <= 0)
        {
            /* Add to Shot cooldown */
            ShotCooldown += LightBulletCooldown;

            /* Create our new bullet */
            newBullet = Instantiate(Bullet, gameObject.transform);

            /* Bullet Position = bulletSpawnDist from centre of player, towards the crasshair*/
            newBullet.transform.localPosition = curCrosshair.transform.localPosition.normalized * bulletSpawnDist;

            /* Bullet Velocity = LightBulletVel, towards the crasshair*/
            Rigidbody2D bulletVel = newBullet.GetComponent<Rigidbody2D>();
            bulletVel.velocity = (curCrosshair.transform.localPosition.normalized * LightBulletVel);
            /* Add Player Velocity */
            bulletVel.velocity += myBody.velocity;      

            /* Spread, does nothing */
            Vector3 perpendicular = Quaternion.AngleAxis(90, curCrosshair.transform.localPosition.normalized) 
                * curCrosshair.transform.localPosition.normalized;
            /* Apply Bullet Spread */
            bulletVel.AddForce(perpendicular);
        }
    }

    public void PlayerAimStart(Vector2 joyPos)
    {
        if (curCrosshair != null)  // Updates happen too fast! Fix to ghost crosshair bug.
            Destroy(curCrosshair);

        joyPos.Normalize();
        joyPos *= crosshairDist;
        
        curCrosshair = Instantiate(Crosshair, this.transform);
        curCrosshair.transform.localPosition = joyPos;
    }
    public void PlayerAimContinue(Vector2 joyPos)
    {
        joyPos.Normalize();
        joyPos *= crosshairDist;
        
        curCrosshair.transform.localPosition = joyPos;
    }
    public void PlayerAimEnd()
    {
        if (curCrosshair != null)  // Updates happen too fast!
            Destroy(curCrosshair);
    }


	// Use this for initialization
	void Start () {
        ShotCooldown = 0;

        curWeapon = Weapon.LIGHT_GUN;   // Default to the light gun

        myBody = myPlayer.GetComponent<Rigidbody2D>();
	}

    void FixedUpdate()
    {
        if (ShotCooldown > 0)
            ShotCooldown--;
    }
	
	// Update is called once per frame
	void Update ()
    {

    }
}
