using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSequenceMinigame : MiniGame {


    //Enum for if the button we have pressed is what is next in the sequence,
    //Either PASS for correct, FAIL for incorrect or NONE if it is not a key used
    //in the minigame or no key pressed
    private enum MINIGAME_BUTTON_PRESS
    {
        PASS,
        FAIL,
        NONE
    }

    private ControlManager.DPAD_BUTTON[] buttonSequence;
    private int sequenceProgress = 0;
    private int sequenceStartLength = 5;
    private ControlManager.DPAD_BUTTON[] possibleButtons = { ControlManager.DPAD_BUTTON.UP,
                                                            ControlManager.DPAD_BUTTON.DOWN,
                                                            ControlManager.DPAD_BUTTON.LEFT,
                                                            ControlManager.DPAD_BUTTON.RIGHT };

    private ConstructionCharacterController minigamePlayer;
    private PartConnector minigamePart;
    private bool minigameActive = false;

    [Header("Button Prompts")]

    [SerializeField]
    private Sprite buttonImageDPadUp, buttonImageDPadDown, buttonImageDPadLeft, buttonImageDPadRight;

    [SerializeField]
    private GameObject controlButtonRenderer;

    [SerializeField]
    private Vector3 controlPromptScale;

    [SerializeField]
    private Vector3 spriteOffset, spriteRotation;

    /// <summary>
    /// Initalises the minigame
    /// </summary>
    /// <param name="player">Player that is playing the game</param>
    /// <param name="minigamePartConnector">Part that this minigame is being played about</param>
    public override void InitaliseMinigame(ConstructionCharacterController player, PartConnector minigamePartConnector)
    {
        minigamePlayer = player;
        minigamePart = minigamePartConnector;
        minigameActive = true;

        //Generate a sequence of buttons for the player to press
        buttonSequence = GenerateButtonSequence(sequenceStartLength);

        //Stop the player from moving during this segment
        player.SetFrozen(true);

        //Set Control Button Renderer to correct postion
        transform.position = minigamePart.transform.position;
        controlButtonRenderer.transform.position = minigamePart.transform.position;
        


    }
	
	// Update is called once per frame
	private void Update () {


        //If the minigame is active and the player is not null then do the minigame
        if (minigameActive && minigamePlayer)
        {
            DoMinigame();

            //Check if minigame is complete
            if (sequenceProgress >= (buttonSequence.Length - 1))
            {
                FinishMinigame();
                return;
            }

            DrawControlPrompt();
            
        }
	}

    /// <summary>
    /// Does minigame processing
    /// </summary>
    private void DoMinigame()
    {
        //Get the current button that needs to be pressed
        ControlManager.DPAD_BUTTON currentButton = buttonSequence[sequenceProgress];

        //Check that our target button has been pressed
        if (CheckMiniGameButton(currentButton, minigamePlayer) == MINIGAME_BUTTON_PRESS.PASS)
        {
            //Move on the sequence
            sequenceProgress++;
        }
        
    }

    /// <summary>
    /// Draws the control prompt of the next control that the user need to press
    /// </summary>
    private void DrawControlPrompt()
    {

        Sprite spriteToRender;

        switch (buttonSequence[sequenceProgress])
        {
            case ControlManager.DPAD_BUTTON.UP:
                spriteToRender = buttonImageDPadUp;
                break;
            case ControlManager.DPAD_BUTTON.DOWN:
                spriteToRender = buttonImageDPadDown;
                break;
            case ControlManager.DPAD_BUTTON.LEFT:
                spriteToRender = buttonImageDPadLeft;
                break;
            case ControlManager.DPAD_BUTTON.RIGHT:
                spriteToRender = buttonImageDPadRight;
                break;
            default:
                //If it is none of these options
                //then don't try and render anything
                return;
        }

        controlButtonRenderer.GetComponent<SpriteRenderer>().sprite = spriteToRender;
        controlButtonRenderer.transform.localScale = controlPromptScale;
        controlButtonRenderer.transform.eulerAngles = spriteRotation;
        controlButtonRenderer.transform.position = minigamePlayer.gameObject.transform.position + spriteOffset;

    }

    /// <summary>
    /// Check if the correct key has been pressed in the minigame sequence
    /// </summary>
    /// <param name="button"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    private MINIGAME_BUTTON_PRESS CheckMiniGameButton(ControlManager.DPAD_BUTTON button, ConstructionCharacterController player)
    {

        //Check for correct key press
        if (ControlManager.GetDPadButtonThisFrame(button, player))
        {
            return MINIGAME_BUTTON_PRESS.PASS;
        }
        else
        {
            //Check all of the possible controls that we could be using in the minigame, if one of those has
            //been pressed then fail the test else return none. Here all buttons are checked, including the one
            //that is correct however this is already caught by the if statement above so should never get
            //to this stage
            for (int i = 0; i < possibleButtons.Length; i++)
            {
                //Check if we have pressed one of those buttons
                ControlManager.DPAD_BUTTON buttonToCheck = possibleButtons[i];

                if (ControlManager.GetDPadButtonThisFrame(buttonToCheck, player))
                {
                    //We have pressed the wrong button, return fail state
                    return MINIGAME_BUTTON_PRESS.FAIL;
                }
            }
        }

        //Return none if none of the other conditions have been met
        return MINIGAME_BUTTON_PRESS.NONE;
        
    }

    /// <summary>
    /// Resets the minigame
    /// </summary>
    private void ResetMinigame()
    {
        //Generate a new sequence
        buttonSequence = GenerateButtonSequence(sequenceStartLength);

        //Reset our progress
        sequenceProgress = 0;
    }

    /// <summary>
    /// Completes the minigame that we are currently running
    /// </summary>
    private void FinishMinigame()
    {
        minigameActive = false;

        //Set part connector to conenct the parts and finishing the minigame
        minigamePart.MinigameSuccess();

        //Allow the player to move
        minigamePlayer.SetFrozen(false);

        //Delete this minigame
        Destroy(gameObject);
    }

    /// <summary>
    /// Generates a sequence of buttons
    /// </summary>
    /// <returns>Sequence of buttons</returns>
    public ControlManager.DPAD_BUTTON[] GenerateButtonSequence(int length)
    {

        ControlManager.DPAD_BUTTON[] buttonSequence = new ControlManager.DPAD_BUTTON[length];

        //Generate an array of strings that contain the names of the controls as they are in unitys input manager
        for(int i = 0; i < length; i++)
        {
            int randomButtonIndex = Random.Range(0, possibleButtons.Length);
            buttonSequence[i] = possibleButtons[randomButtonIndex];
        }

        return buttonSequence;
    }
}
