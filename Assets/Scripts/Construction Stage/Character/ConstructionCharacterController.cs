using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConstructionCharacterController : MonoBehaviour {

    [SerializeField]
    private float speed = 5.0f;
    private float gravity = -1000.0f;
    private float groundStick = -1.0f;

    public int playerID = 1;

    [SerializeField]
    private GameObject playerModel;

    [SerializeField]
    private AudioSource walkingAudioSource;
    private const float walkingSoundSpacing = 0.4f;
    private float walkingSoundTimer = 0f;
    //Vars for upper and lower limits of audio pitch chnage
    private float walkingSoundLowerPitchBound = 0.8f;
    private float walkingSoundUpperPitchBound = 1.2f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 userMovement;
    private Quaternion targetRotation;
    private float playerRotationSpeed = 0.3f;
    private bool isGrounded = false;
    private bool isFrozen = false;

    // Use this for initialization
    void Start () {

        //Get references
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrozen)
        {
            DoMovement();
            DoPlayerWalkSound();
        }

    }

    private void RotatePlayer()
    {

        //Calculate new rotation based of player inputs
        Vector3 nextDir = new Vector3(ControlManager.GetAxisInput("Horizontal", playerID), 0, ControlManager.GetAxisInput("Vertical", playerID));

        //If we have made an input get our new rotation
        if (nextDir != Vector3.zero)
            targetRotation = Quaternion.LookRotation(-nextDir);

        //Apply rotation via slerp to smooth it out
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetRotation, playerRotationSpeed);
    }

    /// <summary>
    /// Does movement for player character
    /// </summary>
    private void DoMovement()
    {
        //Stop movement while in the air
        if (isGrounded)
        {
            //Movement relattive the direction of camera/player
            userMovement = -(ControlManager.GetAxisInput("Vertical", playerID) * transform.right) + (ControlManager.GetAxisInput("Horizontal", playerID) * transform.forward);
        }


        //Add Gravity
        velocity.y += gravity * Time.deltaTime;

        //Check if the player is touching the ground and falling
        if (isGrounded && velocity.y < 0)
        {
            //Force the player to stick to the ground and not hover over a slope
            velocity.y = groundStick;
        }


        //Apply Movement and rotation, get the collision flags back so we know if we have hit the floor or not
        CollisionFlags lastCollisionFlags = controller.Move((userMovement * Time.deltaTime * speed) + (velocity * Time.deltaTime));

        //We bitwise & operator check
        // 0001 and 1111 = 0001
        // 0000 and 1111 = 0000
        // Both need to be true. So we are checking for the right 'bit' for the Collsion Flags.Below has been set
        isGrounded = (lastCollisionFlags & CollisionFlags.Below) > 0;

        //Check for collision with ceiling using collision flags
        if ((lastCollisionFlags & CollisionFlags.Above) > 0 && velocity.y > 0)
        {
            //Kill all vertical veloctiy when jumping in to the ceiling
            velocity.y = 0;
        }

        RotatePlayer();
    }

    /// <summary>
    /// Does audio logic to play walking sound
    /// </summary>
    private void DoPlayerWalkSound()
    {
        //If the user is actually moving then play the sound at
        //set invervals
        if (userMovement.x != 0 || userMovement.z != 0)
        {
            if (walkingSoundTimer <= 0)
            {
                //Choose a "random" pitch so the sound is not repetative
                walkingAudioSource.pitch = Random.Range(walkingSoundLowerPitchBound, walkingSoundUpperPitchBound);

                walkingAudioSource.Play();
                walkingSoundTimer = walkingSoundSpacing;
            }
        }

        walkingSoundTimer -= Time.deltaTime;
    }


    /// <summary>
    /// Sets if the player character can move or not
    /// </summary>
    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
    }
}



