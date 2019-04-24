using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_gameController : MonoBehaviour {
    void Awake() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1350, 900, true);
        Application.targetFrameRate = 60;
    }
}
