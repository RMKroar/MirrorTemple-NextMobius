using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_extraSummon : MonoBehaviour, IPointerClickHandler {
    GameObject player;
    Vector3 screenPoint;

    // Use this for initialization
    void Start () {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        transform.position = curPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (getLocalPlayer())
        {
            GameObject place = getPlace();
            if (place == null)
            {
                Destroy(gameObject);
            }
            else
            {
                spawn(place);
            }
        }
    }

    public GameObject getPlace()
    {
        GameObject place = null;
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Place"))
        {
            if (Vector3.Distance(transform.localPosition, cur.transform.localPosition) <= 30)
            {
                place = cur;
                break;
            }
        }
        return place;
    }

    public void spawn(GameObject place)
    {
        ArrayList data = DB_card.getIdentity(scr_storeDeck.extraID);
        data.Add(scr_storeDeck.extraID);
        player.GetComponent<scr_playerController>().direct_informData(data, place.transform.localPosition);
        player.GetComponent<scr_playerInGame>().setCardSpawn(player, place.transform.localPosition, data);
        scr_guiController.extraFlag = false;
        Destroy(gameObject);
    }

    public bool getLocalPlayer()
    {
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (cur.GetComponent<scr_playerController>().isPlayer)
            {
                player = cur;
                return true;
            }
        }
        return false;
    }
}
