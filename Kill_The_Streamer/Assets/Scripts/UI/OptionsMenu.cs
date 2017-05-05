using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    private Text maxEnemiesTextNumber;
    private Slider maxEnemiesSlider;

    private Button upButton;
    private Button downButton;
    private Button leftButton;
    private Button rightButton;
    private Button switchWeaponButton;

    private int maxEnemies = 1024;
    private KeyCode upKeyCode;
    private KeyCode downKeyCode;
    private KeyCode leftKeyCode;
    private KeyCode rightKeyCode;
    private KeyCode switchWeaponKeyCode;

    private int maxEnemiesTemp;
    private KeyCode upKeyCodeTemp;
    private KeyCode downKeyCodeTemp;
    private KeyCode leftKeyCodeTemp;
    private KeyCode rightKeyCodeTemp;
    private KeyCode switchWeaponKeyCodeTemp;

    private void Start()
    {
        maxEnemiesTextNumber = transform.FindChild("MaxEnemiesNumber").GetComponent<Text>();
        maxEnemiesSlider = transform.FindChild("MaxEnemiesSlider").GetComponent<Slider>();

        upButton = transform.FindChild("UpButton").GetComponent<Button>();
        downButton = transform.FindChild("DownButton").GetComponent<Button>();
        leftButton = transform.FindChild("LeftButton").GetComponent<Button>();
        rightButton = transform.FindChild("RightButton").GetComponent<Button>();
        switchWeaponButton = transform.FindChild("SwitchWeaponButton").GetComponent<Button>();

        maxEnemiesTemp = maxEnemies;
        upKeyCodeTemp = upKeyCode;
        downKeyCodeTemp = downKeyCode;
        leftKeyCodeTemp = leftKeyCode;
        rightKeyCodeTemp = rightKeyCode;
        switchWeaponKeyCodeTemp = switchWeaponKeyCode;
    }

    public void ChangeMaxEnemiesTextNumber()
    {
        maxEnemiesTemp = (int)maxEnemiesSlider.value;
        maxEnemiesTextNumber.text = maxEnemiesSlider.value.ToString();
    }

    public void ClickUpButton()
    {

    }

    public void ClickDownButton()
    {

    }

    public void ClickLeftButton()
    {

    }

    public void ClickRightButton()
    {

    }

    public void ClickSwitchWeaponsButton()
    {

    }

    public void Apply()
    {
        maxEnemies = maxEnemiesTemp;
        upKeyCode = upKeyCodeTemp;
        downKeyCode = downKeyCodeTemp;
        leftKeyCode = leftKeyCodeTemp;
        rightKeyCode = rightKeyCodeTemp;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
