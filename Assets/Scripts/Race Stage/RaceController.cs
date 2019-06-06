using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceController : MonoBehaviour {

    [SerializeField]
    private CustomCarController[] cars;

    [SerializeField]
    private CarPartEffects carPartProperties;

    //UIs for Pre, during and post race
    [SerializeField]
    private GameObject PreRaceUI, InRaceUI, PostRaceUI;

    [SerializeField]
    private int totalRaceLaps = 3;

    //Enum for storing the current state of the race
    private enum RaceStatues
    {
        RACE_WARMUP,
        RACE_ACTIVE,
        RACE_FINSIHED
    }
    private RaceStatues currentRaceStatus = RaceStatues.RACE_WARMUP;

    private void Start()
    {
        //Start the race in warm up mode
        StartWarmup();
    }

    private void Update()
    {
        //Update different UIs based on the race stage
        switch (currentRaceStatus)
        {
            case RaceStatues.RACE_WARMUP:
                DoWarmupStep();
                break;
            case RaceStatues.RACE_FINSIHED:
                DoRaceFinishedStep();
                break;
        }

    }

    /// <summary>
    /// Starts the race warmup stage
    /// </summary>
    private void StartWarmup()
    {
       
        //Freeze all of the player cars
        foreach(CustomCarController car in cars)
        {
            car.SetFrozen(true);
        }

        //Set UI active status
        PreRaceUI.SetActive(true);
        InRaceUI.SetActive(false);
        PostRaceUI.SetActive(false);

        PreRaceUI.GetComponent<PreRaceUIController>().InitaliseUI();

    }
    
    /// <summary>
    /// Does warmup step, updating the warm up UI
    /// </summary>
    private void DoWarmupStep()
    {

        PreRaceUI.GetComponent<PreRaceUIController>().UpdateUI();

        //If this UI is finished then set us to go racing
        if (PreRaceUI.GetComponent<PreRaceUIController>().GetUIFinished())
        {
            StartRace();
        }
    }

    /// <summary>
    /// Starts the race section of the game
    /// </summary>
    private void StartRace()
    {

        //UnFreeze all of the player cars
        foreach (CustomCarController car in cars)
        {
            car.SetFrozen(false);
        }

        currentRaceStatus = RaceStatues.RACE_ACTIVE;

        //Set UI active status
        PreRaceUI.SetActive(false);
        InRaceUI.SetActive(true);
        PostRaceUI.SetActive(false);
    }

    /// <summary>
    /// Starts of the race finsihed UI Functions
    /// </summary>
    public void StartRaceFinished(Dictionary<int, int> finalRacePositions)
    {

        //Set Status
        currentRaceStatus = RaceStatues.RACE_FINSIHED;

        //Set UI active status
        PreRaceUI.SetActive(false);
        InRaceUI.SetActive(false);
        PostRaceUI.SetActive(true);

        //Transfer Postions through to UI
        PostRaceUI.GetComponent<PostRaceUI>().InitaliseUI(finalRacePositions);

    }

    /// <summary>
    /// Updates the UI in the race finsihed segment of the game
    /// </summary>
    private void DoRaceFinishedStep()
    {
        PostRaceUI.GetComponent<PostRaceUI>().UpdateUI();
    }
    

    /// <summary>
    /// Sets the part data of all the cars
    /// </summary>
    /// <param name="carData"></param>
    /// <param name="numOfCars"></param>
    public void SetCarData(Dictionary<CarPart.PART_TYPE, CarPart.PART_QUALITY>[] carData)
    {
        //Loop through all of the cars and send of the part data for that car
        for(int i = 0; i < cars.Length; i++)
        {
            //Pass car data in to car function to load car data
            cars[i].LoadCarPartsOnToCar(carData[i], carPartProperties);
        }
    }
    
    /// <summary>
    /// Returns a list of the active cars
    /// </summary>
    /// <returns></returns>
    public CustomCarController[] GetCarList()
    {
        return cars;
    }

    /// <summary>
    /// Returns a list of the active cars
    /// </summary>
    /// <returns></returns>
    public CarLapData[] GetCarsLapDataComponent()
    {
        int i = 0; //incrementor for car array
        CarLapData[] carLapDatas = new CarLapData[cars.Length];

        foreach(CustomCarController car in cars)
        {
            carLapDatas[i] = car.gameObject.GetComponent<CarLapData>();
            i++;
        }

        return carLapDatas;
    }

    /// <summary>
    /// Checks each car and if it has finsihed the race and sets their status. Then checks if all cars have
    /// finsihed the race
    /// </summary>
    public bool GetRaceFinished()
    {

        bool allCarsRaceFinised = true;

        //Check each car if they have completed the required laps, if so then set that car to have completed the race
        foreach(CustomCarController carController in cars)
        {
            //Get car lap data component
            CarLapData carData = carController.gameObject.GetComponent<CarLapData>();
            if(carData.GetNumberOfCompletedLaps() >= totalRaceLaps && !carData.GetRaceFinished())
            {
                //Set the race to be finished and stop the car from accepting more inputs
                carData.SetRaceCompleted();
                carController.SetFrozen(true);
            }else if (!carData.GetRaceFinished())
            {
                //If Race is not finished then set to false
                allCarsRaceFinised = false;
            }
            
        }

        //Return if all the cars have finsihed the race
        return allCarsRaceFinised;
    }

    /// <summary>
    /// Gets the number of total race laps
    /// </summary>
    public int GetTotalRaceLaps()
    {
        return totalRaceLaps;
    }

}
