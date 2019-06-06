using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    [SerializeField]
    private Vector3 toolHoldOffset, partHoldOffset;

    public GameObject heldTool { private set; get; }
    public GameObject heldPart { private set; get; }

    private const string PartTag = "Part";
    private const string ToolTag = "Tool";

    void Update()
    {

        //Hold Objects   
        HoldObjects();

        //Drop Parts / Tools
        if (ControlManager.GetButtonInput("X", gameObject.GetComponent<ConstructionCharacterController>()) && heldTool != null)
        {
            DropObject(heldTool);
            heldTool = null;
        }

        if (ControlManager.GetButtonInput("B", gameObject.GetComponent<ConstructionCharacterController>()) && heldPart != null)
        {
            DropObject(heldPart);
            heldPart = null;
        }
    }

    /// <summary>
    /// Object Pick Up Script
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        //Check if this a pickupable object
        bool isPickupable = (other.tag == PartTag || other.tag == ToolTag);
        bool canPickup = false;
        HoldableObject holdableObject = null;

        if (other.tag == PartTag)
        {
            //Get Part Stuff
            HoldablePart holdablePart = other.GetComponent<HoldablePart>();

            //If the object is not being held and we have the right tool for this item then allow ust to pick it up
            if (heldTool != null)
            {
                if (!holdablePart.beingHeld && holdablePart.requiredTool == heldTool.GetComponent<Tool>().myToolType)
                {
                    canPickup = true;
                    holdableObject = holdablePart;
                }
            }
        }else if (other.tag == ToolTag)
        {
            //make sure tool is not being held
            holdableObject = other.GetComponent<HoldableObject>();

            if (!holdableObject.beingHeld)
            {
                canPickup = true;
            }
        }

        //If we can pickup the object then allow the user to pick it up if they have press A
        if(canPickup && ControlManager.GetButtonInput("A", gameObject.GetComponent<ConstructionCharacterController>()) && holdableObject != null)
        {
            PickupObject(other.gameObject);
            holdableObject.beingHeld = true;
        }

    }

    /// <summary>
    /// Pickup an object
    /// </summary>
    /// <param name="obj"></param>
    private void PickupObject(GameObject obj)
    {

        //Work out if we have a tool or item
        if(obj.tag == ToolTag)
        {

            if(heldTool != null)
            {
                DropObject(heldTool);
            }

            //Set our held tool
            heldTool = obj;
        }else if(obj.tag == PartTag)
        {

            //Reset held tool if we are currently holding one
            if (heldPart != null)
            {
                DropObject(heldPart);
            }

            //Set out held object
            heldPart = obj;
        }

    }

    public void DropObject(GameObject obj)
    {
        //Set the held objects being held property to false
        obj.GetComponent<HoldableObject>().beingHeld = false;

        //Work out if we have a tool or item
        if (obj.tag == ToolTag)
        {
            //Set our held tool
            heldTool.transform.position = heldTool.GetComponent<Tool>().startpos;
            heldTool = null;
        }
        else if (obj.tag == PartTag)
        {
            //Set out held object
            heldPart = null;
        }

    }

    /// <summary>
    /// Hold our objects
    /// </summary>
    private void HoldObjects()
    {

        //Get Player Postion, so that we can hold relative to that
        Vector3 playerPostion = gameObject.transform.position;


        //Hold Tool
        if (heldTool != null) {
            HoldPart(heldTool, playerPostion + toolHoldOffset);
        }

        //Hold Part
        if(heldPart != null)
        {
            HoldPart(heldPart, playerPostion + partHoldOffset);
        }
    }

    private void HoldPart(GameObject obj, Vector3 postion)
    {

        //Hold our object in position
        obj.transform.position = postion;

    }

}
