using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    //List of Checkpoints
    [SerializeField]
    private List<Checkpoint> checkpoints;
    public StartFinishLine startFinishLine;

    [SerializeField]
    private RaceController raceController;

    //Array of cars and sorted array for cars in postion
    private CarLapData[] carLapDatas;
    private CarLapData[] carsInPositions;

    private float trackDistance = 0f;

    private void Start()
    {

        //Get list of cars
        carLapDatas = raceController.GetCarsLapDataComponent();

        //Set up car position array
        carsInPositions = new CarLapData[carLapDatas.Length];

        //Initalise checkpoints by reseting all cars
        foreach (CarLapData car in carLapDatas)
        {
            car.ResetLap(checkpoints);
        }

        //Check that all our checkpoints are valid
        foreach(Checkpoint checkpoint in checkpoints)
        {
            if(checkpoint == null)
            {
                checkpoints.Remove(checkpoint);
            }
        }

        ////Precalculate the total distance of the circuit so that it can be used for car distance calculations
        for (int i = 0; i < checkpoints.Count; i++)
        {
            Vector3 currentCheckpoint = checkpoints[i].transform.position;
            Vector3 nextCheckpoint;

            //Check we are not going to go out of range, if we are then the next (and final checkpoint) is that start/finish line
            if ((i + 1) > (checkpoints.Count - 1))
            {
                nextCheckpoint = startFinishLine.transform.position;
            }
            else
            {
                nextCheckpoint = checkpoints[i + 1].transform.position;
            }

            //Add Distances to lap distance var
            trackDistance += Vector3.Distance(currentCheckpoint, nextCheckpoint);
        }

    }

    private void Update()
    {
        //Update the current postions of the cars
        UpdateCarPostions();
    }

    /// <summary>
    /// Registers a car passing through a specific checkpoint
    /// </summary>
	public void RegisterCheckpointPass(CarLapData car, Checkpoint checkpoint)
    {
        //Find the checkpoint in the unvisited list then move it to the visited list
        if (car.unvisitedCheckpoints.Contains(checkpoint))
        {
            car.unvisitedCheckpoints.Remove(checkpoint);
            car.visitedCheckpoints.Add(checkpoint);
        }
    }

    /// <summary>
    /// Registers car passing through the start finish line, checks if lap
    /// has been succesfully completed and finishes the lap if needed
    /// </summary>
    /// <param name="car"></param>
    public void RegisterStartFinishPass(CarLapData car)
    {
        //If lap is successfully completed then finish up the lap for the car and check if
        //we have finsihed the race
        if (CheckLapComplete(car))
        {
            car.FinishLap();
            car.ResetLap(checkpoints);

            //Check if race has finished
            if (raceController.GetRaceFinished())
            {
                //Create Final Position Dictonary
                //Int Position, Int Car Player ID
                Dictionary<int, int> finalPositionsDictonary = GenerateFinalPostionsDictonary();
                raceController.StartRaceFinished(finalPositionsDictonary);
            }
            
        }
    }

    /// <summary>
    /// Generate a final dictonary of the positions of each of the car
    /// </summary>
    /// <returns></returns>
    private Dictionary<int, int> GenerateFinalPostionsDictonary()
    {
        //Dictonary to store positions
        //Int Postion, Int Player Number
        Dictionary<int, int> finalPositionsDictonary = new Dictionary<int, int>();

        //Add car positions to dictonary with thier player ids
        foreach(CarLapData car in carLapDatas)
        {
            //Check if postion is already in dictonary (i.e two cars finsihed at the same time and have the same postion), 
            //if it is add 1 to the dictonary final pos value as this one was proccessed later

            int positionToSave = car.finalPostion;

            while(finalPositionsDictonary.ContainsKey(positionToSave))
            {
                positionToSave += 1;
            }

            finalPositionsDictonary.Add(positionToSave, car.playerID);
        }

        //return dictonary
        return finalPositionsDictonary;
    }

    /// <summary>
    /// Checks if a car has completed a lap
    /// </summary>
    /// <returns>If lap has been successfully completed</returns>
    private bool CheckLapComplete(CarLapData car)
    {
        //If we have no unvisited checkpoints left then we have completed the lap
        if(car.unvisitedCheckpoints.Count == 0)
        {
            return true;
        }

        //If we still have unvisited check points then we have not completed the lap
        return false;
    }

    /// <summary>
    /// Updates the position of all of the cars attached to this manager
    /// </summary>
    private void UpdateCarPostions()
    {

        //Sorted Dictonary sorts ths distances from cars to leaders next checkpoint and stores the car with that data
        //so that we can make an sorted array of the cars in position order
        SortedDictionary<float, CarLapData> distanceCarDictonary = new SortedDictionary<float, CarLapData>();

        //Record the check point that the leading car is at
        int leadingCheckpointCount = 0;
        int leadingLapsCount = 0;
        
        //Find out the number of checkpoints and laps that the leading car has passed 
        foreach(CarLapData car in carLapDatas)
        {
            int currentCheckpointCount = car.visitedCheckpoints.Count;
            int currentLapCount = car.GetNumberOfCompletedLaps();

            if (currentCheckpointCount > leadingCheckpointCount)
            {
                leadingCheckpointCount = currentCheckpointCount;
            }

            if(currentLapCount > leadingLapsCount)
            {
                leadingLapsCount = currentLapCount;
            }
        }

        //Work out the total distance from each car to the leading cars next checkpoint, by working out the distance to this cars next checkpoint, then, if 
        //nessasarry working out the distance between the intermediate checkpoints
        foreach(CarLapData car in carLapDatas)
        {
            //Work out the distance to our next checkpoint

            //Get our next checkpoint
            Vector3 nextCheckpointPosition = GetCarsNextCheckpoint(car);

            float distanceToNextCheckpoint = Vector3.Distance(car.transform.position, nextCheckpointPosition);
            float intermediateCheckpointDistance = 0;

            //If we are on the lead lap then worry about the distance to the leaders checkpoint otherwise
            //worry about the distance to the start finsih line

            if (car.GetNumberOfCompletedLaps() == leadingLapsCount)
            {
                //Work out if we need to work out the distance to intermediate checkpoints
                if (car.visitedCheckpoints.Count < leadingCheckpointCount)
                {
                    int checkpointDeficit = leadingCheckpointCount - car.visitedCheckpoints.Count;

                    for (int i = 0; i < checkpointDeficit; i++)
                    {
                           
                        intermediateCheckpointDistance += Vector3.Distance(GetCarsNextCheckpoint(car, 0), GetCarsNextCheckpoint(car, 1));
                    }
                }
            }
            else
            {
                //Add on the distance for our laps defict
                intermediateCheckpointDistance += trackDistance * (leadingLapsCount - car.GetNumberOfCompletedLaps());

                //Go through the checkpoints until we get to the start finishline
                for(int i = 0; i < car.unvisitedCheckpoints.Count; i++)
                {
                    intermediateCheckpointDistance += Vector3.Distance(GetCarsNextCheckpoint(car, 0), GetCarsNextCheckpoint(car, 1));
                }
            }

            //Add the total distance to the leading cars next checkpoint to the dictonary of distances
            float distanceToLeadersNextCheckpoint = distanceToNextCheckpoint + intermediateCheckpointDistance;
            distanceCarDictonary.Add(distanceToLeadersNextCheckpoint, car);
            
        }

        int sortedDistancesItterator = 0; //Itterator for adding values to the sorted distances array in the for each loop
        
        //Assign the cars their positions
        foreach(var distanceCarPair in distanceCarDictonary)
        {
            CarLapData currentCar = distanceCarPair.Value;
            carsInPositions[sortedDistancesItterator] = currentCar;
            currentCar.AssignPosition(sortedDistancesItterator + 1);
            sortedDistancesItterator++;
        }



    }



    /// <summary>
    /// Gets the next checkpoint position for a given car or the start finish line if the car is going to refrence
    /// the checkpoint array out of range
    /// </summary>
    /// <param name="car">Car to check</param>
    /// <param name="checkpointID">Number of checkpoints in advance to get</param>
    /// <returns>Vector 3 of Checkpoint</returns>
    private Vector3 GetCarsNextCheckpoint(CarLapData car, int checkpointID = 0)
    {
        if (car.unvisitedCheckpoints.Count <= checkpointID)
        {
            return startFinishLine.transform.position;
        }
        else
        {
            return car.unvisitedCheckpoints[checkpointID].transform.position;
        }
    }


}
