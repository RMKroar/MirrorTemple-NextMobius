using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_picGUI : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    public GameObject card;
    GameObject ins;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ins == null) {
            scr_unit data = transform.parent.GetComponent<scr_unit>();
            float pos_y = transform.parent.GetComponent<Transform>().localPosition.y;

            ins = Instantiate(card);
            ins.transform.SetParent(GameObject.Find("Canvas").transform);
            ins.transform.localPosition = (pos_y > 0)? new Vector3(-195f, -215f, -20) : new Vector3(-195f, 215f, -20);
            ins.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            ins.GetComponent<scr_card>().setIdentity(data.terial, data.attack, data.health, data.rank,
                data.cardname, data.description, data.card_tag, data.pic_url, data.id);
        }      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eraseCard();
    }

    public void eraseCard() {
        if (ins != null) Destroy(ins);
    }
}
