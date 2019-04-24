using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_audioFade : MonoBehaviour {
    public int delay;
    int time;

    AudioSource aud;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.volume = 0;
    }

    // Update is called once per frame
    void Update () {
        if (time <= delay) {
            aud.volume = ((float)++time / delay) * 0.6f;
        }
    }
}
