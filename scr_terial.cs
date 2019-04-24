using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_terial : MonoBehaviour {
    public Sprite deactive;
    public Sprite active;
    public int number;

    GameObject player;
    SpriteRenderer rend;

	void Start () {
        rend = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (getPlayer()) {
            rend.sprite = (player.GetComponent<scr_playerIdentity>().terial >= number) ? active : deactive;
        }
	}

    public bool getPlayer()
    {
        bool flag = false;
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (cur.GetComponent<scr_playerController>().isPlayer)
            {
                player = cur;
                flag = true;
                break;
            }
        }
        return flag;
    }
}
