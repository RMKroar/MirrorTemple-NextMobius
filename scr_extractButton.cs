using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_extractButton : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IPointerExitHandler,
    IDragHandler, IEndDragHandler
{
    private Vector3 savePos;
    private bool drag = false;
    private bool onMouse = false;

    Vector3 screenPoint;
    Vector3 offset;

    GameObject sav_parent;

    public void Start()
    {
        sav_parent = transform.parent.gameObject;
    }

    public void Update()
    {
        if (!onMouse && !drag)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        scr_playerIdentity data = sav_parent.GetComponent<scr_playerIdentity>();
        scr_playerController data2 = sav_parent.GetComponent<scr_playerController>();
        scr_meditator med = GameObject.FindGameObjectWithTag("Meditator").GetComponent<scr_meditator>();
        if (data2.myturn && med.turnCnt != data.sav_turn && data.terial >= 2)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            onMouse = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouse = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        savePos = gameObject.transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3
        (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        drag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scr_playerIdentity data = sav_parent.GetComponent<scr_playerIdentity>();
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
        {
            
            scr_unit unitData = cur.GetComponent<scr_unit>();

            gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
            if (Vector3.Distance(gameObject.transform.localPosition, cur.transform.localPosition) <= 60f &&
                !unitData.isEnemy && !unitData.extracted)
            {
                data.Cmdextract(cur);
                break;
            }
            
        }

        gameObject.transform.position = savePos;
        drag = false;
    }
}
