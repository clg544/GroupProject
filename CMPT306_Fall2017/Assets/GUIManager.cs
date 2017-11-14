using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

    /* Player Combat Scripts holding values */
    public PlayerCombat FightyCombat;
    public PlayerCombat ShootyCombat;

    public Slider FightyHealthBar;
    public Slider FightyCooldownBar;
    public Slider ShootyHealthBar;
    public Slider ShootyCooldownBar;
    public Text ShootySelectedWeapon;
    public Text FightySelectedWeapon;

    /* GUI Objects to update */


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        FightyHealthBar.value = FightyCombat.getDamageRatio();
        FightyCooldownBar.value = FightyCombat.getCooldownRatio();
        FightySelectedWeapon.text = FightyCombat.getWeaponName();

        ShootyHealthBar.value = ShootyCombat.getDamageRatio();
        ShootyCooldownBar.value = ShootyCombat.getCooldownRatio();
        ShootySelectedWeapon.text = ShootyCombat.getWeaponName();
    }
}
