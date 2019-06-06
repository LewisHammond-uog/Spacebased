using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarConstructionManager : MonoBehaviour {

    public GameObject assignedPlayer;

    [SerializeField]
    private PartConnector partConnector;

    private const CarPart.PART_QUALITY defaultCarPartQuality = CarPart.PART_QUALITY.POOR;

    /// <summary>
    /// Returns an dictonary of car part qualities
    /// </summary>
    /// <returns></returns>
    public Dictionary<CarPart.PART_TYPE, CarPart.PART_QUALITY> GetCarPartQualities()
    {

        //Get all of the parts connected on the part connector
        var connectedCarParts = partConnector.GetAttachedParts();

        //Dictonary for storing all the car parts and qualities
        var carPartQualtiyDictonary = new Dictionary<CarPart.PART_TYPE, CarPart.PART_QUALITY>();

        //For each car part type check if it is in the car part dictonary that we have from the part connector,
        //if it is then add it and its quality to the carPartQuality dictonary, if no part was connected of that
        //type then assign it a default value of null
        Array carPartTypes = Enum.GetValues(typeof(CarPart.PART_TYPE));

        foreach(CarPart.PART_TYPE connectablePartType in carPartTypes)
        {
            if (connectedCarParts.ContainsKey(connectablePartType))
            {
                CarPart part = connectedCarParts[connectablePartType].GetComponent<CarPart>();
                carPartQualtiyDictonary.Add(connectablePartType, part.myPartQuality);
            }
            else
            {
                //Assign default "bad" value
                carPartQualtiyDictonary.Add(connectablePartType, defaultCarPartQuality);
            }
        }

        //Return the completed dictonary
        return carPartQualtiyDictonary;

    }

}
