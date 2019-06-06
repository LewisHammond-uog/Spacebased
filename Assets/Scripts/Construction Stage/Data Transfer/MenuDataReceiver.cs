using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Receives data from the Menu classes
/// </summary>
public class MenuDataReceiver : MonoBehaviour {

    RaceLoadingData dataController;

    //List of all the player characters
    [SerializeField]
    GameObject[] players = new GameObject[4];


	// Use this for initialization
	void Start () {

        //Find the data object in the scene
        dataController = FindObjectOfType<RaceLoadingData>();

        if (dataController != null)
        {
            //Get the character infomation
            Dictionary<int, CharacterMenuController.CHARACTERS> loadedCharacterData = dataController.playerCharacterDictonary;
            TrackSelectionMenuController.TRACKS loadedSelectedTrack = dataController.selectedTrack;

            LoadPlayerCharacterModels(loadedCharacterData);
            //Set the track to load
            gameObject.GetComponent<ConstructionController>().SetTrackToLoad(loadedSelectedTrack);

            //Now the data is received we can delete the data controler
            Destroy(dataController.gameObject);
        }

    }

    private void Update()
    {
        
    }

    /// <summary>
    /// Loads in the correct character models on to the correct characters in the construction stage
    /// </summary>
    /// <param name="characterData">Character Data</param>
    private void LoadPlayerCharacterModels(Dictionary<int, CharacterMenuController.CHARACTERS> characterData)
    {
        //Apply the character infomation to the characters
        foreach (GameObject player in players)
        {
            //Get the player ID and then apply the correct character on to that player
            int playerID = player.GetComponent<ConstructionCharacterController>().playerID;
            player.GetComponent<CharacterModelSelector>().SetCharacterModel(characterData[playerID]);
        }
    }

}
