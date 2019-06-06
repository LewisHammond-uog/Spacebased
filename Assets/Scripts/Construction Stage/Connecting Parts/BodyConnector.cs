using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class BodyConnector : PartConnector {

//    [SerializeField]
//    private GameObject hologramCar;

//    /// <summary>
//    /// Enables the body of the car so that it can be edited by the player
//    /// </summary>
//    /// <param name="part"></param>
//    /// <param name="player"></param>
//    protected override void ConnectPart(CarPart part, Pickup player)
//    {
//        partPrefab.SetActive(true);
//        hologramCar.SetActive(false);

//        //Connect part to connection point that we have touched
//        player.DropObject(part.gameObject);
//        player.DropObject(player.heldTool);

//        //Setup attached part
//        attachedPart = partPrefab;
//        attachedPart.GetComponent<CarPart>().myPartQuality = part.myPartQuality;
//        attachedPart.GetComponent<CarPart>().myPartType = part.myPartType;
//        attachedPart.GetComponent<CarPart>().ChooseModel(); //As this part is already started when the scene wakes, make sure that we force the correct quality model

//        //Delete old pickup part
//        Destroy(part.gameObject);
//    }
//}
