using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMovement : MonoBehaviour {

    [SerializeField]
    private float movementSpeed = 2.0f;

    //Animated Textures
    private int materialIndex = 0;
    [SerializeField]
    private Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    private string textureName = "_MainTex";
    [SerializeField]
    private new Renderer renderer;
    Vector2 uvOffset = Vector2.zero;

    /// <summary>
    /// Move the parts/player along the conveyor
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Part" && other.tag != "Player")
        {
            return;
        }

        other.transform.position += new Vector3(movementSpeed * Time.deltaTime, 0, 0);
    }


    /// <summary>
    /// Late update renders out the scrolling texure of the conveyor belt
    /// </summary>
    void LateUpdate()
    {
        uvOffset += (uvAnimationRate * Time.deltaTime);
        if (renderer.enabled)
        {
            renderer.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
        }
    }
}
