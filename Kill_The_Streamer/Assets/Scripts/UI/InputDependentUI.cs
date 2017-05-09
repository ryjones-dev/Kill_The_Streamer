using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputDependentUI : MonoBehaviour
{
    public Text pickupText;
    public Text dashText;
    public Text swapText;
    public Text outOfAmmoText;

    private void Start()
    {
        pickupText.text = "Press '" + Settings.pickupWeaponKeyCode.ToString() + "'";
        dashText.text = "Press 'Spacebar' or '" + Settings.dashKeyCode.ToString() + "' to Dash";
        swapText.text = "Press '" + Settings.switchWeaponKeyCode + "' to Swap";
        outOfAmmoText.text = "Out of Ammo:\nPress '" + Settings.switchWeaponKeyCode + "'";
    }
}
