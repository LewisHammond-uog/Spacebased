using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrackSelectionMenuController : MonoBehaviour {

    private RaceLoadingData dataController;

    //Name of construction scene to progress to once this one is done
    [SerializeField]
    private string nextSceneName;

    //Enum for selected track
    public enum TRACKS
    {
        TRACK_PLANETZ,
        TRACK_REDPLANET,
        TRACK_ICEGIANT
    }

    //Array of Tracks
    [SerializeField]
    private TrackSelectionOption[] trackArray = new TrackSelectionOption[3];
    private TrackSelectionOption selectedTrack;
    private TRACKS confrimedTrack;
    private bool trackConfirmed = false;
    private int selectedTrackIndex = 0;

    //Values for a cool down on track character so we dont "wizz" through them
    private const float trackChangeCooldownDuration = 0.2f;
    private float trackChangeCountdown = 0f;

    private void Start()
    {
        //Set Selected Track to first track in list if
        SetSelectedTrack(0);

        //Find the race data transferer
        dataController = FindObjectOfType<RaceLoadingData>();
    }

    private void Update()
    {
        if (!trackConfirmed)
        {
            DoTrackSelection();

            //If the leading player has pressed a then confrim this track and hand off data
            if (ControlManager.GetButtonInputThisFrame("A", ControlManager.leadPlayerID))
            {
                ConfrimTrackSelection();
                HandOffData();
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    /// <summary>
    /// Moves the selected track based on player input
    /// </summary>
    private void DoTrackSelection()
    {

        

        //Reset countdown timer if we return the stick to the center of {the axis
        if (ControlManager.GetAxisInput("Horizontal", 1) == 0f)
        {
            trackChangeCountdown = 0f;
        }

        //If the character change cool down is over
        if (trackChangeCountdown <= 0)
        {
            //Do Movement of character from left to right
            if (ControlManager.GetAxisInput("Horizontal", ControlManager.leadPlayerID) > 0.5f)
            {
                //Check if we are going to go ove the end of the character list, if
                //we are then loop to 0
                if (selectedTrackIndex + 1 >= trackArray.Length)
                {
                    SetSelectedTrack(0);
                }
                else
                {
                    SetSelectedTrack(selectedTrackIndex + 1);
                }

                //Reset countdowntimer
                trackChangeCountdown = trackChangeCooldownDuration;
            }
            else if (ControlManager.GetAxisInput("Horizontal", ControlManager.leadPlayerID) < -0.5f)
            {
                //Check if we are going to go ove the end of the character list, if
                //we are then loop to 0
                if (selectedTrackIndex - 1 < 0)
                {
                    SetSelectedTrack(trackArray.Length - 1);
                }
                else
                {
                    SetSelectedTrack(selectedTrackIndex - 1);
                }

                //Reset countdowntimer
                trackChangeCountdown = trackChangeCooldownDuration;
            }


        }
        
    }

    /// <summary>
    /// Sets a track as selected based on the input track index
    /// </summary>
    /// <param name="trackIndex">Track Index to set as selected</param>
    private void SetSelectedTrack(int trackIndex)
    {

        selectedTrack = trackArray[trackIndex];
        selectedTrackIndex = trackIndex;
            

        //Make sure that all of the other tracks are unselected
        foreach(TrackSelectionOption track in trackArray)
        {
            if (track != selectedTrack)
            {
                track.SetSelected(false);
            }
        }

        //Set this track as selected
        selectedTrack.SetSelected(true);
        
    }

    /// <summary>
    /// Gets the actual track that is currently selected and sets it as the confirmed track
    /// </summary>
    private void ConfrimTrackSelection()
    {
        confrimedTrack = selectedTrack.track;
    }

    /// <summary>
    /// Hands of data about the currently selected track to the race data controller
    /// </summary>
    private void HandOffData()
    {
        dataController.selectedTrack = confrimedTrack;
    }
}

[System.Serializable]
public class TrackSelectionOption
{
    public TrackSelectionMenuController.TRACKS track;
    public Image trackBackingRectangle;
    public Text trackName;

    private Color selectedColour = Color.yellow;
    private Color unSelectedColour = Color.white;

    /// <summary>
    /// Sets track to either selected or unselected status
    /// </summary>
    /// <param name="selected">wWhether track should be selected</param>
    public void SetSelected(bool selected)
    {
        if (selected)
        {
            trackBackingRectangle.color = selectedColour;
            trackName.color = selectedColour;
        }
        else
        {
            trackBackingRectangle.color = unSelectedColour;
            trackName.color = unSelectedColour;
        }
    }
}
