using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionPlayerUI : MonoBehaviour {

    [SerializeField]
    CharacterMenuController menuController;

    public CharacterMenuController.CHARACTERS selectedCharacter { private set; get; }
    public CharacterMenuController.CHARACTERS confirmedCharacter { private set; get; }
    private int selectedCharacterIndex = 0;
    public bool characterConfirmed { private set; get; }

    //Image to draw character to
    [SerializeField]
    private Image characterImage;

    //Text Displayed when character is confirmed
    [SerializeField]
    private GameObject readyText;

    [SerializeField]
    private int playerID;

    //Values for a cool down on chaging character so we dont "wizz" through them
    private const float characterChangeCooldownDuration = 0.2f;
    private float characterChangeCountdown = 0f;

	// Use this for initialization
	void Start () {
        //Intitalise Selected Character
        selectedCharacter = menuController.avalaibleCharacters[selectedCharacterIndex];
	}
	
	// Update is called once per frame
	void Update () {

        if (!characterConfirmed)
        {
            DoCharacterSelection();

            if (ControlManager.GetButtonInputThisFrame("A", playerID))
            {
                ConfirmCharacterSelection();
            }
        }

    }

    /// <summary>
    /// Gets the player ID of this player
    /// </summary>
    public int GetPlayerID()
    {
        return playerID;
    }

    /// <summary>
    /// Sets character image for the currently selected character
    /// </summary>
    private void SetCharacterImage()
    {
        characterImage.sprite = menuController.characterIconDictonary[selectedCharacter];
    }

    /// <summary>
    /// Moves the selected character based on user input
    /// </summary>
    private void DoCharacterSelection()
    {

        //Reset countdown timer if we return the stick to the center of {the axis
        if(ControlManager.GetAxisInput("Horizontal", playerID) == 0f)
        {
            characterChangeCountdown = 0f;
        }

        //If the character change cool down is over
        if (characterChangeCountdown <= 0)
        {
            //Do Movement of character from left to right
            if (ControlManager.GetAxisInput("Horizontal", playerID) > 0.5f)
            {
                //Check if we are going to go ove the end of the character list, if
                //we are then loop to 0
                if (selectedCharacterIndex + 1 >= menuController.avalaibleCharacters.Count)
                {
                    SetSelectedCharacter(0);
                }
                else
                {
                    SetSelectedCharacter(selectedCharacterIndex + 1);
                }

                //Reset countdowntimer
                characterChangeCountdown = characterChangeCooldownDuration;
            }
            else if (ControlManager.GetAxisInput("Horizontal", playerID) < -0.5f)
            {
                //Check if we are going to go ove the end of the character list, if
                //we are then loop to 0
                if (selectedCharacterIndex - 1 < 0)
                {
                    SetSelectedCharacter(menuController.avalaibleCharacters.Count - 1);
                }
                else
                {
                    SetSelectedCharacter(selectedCharacterIndex - 1);
                }

                //Reset countdowntimer
                characterChangeCountdown = characterChangeCooldownDuration;
            }

            
        }

        //Take time from countdown
        characterChangeCountdown -= Time.deltaTime;
    }

    /// <summary>
    /// Confirm and lock in the current character selection
    /// </summary>
    private void ConfirmCharacterSelection()
    {
 
        confirmedCharacter = selectedCharacter;
        characterConfirmed = true;

        //Remove character so that no one else can select it
        menuController.RemoveCharacter(confirmedCharacter);

        //Show Ready Text
        readyText.SetActive(true);
    }

    /// <summary>
    /// Sets the selected character to a given character index in the CharacterMenuController Character List,
    /// if that character exists within the array
    /// </summary>
    /// <param name="newCharacterIndex">Index of character to select</param>
    public void SetSelectedCharacter(int newCharacterIndex)
    {
        selectedCharacterIndex = newCharacterIndex;
        selectedCharacter = menuController.avalaibleCharacters[selectedCharacterIndex];

        SetCharacterImage();
    }

}
