using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartConnector : MonoBehaviour {

    [SerializeField]
    private CarConstructionManager carManager;

    [SerializeField]
    private GameObject miniGameTemplate;
    private GameObject currentMiniGame;

    private GameObject attemptedConnectionPart;
    private GameObject attemptedConnectionPlayer;

    private Dictionary<CarPart.PART_TYPE, GameObject> attachedParts = new Dictionary<CarPart.PART_TYPE, GameObject>();
    private GameObject[] attachedTyres; //Array stores all our attached tires because attached parts dictonary only stores the first

    [SerializeField]
    private GameObject bodyPrefab, enginePrefab, turboPrefab, tyrePrefab;

    //Hologram components to be disabled when a part is attached
    [SerializeField]
    private GameObject bodyHologram, engineHologram, turboHologram, tyreHologram;

    //Store the indexes of the left and right side tyres so that we can rotate tyres on the right side
    private readonly int[] leftSideTyreIndexes = new int[] { 0, 1};
    private readonly int[] rightSideTyreIndexes = new int[] { 2, 3 };

    [SerializeField]
    private AudioSource partAttachSound;


    /// <summary>
    /// When the player enters the attachment trigger
    /// </summary>
    /// <param name="other"></param>
    /// <returns>
    private void OnTriggerEnter(Collider other)
    { 

        if (other.gameObject == carManager.assignedPlayer)
        {

            //Get the player that is trying to connect a part
            attemptedConnectionPlayer = other.gameObject;

            //Get the player pickup component
            Pickup playerPickup = attemptedConnectionPlayer.GetComponent<Pickup>();

            //Check that we are holding both a part and a tool
            if (playerPickup.heldTool != null && playerPickup.heldPart != null)
            {
                //Check if we are holding the right tool for the part that we are trying to attach
                if(playerPickup.heldPart.GetComponent<HoldablePart>().requiredTool == playerPickup.heldTool.GetComponent<Tool>().myToolType)
                {
                    //We have the right tool and part, start a minigame to attempt to connect a part and set the part that we are trying to connect
                    attemptedConnectionPart = playerPickup.heldPart;
                    partAttachSound.Play();
                    StartMinigame(attemptedConnectionPlayer);
                }
            }
        }
    }

    /// <summary>
    /// Starts a minigame to attach a part
    /// </summary>
    private void StartMinigame(GameObject player)
    {
        //Null Check Minigame and check we are curretly not in a minigame
        if(miniGameTemplate != null && currentMiniGame == null)
        {
            //Create and initalise a minigame
            currentMiniGame = Instantiate(miniGameTemplate);
            currentMiniGame.GetComponent<MiniGame>().InitaliseMinigame(player.GetComponent<ConstructionCharacterController>(), this);
            currentMiniGame.transform.parent = transform;
        }
    }

    /// <summary>
    /// Triggered when player successfully completes a connection minigame and connects the part they
    /// attempted to connect
    /// </summary>
    public void MinigameSuccess()
    {
        ConnectPart(attemptedConnectionPart.GetComponent<PartPickup>(), attemptedConnectionPlayer.GetComponent<Pickup>());
        currentMiniGame = null;
    }

    /// <summary>
    /// Connects a part to the car
    /// </summary>
    /// <param name="connectingPart"></param>
    /// <param name="player"></param>
    private void ConnectPart(PartPickup connectingPart, Pickup player)
    {

        //If we already have a part of this type, destroy it 
        if (attachedParts.ContainsKey(connectingPart.myPartType))
        {
            Destroy(attachedParts[connectingPart.myPartType]);
            attachedParts.Remove(connectingPart.myPartType);
        }

        GameObject connectedPart = new GameObject();

        //Choose what part prefab we should spawn
        GameObject partPrefab = GetPartToSpawn(connectingPart.myPartType);

        if (connectingPart.myPartType != CarPart.PART_TYPE.TYRE)
        {
            //Create new part and give it the properties of the old part
            connectedPart = Instantiate(partPrefab);
            connectedPart.GetComponent<CarPart>().myPartQuality = connectingPart.myPartQuality;
            connectedPart.GetComponent<CarPart>().myPartType = connectingPart.myPartType;
            connectedPart.transform.parent = gameObject.transform;
            SetPartInPosition(connectingPart.myPartType, connectedPart); //Set part postion
        }
        else
        {
            GameObject[] tyres = new GameObject[4];

            //For tyres we must create and apply to 4 new objects
            for(int i = 0; i < 4; i++)
            {
                tyres[i] = Instantiate(partPrefab);
                tyres[i].GetComponent<CarPart>().myPartQuality = connectingPart.myPartQuality;
                tyres[i].GetComponent<CarPart>().myPartType = connectingPart.myPartType;
                tyres[i].transform.parent = gameObject.transform;

                //Flip Tyres on the right side
                foreach(int tyreIndex in rightSideTyreIndexes)
                {
                    if(i == tyreIndex)
                    {
                        tyres[i].transform.Rotate(0, 180, 0);
                    }
                }

            }

            //Store attached tires
            attachedTyres = tyres;

            //Set all tyres in postion
            SetTyresInPosition(tyres);

            //Set the "connected part" to the first tyre as we don't need to store all of them becuase all
            //tyres have the same quality values
            connectedPart = tyres[0];

        }

        //Add this part to the connected parts dictonary
        attachedParts.Add(connectingPart.myPartType, connectedPart);

        //Disable Hologram
        DisableHologram(connectingPart.myPartType);

        //Delete old pickup part
        Destroy(connectingPart.gameObject);

        //Drop the tool that the player is golding
        if (player.heldTool != null)
        {
            player.DropObject(player.heldTool);
        }

    }

     /// <summary>
    /// Disconnects a part from this car
    /// </summary>
    /// <param name="destroyPart"></param>
    private void DisconnectPart(GameObject destroyPart)
    {
        attachedParts.Remove(destroyPart.GetComponent<CarPart>().myPartType);
        Destroy(destroyPart);
    }

    /// <summary>
    /// Sets a given part to the correct position on the car, not for tyres
    /// </summary>
    /// <param name="partType">Type of part</param>
    /// <param name="carObject">Physical Car object in the world</param>
    private void SetPartInPosition(CarPart.PART_TYPE partType, GameObject partObject)
    {

        Vector3 partPosition = Vector3.zero;

        //Get the quality of the currently attached body
        CarPart.PART_QUALITY bodyQuality = GetBodyQuality();

        //Switch to get the correct postion
        switch (partType)
        {
            case CarPart.PART_TYPE.BODY:
                partPosition = Vector3.zero;

                //When we are updaing the body, update all of the other parts attached to the car
                //so that they all look correct
                ResetPartsPosition();

                break;
            case CarPart.PART_TYPE.ENGINE:
                partPosition = CarPartAttachmentPostions.GetEngineAttachmentPosition(bodyQuality);
                break;
            case CarPart.PART_TYPE.TURBO:
                partPosition = CarPartAttachmentPostions.GetTurboAttachmentPosition(bodyQuality);
                break;
            default:
                //If we aren't using a part here (i.e tires) just return
                return;
        }

        //Apply the position that we have selected
        partObject.transform.localPosition = partPosition;
       
    }

    /// <summary>
    /// Sets tyres in correct postion on car
    /// </summary>
    /// <param name="tyreObjectArray">Array of tyres</param>
    private void SetTyresInPosition(GameObject[] tyreObjectArray)
    {
        //Get an array of all of the tyre attachment points
        Vector3[] tyreAttachmentPoints = CarPartAttachmentPostions.GetTyreAttachmentPositions(GetBodyQuality());

        //For each of the 4 tyres, set them to go to the connection points
        for (int i = 0; i < 4; i++)
        {
            tyreObjectArray[i].transform.localPosition = tyreAttachmentPoints[i];
        }
    }

    /// <summary>
    /// Resets all of the parts that are currently attached to the car, around the body.
    /// Does not reset the body poistion
    /// </summary>
    private void ResetPartsPosition()
    {
        //Loop through all of the attached parts and reposition them
        foreach(var attachedPart in attachedParts)
        {
            //Special case for attaching all tyres
            if(attachedPart.Key == CarPart.PART_TYPE.TYRE)
            {
                SetTyresInPosition(attachedTyres);
            }
            else if(attachedPart.Key != CarPart.PART_TYPE.BODY)
            {
                SetPartInPosition(attachedPart.Key, attachedPart.Value);
            }
        }
    }

    /// <summary>
    /// Gets the quality of the car body that we are attaching to
    /// </summary>
    /// <returns>Quality of car body</returns>
    private CarPart.PART_QUALITY GetBodyQuality()
    {

        //Work out the quality of the body so that we fit the parts properly,
        //if we don't have a body then assume we have a poor body
        if (attachedParts.ContainsKey(CarPart.PART_TYPE.BODY))
        {
            return attachedParts[CarPart.PART_TYPE.BODY].GetComponent<CarPart>().myPartQuality;
        }else if(attemptedConnectionPart.GetComponent<PartPickup>().myPartType == CarPart.PART_TYPE.BODY){
            return attemptedConnectionPart.GetComponent<PartPickup>().myPartQuality;
        }

        //No Body, assume poor
        return CarPart.PART_QUALITY.POOR;
    }

    /// <summary>
    /// Gets the part to spawn based on part type
    /// </summary>
    /// <param name="partType">Part Type</param>
    /// <returns>Gameobject of Part</returns>
    private GameObject GetPartToSpawn(CarPart.PART_TYPE partType)
    {

        GameObject partObject;

        //Switch to get the correct postion
        switch (partType)
        {
            case CarPart.PART_TYPE.BODY:
                partObject = bodyPrefab;
                break;
            case CarPart.PART_TYPE.ENGINE:
                partObject = enginePrefab;
                break;
            case CarPart.PART_TYPE.TURBO:
                partObject = turboPrefab;
                break;
            case CarPart.PART_TYPE.TYRE:
                partObject = tyrePrefab;
                break;
            default:
                partObject = null;
                break;
        }

        //Return the part object that has been selected
        return partObject;
    }

    /// <summary>
    /// Disables a specified hologram element
    /// </summary>
    /// <param name="partType">Part of hologram to disable</param>
    private void DisableHologram(CarPart.PART_TYPE partType)
    {

        GameObject hologramObject;

        switch (partType)
        {
            case CarPart.PART_TYPE.BODY:
                hologramObject = bodyHologram;
                break;
            case CarPart.PART_TYPE.ENGINE:
                hologramObject = engineHologram;
                break;
            case CarPart.PART_TYPE.TURBO:
                hologramObject = turboHologram;
                break;
            case CarPart.PART_TYPE.TYRE:
                hologramObject = tyreHologram;
                break;
            default:
                return;
        }

        //Disable Object
        hologramObject.SetActive(false);
    }

    /// <summary>
    /// Gets a list of all the parts connected to this car
    /// </summary>
    /// <returns>Dictonary of parts that are connected to this part connector</returns>
    public Dictionary<CarPart.PART_TYPE, GameObject> GetAttachedParts()
    {
        return attachedParts;
    }

}


