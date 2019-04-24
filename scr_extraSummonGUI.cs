using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_extraSummonGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject card;
    GameObject ins;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ins = Instantiate(card, GameObject.Find("Canvas").transform);
        ins.GetComponent<scr_cardGUI>().enabled = false;
        DB_card.setIdentity(ins, scr_storeDeck.extraID, Vector3.zero, new Vector3(1.2f, 1.2f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ins != null) Destroy(ins);
    }
}
