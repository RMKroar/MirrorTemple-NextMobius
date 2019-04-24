using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_particle : MonoBehaviour {
    public int scale;
    public int durability;

    int time = 0;
    bool flag = false;

    void Start()
    {
        if(tag == "ParticleDamage") {
            DecreaseSound();
        }

        transform.SetParent(GameObject.Find("Canvas").transform);
        transform.localScale = Vector3.one * scale;
    }

    // Update is called once per frame
    void Update() {
        time++;
        if (!flag) {
            transform.SetParent(GameObject.Find("Canvas").transform);
            transform.localScale = Vector3.one * scale;
            flag = true;
        } 

        if (time >= durability) {
            Destroy(gameObject);
        }
    }

    void DecreaseSound()
    {
        AudioSource aud = GetComponent<AudioSource>();
        float count = 1;

        foreach(GameObject cur in GameObject.FindGameObjectsWithTag(tag))
        {
            if (cur != gameObject) count++;
        }

        aud.volume = 1f / count;
        Debug.Log(aud.volume);
    }
}
