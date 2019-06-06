using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour {

	public enum TOOL_TYPE
    {
        ENGINETOOL,
        TURBOTOOL,
        TYRETOOL,
        BODYTOOL
    }

    [SerializeField]
    public TOOL_TYPE myToolType;

    public Vector3 startpos;

    private void Start()
    {
        startpos = transform.position;
    }
}
