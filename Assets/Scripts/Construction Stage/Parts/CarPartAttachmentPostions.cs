using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data class that contains infomation about where different
/// bodies attach different parts to
/// </summary>
public static class CarPartAttachmentPostions {

    //POsitions for each car part to attach for on each body type
    #region Attachment Positions

    //Epic Body
    private static readonly Vector3 epicBodyRaceOffset = new Vector3(-0.37f, 0f, 0f);
    private static readonly Vector3 epicBodyEnginePosition = new Vector3(-0.35f, 0.6f,-2.3f);
    private static readonly Vector3 epicBodyTurboPosition = new Vector3(-0.45f,1.18f,-4.78f);
    private static readonly Vector3[] epicBodyTyresPosition = new Vector3[4] {new Vector3(4.91f,0,3.18f),
                                                                                new Vector3(4.91f,0,-2.97f),
                                                                                new Vector3(-5.9f,0, 3.18f),
                                                                                new Vector3(-5.9f,0, -2.97f),};

    //Good Body
    private static readonly Vector3 goodBodyRaceOffset = new Vector3(-0.04f, 0f, 0f);
    private static readonly Vector3 goodBodyEnginePosition = new Vector3(0f, 0f, -2.49f);
    private static readonly Vector3 goodBodyTurboPosition = new Vector3(0f, 2.55f, -5.48f);
    private static readonly Vector3[] goodBodyTyresPosition = new Vector3[4] {new Vector3(2.78f, 0f,-4.21f),
                                                                                new Vector3(2.78f, 0f,-3.67f),
                                                                                new Vector3(-2.84f,0f,4.21f),
                                                                                new Vector3(-2.84f,0f,-3.59f),};

    //Okay Body
    private static readonly Vector3 okayBodyRaceOffset = new Vector3(-0.09f, 0f, 0f);
    private static readonly Vector3 okayBodyEnginePosition = new Vector3(0f, 0f, -1.68f);
    private static readonly Vector3 okayBodyTurboPosition = new Vector3(-0.07f, 2.2f, -4.87f);
    private static readonly Vector3[] okayBodyTyresPosition = new Vector3[4] {new Vector3(2.92f, 0f, 2.72f),
                                                                                new Vector3(2.92f, 0f, -2.28f),
                                                                                new Vector3(-3.14f, 0f, 2.72f),
                                                                                new Vector3(-3.14f, 0f, -2.27f)};

    //Poor Body
    private static readonly Vector3 poorBodyRaceOffset = new Vector3(0.81f, 0, 0);
    private static readonly Vector3 poorBodyEnginePosition = new Vector3(0.37f, 0.64f, -3.34f);
    private static readonly Vector3 poorBodyTurboPosition = new Vector3(0.5f, 2.05f, -5.03f);
    private static readonly Vector3[] poorBodyTyresPosition = new Vector3[4] {new Vector3(3.48f, 0.83f, 3.09f),
                                                                                new Vector3(3.48f, 0.83f, -2.42f),
                                                                                new Vector3(-2.42f, 0.83f, 3.09f),
                                                                                new Vector3(-2.73f, 0.83f, -2.42f)};

    #endregion


    //Functions for getting different positions

    /// <summary>
    /// Gets the position to attach the Engine on a given body type as
    /// a local position
    /// </summary>
    /// <param name="bodyQuality">Quality of body that part is being attached to</param>
    /// <returns>Position to attach part to</returns>
    public static Vector3 GetBodyOfsetPositions(CarPart.PART_QUALITY bodyQuality)
    {
        //Switch based on body quality,
        //return correct postion for part
        switch (bodyQuality)
        {
            case CarPart.PART_QUALITY.EPIC:
                return epicBodyRaceOffset;
            case CarPart.PART_QUALITY.GOOD:
                return goodBodyRaceOffset;
            case CarPart.PART_QUALITY.OKAY:
                return okayBodyRaceOffset;
            case CarPart.PART_QUALITY.POOR:
                return poorBodyRaceOffset;
            default:
                //return default of a zeroed vector
                return Vector3.zero;
        }
    }

    /// <summary>
    /// Gets the position to attach the Engine on a given body type as
    /// a local position
    /// </summary>
    /// <param name="bodyQuality">Quality of body that part is being attached to</param>
    /// <returns>Position to attach part to</returns>
    public static Vector3 GetEngineAttachmentPosition(CarPart.PART_QUALITY bodyQuality)
    {
        //Switch based on body quality,
        //return correct postion for part
        switch (bodyQuality)
        {
            case CarPart.PART_QUALITY.EPIC:
                return epicBodyEnginePosition;
            case CarPart.PART_QUALITY.GOOD:
                return goodBodyEnginePosition;
            case CarPart.PART_QUALITY.OKAY:
                return okayBodyEnginePosition;
            case CarPart.PART_QUALITY.POOR:
                return poorBodyEnginePosition;
            default:
                //return default of a zeroed vector
                return Vector3.zero;
        }
    }

    /// <summary>
    /// Gets the position to attach the Turbo on a given body type as
    /// a local position
    /// </summary>
    /// <param name="bodyQuality">Quality of body that part is being attached to</param>
    /// <returns>Position to attach part to</returns>
    public static Vector3 GetTurboAttachmentPosition(CarPart.PART_QUALITY bodyQuality)
    {
        //Switch based on body quality,
        //return correct postion for part
        switch (bodyQuality)
        {
            case CarPart.PART_QUALITY.EPIC:
                return epicBodyTurboPosition;
            case CarPart.PART_QUALITY.GOOD:
                return goodBodyTurboPosition;
            case CarPart.PART_QUALITY.OKAY:
                return okayBodyTurboPosition;
            case CarPart.PART_QUALITY.POOR:
                return poorBodyTurboPosition;
            default:
                //return default of a zeroed vector
                return Vector3.zero;
        }
    }

    /// <summary>
    /// Gets the position to attach the Tyres on a given body type as
    /// a local position
    /// </summary>
    /// <param name="bodyQuality">Quality of body that part is being attached to</param>
    /// <returns>Position to attach part to</returns>
    public static Vector3[] GetTyreAttachmentPositions(CarPart.PART_QUALITY bodyQuality)
    {
        //Switch based on body quality,
        //return correct postion for part
        switch (bodyQuality)
        {
            case CarPart.PART_QUALITY.EPIC:
                return epicBodyTyresPosition;
            case CarPart.PART_QUALITY.GOOD:
                return goodBodyTyresPosition;
            case CarPart.PART_QUALITY.OKAY:
                return okayBodyTyresPosition;
            case CarPart.PART_QUALITY.POOR:
                return poorBodyTyresPosition;
            default:
                //return default of a null
                return null;
        }
    }
}
