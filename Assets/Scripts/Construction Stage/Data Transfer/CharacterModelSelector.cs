using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelSelector : MonoBehaviour {

    //Models for different characters 
    [SerializeField]
    private GameObject alienModel, spacemanModel, scientistModel, robotModel;

    /// <summary>
    /// Changes the character model depending on the input character
    /// </summary>
	public void SetCharacterModel(CharacterMenuController.CHARACTERS character)
    {

        //Deactivate all of the character models
        alienModel.SetActive(false);
        spacemanModel.SetActive(false);
        scientistModel.SetActive(false);
        robotModel.SetActive(false);

        //Switch character model that is active based on the input character
        switch (character)
        {
            case CharacterMenuController.CHARACTERS.CHARACTER_ALIEN:
                alienModel.SetActive(true);
                break;
            case CharacterMenuController.CHARACTERS.CHARACTER_SPACEMAN:
                spacemanModel.SetActive(true);
                break;
            case CharacterMenuController.CHARACTERS.CHARACTER_SCIENTIST:
                scientistModel.SetActive(true);
                break;
            case CharacterMenuController.CHARACTERS.CHARACTER_ROBOT:
                robotModel.SetActive(true);
                break;
            default:
                alienModel.SetActive(true);
                break;
        }

    }
}
