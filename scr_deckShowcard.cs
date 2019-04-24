using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class scr_deckShowcard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    ArrayList extras = new ArrayList() {
        "PEIN", "BAN", "PAN", "NATIS", "ROGER",
        "LATIEN", "SENA", "BT", "TONUS", "LICA",
        "MACHINA", "EURIEL", "INFINITAS", "SAYRUN", "HETERO"
    };

    public GameObject card;
    public GameObject deckPanel;
    public string id = null;
    public bool inDeck = false;
    public bool flag = false;
    public bool inGame = false;

    GameObject ins;

    void Start() {
        deckPanel = GameObject.Find("DeckPanels");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!inGame)
        {
            if (!flag)
            {
                scr_deckPanel deckData = deckPanel.GetComponent<scr_deckPanel>();

                if (!inDeck && scr_storeDeck.store_deck.Count < 30)
                {
                    int elementCnt = 0;
                    int elementCntDelta = (int)DB_card.getIdentity(id)[3];
                    int insertIndex = -1;
                    foreach (string curId in scr_storeDeck.store_deck)
                    {
                        if (curId == id)
                        {
                            elementCnt++;
                            insertIndex = scr_storeDeck.store_deck.IndexOf(id);
                        }
                    }

                    if (elementCnt < 3 - elementCntDelta)
                    {
                        if (insertIndex == -1) scr_storeDeck.store_deck.Add(id);
                        else scr_storeDeck.store_deck.Insert(insertIndex, id);
                        deckData.refresh();
                        if (ins != null) Destroy(ins);
                    }
                }
                else
                {
                    scr_storeDeck.store_deck.Remove(id);
                    deckData.refresh();
                    if (ins != null) Destroy(ins);
                }
            }
            else
            {
                int getExIndex = extras.IndexOf(id);

                if (getExIndex == extras.Count - 1) id = (string)extras[0];
                else id = (string)extras[getExIndex + 1];

                transform.GetChild(0).GetComponent<Text>().text = (string)DB_card.getIdentity(id)[4];
                scr_storeDeck.extraID = id;
                if (ins != null) Destroy(ins);
                ins = Instantiate(card, GameObject.Find("DDeckPanel").transform);
                ins.GetComponent<scr_cardGUI>().enabled = false;
                DB_card.setIdentity(ins, id, new Vector3(810f, -370f, 0), new Vector3(1.2f, 1.2f));
            }
        }
        else {
            if (!inDeck) {
                switch (id)
                {
                    case "Necropia":
                        scr_playerInGame.hand.Add("Dead_Necropia");
                        scr_playerInGame.tomb.Remove("Necropia");
                        if (ins != null) Destroy(ins);
                        reloadList(false);                        
                        break;
                    case "Keres":
                        scr_playerInGame.hand.Add("Keres");
                        scr_playerInGame.tomb.Remove("Keres");
                        if (ins != null) Destroy(ins);
                        reloadList(false);                        
                        break;
                    case "#grave_robber":
                        foreach (string cardId in scr_playerInGame.tomb)
                        {
                            string cardTag = (string)DB_card.getIdentity(cardId)[6];
                            int cardCost = (int)DB_card.getIdentity(cardId)[0];
                            if (cardTag.Contains("Dark") && cardCost == 5)
                            {
                                scr_playerInGame.tomb.Remove(cardId);
                                scr_playerInGame.hand.Add(cardId);
                                break;
                            }
                        }
                        scr_playerInGame.tomb.Remove("#grave_robber");
                        if (ins != null) Destroy(ins);
                        reloadList(false);                        
                        break;
                    case "#dark_ora":
                        ArrayList table = new ArrayList();
                        foreach (string cardId in scr_playerInGame.tomb)
                        {
                            string cardTag = (string)DB_card.getIdentity(cardId)[6];
                            int cardCost = (int)DB_card.getIdentity(cardId)[0];
                            if (cardTag.Contains("Dark") && cardCost <= 4) table.Add(cardId);
                        }
                        if (table.Count != 0)
                        {
                            int drawIndex = Random.Range(0, table.Count);
                            scr_playerInGame.deck.Add(table[drawIndex]);
                        }
                        scr_playerInGame.tomb.Remove("#dark_ora");
                        if (ins != null) Destroy(ins);
                        reloadList(false);
                        break;
                    case "#emergency_deposit":
                        getPlayer().GetComponent<scr_playerIdentity>().CmdGainStatus(
                            getPlayer().GetComponent<scr_playerController>().isHost, 2, 0);
                        scr_playerInGame.tomb.Remove("#emergency_deposit");
                        if (ins != null) Destroy(ins);
                        reloadList(false);
                        break;
                    case "soulful_icefox":
                        while (true) {
                            if (scr_playerInGame.tomb.Contains("kitten")) {
                                getPlayer().GetComponent<scr_playerIdentity>().CmdGainStatus(
                                    getPlayer().GetComponent<scr_playerController>().isHost, 1, 0);

                                scr_playerInGame.tomb.Remove("kitten");
                            }
                            else break;
                        }                        
                        scr_playerInGame.tomb.Remove("soulful_icefox");
                        if (ins != null) Destroy(ins);
                        reloadList(false);
                        break;
                    default:
                        break;
                }
            }           
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!inGame)
        {
            ins = Instantiate(card, GameObject.Find("DDeckPanel").transform);
            ins.GetComponent<scr_cardGUI>().enabled = false;
            DB_card.setIdentity(ins, id, new Vector3(810f, -370f, 0), new Vector3(1.2f, 1.2f));
        }
        else {
            ins = Instantiate(card, GameObject.Find("Canvas").transform);
            ins.GetComponent<scr_cardGUI>().enabled = false;
            DB_card.setIdentity(ins, id, Vector3.zero, new Vector3(1.2f, 1.2f));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(ins != null) Destroy(ins);
    }

    public void setId(string id) {
        this.id = id;
    }

    void reloadList(bool isDeck) {
        GameObject guiCon = GameObject.Find("GUIController");
        guiCon.GetComponent<scr_guiController>().reloadList(isDeck);
    }

    public GameObject getPlayer()
    {
        GameObject player = null;
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (cur.GetComponent<scr_playerController>().isPlayer)
            {
                player = cur;
            }
        }
        return player;
    }
}
