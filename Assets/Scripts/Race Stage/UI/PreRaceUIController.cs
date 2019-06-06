using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreRaceUIController : MonoBehaviour {

    [SerializeField]
    private Text countdownText;
    private string countdownPrevFrame;
    private const float raceWarmupDuration = 3f;
    private float raceWarmupTimer;
    private bool UIFinsihed = false;

    [SerializeField]
    private AudioSource countdownBeep;

    [SerializeField]
    private AudioSource countdownFinish;


    /// <summary>
    /// Intitalise and setup all of the values and methods for the UI
    /// </summary>
	public void InitaliseUI()
    {
        //Setup Countdown Timer
        raceWarmupTimer = raceWarmupDuration;
    }

    /// <summary>
    /// Updates the status of the UI
    /// </summary>
    public void UpdateUI() {

        //Countdown timer
        raceWarmupTimer -= Time.deltaTime;

        //Update UI Text
        if (Mathf.RoundToInt(raceWarmupTimer) == 0)
        {
            countdownText.text = "GO!";
            countdownBeep.Play();
            UIFinsihed = true;

        }
        else
        {
            countdownText.text = Mathf.RoundToInt(raceWarmupTimer).ToString();

            if(countdownText.text != countdownPrevFrame)
            {
                countdownBeep.Play();
            }

            countdownPrevFrame = countdownText.text;
        }

    }

    

    /// <summary>
    /// Gets if this UI element is finished
    /// </summary>
    /// <returns></returns>
    public bool GetUIFinished()
    {
        return UIFinsihed;
    }
}
