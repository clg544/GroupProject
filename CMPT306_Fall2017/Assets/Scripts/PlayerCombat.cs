using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    
    public enum PlayerClass { FIGHTY, SHOOTY};

    private float AttackCooldown;     // Current cooldown value
    
    public PlayerBehavior myPlayer;
    public Rigidbody2D myBody;
    public PlayerClass myClass;                  // True=Fighty, False=Shooty
    
    /* Player Health */
    private float curHealth;
    public float maxHealth;

    /* Stamina representation */
    public float lastCountdownTime;

    /* Prefabs for attack objects */
    public GameObject Crosshair;
    GameObject curCrosshair;
    public GameObject Bullet;       // Shooty Bullet
    public GameObject FightySwing;  // Fighty Swing

    /* Position Variables */
    public enum Direction { Up, Right, Down, Left }
    Direction curDirection;

    /* Fighty Variables */
    public float SwingDist;
    public Vector3 SwingVect;
    public float swingDuration;
    public float swingCooldown;
    public float swingDamage;

    /* Shooty Variables */
    public float bulletSpawnDist;  // How far to spawn the bullet
    public float crosshairDist;    // How away the crasshair is from the player
    public float LightBulletVel;     // How fast light bullets travel
    public float LightBulletCooldown;    // How many frames before we can shoot again
    public float LightBulletDamage;     // How fast light bullets travel
    public float LightBulletSpread;     // Spread applied to light bullets

    public float HeavyBulletVel;     // How fast light bullets travel
    public float HeavyBulletCooldown;    // How many frames before we can shoot again
    public float HeavyBulletMassRatio;   // Mass Multiplier of heavy bullet over light bullet
    public float HeavyBulletDamage;     // How much damage this causes
    public float HeavyBulletSpread;     // Spread applied to Heavy bullets
    public float recoil;            // Universal recoil multiplier
    
    /* Weapon variables */
    public enum Weapon { LIGHT_GUN, HEAVY_GUN };
    public Weapon curWeapon;

    /**
     * Shoot - Shoot the current weapon 
     */
    public void Shoot()
    {
        if (curCrosshair == null)
            return;

        if (AttackCooldown > 0)
            return; 

        switch (curWeapon)
        {
            case Weapon.LIGHT_GUN:
                LightShot();
                break;
            case Weapon.HEAVY_GUN:
                HeavyShot();
                break;
        }
    }
    /**
     * LightShot - Instantiate a bullet at the player with trajectory decided by curCrosshair 
     */
    private void LightShot()
    {
        GameObject newBullet;

        if (AttackCooldown <= 0)
        {
            /* Add to Shot cooldown */
            AttackCooldown += LightBulletCooldown;
            lastCountdownTime = LightBulletCooldown;

             /* Create our new bullet */
             newBullet = Instantiate(Bullet, gameObject.transform);
            newBullet.GetComponent<BulletScript>().damage = LightBulletDamage;

            /* Bullet Position = bulletSpawnDist from centre of player, towards the crasshair*/
            newBullet.transform.localPosition = curCrosshair.transform.localPosition.normalized * bulletSpawnDist;

            /* Bullet Velocity = LightBulletVel, towards the crasshair*/
            Rigidbody2D bulletVel = newBullet.GetComponent<Rigidbody2D>();
            bulletVel.velocity = (curCrosshair.transform.localPosition.normalized * LightBulletVel);
            /* Add Player Velocity */
            bulletVel.velocity += myBody.velocity;
            /* Push player with bulletVel */
            myBody.AddForce(-bulletVel.velocity * bulletVel.mass * recoil);

            /* Add Bullet Spread */
            Vector3 perpendicular = Vector3.Cross(bulletVel.velocity, new Vector3(0, 0, 1)).normalized;

            /* Apply Bullet Spread */
            bulletVel.AddForce(perpendicular * Random.Range(-1.0F, 1.0F) * LightBulletSpread);
        }
    }
    /**
     * HeavyShot - Instantiate a bullet at the player with trajectory decided by curCrosshair 
     */
    private void HeavyShot()
    {
        GameObject newBullet;

        if (AttackCooldown <= 0)
        {
            /* Add to Shot cooldown */
            AttackCooldown += HeavyBulletCooldown;
            lastCountdownTime = HeavyBulletCooldown;

            /* Create our new bullet */
            newBullet = Instantiate(Bullet, gameObject.transform);
            newBullet.GetComponent<BulletScript>().damage = HeavyBulletDamage;

            /* Bullet Position = bulletSpawnDist from centre of player, towards the crasshair*/
            newBullet.transform.localPosition = curCrosshair.transform.localPosition.normalized * bulletSpawnDist;

            /* Bullet Velocity = LightBulletVel, towards the crasshair*/
            Rigidbody2D bulletVel = newBullet.GetComponent<Rigidbody2D>();
            bulletVel.velocity = (curCrosshair.transform.localPosition.normalized * HeavyBulletVel);
            /* Add Player Velocity */
            bulletVel.velocity += myBody.velocity;
            /* Push player with bulletVel */
            myBody.AddForce(-bulletVel.velocity * bulletVel.mass * HeavyBulletMassRatio * recoil);

            /* Add Bullet Spread */
            Vector3 perpendicular = Vector3.Cross(bulletVel.velocity, new Vector3(0, 0, 1)).normalized;

            /* Apply Bullet Spread */
            bulletVel.AddForce(perpendicular * Random.Range(-1.0F, 1.0F) * HeavyBulletSpread);
        }
    }
    
    public void FighterAttack()
    {
        if (curCrosshair == null)
            return;

        if (AttackCooldown > 0)
            return;
        
        GameObject curAttack = Instantiate(FightySwing, gameObject.transform);
        curAttack.GetComponent<PlayerAttackScript>().attackDamage = swingDamage;

        curAttack.transform.localPosition = SwingVect;
        curAttack.transform.Rotate(0, 0, 90 + (-90 * (int)curDirection), Space.Self);

        AttackCooldown += swingCooldown;
        lastCountdownTime = swingCooldown;
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

    public void SwitchWeapon()
    {
        if (myClass != PlayerClass.SHOOTY)
            return;

        if (AttackCooldown > 0)
            return;

        /* Invert weapon choice, as long as there are only 2 weapons */
        if (curWeapon == Weapon.LIGHT_GUN)
        {
            curWeapon = Weapon.HEAVY_GUN;
        }
        else
        {
            curWeapon = Weapon.LIGHT_GUN;
        }

        AttackCooldown += 1;

        Debug.Log(curWeapon);
    }
    
    /**
     * Damage the player
     */
    public void ApplyDamage(int dam)
    {
        Debug.Log("Playerbehavior:ApplyDamage(int dam): Not Yet Implemented");
    }

    /* Pass values to the GUI */ 
    public float getDamageRatio()
    {
        return curHealth / maxHealth;
    }
    public float getCooldownRatio()
    {
        return (1 - AttackCooldown) / lastCountdownTime;
    }
    public string getWeaponName()
    {
        switch (curWeapon)
        {
            case Weapon.LIGHT_GUN:
                return "Machine Gun";

            case Weapon.HEAVY_GUN:
                return "Heavy Rifle";

            default:
                return "<Empty>";
        }
    }

    // Use this for initialization
    void Start () {
        myBody = gameObject.GetComponent<Rigidbody2D>();

        curHealth = maxHealth;
        AttackCooldown = 0;

        // Are we Fighty? Yes:No; 
        myClass = (gameObject.name == "Fighty") ? PlayerClass.FIGHTY:PlayerClass.SHOOTY;

        SwingVect = new Vector3(SwingDist, 0, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (AttackCooldown > 0)
            AttackCooldown -= Time.deltaTime;

        Vector3 frameDir;
        /* Set Direction */
        if (curCrosshair == null)
            frameDir = myBody.velocity;
        else
            frameDir = curCrosshair.transform.localPosition;

        /* Arbitrarily prefers the horizontal directions */
        /* Keep last direction if not moving/aiming      */
        if(frameDir.x > .5F)
        {
            curDirection = Direction.Right;
            SwingVect = new Vector3(SwingDist, 0, 0);
        }
        else if(frameDir.x < -.5F)
        {
            curDirection = Direction.Left;
            SwingVect = new Vector3(-SwingDist, 0, 0);
        }
        else if (frameDir.y > .5F)
        {
            curDirection = Direction.Up;
            SwingVect = new Vector3(0, SwingDist, 0);
        }
        else if (frameDir.y < -.5)
        {
            curDirection = Direction.Down;
            SwingVect = new Vector3(0, -SwingDist, 0);
        }
    }
}
