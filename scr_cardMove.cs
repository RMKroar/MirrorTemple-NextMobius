using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_cardMove : MonoBehaviour {
    public float scaleSpeed;
    public float angleMult;
    public float moveSpeed;
    public float moveSpeedDelta;
    public float durability;
    public float delay;

    public int moveCode = 0;

    float sav_durability = 0;
    float scaleDelta;
    bool flag = false;

    void Start () {
        transform.localScale = new Vector3(0.3f, 0.3f, 1);
	}
	
	// Update is called once per frame
	void Update () {
        durability--;
        if (moveCode == 0) DrawingAction();
        else if (moveCode == 1) WhatOtherUsedAction();
	}

    void DrawingAction() {
        if (!flag)
        {
            transform.localPosition += new Vector3(0, 0, -scaleSpeed);
            if (transform.localPosition.z <= -500)
            {
                sav_durability = durability;
                scaleDelta = (-transform.localPosition.z) / (durability - delay);
                transform.localPosition += new Vector3(0.2f, 0.2f * angleMult, scaleDelta);
                flag = true;
            }
        }
        else
        {
            if (sav_durability - durability >= delay)
            {
                transform.localPosition += new Vector3(moveSpeed, moveSpeed * angleMult, scaleDelta);
                moveSpeed -= moveSpeedDelta;
                if (moveSpeed <= 0) moveSpeed = 0.2f;
            }
        }

        if (durability <= 0)
        {
            scr_playerInGame.hand.Add(GetComponent<scr_card>().id);
            Destroy(gameObject);
        }
    }

    void WhatOtherUsedAction()
    {
        if (transform.localPosition.z >= -520) transform.localPosition += new Vector3(0, 0, -scaleSpeed);
        transform.localPosition += new Vector3(moveSpeed, moveSpeed * angleMult, 0);
        if (moveSpeed >= 0.5f) moveSpeed -= moveSpeedDelta;
        else moveSpeed = 0f;

        if (durability <= 30)
        {
            Color col = new Color(1f, 1f, 1f, durability / 30f);
            Color txtCol = new Color(0f, 0f, 0f, durability / 30f);
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = col;
            transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = col;
            transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = col;
            transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().color = col;
            transform.GetChild(6).gameObject.GetComponent<SpriteRenderer>().color = col;
            transform.GetChild(4).gameObject.GetComponent<Text>().color = txtCol;
            transform.GetChild(5).gameObject.GetComponent<Text>().color = txtCol;
            GetComponent<SpriteRenderer>().color = col;

            if (durability <= 0) Destroy(gameObject);
        }
    }
}
