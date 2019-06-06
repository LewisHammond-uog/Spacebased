using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPart : MonoBehaviour
{
    public enum PART_QUALITY
    {
        EPIC,
        GOOD,
        OKAY,
        POOR
    };

    public enum PART_TYPE
    {
        ENGINE,
        TURBO,
        TYRE,
        BODY
    }

    public PART_QUALITY myPartQuality = PART_QUALITY.OKAY;
    public PART_TYPE myPartType = PART_TYPE.BODY;

    [SerializeField]
    private GameObject poorModel, okayModel, goodModel, epicModel;

    private void Start()
    {
        ChooseModel();
    }

    /// <summary>
    /// Choose what model to spawn as a child
    /// </summary>
    private void ChooseModel()
    {

        GameObject modelToCreate;

        switch (myPartQuality)
        {
            case (PART_QUALITY.POOR):
                modelToCreate = poorModel;
                break;
            case (PART_QUALITY.OKAY):
                modelToCreate = okayModel;
                break;
            case (PART_QUALITY.GOOD):
                modelToCreate = goodModel;
                break;
            case (PART_QUALITY.EPIC):
                modelToCreate = epicModel;
                break;
            default:
                //Default Behaviour, don't choose a model
                //and just return out of function
                return;
        }

        //Spawn Selected Model
        GameObject model = Instantiate(modelToCreate, transform);
        model.transform.parent = gameObject.transform;
        model.transform.localPosition = Vector3.zero;
        
    }
}
