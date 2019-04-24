using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_canvas : MonoBehaviour {
    public GameObject place;

    void Start()
    {
        int[] coordsX = new int[3] { -400, -200, 0 };
        int[] coordsY = new int[4] { 335, 115, -115, -335 };

        setPlace(coordsX, coordsY);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void setPlace(int[] coordsX, int[] coordsY) {
        for (int i = 0; i <= 3; i++) {
            for (int j = 0; j <= 2; j++) {
                GameObject pl = Instantiate(place, new Vector3(0, 0, 0), Quaternion.identity);
                pl.transform.SetParent(gameObject.transform);
                pl.GetComponent<Transform>().localPosition = new Vector3(coordsX[j], coordsY[i], 0);
                pl.GetComponent<Transform>().localScale = Vector3.one;
            }
        }
    }

    public void displayCard(GameObject card, string id, int posX, int expansion)
    {
        GameObject c = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
        c.transform.SetParent(gameObject.transform);
        DB_card.setIdentity(c, id, new Vector3(posX, -335 + 160 * expansion, 0), new Vector3(0.3f, 0.3f, 1));
    }

    public void enableSelect() {
        transform.GetChild(2).gameObject.SetActive(true);
    }
}
