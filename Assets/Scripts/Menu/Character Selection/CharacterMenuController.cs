using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMenuController : MonoBehaviour {

    public enum CHARACTERS
    {
        CHARACTER_ALIEN,
        CHARACTER_SPACEMAN,
        CHARACTER_SCIENTIST,
        CHARACTER_ROBOT
    }

    //List of the avaliable characters
    [HideInInspector]
    public List<CHARACTERS> avalaibleCharacters;

    //Images for characters
    [SerializeField]
    private Sprite alienCharacterIcon, spacemanCharacterIcon, scientistCharacterIcon, robotCharacterIcon;
    public Dictionary<CHARACTERS, Sprite> characterIconDictonary;

    //List of attached player UI Controllers
    [SerializeField]
    private CharacterSelectionPlayerUI[] players = new CharacterSelectionPlayerUI[4];

    [SerializeField]
    private RaceLoadingData dataController;

    //Name of scene to transfer to after players have selected thier characters
    [SerializeField]
    private string trackSelectionSceneName;

    private void Awake()
    {
        //Add all of the characters to the avaliable characters array
        var CharactersAsSystemArray = Enum.GetValues(typeof(CHARACTERS));
        foreach(CHARACTERS character in CharactersAsSystemArray)
        {
            avalaibleCharacters.Add(character);
        }

        //Setup Dictonary to store characters with thier icons
        characterIconDictonary = new Dictionary<CHARACTERS, Sprite>()
        {
            {CHARACTERS.CHARACTER_ALIEN, alienCharacterIcon},
            {CHARACTERS.CHARACTER_SPACEMAN, spacemanCharacterIcon},
            {CHARACTERS.CHARACTER_SCIENTIST, scientistCharacterIcon},
            {CHARACTERS.CHARACTER_ROBOT, robotCharacterIcon}
        };
    }

    private void Update()
    {
        //Check if all players are ready and we can move on to the next scene
        if (AllPlayersReady())
        {
            HandOffData();
            SceneManager.LoadScene(trackSelectionSceneName);
        }
    }

    /// <summary>
    /// Removes a character from the avaliable list of characters
    /// </summary>
    /// <param name="characterToRemove">Character to remove</param>
    public void RemoveCharacter(CHARACTERS characterToRemove)
    {

        //Remove the Character from the list
        avalaibleCharacters.Remove(characterToRemove);

        //Check if any player has this character selected, if the have then deselect it
        foreach (CharacterSelectionPlayerUI player in players)
        {
            CHARACTERS playerSelectedCharacer = player.selectedCharacter;

            //If the player has not confrimed their character and they have the character we are going to remove selected
            //stop them from having that character selected
            if (!player.characterConfirmed && playerSelectedCharacer == characterToRemove)
            {
                player.SetSelectedCharacter(0);
            }
        }


    }

    /// <summary>
    /// Checks if all players are ready (have selected their characters)
    /// </summary>
    /// <returns>All Players Ready</returns>
    private bool AllPlayersReady()
    {
        foreach(CharacterSelectionPlayerUI player in players)
        {
            //If any player has not confrimed then all players are not ready, return false
            if(player.characterConfirmed == false)
            {
                return false;
            }
        }

        //If all players have confrimed a character then we have all characters ready
        return true;
    }

    /// <summary>
    /// Hands of the character data to the data controller which will use it to load characters in the 
    /// construction scene
    /// </summary>
    private void HandOffData()
    {

        if (dataController != null)
        {
            //Get the confrimed character of each of the players then transfer it to a data object
            foreach (CharacterSelectionPlayerUI player in players)
            {
                dataController.playerCharacterDictonary.Add(player.GetPlayerID(), player.confirmedCharacter);
            }
        }

    }
}
