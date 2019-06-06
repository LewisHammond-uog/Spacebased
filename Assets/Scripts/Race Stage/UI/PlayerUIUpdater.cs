using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIUpdater : MonoBehaviour {

    [SerializeField]
    private int UIPlayerID = 0;

    [Header("Race Management")]
    [SerializeField]
    private RaceController raceManager;

    [SerializeField]
    private GameObject racingUI, finsihedUI;

    [Header("UI Elements")]
    [SerializeField]
    private Slider boostBar;
    [SerializeField]
    private Text boostText, currentLapText, totalLapsText, currentLapTimeText, bestLaptimeText, currentPositionText, currentPositionSuffixText;


    //Dictonary to store the suffixes for different position values
    private readonly Dictionary<int, string> postionSuffixDictonary = new Dictionary<int, string>
    {
        {1,"st"}, {2,"nd"}, {3, "rd"}, {4, "th"}
    };

    //Enum for boost status
    private enum BoostUIStatus
    {
        DRAIN,
        RECHARGE,
        STATIC
    }
    private BoostUIStatus boostState = BoostUIStatus.STATIC;
    private float boostDrainRate;
    private float boostRechargeRate;
    private const string boostStaticText = "BOOST READY";
    private const string boostDrainText = "BOOST DEPLOYED";
    private const string boostRechargeText = "BOOST RECHARGING";


    // Use this for initialization
    void Start() {
        //Subscribe to lap times and lap change events
        CarLapData.NewBestLap += UpdateBestLap;
        CarLapData.UpdateCurrentLapTime += UpdateCurrentLap;
        CarLapData.NewLap += UpdateLapCounter;
        CarLapData.UpdatePosition += UpdatePostionIndicator;
        CarLapData.RaceFinsihedForCar += ShowFinsishedUI;

        CustomCarController.StartBoost += StartBoostDrain;
        CustomCarController.StopBoost += StartBoostRecharge;

        //Set Race Lap Display 
        totalLapsText.text = raceManager.GetTotalRaceLaps().ToString();

        //Initalise the lap timer to read 0
        UpdateBestLap(0f, UIPlayerID);
    }

    private void Update()
    {
        UpdateBoostUI();
    }

    private void OnDestroy()
    {
        //UnSubscribe to lap times and lap change events
        CarLapData.NewBestLap -= UpdateBestLap;
        CarLapData.UpdateCurrentLapTime -= UpdateCurrentLap;
        CarLapData.NewLap -= UpdateLapCounter;
        CarLapData.UpdatePosition -= UpdatePostionIndicator;
        CarLapData.RaceFinsihedForCar -= ShowFinsishedUI;

        CustomCarController.StartBoost -= StartBoostDrain;
        CustomCarController.StopBoost -= StartBoostRecharge;
    }

    /// <summary>
    /// Hides the normal race UI for this player and just shows the finsihed UI when the other cars have not finished
    /// </summary>
    public void ShowFinsishedUI(int playerID)
    {
        if (playerID == UIPlayerID)
        {
            racingUI.SetActive(false);
            finsihedUI.SetActive(true);
        }
    }

    /// <summary>
    /// Updates the boost UI based on the current boost UI State
    /// </summary>
    private void UpdateBoostUI()
    {
        //Choose What to do based on the current UI Status
        switch (boostState)
        {
            case BoostUIStatus.DRAIN:
                boostBar.value -= boostDrainRate * Time.deltaTime;
                break;
            case BoostUIStatus.RECHARGE:
                boostBar.value += boostRechargeRate * Time.deltaTime;
                break;
                
        }

        //If boost is at max then boost status should be static
        if(boostBar.value >= boostBar.maxValue)
        {
            boostState = BoostUIStatus.STATIC;
            boostText.text = boostStaticText;
        }

        //Update Colour of Boost UI
        boostBar.image.color = Color.Lerp(Color.red, Color.green, boostBar.value / boostBar.maxValue);
    }

    /// <summary>
    /// Starts the Boost UI Draining the boost bar
    /// </summary>
    /// <param name="boostDuration"></param>
    /// <param name="boostCooldown"></param>
    private void StartBoostDrain(float boostDuration, float boostCooldown, int playerID)
    {
        //Check we are updaing the right UI
        if (playerID == UIPlayerID)
        {
            //Change Boost Drain Rate
            boostDrainRate = 1 / boostDuration;
            boostText.text = boostDrainText;
            boostState = BoostUIStatus.DRAIN;
        }
    }

    /// <summary>
    /// Starts the Boost Bar UI Recharging
    /// </summary>
    /// <param name="boostDuration"></param>
    /// <param name="boostCooldown"></param>
    private void StartBoostRecharge(float boostDuration, float boostCooldown, int playerID)
    {
        //Check we are updaing the right UI
        if (playerID == UIPlayerID)
        {
            boostRechargeRate = 1 / boostCooldown;
            boostText.text = boostRechargeText;
            boostState = BoostUIStatus.RECHARGE;
        }
    }

    private void UpdatePostionIndicator(int position, int playerID)
    {
        //Check we are updaing the right UI
        if (playerID == UIPlayerID)
        {
            currentPositionText.text = position.ToString();
            currentPositionSuffixText.text = postionSuffixDictonary[position];
        }
    }

    /// <summary>
    /// Updates the current lap time counter
    /// </summary>
    /// <param name="laptime">Lap Time</param>
    /// <param name="playerID">Player UI to Update</param>
    private void UpdateCurrentLap(float laptime, int playerID)
    {
        //Check we are updaing the right UI
        if(playerID == UIPlayerID)
        {
            currentLapTimeText.text = TimerHelper.FormatLapTime(laptime);
        }
    }

    /// <summary>
    /// Updates the best lap time for this player
    /// </summary>
    /// <param name="laptime"></param>
    /// <param name="playerID"></param>
    private void UpdateBestLap(float laptime, int playerID)
    {
        //Check if we are updating the UI relating to the correct car
        if (playerID == UIPlayerID)
        {
            bestLaptimeText.text = TimerHelper.FormatLapTime(laptime);
        }
    }

    /// <summary>
    /// Updates the lap counter
    /// </summary>
    /// <param name="numberOfLaps"></param>
    /// <param name="playerID"></param>
    private void UpdateLapCounter(int numberOfLaps, int playerID)
    {
        //Check if we are updating the UI relating to the correct car
        if (playerID == UIPlayerID)
        {
            currentLapText.text = numberOfLaps.ToString();
        }
    }
}
