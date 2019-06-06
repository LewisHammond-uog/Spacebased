using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlsInstructionMenu : MonoBehaviour {

    [SerializeField]
    private string nextSceneName;

    [SerializeField]
    private Text countdownText;
    private const string countdownDoneText = "LOADING...";

    private const float waitDuration = 15.5f;
    private float waitTimer;

    private void Start()
    {
        //Initalise Timer
        waitTimer = waitDuration;
    }

    // Update is called once per frame
    void Update () {
		
        //If the timer is over or the lead player has pressed A
        if(waitTimer <= 0 || ControlManager.GetButtonInputThisFrame("A", ControlManager.leadPlayerID))
        {
            countdownText.text = countdownDoneText;
            SceneManager.LoadScene(nextSceneName);
        }
        else if(waitTimer > 0)
        {
            countdownText.text = Mathf.RoundToInt(waitTimer).ToString();
            //Take away from the wait timer
            waitTimer -= Time.deltaTime;
        }
        else
        {
            //Update UI
            countdownText.text = Mathf.RoundToInt(waitTimer).ToString();

            //Take away from the wait timer
            waitTimer -= Time.deltaTime;
        }

	}
}
