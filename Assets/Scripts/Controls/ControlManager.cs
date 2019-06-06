using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ControlManager{

    private const string joystickPrefix = "J";
    private const float dPadActivationThreshold = 0.75f;

    //Player Responsable for leading through menus (i.e main menu and track selection)
    public const int leadPlayerID = 1;

    //Enum for D-Pad Direction
    public enum DPAD_BUTTON
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    /// <summary>
    /// Gets the amount that an axis is being pushed in from axis name and player num
    /// </summary>
    /// <param name="axisName"></param>
    /// <param name="playerNum"></param>
    /// <returns>Axis Value</returns>
    public static float GetAxisInput(string axisName, int playerNum)
    {
        return Input.GetAxis(joystickPrefix + playerNum + axisName);
    }


    /// <summary>
    /// Gets the amount that an axis is being pushed in from axis name and calling object
    /// </summary>
    /// <param name="axisName"></param>
    /// <param name="callingObject"></param>
    /// <returns></returns>
    public static float GetAxisInput(string axisName, ConstructionCharacterController callingObject)
    {
        return GetAxisInput(axisName, GetPlayerID(callingObject));
    }

    /// <summary>
    /// Gets if a particular button has ben pressed from a given button and player num
    /// </summary>
    /// <param name="axisName"></param>
    /// <param name="playerNum"></param>
    /// <returns>Button Pressed</returns>
    public static bool GetButtonInput(string buttonName, int playerNum)
    {
        return Input.GetButton(joystickPrefix + playerNum + buttonName);
    }

    /// <summary>
    /// Gets if a particular button was pressed from button name and calling player object
    /// </summary>
    /// <param name="buttonName"></param>
    /// <param name="callingObject"></param>
    /// <returns></returns>
    public static bool GetButtonInput(string buttonName, ConstructionCharacterController callingObject)
    {
        return GetButtonInput(buttonName, GetPlayerID(callingObject));
    }

    /// <summary>
    /// Gets if a particular button was pressed this frame from a given button and player num
    /// </summary>
    /// <param name="axisName"></param>
    /// <param name="playerNum"></param>
    /// <returns>Button Pressed</returns>
    public static bool GetButtonInputThisFrame(string buttonName, int playerNum)
    {
        return Input.GetButtonDown(joystickPrefix + playerNum + buttonName);
    }

    /// <summary>
    /// Gets if a particular button pressed this frame from button name and calling player object
    /// </summary>
    /// <param name="buttonName"></param>
    /// <param name="callingObject"></param>
    /// <returns></returns>
    public static bool GetButtonInputThisFrame(string buttonName, ConstructionCharacterController callingObject)
    {
        return GetButtonInputThisFrame(buttonName, GetPlayerID(callingObject));
    }


    /// <summary>
    /// Gets if a particular button was pressed this freame
    /// </summary>
    public static bool GetDPadButtonThisFrame(DPAD_BUTTON buttonName, int playerNum)
    {
        //Get amount of input from the dpad axises
        float dPadHorizontalInput = Input.GetAxis("J" + playerNum + "DPadHorizontal");
        float dPadVerticalInput = Input.GetAxis("J" + playerNum + "DPadVertical");

        //Check dpad press based on button that we are trying to check
        switch (buttonName)
        {
            case DPAD_BUTTON.UP:
                if(dPadVerticalInput > dPadActivationThreshold)
                {
                    return true;
                }
                break;
            case DPAD_BUTTON.DOWN:
                if (dPadVerticalInput < (-dPadActivationThreshold))
                {
                    return true;
                }
                break;
            case DPAD_BUTTON.RIGHT:
                if (dPadHorizontalInput > dPadActivationThreshold)
                {
                    return true;
                }
                break;
            case DPAD_BUTTON.LEFT:
                if (dPadHorizontalInput < (-dPadActivationThreshold))
                {
                    return true;
                }
                break;
        }

        return false;
    }

    public static bool GetDPadButtonThisFrame(DPAD_BUTTON buttonName, ConstructionCharacterController callingObject)
    {
        return GetDPadButtonThisFrame(buttonName, GetPlayerID(callingObject));
    }

    /// <summary>
    /// Gets the player id from a calling construction character controller
    /// </summary>
    /// <param name="playerController">Character Controller to get id from</param>
    /// <returns>Player ID</returns>
    private static int GetPlayerID(ConstructionCharacterController playerController)
    {
        return playerController.playerID;
    }
}
