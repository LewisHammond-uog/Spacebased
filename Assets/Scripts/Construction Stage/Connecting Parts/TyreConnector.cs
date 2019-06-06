using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyreConnector : PartConnector {

    ///// <summary>
    ///// Connects 4 Tyres to the car based off the currently held tyres by the player
    ///// </summary>
    ///// <param name="part"></param>
    ///// <param name="player"></param>
    //protected override void ConnectPart(CarPart part, Pickup player)
    //{

    //    //Check if we have a part already connected
    //    if (attachedPart != null)
    //    {
    //        DisconnectPart(attachedPart);
    //    }

    //    //Get the tyre locations for this object
    //    GameObject[] tyreAttachPoints = carManager.GetTyreConnectors();

    //    //Put all the tyres on the connector
    //    for (int i = 0; i < tyreAttachPoints.Length; i++)
    //    {
    //        //Create a tyre and put it on the stack, give it the correct properties
    //        GameObject newTyre = Instantiate(partPrefab);
    //        newTyre.GetComponent<CarPart>().myPartType = CarPart.PART_TYPE.TYRE;
    //        newTyre.GetComponent<CarPart>().myPartQuality = part.myPartQuality;

    //        //Transform it to be on the right postion
    //        newTyre.transform.position = tyreAttachPoints[i].transform.position;
    //        newTyre.transform.parent = tyreAttachPoints[i].transform;

    //        //Set the Attach Points Attached Part
    //        tyreAttachPoints[i].GetComponent<PartConnector>().attachedPart = newTyre;
    //    }


    //    //Destroy the tyre stack
    //    Destroy(player.heldPart);
    //}

    ///// <summary>
    ///// Disconnects all tyres from the car
    ///// </summary>
    ///// <param name="part"></param>
    //protected override void DisconnectPart(GameObject part)
    //{
    //    //Get the tyre locations for this object
    //    GameObject[] tyreAttachPoints = carManager.GetTyreConnectors();

    //    //Put all the tyres on the connector
    //    for (int i = 0; i < tyreAttachPoints.Length; i++)
    //    {
    //        //Destory Part
    //        Destroy(part);

    //        //Set the Attach Points Attached Part
    //        tyreAttachPoints[i].GetComponent<PartConnector>().attachedPart = null;
    //    }
    //}
}
