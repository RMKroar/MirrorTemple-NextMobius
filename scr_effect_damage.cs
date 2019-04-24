using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_effect_damage : MonoBehaviour {
    public int value;
    Color color = Color.clear;

    public float speed = 3;
    int time = 0;
    bool flag = false;

    void Start()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        transform.localScale = Vector3.one * 2f;
        if (value > 0) color = Color.red;
        else if (value < 0) {
            color = Color.green;
            value = -value;
        } 
        else color = Color.white;

        if (value <= 9) gameObject.GetComponent<SpriteRenderer>().sprite = scr_card.numbers[value];
        else {
            gameObject.GetComponent<SpriteRenderer>().sprite = scr_card.numbers[value % 10];
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = scr_card.numbers[value / 10];
            transform.localPosition += new Vector3(21, 0, 0);
        }
    }

    void Update()
    {
        time++;

        if (!flag) 
        {
            transform.SetParent(GameObject.Find("Canvas").transform);
            transform.localScale = Vector3.one * 2f;

            if (color == Color.clear) {
                if (value > 0) color = Color.red;
                else if (value < 0)
                {
                    color = Color.green;
                    value = -value;
                }
                else color = Color.white;
            }

            if (value <= 9) gameObject.GetComponent<SpriteRenderer>().sprite = scr_card.numbers[value];
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = scr_card.numbers[value % 10];
                transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = scr_card.numbers[value / 10];
            }
            flag = true;
        }

        gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1f - time / 60f);
        if (value >= 10) transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1f - time / 60f);
        transform.localPosition += new Vector3(0, speed * (1f - (float)time/60), 0);

        if (time >= 60) {
            Destroy(gameObject);
        }
    }
}
