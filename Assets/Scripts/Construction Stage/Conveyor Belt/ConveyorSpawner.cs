using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConveyorSpawner : MonoBehaviour {

    //Spawn Timers for item spawning
    [SerializeField]
    private float spawnSpacing = 2.0f; //Time between items being spawned
    private float spawnCountdown = 0.0f; //Countdown for item being spawned

    //Postion to Spawn Objects
    [SerializeField]
    private Vector3 partCreatePosition;

    //Prefabs for the part that we can spawn
    [SerializeField]
    private GameObject tyrePrefab, enginePrefab, turboPrefab, bodyPrefab;

    //Perfab for part object pool
    //[SerializeField]
    //private GameObject carPartObjectPool;

    // Use this for initialization
    void Start() {

        spawnCountdown = spawnSpacing;

    }
	
	// Update is called once per frame
	void Update () {
        spawnCountdown -= Time.deltaTime;

        if(spawnCountdown < 0)
        {
            SpawnPart();
            spawnCountdown = spawnSpacing;
        }
	}

    /// <summary>
    /// Spawns a part with random quality
    /// </summary>
    private void SpawnPart()
    {

        GameObject partToSpawn = null;

        //Choose Part and Quality
        CarPart.PART_TYPE partTypeSpawned = ChoosePartToSpawn(ref partToSpawn);
        CarPart.PART_QUALITY partQualityToSpawn = ChoosePartQualityToSpawn();

        //Check that we have a part to spawn
        if (partToSpawn != null)
        {
            //Create Part
            GameObject createdPart = Instantiate(partToSpawn);
            createdPart.transform.localPosition = partCreatePosition;
            createdPart.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            createdPart.transform.rotation = Quaternion.Euler(-180, 90, 0);
            //createdPart.transform.parent = carPartObjectPool.transform;
            

            //Set part quality
            createdPart.GetComponent<PartPickup>().myPartQuality = partQualityToSpawn;
            createdPart.GetComponent<PartPickup>().myPartType = partTypeSpawned;
        }
    }

    /// <summary>
    /// Selectes a part to spawn
    /// </summary>
    /// <returns>Game Object prefab to Spawn</returns>
    private CarPart.PART_TYPE ChoosePartToSpawn(ref GameObject obj)
    {

        //Intialise random part to be last spawned part, so that we generate a new one
        CarPart.PART_TYPE randomPartSelection;
        GameObject partToCreate = null;

        randomPartSelection = (CarPart.PART_TYPE)UnityEngine.Random.Range(0, 4);

        //Select Prefab to Spawn
        switch (randomPartSelection)
        {
            case CarPart.PART_TYPE.BODY:
                partToCreate = bodyPrefab;
                break;
            case CarPart.PART_TYPE.TYRE:
                partToCreate = tyrePrefab;
                break;
            case CarPart.PART_TYPE.TURBO:
                partToCreate = turboPrefab;
                break;
            case CarPart.PART_TYPE.ENGINE:
                partToCreate = enginePrefab;
                break;
        }

        //Pass back part to actually spawn
        obj = partToCreate;

        //Return the part we have selected
        return randomPartSelection;

    }

    /// <summary>
    /// Chooses a part quality for a part to be spawned with
    /// </summary>
    /// <returns>Part Quality</returns>
    private CarPart.PART_QUALITY ChoosePartQualityToSpawn()
    {
        //Initliase qulaity var
        CarPart.PART_QUALITY quality = CarPart.PART_QUALITY.OKAY;

        //Choose a random part qulaity
        quality = (CarPart.PART_QUALITY)UnityEngine.Random.Range(0, 4);

        //Return the quality
        return quality;

    }
}
