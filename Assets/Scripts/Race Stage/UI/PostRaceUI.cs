using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PostRaceUI : MonoBehaviour {

    [SerializeField]
    private Text firstPlacePlayerText, secondPlacePlayerText, thirdPlacePlayerText, forthPlacePlayerText;

    [SerializeField]
    private string mainMenuSceneName;

    private const string playerTextPrefix = "PLAYER ";
    /// <summary>
    /// Initalises This UI
    /// </summary>
	public void InitaliseUI(Dictionary<int, int> poistionPlayerIDDictonary)
    {
        //Apply Text to UI
        firstPlacePlayerText.text = playerTextPrefix + poistionPlayerIDDictonary[1];
        secondPlacePlayerText.text = playerTextPrefix + poistionPlayerIDDictonary[2];
        thirdPlacePlayerText.text = playerTextPrefix + poistionPlayerIDDictonary[3];
        forthPlacePlayerText.text = playerTextPrefix + poistionPlayerIDDictonary[4];
    }


    public void UpdateUI()
    {
        //Check for Lead Player taking game back to main menu
        if (ControlManager.GetButtonInputThisFrame("A", ControlManager.leadPlayerID))
        {
            SceneManager.LoadScene(mainMenuSceneName);

            //Remove the infomation transferer so that the game resets propertly
            RaceLoadingData loadedData = FindObjectOfType<RaceLoadingData>();
            if(loadedData != null)
            {
                Destroy(loadedData.gameObject);
            }

        }
    }
}
