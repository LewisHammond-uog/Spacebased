using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PreConstructionUIController : MonoBehaviour {

    [SerializeField]
    private float constructionWarpupDuration = 10;
    private float constructionWarmupTimer;

    [SerializeField]
    private ConstructionController constructionController;

    [SerializeField]
    private Image backgroundPanel;
    private float backgroundPanelAlpha;
    private float backgroundPanelTargetAlpha = 0.4f;
    private float backgroundPanelFadePerSecond = 0.2f;

    [SerializeField]
    private Text flavourText;
    private const string flavourTextPrefix = "You have ";
    private const string flavourTextSuffix = " seconds to assemble your car";

    [SerializeField]
    private Text countdownText;

    /// <summary>
    /// Initalises Elements of the UI
    /// </summary>
    public void InitaliseUI()
    {
        //Start background panel Alpha at 1 so that we can fade in 
        backgroundPanelAlpha = 1f;
        backgroundPanel.color = new Vector4(0,0,0,backgroundPanelAlpha);

        //Set the flavour text of the UI to show the correct time for the construction phase
        flavourText.text = flavourTextPrefix + constructionController.constructionLevelDuration + flavourTextSuffix;

        //Setup timer
        constructionWarmupTimer = constructionWarpupDuration;
    }

    /// <summary>
    /// Updates the status of the UI
    /// </summary>
    public void UpdateUI()
    {
        //Take away from warmup timer
        constructionWarmupTimer -= Time.deltaTime;

        //Update UI Text
        if(Mathf.RoundToInt(constructionWarmupTimer) == 0)
        {
            countdownText.text = "GO!";
        }
        else
        {
            countdownText.text = Mathf.RoundToInt(constructionWarmupTimer).ToString();
        }

        //If the background has not faded out to its target alpha then keep fading out
        if(backgroundPanelAlpha > backgroundPanelTargetAlpha)
        {
            backgroundPanelAlpha -= backgroundPanelFadePerSecond * Time.deltaTime;
            backgroundPanel.color = new Vector4(0, 0, 0, backgroundPanelAlpha);
        }

    }

    /// <summary>
    /// Returns whether this UI has finsihed its segment
    /// </summary>
    /// <returns></returns>
    public bool GetUIFinished()
    {
        //If timer has reached 0 then we are done
        if(constructionWarmupTimer <= 0f)
        {
            return true;
        }

        return false;
    }
}
