using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_deckCntUI : MonoBehaviour {
    public GameObject deckPanel;
    int deckCnt = 0;

	void Update () {
        deckCnt = scr_storeDeck.store_deck.Count;
        transform.GetChild(0).gameObject.GetComponent<Text>().text =
            "덱 매수 : " + deckCnt + "/30"; 
	}
}
