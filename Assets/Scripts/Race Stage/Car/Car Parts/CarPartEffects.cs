using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class stores the properties of all car parts with qualities
/// </summary>
public class CarPartEffects : MonoBehaviour {

    //CAR PART MODELS

    [SerializeField]
    protected GameObject epicBody, goodBody, okayBody, poorBody;

    [SerializeField]
    protected GameObject epicEngine, goodEngine, okayEngine, poorEngine;

    [SerializeField]
    protected GameObject epicTurbo, goodTurbo, okayTurbo, poorTurbo;

    [SerializeField]
    protected GameObject epicTyres, goodTyres, okayTyres, poorTyres;

    //Dictonaries that will store thhe different part models for each part quality
    private Dictionary<CarPart.PART_QUALITY, GameObject> bodyModelDictonary;
    private Dictionary<CarPart.PART_QUALITY, GameObject> engineModelDictonary;
    private Dictionary<CarPart.PART_QUALITY, GameObject> turboModelDictonary;
    private Dictionary<CarPart.PART_QUALITY, GameObject> tyreModelDictonary;
    

    private void Awake()
    {
        //Call function to setup dictonaries thta store car part models with their qualities
        InitaliseCarModelDictonaries();
    }

    /// <summary>
    /// Intialises the dictonaries that store the correct models for the correct qualities of all car parts
    /// </summary>
    private void InitaliseCarModelDictonaries()
    {

        bodyModelDictonary = new Dictionary<CarPart.PART_QUALITY, GameObject>()
        {
            {CarPart.PART_QUALITY.EPIC, epicBody},
            {CarPart.PART_QUALITY.GOOD, goodBody},
            {CarPart.PART_QUALITY.OKAY, okayBody},
            {CarPart.PART_QUALITY.POOR, poorBody}
        };

        engineModelDictonary = new Dictionary<CarPart.PART_QUALITY, GameObject>()
        {
            {CarPart.PART_QUALITY.EPIC, epicEngine},
            {CarPart.PART_QUALITY.GOOD, goodEngine},
            {CarPart.PART_QUALITY.OKAY, okayEngine},
            {CarPart.PART_QUALITY.POOR, poorEngine}
        };

        turboModelDictonary = new Dictionary<CarPart.PART_QUALITY, GameObject>()
        {
            {CarPart.PART_QUALITY.EPIC, epicTurbo},
            {CarPart.PART_QUALITY.GOOD, goodTurbo},
            {CarPart.PART_QUALITY.OKAY, okayTurbo},
            {CarPart.PART_QUALITY.POOR, poorTurbo}
        };

        tyreModelDictonary = new Dictionary<CarPart.PART_QUALITY, GameObject>()
        {
            {CarPart.PART_QUALITY.EPIC, epicTyres},
            {CarPart.PART_QUALITY.GOOD, goodTyres},
            {CarPart.PART_QUALITY.OKAY, okayTyres},
            {CarPart.PART_QUALITY.POOR, poorTyres}
        };

    }

    /// <summary>
    /// Gets the car part properties and model for bodies
    /// </summary>
    /// <param name="quality">Body Quality</param>
    /// <returns></returns>
    public CarPartPropertiesBody GetPartPropertiesBody(CarPart.PART_QUALITY quality)
    {
        CarPartPropertiesBody body = new CarPartPropertiesBody(quality, bodyModelDictonary);
        return body;
    }

    /// <summary>
    /// Gets the car part properties and model for engines
    /// </summary>
    /// <param name="quality">Engine Quality</param>
    /// <returns></returns>
    public CarPartPropertiesEngine GetPartPropertiesEngine(CarPart.PART_QUALITY quality)
    {
        CarPartPropertiesEngine engine = new CarPartPropertiesEngine(quality, engineModelDictonary);
        return engine;
    }

    /// <summary>
    /// Gets the car part properties and model for turbos
    /// </summary>
    /// <param name="quality">Turbo Quality</param>
    /// <returns></returns>
    public CarPartPropertiesTurbo GetPartPropertiesTurbo(CarPart.PART_QUALITY quality)
    {
        CarPartPropertiesTurbo turbo = new CarPartPropertiesTurbo(quality, turboModelDictonary);
        return turbo;
    }

    /// <summary>
    /// Gets the car part properties and model for tyres
    /// </summary>
    /// <param name="quality">Tyre Quality</param>
    /// <returns></returns>
    public CarPartPropertiesTyres GetPartPropertiesTyres(CarPart.PART_QUALITY quality)
    {
        CarPartPropertiesTyres tyres = new CarPartPropertiesTyres(quality, tyreModelDictonary);
        return tyres;
    }

    //------------------------------------------------------------------------------------------
    // Below is a list of classes that modify the car qualities and properties, they are returned by
    // the functions that give the car properties of parts
    // Here are a list of properies that each part controls:
    // BODY: Turn Angle
    // ENGINE: Acceleration, Max Speed
    // TURBO: Boost Speed, Boost Cooldown
    // TYRES: Turn Speed
    //------------------------------------------------------------------------------------------

    #region Car Part Properties Classes

    //Car body class
    public class CarPartPropertiesBody{

        //Private Vars that determine performance levels at each quality, for easy editing
        private const float epicTurnAngle = 150f;
        private const float goodTurnAngle = 140f;
        private const float okayTurnAngle = 130f;
        private const float poorTurnAngle = 130f;

        public float partTurnAngle;
        public GameObject model;

        //Constructor receives quality and runs through to set quality variables
        public CarPartPropertiesBody(CarPart.PART_QUALITY quality, Dictionary<CarPart.PART_QUALITY, GameObject> modelDictonary)
        {
            //Choose Part Properies
            switch (quality)
            {
                case CarPart.PART_QUALITY.EPIC:
                    partTurnAngle = epicTurnAngle;
                    break;
                case CarPart.PART_QUALITY.GOOD:
                    partTurnAngle = goodTurnAngle;
                    break;
                case CarPart.PART_QUALITY.OKAY:
                    partTurnAngle = okayTurnAngle;
                    break;
                case CarPart.PART_QUALITY.POOR:
                    partTurnAngle = poorTurnAngle;
                    break;
                default:
                    partTurnAngle = poorTurnAngle;
                    break;
            }

            //Choose model
            model = modelDictonary[quality];
        }
    }

    //Car engine class
    public class CarPartPropertiesEngine
    {

        //Private Vars that determine performance levels at each quality, for easy editing
        private const float epicAcceleration = 30f;
        private const float goodAcceleration = 27.5f;
        private const float okayAcceleration = 25.5f;
        private const float poorAcceleration = 23f;
        
        private const float epicMaxSpeed = 80f;
        private const float goodMaxSpeed = 79f;
        private const float okayMaxSpeed = 77f;
        private const float poorMaxSpeed = 76f;

        public float partAcceleration;
        public float partMaxSpeed;
        public GameObject model;

        //Constructor receives quality and runs through to set quality variables
        public CarPartPropertiesEngine(CarPart.PART_QUALITY quality, Dictionary<CarPart.PART_QUALITY, GameObject> modelDictonary)
        {
            switch (quality)
            {
                //Choose Part Properies
                case CarPart.PART_QUALITY.EPIC:
                    partAcceleration = epicAcceleration;
                    partMaxSpeed = epicMaxSpeed;
                    break;
                case CarPart.PART_QUALITY.GOOD:
                    partAcceleration = goodAcceleration;
                    partMaxSpeed = goodMaxSpeed;
                    break;
                case CarPart.PART_QUALITY.OKAY:
                    partAcceleration = okayAcceleration;
                    partMaxSpeed = okayMaxSpeed;
                    break;
                case CarPart.PART_QUALITY.POOR:
                    partAcceleration = poorAcceleration;
                    partMaxSpeed = poorMaxSpeed;
                    break;
                default:
                    partAcceleration = poorAcceleration;
                    partMaxSpeed = poorMaxSpeed;
                    break;
            }

            //Choose model
            model = modelDictonary[quality];
        }
    }

    //Car turbo class
    public class CarPartPropertiesTurbo
    {

        //Private Vars that determine performance levels at each quality, for easy editing
        private const float epicBoostSpeedBoost = 15f;
        private const float goodBoostSpeedBoost = 15f;
        private const float okayBoostSpeedBoost = 15f;
        private const float poorBoostSpeedBoost = 15f;
                
        private const float epicBoostCooldown = 20f;
        private const float goodBoostCooldown = 30f;
        private const float okayBoostCooldown = 25f;
        private const float poorBoostCooldown = 35f;

        private readonly Color epicTurboParticlesColor = new Color32(200,000,236,255); //Dark Purple
        private readonly Color goodTurboParticlesColor = new Color32(001,195,046,255); //Dark Green
        private readonly Color okayTurboParticlesColor = new Color32(147,178,000,255); //Yellow-Green
        private readonly Color poorTurboParticlesColor = new Color32(255,000,000,255); //Bright Red

        public float partSpeedBoost;
        public float partBoostCooldown;
        public Color particleColour;
        public GameObject model;

        //Constructor receives quality and runs through to set quality variables
        public CarPartPropertiesTurbo(CarPart.PART_QUALITY quality, Dictionary<CarPart.PART_QUALITY, GameObject> modelDictonary)
        {
            //Choose Part Properies
            switch (quality)
            {
                case CarPart.PART_QUALITY.EPIC:
                    partSpeedBoost = epicBoostSpeedBoost;
                    partBoostCooldown = epicBoostCooldown;
                    particleColour = epicTurboParticlesColor;
                    break;
                case CarPart.PART_QUALITY.GOOD:
                    partSpeedBoost = goodBoostSpeedBoost;
                    partBoostCooldown = goodBoostCooldown;
                    particleColour = goodTurboParticlesColor;
                    break;
                case CarPart.PART_QUALITY.OKAY:
                    partSpeedBoost = okayBoostSpeedBoost;
                    partBoostCooldown = okayBoostCooldown;
                    particleColour = okayTurboParticlesColor;
                    break;
                case CarPart.PART_QUALITY.POOR:
                    partSpeedBoost = poorBoostSpeedBoost;
                    partBoostCooldown = poorBoostCooldown;
                    particleColour = poorTurboParticlesColor;
                    break;
                default:
                    partSpeedBoost = poorBoostSpeedBoost;
                    partBoostCooldown = poorBoostCooldown;
                    particleColour = poorTurboParticlesColor;
                    break;
            }

            //Choose model
            model = modelDictonary[quality];
        }
    }

    //Car Tyres class
    public class CarPartPropertiesTyres
    {

        //Private Vars that determine performance levels at each quality, for easy editing
        private const float epicTurnSpeed = 1.5f;
        private const float goodTurnSpeed = 1.25f;
        private const float okayTurnSpeed = 1.0f;
        private const float poorTurnSpeed = 0.75f;

        private const bool epicAllowWheelSpinning = true;
        private const bool goodAllowWheelSpinning = true;
        private const bool okayAllowWheelSpinning = false;
        private const bool poorAllowWheelSpinning = true;

        public float partTurnSpeed;
        public float partBoostCooldown;
        public bool allowWheelSpinning;
        public GameObject model;

        //Constructor receives quality and runs through to set quality variables
        public CarPartPropertiesTyres(CarPart.PART_QUALITY quality, Dictionary<CarPart.PART_QUALITY, GameObject> modelDictonary)
        {
            switch (quality)
            {
                case CarPart.PART_QUALITY.EPIC:
                    partTurnSpeed = epicTurnSpeed;
                    allowWheelSpinning = epicAllowWheelSpinning;
                    break;
                case CarPart.PART_QUALITY.GOOD:
                    partTurnSpeed = goodTurnSpeed;
                    allowWheelSpinning = goodAllowWheelSpinning;
                    break;
                case CarPart.PART_QUALITY.OKAY:
                    partTurnSpeed = okayTurnSpeed;
                    allowWheelSpinning = okayAllowWheelSpinning;
                    break;
                case CarPart.PART_QUALITY.POOR:
                    partTurnSpeed = poorTurnSpeed;
                    allowWheelSpinning = poorAllowWheelSpinning;
                    break;
                default:
                    partTurnSpeed = poorTurnSpeed;
                    break;
            }

            //Choose model
            model = modelDictonary[quality];
        }
    }

    #endregion
}

