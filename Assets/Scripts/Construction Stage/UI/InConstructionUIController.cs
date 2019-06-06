using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InConstructionUIController : MonoBehaviour {

    [SerializeField]
    InConstructionUIPlayer[] playerUIList;

    [Space(5)]
    //Race Construction Controller that we can get the remaining time from
    [SerializeField]
    private ConstructionController constructionController;

    [SerializeField]
    private Text countdownTextMinutes, countdownTextSeconds;

    /// <summary>
    /// Initalises elements of the InConstruction UI
    /// </summary>
	public void InitaliseUI()
    {
        //Initalise each of the player UI
        foreach(InConstructionUIPlayer playerUI in playerUIList)
        {
            playerUI.InitaliseUI();
        }

        //Startup the timer
    }

    public void UpdateUI()
    {
        //Update each of the player UIs
        foreach (InConstructionUIPlayer playerUI in playerUIList)
        {
            playerUI.UpdateUI();
        }

        //Update Timer UI
        //Calcuate Minutes/Seconds/Milliseconds
        int timeMinutes = Mathf.FloorToInt((constructionController.constructionTimer) / 60);
        int timeSeconds = Mathf.FloorToInt((constructionController.constructionTimer) % 60);

        countdownTextMinutes.text = TimerHelper.ConvertTimeToString(timeMinutes);
        countdownTextSeconds.text = TimerHelper.ConvertTimeToString(timeSeconds);

    }
}
