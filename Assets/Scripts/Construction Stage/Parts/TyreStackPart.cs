using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyreStackPart : CarPart
{
    public GameObject tyrePrefab;

    public const int TyreCount = 4;
    public float TyreSeperation = 2.0f;

    public GameObject[] tyreArray = new GameObject[TyreCount];

    public int currentTyreCount;

    // Use this for initialization
    void Start()
    {

        //Moveour selves down
        transform.position -= new Vector3(0, 5, 0);

        //Generate tyres
        for (int i = 0; i < TyreCount; i++)
        {
            GameObject newTyre = Instantiate(tyrePrefab);
            newTyre.transform.Rotate(0, 0, -90);
            newTyre.transform.position = transform.position + new Vector3(0, TyreSeperation * i, 0);
            newTyre.transform.parent = gameObject.transform;
            newTyre.GetComponent<CarPart>().myPartType = PART_TYPE.TYRE;
            newTyre.GetComponent<CarPart>().myPartQuality = myPartQuality;

            tyreArray[i] = newTyre;

        }

        currentTyreCount = TyreCount;

    }

    public void RemoveTyre()
    {
        Destroy(tyreArray[currentTyreCount - 1].gameObject);
        currentTyreCount -= 1;
    }
}
