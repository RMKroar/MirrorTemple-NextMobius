using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_lobbyCam : MonoBehaviour {
	void Update () {
        Canvas canvas = gameObject.GetComponent<Canvas>();
        if (SceneManager.GetActiveScene().name == "Battlefield")
        {
            canvas.enabled = false;
        }
        else {
            canvas.enabled = true;
        }
	}
}
