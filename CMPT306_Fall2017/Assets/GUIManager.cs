using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

    GameObject FightyObject;
    GameObject ShootyObject;
    GameObject TetherManager;

    /* Player Combat Scripts holding values */
    PlayerCombat FightyCombat;
    PlayerCombat ShootyCombat;
    PlayerTetherScript TetherScript;

    /* GUI Objects to update */
    public Slider FightyHealthBar;
    public Slider FightyCooldownBar;

    public Slider ShootyHealthBar;
    public Slider ShootyCooldownBar;
    public Text ShootySelectedWeapon;
    public Text FightySelectedWeapon;

    public Slider TetherSlider;


    // Use this for initialization
    void Start ()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in players)
        {
            if (p.name == "Fighty")
            {
                FightyObject = p;
            }
            else if (p.name == "Shooty")
            {
                ShootyObject = p;
            }
        }

        FightyCombat = FightyObject.GetComponent<PlayerCombat>();
        ShootyCombat = ShootyObject.GetComponent<PlayerCombat>();

        TetherManager = GameObject.FindGameObjectWithTag("TetherManager");
        TetherScript = TetherManager.GetComponent<PlayerTetherScript>();
    }
	
	// Update is called once per frame
	void Update () {
        FightyHealthBar.value = FightyCombat.getDamageRatio();
        FightyCooldownBar.value = FightyCombat.getCooldownRatio();
        FightySelectedWeapon.text = FightyCombat.getWeaponName();

        ShootyHealthBar.value = ShootyCombat.getDamageRatio();
        ShootyCooldownBar.value = ShootyCombat.getCooldownRatio();
        ShootySelectedWeapon.text = ShootyCombat.getWeaponName();

        TetherSlider.value = TetherScript.getDamageRatio();
    }
}
