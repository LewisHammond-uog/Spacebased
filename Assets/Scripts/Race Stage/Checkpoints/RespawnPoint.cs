using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for a race respawn point
/// </summary>
public class RespawnPoint : MonoBehaviour {

    /// <summary>
    /// If a car has entered the respawn point then check if 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        //Check the object is a car
        if(other.GetComponent<CarLapData>() != null)
        {
            //Set respawn point to this
            other.GetComponent<CarLapData>().SetRespawnPositionAndRotation(this);

        }

    }

}
