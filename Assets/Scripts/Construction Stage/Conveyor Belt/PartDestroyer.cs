using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartDestroyer : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Part")
        {
            return;
        }

        Destroy(other.gameObject);
    }
}
