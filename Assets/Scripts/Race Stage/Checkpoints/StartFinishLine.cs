using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFinishLine : Checkpoint {

    protected override void OnTriggerEnter(Collider other)
    {
        //When the car passes in to the checkpoint send off to the checkpoint manager
        //that this checkpoint has been passed so that it can check if this car has 
        //completed all the checkpoints at the end of the lap
        if (other.tag == "Player Car")
        {
            myCheckpointManager.RegisterStartFinishPass(other.GetComponent<CarLapData>());
        }

    }

}
