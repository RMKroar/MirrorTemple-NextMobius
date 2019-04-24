using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_deadBehaviour : MonoBehaviour {
    SpriteRenderer rend;
    int alph = 30;
	
	void Start () {
        rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        rend.color = new Color(1f, 1f, 1f, alph-- / 30f);
        if (alph < 0) Destroy(gameObject);
	}
}
