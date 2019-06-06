using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    //Control Prompts so we can ping pong its size
    [SerializeField]
    private GameObject controlPrompts;
    private readonly Vector3 controlPromptsBaseSize = Vector3.one;
    private readonly Vector3 controlPromptsMaxSize = new Vector3(1.1f, 1.1f, 1.1f);
    private const float promptsPingPongSpeed = 0.1f;

    private enum AnimateDirection
    {
        INCREASE,
        DECREASE
    }
    private AnimateDirection controlPromptsScaleDirection = AnimateDirection.INCREASE;

    //Scenes
    [SerializeField]
    private string startSceneName;

    //Music Controler
    [SerializeField]
    private MusicController musicControllerPrefab;

    void Start()
    {
        //If a music controler doesn't exist then create on so that we have music
        if(FindObjectOfType<MusicController>() == null)
        {
            Instantiate(musicControllerPrefab);
        }
    }

    // Update is called once per frame
    void Update () {

        DoPromptPingPong();

        //Check if player 1 has pressed to start the game or quit
        if(ControlManager.GetButtonInputThisFrame("A", ControlManager.leadPlayerID))
        {
            SceneManager.LoadScene(startSceneName);
        }else if(ControlManager.GetButtonInputThisFrame("B", ControlManager.leadPlayerID))
        {
            Application.Quit();
        }


	}

    /// <summary>
    /// Ping Pongs the control prompt text and butons on screen
    /// </summary>
    private void DoPromptPingPong()
    {
        //Determine Direction for control scale to do
        if (controlPrompts.transform.localScale.x >= controlPromptsMaxSize.x)
        {
            controlPromptsScaleDirection = AnimateDirection.DECREASE;
        }
        else if (controlPrompts.transform.localScale.x <= controlPromptsBaseSize.x)
        {
            controlPromptsScaleDirection = AnimateDirection.INCREASE;
        }

        //Actually move the text/buttons
        if (controlPromptsScaleDirection == AnimateDirection.INCREASE)
        {
            controlPrompts.transform.localScale += Vector3.one * promptsPingPongSpeed * Time.deltaTime;
        }
        else if (controlPromptsScaleDirection == AnimateDirection.DECREASE)
        {
            controlPrompts.transform.localScale -= Vector3.one * promptsPingPongSpeed * Time.deltaTime;
        }
    }
}
