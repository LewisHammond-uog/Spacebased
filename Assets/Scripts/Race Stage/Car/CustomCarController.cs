using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCarController : MonoBehaviour
{

    #region Car Driving Characteristics

    [SerializeField]
    public int playerID = 0;

    //Turning
    private float maxTurnAngle = 100f; //CONTROLLED BY PART QUALITY
    private float turnSpeed = 0.2f; //CONTROLLED BY PART QUALITY
    private float turnAngle = 0;

    private float gravity = 9f;
    private float notGroundedDragMultiplyer = 20f; //If the car is not grounded how many times the normal drag should we apply

    //Speed
    private float maxNormalSpeed = 60f; //CONTROLLED BY PART QUALITY
    private float currentMaxSpeed = 0f;
    private float speed = 0f;
    private float accelerationForce = 50f; //CONTROLLED BY PART QUALITY
    private float brakeForce = 25f; //CONTROLLED BY PART QUALITY
    private float dragForce = 15f;
    private float maxCarLean = 1.0f;

    //Boosting
    private float boostDuration = 5f;
    private float boostSpeedIncrease = 5f; //CONTROLLED BY PART QUALITY
    private float boostCooldown = 20f; //CONTROLLED BY PART QUALITY
    private float boostCountdown = 0f;
    private bool boostActive = false;

    #endregion

    [Header("Attached Objects")]
    [SerializeField]
    private Rigidbody carPhysicsRigidbody;
    private GameObject carVisualBody;

    private Vector3 carCenterOfMass = new Vector3(0, -2f, 0);

    //The amount of space that is allowed between the car and the ground and it
    //still be concidered "grounded"
    private float groundedToleranceDistance = 0.5f;
    private float carHeight;

    //Array to store all of the tyres that are connected to this car
    private GameObject[] connectedTyres;
    private const int steerableWheelLeftIndex = 2;
    private const int steerableWheelRightIndex = 0;
    private const float visualWheelTurnMultiplyer = 0.2f; //How much the visual wheel turns in relation to the actual turn angle
    private const float wheelDirectionOffset = 90; //Offset of tyre model so that it faces the correct direction
    private Vector3 rightWheelOffset = new Vector3(0, 180, 180); //Offset for wheels on the righthand side of the car so that they are round the right way
    private bool spinWheels = false; //Whether we should spin the wheels

    //Store the indexes of the left and right side tyres so that we can rotate tyres on the right side
    private readonly int[] leftSideTyreIndexes = new int[] { 2, 3 };
    private readonly int[] rightSideTyreIndexes = new int[] { 0, 1 };

    //Var for if the car should be frozen and accept no inputs
    private bool isFrozen = false;
    private bool isRaceFinished = false;

    //Event for when boost is activated/deactivated
    public delegate void BoostEvent(float boostDuration, float boostCooldown, int playerID);
    public static event BoostEvent StartBoost;
    public static event BoostEvent StopBoost;


    //Audio Clips
    [SerializeField]
    private AudioSource engineSound;
    [SerializeField]
    private AudioSource boostSound;

    //Particle Effects
    [SerializeField]
    private ParticleSystem turboParticlesPrefab;
    private ParticleSystem turboParticles;

    // Use this for initialization
    void Start()
    {
        //Set the center of mass so that it is below the car, making it more stable
        GetComponent<Rigidbody>().centerOfMass = carCenterOfMass;

        //Get the height of the car, used for grounded calculations
        carHeight = GetComponent<Collider>().bounds.extents.y;

        //Start Current Max Speed at normal speed
        currentMaxSpeed = maxNormalSpeed;
       
        engineSound.Play();

    }

    // Update is called once per frame
    void Update()
    {

        if (!isFrozen)
        {

            DoCarMovement();
            LeanCarBody();
            

            //Activate Boost
            if (boostCountdown < 0)
            {
                if (ControlManager.GetButtonInputThisFrame("A", playerID) && !boostActive)
                {
                    StartCoroutine(DoBoost());
                }
            }

            //Play Engine Sound
            engineSound.pitch = (speed / currentMaxSpeed) * 2;

            //Decrease the boost cooldown
            boostCountdown -= Time.deltaTime;
        }

        if (ControlManager.GetButtonInputThisFrame("B", playerID))
        {
            ResetCar();
        }


    }

    /// <summary>
    /// Resets car to previous checkpoint
    /// </summary>
    private void ResetCar()
    {

        //Teleport player
        transform.position = gameObject.GetComponent<CarLapData>().GetRespawnPosition();
        transform.rotation = gameObject.GetComponent<CarLapData>().GetRespawnRotation();

        //Set speed to 0
        speed = 0;

    }

    /// <summary>
    /// Performs the boost action on the car, increasing its top speed for a set amount of time
    /// </summary>
    private IEnumerator DoBoost()
    {
        //Set boost to active, increase the max speed and then set to wait
        boostActive = true;
        currentMaxSpeed = maxNormalSpeed + boostSpeedIncrease;
        StartBoost(boostDuration, boostCooldown, playerID); //Event for UI Update
        turboParticles.Play(); //Play Particle Effect
        boostSound.Play();
        yield return new WaitForSeconds(boostDuration);

        //Deactiveate boost
        boostActive = false;
        currentMaxSpeed = maxNormalSpeed;
        StopBoost(boostDuration, boostCooldown, playerID); //Event for UI Update
        turboParticles.Stop(); //Stop Particles
        boostSound.Stop();
        boostCountdown = boostCooldown; //Reset Countdown
    }

    /// <summary>
    /// Performs movement on the car
    /// </summary>
    private void DoCarMovement()
    {

        //Calcaulte turn angle of the car
        turnAngle = ControlManager.GetAxisInput("Horizontal", playerID) * maxTurnAngle;

        CalculateVelocity(ControlManager.GetAxisInput("Triggers", playerID), ref speed);

        //If we have tyres connected make them spin, if they are on the front wheels then they should 
        //turn
        if (connectedTyres != null)
        {
            for (int i = 0; i < connectedTyres.Length; i++)
            {
                GameObject currentTyre = connectedTyres[i];
                DoTyreRotation(i, currentTyre);
            }
        }

        //Apply forces to rigidbody
        Vector3 eulerAngleVelocity = new Vector3(0, turnAngle * turnSpeed * (speed / currentMaxSpeed), 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        carPhysicsRigidbody.MoveRotation(carPhysicsRigidbody.rotation * deltaRotation);


        Vector3 carAccelerationForces = Vector3.zero;

        if (IsGrounded())
        {
            carAccelerationForces = carPhysicsRigidbody.rotation * Vector3.forward * speed;
        }

        Vector3 gravityForce = new Vector3(0, gravity, 0);


        carPhysicsRigidbody.velocity = carAccelerationForces - gravityForce;

    }

    /// <summary>
    /// Sets the correct rotation of a given tyre. Both facing the correct direction and spinning the tyre
    /// </summary>
    /// <param name="tyreArrayIndex">Index of the tyre in the connected tyre array</param>
    /// <param name="currentTyre">Game Object of the tyre</param>
    private void DoTyreRotation(int tyreArrayIndex, GameObject currentTyre)
    {
        //If we are a steerable wheel then allow this wheel to move left/right with steering
        if (tyreArrayIndex == steerableWheelLeftIndex || tyreArrayIndex == steerableWheelRightIndex)
        {
            //If tyre is on the right side apply rotation so that it is round the right way
            if (tyreArrayIndex == steerableWheelRightIndex)
            {
                currentTyre.transform.localEulerAngles = new Vector3(0f, (turnAngle * visualWheelTurnMultiplyer) - wheelDirectionOffset, currentTyre.transform.localEulerAngles.z) + rightWheelOffset;
            }
            else
            {
                currentTyre.transform.localEulerAngles = new Vector3(currentTyre.transform.localEulerAngles.x, (turnAngle * visualWheelTurnMultiplyer) - wheelDirectionOffset, currentTyre.transform.localEulerAngles.z);
            }
        }
        else
        {
            //Set wheel rotation
            if (tyreArrayIndex == rightSideTyreIndexes[1])
            {
                currentTyre.transform.localEulerAngles = new Vector3(0, -wheelDirectionOffset, 0) + rightWheelOffset;
            }
            else
            {
                currentTyre.transform.localEulerAngles = new Vector3(0, -wheelDirectionOffset, currentTyre.transform.localEulerAngles.z);
            }
        }

        //"Spin" the wheels
        if (spinWheels)
        {
            currentTyre.transform.Rotate(0f, 0f, speed);
        }
    }

    /// <summary>
    /// Checks if the car is touching the ground
    /// </summary>
    /// <returns>If car is grounded</returns>
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, carHeight + groundedToleranceDistance);
    }

    /// <summary>
    /// Leans the body of the car to "simulate" suspension
    /// </summary>
    private void LeanCarBody()
    {
        //Calculate a lean based on turn angle and speed
        if (carVisualBody != null)
        {
            float bodyLean = (speed * turnAngle) / (currentMaxSpeed * maxTurnAngle); //Normalised lean value
            bodyLean *= maxCarLean;
            carVisualBody.transform.localEulerAngles = new Vector3(carVisualBody.transform.localEulerAngles.x, carVisualBody.transform.localEulerAngles.y, bodyLean);
        }
    }

    /// <summary>
    /// Changes the speed of the car based on player inputs, takes in speed by refrence so it is directly
    /// read from and modified
    /// </summary>
    /// <param name="powerInput">Power Input from controller (-1 to 1)</param>
    /// <param name="currentSpeed">Current Speed of the car (by refrence)</param>
    private void CalculateVelocity(float powerInput, ref float currentSpeed)
    {

        //If we have made an input
        if (Mathf.Abs(powerInput) > 0.05f)
        {
            //If we have a possitive input (accelerating)
            if (powerInput > 0.05f)
            {
                //If we are not going to exceded our maximum currentSpeed then apply our new currentSpeed,
                //else set our currentSpeed to our max currentSpeed
                if ((currentSpeed + (accelerationForce * powerInput * Time.deltaTime)) < currentMaxSpeed)
                {
                    //Apply the force of our motor
                    currentSpeed += accelerationForce * powerInput * Time.deltaTime;
                }
                else
                {
                    currentSpeed = currentMaxSpeed;
                }

            }
            else if (powerInput < 0.05f)
            {
                //If we are not going to exceded our negitive maximum currentSpeed then apply our new currentSpeed,
                //else set our currentSpeed to our negitive max currentSpeed
                if (Mathf.Abs(currentSpeed + (brakeForce * powerInput * Time.deltaTime)) < currentMaxSpeed)
                {
                    //Apply the force of our breaks
                    currentSpeed += brakeForce * powerInput * Time.deltaTime;
                }
                else
                {
                    currentSpeed = -currentMaxSpeed;
                }
            }

        }
        else
        {
            //If the player has not made an input then if our currentSpeed is greater than a small value (0.001) then apply
            //drag, else esimate our currentSpeed to be 0, to stop micro pixel movements
            if (currentSpeed > 0.01f)
            {
                currentSpeed -= dragForce * Time.deltaTime;
            }
            else if (currentSpeed < -0.01f)
            {
                currentSpeed += dragForce * Time.deltaTime;
            }
            else
            {
                currentSpeed = 0;
            }
        }
    }


    /// <summary>
    /// Loads the car part models and properties on to the car
    /// </summary>
    /// <param name="partQualityList"></param>
    /// <param name="carPartProperties"></param>
    public void LoadCarPartsOnToCar(Dictionary<CarPart.PART_TYPE, CarPart.PART_QUALITY> partQualityList, CarPartEffects carPartProperties)
    {

        //BODY
        var bodyProperties = carPartProperties.GetPartPropertiesBody(partQualityList[CarPart.PART_TYPE.BODY]);
        maxTurnAngle = bodyProperties.partTurnAngle;
        LoadCarPartModel(CarPart.PART_TYPE.BODY, bodyProperties.model, partQualityList[CarPart.PART_TYPE.BODY]);

        //ENGINE
        var engineProperties = carPartProperties.GetPartPropertiesEngine(partQualityList[CarPart.PART_TYPE.ENGINE]);
        maxNormalSpeed = engineProperties.partMaxSpeed;
        accelerationForce = engineProperties.partAcceleration;
        LoadCarPartModel(CarPart.PART_TYPE.ENGINE, engineProperties.model, partQualityList[CarPart.PART_TYPE.BODY]);

        //TURBO
        var turboProperties = carPartProperties.GetPartPropertiesTurbo(partQualityList[CarPart.PART_TYPE.TURBO]);
        boostSpeedIncrease = turboProperties.partSpeedBoost;
        boostCooldown = turboProperties.partBoostCooldown;
        LoadCarPartModel(CarPart.PART_TYPE.TURBO, turboProperties.model, partQualityList[CarPart.PART_TYPE.BODY]);

        //TYRES
        var tyreProperties = carPartProperties.GetPartPropertiesTyres(partQualityList[CarPart.PART_TYPE.TYRE]);
        turnSpeed = tyreProperties.partTurnSpeed;
        spinWheels = tyreProperties.allowWheelSpinning;
        LoadCarPartModel(CarPart.PART_TYPE.TYRE, tyreProperties.model, partQualityList[CarPart.PART_TYPE.BODY]);

        //Reset Max Speed
        currentMaxSpeed = maxNormalSpeed;

        //Make sure that all the wheels are facing the right direction
        for(int i = 0; i < connectedTyres.Length; i++)
        {
            DoTyreRotation(i, connectedTyres[i]);
        }

        

    }


    /// <summary>
    /// Loads in the model of a given part and attaches it to the car body in the correct position
    /// </summary>
    /// <param name="partTypeToAttach"></param>
    /// <param name="partModel"></param>
    /// <param name="bodyQuality"></param>
    private void LoadCarPartModel(CarPart.PART_TYPE partTypeToAttach, GameObject partModel, CarPart.PART_QUALITY bodyQuality)
    {


        if (partTypeToAttach != CarPart.PART_TYPE.TYRE)
        {
            //Create model
            GameObject part = Instantiate(partModel);
            part.transform.parent = transform; //Set this to be the parent of the part
            Vector3 partPostion = Vector3.zero;
            switch (partTypeToAttach)
            {
                case CarPart.PART_TYPE.BODY:
                    partPostion = CarPartAttachmentPostions.GetBodyOfsetPositions(bodyQuality);
                    carVisualBody = part;

                    //Rotate part so that it is facing the right way
                    part.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    break;
                case CarPart.PART_TYPE.ENGINE:
                    partPostion = CarPartAttachmentPostions.GetEngineAttachmentPosition(bodyQuality);
                    break;
                case CarPart.PART_TYPE.TURBO:
                    partPostion = CarPartAttachmentPostions.GetTurboAttachmentPosition(bodyQuality);
                    part.transform.localRotation = Quaternion.Euler(0, 180, 0);

                    //Attach Particles
                    turboParticles = Instantiate(turboParticlesPrefab);
                    turboParticles.transform.parent = transform; 
                    turboParticles.transform.localPosition = partPostion;
                    turboParticles.transform.localRotation = Quaternion.Euler(0, 180, 0);

                    break;
            }

            //Apply attachment postion
            part.transform.localPosition = partPostion;

        }
        else
        {
            //Get attachment points
            Vector3[] tyreAttachmentPoints = CarPartAttachmentPostions.GetTyreAttachmentPositions(bodyQuality);

            //Reset array of connected tyres
            connectedTyres = new GameObject[4];

            //Create model and apply attachment positions
            for (int i = 0; i < 4; i++)
            {
                GameObject tyre = Instantiate(partModel);
                tyre.transform.parent = transform;
                tyre.transform.localPosition = tyreAttachmentPoints[i];

                //Add connected tyre to tyres array
                connectedTyres[i] = tyre;
            }
        }


    }


    /// <summary>
    /// Sets if the car is frozen or not
    /// </summary>
    /// <param name="frozen">Whether the car is frozen</param>
    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
    }

}