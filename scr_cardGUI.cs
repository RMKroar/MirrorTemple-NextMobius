using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class scr_cardGUI : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject player;
    public GameObject unit;

    private GameObject ins;
    private Vector3 savePos;
    private bool drag = false;

    Vector3 screenPoint;
    Vector3 offset;

    ArrayList data = new ArrayList();

    public void Start()
    {
        getLocalPlayer();
    }

    public void OnPointerEnter(PointerEventData eventdata)
    {
        if (!drag)
        {
            ins = Instantiate(gameObject);
            ins.transform.SetParent(gameObject.transform.parent);
            ins.GetComponent<Transform>().localPosition = new Vector3(-200, 0, -20);
            ins.GetComponent<Transform>().localScale = Vector3.one;
            ins.GetComponent<scr_cardGUI>().enabled = false;
        }
    }

    public void OnPointerExit(PointerEventData eventdata) {
        if (ins != null) Destroy(ins);
    }

    public void OnBeginDrag(PointerEventData eventdata) {
        if (getLocalPlayer()) {
            if (player.GetComponent<scr_playerController>().myturn) {
                savePos = gameObject.transform.position;
                screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3
                (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

                drag = true;
                if (ins != null) Destroy(ins);
            }
        }
        
    }

    public void OnDrag(PointerEventData eventdata) {
        if (getLocalPlayer()) {
            if (player.GetComponent<scr_playerController>().myturn) {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
                transform.position = curPosition;
            }
        }       
    }

    public void OnEndDrag(PointerEventData eventdata) {
        if (getLocalPlayer()) {
            if (player.GetComponent<scr_playerController>().myturn) {
                if (!gameObject.GetComponent<scr_card>().id.Contains("#"))
                {
                    GameObject place = getPlace();
                    int terial = gameObject.GetComponent<scr_card>().terial;
                    if (place == null || player.GetComponent<scr_playerIdentity>().terial < terial)
                    {
                        gameObject.transform.position = savePos;
                    }
                    else
                    {
                        if (getLocalPlayer())
                        {
                            spawn(place);
                            player.GetComponent<scr_playerIdentity>().CmduseTerial(terial);
                        }
                    }
                }
                else {
                    Transform sav_parent = transform.parent;
                    transform.SetParent(GameObject.Find("Canvas").transform);
                    if (gameObject.GetComponent<scr_card>().attack != -1) {
                        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit")) {
                            if (Vector3.Distance(transform.localPosition, cur.transform.localPosition) <= 60f)
                            {
                                scr_card temp_data = gameObject.GetComponent<scr_card>();
                                scr_playerIdentity player_data = player.GetComponent<scr_playerIdentity>();

                                if (player_data.terial >= temp_data.terial)
                                {
                                    transform.SetParent(player.transform);
                                    player_data.CmdforUnitSpell(cur, player.GetComponent<scr_playerController>().isHost, temp_data.id, temp_data.terial);
                                }
                                break;
                            }
                        }                           
                    }
                    else {
                        if (transform.localPosition.x <= 200f)
                        {
                            scr_card temp_data = gameObject.GetComponent<scr_card>();
                            scr_playerIdentity player_data = player.GetComponent<scr_playerIdentity>();

                            if (player_data.terial >= temp_data.terial)
                            {
                                transform.SetParent(player.transform);
                                player_data.CmdforAllSpell(player.GetComponent<scr_playerController>().isHost, temp_data.id, temp_data.terial);
                            }
                        }
                    }
                    gameObject.transform.position = savePos;
                    transform.SetParent(sav_parent);
                }
            }
            drag = false;
        } 
    }

    public void makeData() {
        data.Add(gameObject.GetComponent<scr_card>().terial);
        data.Add(gameObject.GetComponent<scr_card>().attack);
        data.Add(gameObject.GetComponent<scr_card>().health);
        data.Add(gameObject.GetComponent<scr_card>().rank);
        data.Add(gameObject.GetComponent<scr_card>().cardname);
        data.Add(gameObject.GetComponent<scr_card>().description);
        data.Add(gameObject.GetComponent<scr_card>().card_tag);
        data.Add(gameObject.GetComponent<scr_card>().pic_url);
        data.Add(gameObject.GetComponent<scr_card>().id);
    }

    public GameObject getPlace() {
        GameObject place = null;
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Place")) {
            if (cur.GetComponent<scr_place>().placeable) {
                place = cur;
                break;
            }
        }
        return place;
    }

    public void spawn(GameObject place) {
        makeData();
        player.GetComponent<scr_playerController>().direct_informData(data, place.transform.localPosition);
        player.GetComponent<scr_playerInGame>().setCardSpawn(gameObject, place.transform.localPosition, data);
    }

    public bool getLocalPlayer() {
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player")) {
            if (cur.GetComponent<scr_playerController>().isPlayer) {
                player = cur;
                return true;
            }
        }
        return false;
    }
}
