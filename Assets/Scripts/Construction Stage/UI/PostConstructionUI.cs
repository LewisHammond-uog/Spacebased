using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostConstructionUI : MonoBehaviour {

    private const float constructionFinishedDuration = 15f;
    private const float constructionsControlsDuration = 10f;
    private float constructionFinsihedCountdown;
    private bool uiFinished = false;

    [SerializeField]
    private Image backgroundPanel;
    private float backgroundPanelAlpha;
    private float backgroundPanelTargetAlpha = 0.8f;
    private float backgroundPanelFadePerSecond = 0.5f;

    [SerializeField]
    private Image controlsImage;

    [SerializeField]
    private Text countdownText;
    private const string countdownFinsihedText = "Loading...";

    /// <summary>
    /// Initalises Elements of the UI
    /// </summary>
    public void InitaliseUI()
    {
        //Start background panel Alpha at 0 si tgat we can fade it out
        backgroundPanelAlpha = 0f;
        backgroundPanel.color = new Vector4(0, 0, 0, backgroundPanelAlpha);
        controlsImage.color = Vector4.zero;

        //Setup timer
        constructionFinsihedCountdown = constructionFinishedDuration;

        //SEt timer to be inactive
        countdownText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Updates the status of the UI
    /// </summary>
    public void UpdateUI()
    {
        //Take away from warmup timer
        constructionFinsihedCountdown -= Time.deltaTime;

        //If the background has not faded in to its target alpha then keep fading in
        if (backgroundPanelAlpha < backgroundPanelTargetAlpha)
        {
            backgroundPanelAlpha += backgroundPanelFadePerSecond * Time.deltaTime;
            backgroundPanel.color = new Vector4(0, 0, 0, backgroundPanelAlpha);
        }

        //If Construction Finished Timer only has 10 seconds left then show the controls for the next stage of the game
        if(constructionFinsihedCountdown < constructionsControlsDuration && !uiFinished)
        {
            if (!countdownText.gameObject.activeInHierarchy)
            {
                countdownText.gameObject.SetActive(true);
            }

            countdownText.text = Mathf.RoundToInt(constructionFinsihedCountdown).ToString();
            controlsImage.color = new Vector4(1, 1, 1, 1);
        }

        if(constructionFinsihedCountdown < 0)
        {
            if (countdownText.gameObject.activeInHierarchy)
            {
                countdownText.text = countdownFinsihedText;
            }

            uiFinished = true;
        }

    }

    /// <summary>
    /// Gets if this UI is finsihed
    /// </summary>
    public bool GetUIFinsihed()
    {
        return uiFinished;
    }
}
