using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_uiBehaviour : MonoBehaviour {
    public Vector3 standardValue;
    public Vector3 deltaValue;
    public float deltaSpeed;

    public float time = 0;
	
	// Update is called once per frame
	void Update () {
        time += deltaSpeed;
        transform.localPosition = standardValue + deltaValue * Mathf.Sin(time);
	}
}
