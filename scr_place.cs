using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_place : MonoBehaviour { 
    public bool placeable = false;
    void OnMouseEnter()
    {
        if(!getUnits() && transform.localPosition.y < 0) placeable = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }
    void OnMouseExit()
    {
        placeable = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
    }

    private bool getUnits() {
        bool res = false;
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit")) {
            if (Vector3.Distance(cur.transform.localPosition, transform.localPosition) <= 10f) {
                res = true;
                break;
            }
        }
        return res;
    }
}
