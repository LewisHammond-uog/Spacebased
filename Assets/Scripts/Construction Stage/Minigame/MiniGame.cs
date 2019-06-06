using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class that makes sure that all minigames have an initalise
/// </summary>
public abstract class MiniGame : MonoBehaviour {

    public abstract void InitaliseMinigame(ConstructionCharacterController player, PartConnector minigamePartConnector);

}
