using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    private Text maxEnemiesTextNumber;
    private Slider maxEnemiesSlider;

    private int maxEnemies;

    // 0 - move up, 1 - move down, 2 - move left, 3 - move right, 4 - pick up weapon, 5 - switch weapon, 6 - dash
    private Button[] inputActionButtons;
    private KeyCode[] inputActions;

    private int inputIndex;
    private bool listeningForInput;

    private void Start()
    {
        maxEnemiesTextNumber = transform.FindChild("MaxEnemiesNumber").GetComponent<Text>();
        maxEnemiesSlider = transform.FindChild("MaxEnemiesSlider").GetComponent<Slider>();

        maxEnemies = Settings.maxEnemies;

        int inputActionLength = 7;

        inputActionButtons = new Button[inputActionLength];
        inputActionButtons[0] = transform.FindChild("UpButton").GetComponent<Button>();
        inputActionButtons[1] = transform.FindChild("DownButton").GetComponent<Button>();
        inputActionButtons[2] = transform.FindChild("LeftButton").GetComponent<Button>();
        inputActionButtons[3] = transform.FindChild("RightButton").GetComponent<Button>();
        inputActionButtons[4] = transform.FindChild("PickupWeaponButton").GetComponent<Button>();
        inputActionButtons[5] = transform.FindChild("SwitchWeaponButton").GetComponent<Button>();
        inputActionButtons[6] = transform.FindChild("DashButton").GetComponent<Button>();

        inputActions = new KeyCode[inputActionLength];
        inputActions[0] = Settings.upKeyCode;
        inputActions[1] = Settings.downKeyCode;
        inputActions[2] = Settings.leftKeyCode;
        inputActions[3] = Settings.rightKeyCode;
        inputActions[4] = Settings.pickupWeaponKeyCode;
        inputActions[5] = Settings.switchWeaponKeyCode;
        inputActions[6] = Settings.dashKeyCode;

        maxEnemiesSlider.value = Settings.maxEnemies;
        maxEnemiesTextNumber.text = Settings.maxEnemies.ToString();

        for (int i = 0; i < inputActionLength; i++)
        {
            inputActionButtons[i].GetComponentInChildren<Text>().text = inputActions[i].ToString();
        }

        inputIndex = -1;
        listeningForInput = false;
    }

    private void OnGUI()
    {
        if(listeningForInput)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                if(e.keyCode == KeyCode.Escape)
                {
                    inputIndex = -1;
                    listeningForInput = false;
                    return;
                }

                inputActionButtons[inputIndex].GetComponentInChildren<Text>().text = e.keyCode.ToString();
                inputActions[inputIndex] = e.keyCode;

                inputIndex = -1;
                listeningForInput = false;
            } else if(e.isMouse)
            {
                if(e.clickCount > 0)
                {
                    inputIndex = -1;
                    listeningForInput = false;
                }
            }
        }
    }

    public void ChangeMaxEnemiesTextNumber()
    {
        maxEnemies = (int)maxEnemiesSlider.value;
        maxEnemiesTextNumber.text = maxEnemiesSlider.value.ToString();
    }

    public void ClickInputButton(int index)
    {
        inputIndex = index;
        listeningForInput = true;
    }

    public void Apply()
    {
        Settings.maxEnemies = maxEnemies;
        Settings.upKeyCode = inputActions[0];
        Settings.downKeyCode = inputActions[1];
        Settings.leftKeyCode = inputActions[2];
        Settings.rightKeyCode = inputActions[3];
        Settings.pickupWeaponKeyCode = inputActions[4];
        Settings.switchWeaponKeyCode = inputActions[5];
        Settings.dashKeyCode = inputActions[6];
    }

    public void BackToMainMenu()
    {
        SceneManager.UnloadSceneAsync("Options");
    }
}
