using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLapData : MonoBehaviour {

    //Race Manager
    [SerializeField]
    CheckpointManager checkpointManager;

    //Lists for unvisited and visitied checkpoints
    public List<Checkpoint> visitedCheckpoints = new List<Checkpoint>();
    public List<Checkpoint> unvisitedCheckpoints = new List<Checkpoint>();

    //Stores respawn infomation
    private Vector3 carRespawnPosition;
    private readonly Vector3 carRespawnOffset = new Vector3(0f, 5f, 0f); //Offset of respawn so that the car spawns in the air
    private Quaternion carRespawnRotation;

    private float currentlapTime = 0;
    private float bestLapTime = Mathf.Infinity; //Set Best Lap to Infinity so that we beat it on the first lap
    private int completedLaps = 0;
    private int currentPosition = 0;
    public int finalPostion { private set; get; }
    private bool myRaceFinished = false;

    public int playerID { private set; get; }


    #region UI Update Events

    //Event for new best and current lap time
    public delegate void LapTimeUpdate(float laptime, int playerID);
    public static event LapTimeUpdate NewBestLap;
    public static event LapTimeUpdate UpdateCurrentLapTime;

    //Event for updating the number of completed laps
    public delegate void LapCountUpdate(int currentLap, int playerID);
    public static event LapCountUpdate NewLap;

    //Event for race over
    public delegate void RaceOverEvent(int playerID);
    public static event RaceOverEvent RaceFinsihedForCar;

    //Event for updating position
    public delegate void PositionUpdate(int position, int playerID);
    public static event PositionUpdate UpdatePosition;

    #endregion


    private void Start () {
        //Get our player number from the car controller that must be attached to
        //this object
        playerID = GetComponent<CustomCarController>().playerID;

        //Set first respawn point to start finsih line
        carRespawnPosition = checkpointManager.startFinishLine.transform.position + carRespawnOffset;
        carRespawnRotation = checkpointManager.startFinishLine.transform.rotation;
    }
	
	// Update is called once per frame
	private void Update () {

        //Increase Lap Time
        if (UpdateCurrentLapTime != null)
        {
            currentlapTime += Time.deltaTime;
            UpdateCurrentLapTime(currentlapTime, playerID);
        }

	}

    /// <summary>
    /// Gets the point that this car should respawn at
    /// </summary>
    public Vector3 GetRespawnPosition()
    {
        return carRespawnPosition;
    }

    /// <summary>
    /// Gets the rotation of that the car shoud respawn at
    /// </summary>
    public Quaternion GetRespawnRotation()
    {
        return carRespawnRotation;
    }

    /// <summary>
    /// Sets the respawn point for if we reset the car
    /// </summary>
    public void SetRespawnPositionAndRotation(RespawnPoint respawnPoint)
    {
        carRespawnPosition = transform.position + carRespawnOffset;
        carRespawnRotation = transform.rotation;
    }

    /// <summary>
    /// Resets the current lap data for this car
    /// </summary>
    /// <param name="car">Car Data Controller</param>
    public void ResetLap(List<Checkpoint> checkpoints)
    {
        //Clear the visited list and then fill up the visited list
        visitedCheckpoints.Clear();
        unvisitedCheckpoints = new List<Checkpoint>(checkpoints); //Create a new list from checkpoints rather than refrencing that list

        //Reset the lap timer
        ResetLapTime();
    }

    /// <summary>
    /// Finishes up a lap by recording lap time and increasing lap time, Only use when
    /// lap has been succesfully completed
    /// </summary>
    public void FinishLap()
    {
        completedLaps++;
        if(currentlapTime < bestLapTime)
        {
            bestLapTime = currentlapTime;
            NewBestLap(bestLapTime, playerID); //Call new best lap event to update UI
        }

        NewLap(completedLaps + 1, playerID); // Call event to change UI
    }

    /// <summary>
    /// Resets the current lap time
    /// </summary>
    public void ResetLapTime()
    {
        currentlapTime = 0;
    }

    /// <summary>
    /// Gets the number of laps this car has completed
    /// </summary>
    /// <returns></returns>
    public int GetNumberOfCompletedLaps()
    {
        return completedLaps;
    }

    /// <summary>
    /// Gets if this car has completed the race
    /// </summary>
    /// <returns></returns>
    public bool GetRaceFinished()
    {
        if (myRaceFinished)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Sets if this cars race should be finsihed
    /// </summary>
    public void SetRaceCompleted()
    {
        myRaceFinished = true;

        //Set our final position
        finalPostion = currentPosition;

        //Show FINSIHED UI
        if (RaceFinsihedForCar != null)
        {
            RaceFinsihedForCar(playerID);
        }
    }

    /// <summary>
    /// Gets the distance to the next checkpoint
    /// </summary>
    /// <returns>Distance to next checkpoint</returns>
    public float GetDistanceToNextCheckpoint()
    {
        return Vector3.Distance(transform.position, visitedCheckpoints[0].transform.position);
    }
    ///Test

    /// <summary>
    /// Assigns the this car their current postion in the race
    /// </summary>
    /// <param name="postion"></param>
    public void AssignPosition(int postion)
    {
        if (UpdatePosition != null)
        {
            currentPosition = postion;
            UpdatePosition(postion, playerID);
        }
    }
}
