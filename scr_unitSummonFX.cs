using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_unitSummonFX : MonoBehaviour
{
    int time = 0;

    // Update is called once per frame
    public void Update()
    {
        time++;
        if (time <= 15)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)time / 15);
        }
        if (time >= 30 && time <= 60)
        {
            if (time == 30)
            {
                GameObject par = transform.parent.gameObject;
                par.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                for (int i = 0; i < 4; i++) {
                    par.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
                par.transform.GetChild(1).GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                par.transform.GetChild(2).GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)(60 - time) / 30);
        }
    }
}
