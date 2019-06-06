using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores data for race loading (i.e Track and Character Selection)
/// </summary>
public class RaceLoadingData : MonoBehaviour {

    //Dictonary to store which character is assigned to which player
    public Dictionary<int, CharacterMenuController.CHARACTERS> playerCharacterDictonary = new Dictionary<int, CharacterMenuController.CHARACTERS>();
    public TrackSelectionMenuController.TRACKS selectedTrack;

    private void Start()
    {
        //We are using this class to transfer data, don't destroy it when we leave the scene
        DontDestroyOnLoad(this);
    }

}
