using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConstructionController : MonoBehaviour {

    [Header("Construction")]
    [SerializeField]
    public float constructionLevelDuration = 120; //Time for construction in seconds
    public float constructionTimer;

    //CARs Must be storeed P1 > P2 > P3 > P4
    [SerializeField]
    private CarConstructionManager[] cars = new CarConstructionManager[4];
    //Array of players
    [SerializeField]
    private ConstructionCharacterController[] players = new ConstructionCharacterController[4];

    [SerializeField]
    private GameObject PreConstructionUI, InConstructionUI, PostConstructionUI;

    [SerializeField]
    private AudioSource endSound;

    //Dictonary to store qualities of car parts once construction has finished
    private Dictionary<CarPart.PART_TYPE, CarPart.PART_QUALITY>[] finalCarParts = new Dictionary<CarPart.PART_TYPE, CarPart.PART_QUALITY>[4];

    //String for name of race track to load
    private string racetrackToLoadName;

    //Strings of track names to load
    private string redPlanetTrackName = "RaceTrackRedPlanet";
    private string planetZTrackName = "RaceTrackPlanetZ";
    private string iceGiantTrackName = "RaceTrackIceGiant";


    //Enum for storing the current state of construction
    private enum ConstructionStatues
    {
        CONSTRCUTION_WARMUP,
        CONSTRUCTION_ACTIVE,
        CONSTRUCTION_FINISHED,
        CONSTRUCTION_CLEANUP
    }
    private ConstructionStatues currentConstructionStatus = ConstructionStatues.CONSTRCUTION_WARMUP;
    
    private void Start()
    {
        //Initalise the car part dictonary array so that it is the same length as the cars array
        finalCarParts = new Dictionary<CarPart.PART_TYPE, CarPart.PART_QUALITY>[cars.Length];

        //Initalise Construction Countdown timer
        constructionTimer = constructionLevelDuration;

        //Start Construction warm up phase
        StartWarmup();

        //Set so that we don't loose data when we have finsihed construction so that we can pass it off
        //to the race controller
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

        //Switch to run different functions based on the current construction status
        switch (currentConstructionStatus)
        {
            case ConstructionStatues.CONSTRCUTION_WARMUP:
                //Do Construction Warm Up
                DoWarmUpStep();
                break;
            case ConstructionStatues.CONSTRUCTION_ACTIVE:
                //Run Construction Countdown
                DoConstructionStep();
                break;
            case ConstructionStatues.CONSTRUCTION_FINISHED:
                DoConstructionEndStep();
                break;
            case ConstructionStatues.CONSTRUCTION_CLEANUP:
                //Run cleanup loop
                DoConstructionCleanupStep();
                break;
        }

    }

    /// <summary>
    /// Starts the construction warm up by setting up UI elements and freezing the players
    /// </summary>
    private void StartWarmup()
    {
        //Freeze all the player so that they can't move about
        foreach(ConstructionCharacterController player in players)
        {
            player.SetFrozen(true);
        }

        //Setup UI
        PreConstructionUI.SetActive(true);
        InConstructionUI.SetActive(false);
        PostConstructionUI.SetActive(false);

        currentConstructionStatus = ConstructionStatues.CONSTRCUTION_WARMUP;

        PreConstructionUI.GetComponent<PreConstructionUIController>().InitaliseUI();

    }

    /// <summary>
    /// Runs functionality for Pre Construction UI
    /// </summary>
    private void DoWarmUpStep()
    {
        //Call UI Update Function
        PreConstructionUI.GetComponent<PreConstructionUIController>().UpdateUI();

        //If this UI is finished then start construction
        if (PreConstructionUI.GetComponent<PreConstructionUIController>().GetUIFinished())
        {
            StartConstruction();
        }
    }

    /// <summary>
    /// Starts Construction Stage and sets UI Acitve as well as unfreezing players
    /// </summary>
    private void StartConstruction() {

        //Freeze all the player so that they can't move about
        foreach (ConstructionCharacterController player in players)
        {
            player.SetFrozen(false);
        }

        currentConstructionStatus = ConstructionStatues.CONSTRUCTION_ACTIVE;

        //Setup UI
        PreConstructionUI.SetActive(false);
        InConstructionUI.SetActive(true);
        PostConstructionUI.SetActive(false);

        InConstructionUI.GetComponent<InConstructionUIController>().InitaliseUI();

    }

    /// <summary>
    /// Runs the normal construction loop of reducing the construction timer
    /// and checking if construction is over
    /// </summary>
    private void DoConstructionStep()
    {
        //Countdown Construction Timer
        constructionTimer -= Time.deltaTime;

        InConstructionUI.GetComponent<InConstructionUIController>().UpdateUI();

        //If the timer has run down then trigger the end of contruction and set construction status
        //to over
        if (constructionTimer <= 0)
        {
            EndConstruction();
        }
       
    }

    private void EndConstruction()
    {

        //Freeze all the player so that they can't move about
        foreach (ConstructionCharacterController player in players)
        {
            player.SetFrozen(true);
        }

        //Setup UI
        PreConstructionUI.SetActive(false);
        InConstructionUI.SetActive(false);
        PostConstructionUI.SetActive(true);

        currentConstructionStatus = ConstructionStatues.CONSTRUCTION_FINISHED;

        //Play Sound
        endSound.Play();

        //Itlialise UI
        PostConstructionUI.GetComponent<PostConstructionUI>().InitaliseUI();
    }

    /// <summary>
    /// Does Up Updates for end of construction
    /// </summary>
    private void DoConstructionEndStep()
    {
        //Run Update Step
        PostConstructionUI.GetComponent<PostConstructionUI>().UpdateUI();

        //Check if UI is over, if it is then switch to cleanup mode and move on
        if (PostConstructionUI.GetComponent<PostConstructionUI>().GetUIFinsihed()) {
            StartConstructionCleanup();
        }

    }

    /// <summary>
    /// Ends the construction stage of the game
    /// </summary>
    private void StartConstructionCleanup()
    {
        //Get all of the qualities of all of the parts
        for (int i = 0; i < cars.Length; i++)
        {
            //Add quality dictonary to array of all car dictonaries
            finalCarParts[i] = cars[i].GetCarPartQualities();
        }

        currentConstructionStatus = ConstructionStatues.CONSTRUCTION_CLEANUP;

        //Now that we have all of the car parts and qualities for all of the parts we,
        //can load in the race track scene
        SceneManager.LoadScene(racetrackToLoadName);

    }

    /// <summary>
    /// Does all of the cleanup and data hand off for when construction is over
    /// </summary>
    private void DoConstructionCleanupStep()
    {
        if(FindObjectOfType<RaceController>() != null)
        {
            //Find the race controller, there is not a better way to do this than the slow "Find Object"
            //method between scenes so we pass the data as quickly as possible then destroy this 
            //object
            RaceController controller = FindObjectOfType<RaceController>();
            controller.SetCarData(finalCarParts);

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the race track to load when construction is complete
    /// </summary>
    /// <param name="track"></param>
    public void SetTrackToLoad(TrackSelectionMenuController.TRACKS track)
    {
        //Convert track from enum to string
        string trackNameString = string.Empty;
        switch (track)
        {
            case TrackSelectionMenuController.TRACKS.TRACK_ICEGIANT:
                trackNameString = iceGiantTrackName;
                break;
            case TrackSelectionMenuController.TRACKS.TRACK_PLANETZ:
                trackNameString = planetZTrackName;
                break;
            case TrackSelectionMenuController.TRACKS.TRACK_REDPLANET:
                trackNameString = redPlanetTrackName;
                break;
            default:
                trackNameString = planetZTrackName;
                break;
        }

        //Set selected track to new track name
        racetrackToLoadName = trackNameString;

    }

}
