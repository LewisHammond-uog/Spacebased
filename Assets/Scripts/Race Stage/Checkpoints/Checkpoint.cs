using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [SerializeField]
    protected CheckpointManager myCheckpointManager;

    protected virtual void OnTriggerEnter(Collider other)
    {
        //When the car passes in to the checkpoint send off to the checkpoint manager
        //that this checkpoint has been passed so that it can check if this car has 
        //completed all the checkpoints at the end of the lap
        if(other.tag == "Player Car")
        {
            myCheckpointManager.RegisterCheckpointPass(other.GetComponent<CarLapData>(), this);
        }
    }

}
