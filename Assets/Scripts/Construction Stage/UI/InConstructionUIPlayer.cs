using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InConstructionUIPlayer : MonoBehaviour {

    //Store the player car so that we can text part qualities
    [SerializeField]
    private CarConstructionManager playerCar;

    //UI Serialisation
    [SerializeField]
    private Text bodyQualityText, engineQualityText, turboQualityText, tyreQualityText;

    //Text displayed on UI if no part is fitted
    private string noPartFittedText = string.Empty;

    //Strings for different part qualities
    private string epicQualityText = "EPIC";
    private string goodQualityText = "GOOD";
    private string okayQualityText = "OKAY";
    private string poorQualityText = "POOR";

    //Colours for different part qualities
    private Color epicQualityColour = new Color32(200,000,236,255); //Dark Purple
    private Color goodQualityColour = new Color32(001,195,046,255); //Dark Green
    private Color okayQualityColour = new Color32(147,178,000,255); //Yellow-Green
    private Color poorQualityColour = new Color32(255,000,000,255); //Bright Red

    /// <summary>
    /// Initalises elements of the InConstruction UI
    /// </summary>
	public void InitaliseUI()
    {
        //Set all of the qualities of the parts to a "blank" or filler value
        bodyQualityText.text = noPartFittedText;
        engineQualityText.text = noPartFittedText;
        turboQualityText.text = noPartFittedText;
        tyreQualityText.text = noPartFittedText;
    }

    /// <summary>
    /// Updates elements of the UI that show the qualties of each part 
    /// </summary>
    public void UpdateUI()
    {
        //Go through each part that is connected to the player car and update
        //the UI text based on the quality of the part attached
        foreach(var partQualityPair in playerCar.GetCarPartQualities())
        {
            Text textToUpdate = GetPartTextToUpdate(partQualityPair.Key);

            //Apply Text and colour
            textToUpdate.text = GetPartQualityText(partQualityPair.Value);
            textToUpdate.color = GetPartQualityColour(partQualityPair.Value);
        }
        
    }


    /// <summary>
    /// Gets the correct text element from the UI to update depending on the 
    /// part type that we are updaiting the text for quality of
    /// </summary>
    /// <param name="carPartType">Part Type to get text for</param>
    /// <returns>Text Element</returns>
    private Text GetPartTextToUpdate(CarPart.PART_TYPE carPartType)
    {

        switch (carPartType)
        {
            case CarPart.PART_TYPE.BODY:
                return bodyQualityText;
            case CarPart.PART_TYPE.ENGINE:
                return engineQualityText;
            case CarPart.PART_TYPE.TURBO:
                return turboQualityText;
            case CarPart.PART_TYPE.TYRE:
                return tyreQualityText;
        }

        return null;
    }

    /// <summary>
    /// Gets the text string to display for each quality of part
    /// </summary>
    /// <param name="carPartQuality">Quality of Part</param>
    /// <returns>Text String</returns>
    private string GetPartQualityText(CarPart.PART_QUALITY carPartQuality)
    {

        switch (carPartQuality)
        {
            case CarPart.PART_QUALITY.EPIC:
                return epicQualityText;
            case CarPart.PART_QUALITY.GOOD:
                return goodQualityText;
            case CarPart.PART_QUALITY.OKAY:
                return okayQualityText;
            case CarPart.PART_QUALITY.POOR:
                return poorQualityText;
                    
        }

        return string.Empty;
    }

    /// <summary>
    /// Gets the correct colour for each quality of part
    /// </summary>
    /// <param name="carPartQuality">Quality of Part</param>
    /// <returns>Colour</returns>
    private Color GetPartQualityColour(CarPart.PART_QUALITY carPartQuality)
    {
        switch (carPartQuality)
        {
            case CarPart.PART_QUALITY.EPIC:
                return epicQualityColour;
            case CarPart.PART_QUALITY.GOOD:
                return goodQualityColour;
            case CarPart.PART_QUALITY.OKAY:
                return okayQualityColour;
            case CarPart.PART_QUALITY.POOR:
                return poorQualityColour;

        }

        return poorQualityColour;
    }
}
