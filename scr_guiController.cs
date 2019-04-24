using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class scr_guiController : NetworkBehaviour {
    public GameObject med;
    public GameObject localPlayer;
    public GameObject element;
    public GameObject extraSummon;
    public GameObject terialDisp;
    public GameObject deckButtonText;
    public GameObject tombButtonText;

    public static bool extraFlag = true;

    private int time = 0;
    private int nextTime = 0;

    void Start()
    {
        GameObject ins;

        for (int i = 0; i < 7; i++)
        {
            ins = Instantiate(terialDisp, GameObject.Find("Canvas").transform);
            ins.transform.localPosition = new Vector3(450 - i * 55, -48, 0);
            ins.transform.localScale = Vector3.one;
            ins.GetComponent<scr_terial>().number = i + 1;
        }
        StartCoroutine("CountDisp");
    }

    void Update() {
        time++;  
    }

    IEnumerator CountDisp()
    {
        while(true)
        {
            deckButtonText.GetComponent<Text>().text = "덱 목록\n(" + scr_playerInGame.deck.Count + ")";
            tombButtonText.GetComponent<Text>().text = "유계 목록\n(" + scr_playerInGame.tomb.Count + ")";
            yield return new WaitForSeconds(.1f);
        }
    }

    public void turnEnd() {
        if (time > nextTime) {
            getPlayer();
            bool check = localPlayer.GetComponent<scr_playerController>().myturn;
            if (getMeditator() && check)
            {
                if (isServer)
                {
                    med.GetComponent<scr_meditator>().changeTurn();
                    foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        scr_unit data = unit.GetComponent<scr_unit>();
                        string checkGrow = (string)DB_card.getIdentity(data.id)[5];
                        if (data.hosts && checkGrow.Contains("[성장")) data.grow();
                    }
                }
                else
                {
                    localPlayer.GetComponent<scr_playerController>().direct_cmdrequest();
                    foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        scr_unit data = unit.GetComponent<scr_unit>();
                        string checkGrow = (string)DB_card.getIdentity(data.id)[5];
                        if (!data.hosts && checkGrow.Contains("[성장")) data.grow();
                    }
                }
                nextTime = time + 100;
            }
        }
    }

    public void hideList()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas.transform.GetChild(6).gameObject.activeInHierarchy)
        {
            canvas.transform.GetChild(6).gameObject.SetActive(false);
        }
    }

    public void showDeck() {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas.transform.GetChild(6).gameObject.activeInHierarchy)
        {
            canvas.transform.GetChild(6).gameObject.SetActive(false);
        }
        else {
            canvas.transform.GetChild(6).gameObject.SetActive(true);
            reloadList(true);
        }        
    }

    public void showTomb()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas.transform.GetChild(6).gameObject.activeInHierarchy)
        {
            canvas.transform.GetChild(6).gameObject.SetActive(false);
        }
        else
        {
            canvas.transform.GetChild(6).gameObject.SetActive(true);
            reloadList(false);
        }
    }

    public void extraSummoning() {
        bool checkIfExtraExists = false;
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit")) {
            if (cur.GetComponent<scr_unit>().rank == 3 && cur.transform.localPosition.y < 0) {
                checkIfExtraExists = true;
                break;
            } 
        }

        if (!checkIfExtraExists && checkExtra()) {
            getPlayer();
            if (localPlayer.GetComponent<scr_playerController>().myturn) Instantiate(extraSummon, GameObject.Find("Canvas").transform);
        }      
    }

    public void reloadList(bool isDeck)
    {
        GameObject grid = GameObject.FindGameObjectWithTag("ShowList");
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Element")) {
            Destroy(cur);
        }
        if (isDeck) {
            foreach (string id in scr_playerInGame.deck)
            {
                GameObject ins = Instantiate(element, grid.transform);
                ins.GetComponent<scr_deckShowcard>().id = id;
                ins.GetComponent<scr_deckShowcard>().inGame = true;
                ins.GetComponent<scr_deckShowcard>().inDeck = true;
                ins.transform.GetChild(0).gameObject.GetComponent<Text>().text = (string)DB_card.getIdentity(id)[4];
            }
        }
        else {
            foreach (string id in scr_playerInGame.tomb)
            {
                GameObject ins = Instantiate(element, grid.transform);
                ins.GetComponent<scr_deckShowcard>().id = id;
                ins.GetComponent<scr_deckShowcard>().inGame = true;
                ins.GetComponent<scr_deckShowcard>().inDeck = false;
                ins.transform.GetChild(0).gameObject.GetComponent<Text>().text = (string)DB_card.getIdentity(id)[4];
            }
        }
    }

    public void ExitGame()
    {
        NetworkIdentity networkIdentity = GetComponent<NetworkIdentity>();
        NetworkManager networkManager = NetworkManager.singleton;
        if (networkIdentity.isServer && networkIdentity.isClient)
        {
            networkManager.StopHost();
        }
        else if (networkIdentity.isServer)
        {
            networkManager.StopServer();
        }
        else
        {
            networkManager.StopClient();
        }
        //if (isServer) CmdExitGame();
        //Network.Disconnect();
        //SceneManager.LoadScene("Lobby");
    }

    public bool checkExtra() {
        bool synthesize_firstIng = false;
        bool synthesize_secondIng = false;
        int overlay_count = 0;

        switch (scr_storeDeck.extraID) {
            case "PEIN":
                foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit")) {
                    scr_unit unitData = unit.GetComponent<scr_unit>();
                    if (unit.transform.localPosition.y < 0 && (unitData.id == "investigator_pein" || unitData.id == "synthesizer")) {
                        if (synthesize_firstIng == false) synthesize_firstIng = true;
                        else synthesize_secondIng = true;
                    }
                }
                return (synthesize_firstIng && synthesize_secondIng);
            case "BAN":
                foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit unitData = unit.GetComponent<scr_unit>();
                    if (unit.transform.localPosition.y < 0)
                    {
                        if (unitData.id == "moon_slayer") synthesize_firstIng = true;
                        else if (unitData.id == "mancy") synthesize_secondIng = true;
                        else if (unitData.id == "synthesizer") {
                            if (!synthesize_firstIng) synthesize_firstIng = true;
                            else synthesize_secondIng = true;
                        }
                    }
                }
                return (synthesize_firstIng && synthesize_secondIng);
            case "PAN":
                foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit unitData = unit.GetComponent<scr_unit>();
                    if (unit.transform.localPosition.y < 0)
                    {
                        if (unitData.id == "slum_dancer") synthesize_firstIng = true;
                        else if (unitData.id == "rein") synthesize_secondIng = true;
                        else if (unitData.id == "synthesizer")
                        {
                            if (!synthesize_firstIng) synthesize_firstIng = true;
                            else synthesize_secondIng = true;
                        }
                    }
                }
                return (synthesize_firstIng && synthesize_secondIng);
            case "NATIS":
                foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit unitData = unit.GetComponent<scr_unit>();
                    if (unit.transform.localPosition.y < 0)
                    {
                        if (unitData.id == "wind_walker") synthesize_firstIng = true;
                        else if (unitData.id == "necky") synthesize_secondIng = true;
                        else if (unitData.id == "synthesizer")
                        {
                            if (!synthesize_firstIng) synthesize_firstIng = true;
                            else synthesize_secondIng = true;
                        }
                    }
                }
                return (synthesize_firstIng && synthesize_secondIng);
            case "ROGER":
                foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit unitData = unit.GetComponent<scr_unit>();
                    if (unit.transform.localPosition.y < 0)
                    {
                        if (unitData.id == "wave_fighter") synthesize_firstIng = true;
                        else if (unitData.id == "ken") synthesize_secondIng = true;
                        else if (unitData.id == "synthesizer")
                        {
                            if (!synthesize_firstIng) synthesize_firstIng = true;
                            else synthesize_secondIng = true;
                        }
                    }
                }
                return (synthesize_firstIng && synthesize_secondIng);
            case "LATIEN":
                foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit unitData = unit.GetComponent<scr_unit>();
                    if (unit.transform.localPosition.y < 0)
                    {
                        if (unitData.terial == 6) overlay_count++;
                    }
                }
                return (overlay_count >= 2);
            case "SENA":
                foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit unitData = unit.GetComponent<scr_unit>();
                    if (unit.transform.localPosition.y < 0)
                    {
                        if (unitData.terial == 4) overlay_count++;
                    }
                }
                return (overlay_count >= 2);
            case "BT":
                return (extraFlag && scr_playerInGame.tomb.Count >= 12);
            case "TONUS":
                foreach (string card in scr_playerInGame.deck)
                {
                    if ((int)DB_card.getIdentity(card)[0] == 7)
                    {
                        synthesize_firstIng = true;
                        break;
                    }
                }
                return (extraFlag && synthesize_firstIng);
            case "LICA":
                return (extraFlag && scr_playerInGame.hand.Count <= 2);
            case "MACHINA":
                getPlayer();
                return (localPlayer.GetComponent<scr_playerIdentity>().sacrificeCnt >= 20);
            case "EURIEL":
                return (extraFlag && (scr_playerInGame.deck.Contains("#elemental_book") || scr_playerInGame.hand.Contains("#elemental_book")));
            case "INFINITAS":
                getPlayer();
                return (extraFlag && localPlayer.GetComponent<scr_playerIdentity>().sacrificeCnt >= 10);
            case "SAYRUN":
                getPlayer();
                return (localPlayer.GetComponent<scr_playerIdentity>().sacrificeCnt >= 5);
            case "HETERO":
                getPlayer();
                bool temp_flag = true;
                ArrayList temp_deck = new ArrayList();
                foreach (string cardId in scr_playerInGame.deck)
                {
                    foreach (string compareId in temp_deck)
                    {
                        if (compareId == cardId)
                        {
                            temp_flag = false;
                            break;
                        }
                    }
                    if (!temp_flag) break;
                    temp_deck.Add(cardId);
                }
                return (extraFlag && temp_flag);
            default:
                return false;
        }
    }

    // if there's a meditator, return true.
    public bool getMeditator() {
        bool flag = false;
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Meditator")) {
            med = i;
            flag = true;
        }
        return flag;
    }

    public bool getPlayer() {
        bool flag = false;
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Player")) {
            if (i.GetComponent<scr_playerController>().isPlayer)
            {
                localPlayer = i;
                flag = true;
            }
        }
        return flag;
    }
}
