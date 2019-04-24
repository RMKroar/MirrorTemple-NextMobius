using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class scr_playerController : NetworkBehaviour
{
    public bool isHost;
    public bool isPlayer = false;
    public bool myturn;

    bool turnAssignFlag = false;

    public GameObject meditator;
    public GameObject unit;

    GameObject med;

    public GameObject turnEndText;

    bool sav_myturn = false;
    bool flag = false;

    void Awake()
    {
        if (getPlayers() == 1)
        {
            isHost = true;
        }
        gameObject.GetComponent<scr_playerInGame>().canvas = GameObject.Find("Canvas");
    }

    void Start()
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        if (isLocalPlayer)
        {
            isPlayer = true;
            transform.localPosition = new Vector3(-577f, -225f, 0);
        }
        else {
            transform.localPosition = new Vector3(-577f, 225f, 0);
        }
        transform.localScale = new Vector3(0.435f, 0.435f, 1);
        turnEndText = GameObject.Find("TurnEnd").transform.GetChild(0).gameObject;
        
    }

    void Update()
    {
        if (!flag) {
            transform.SetParent(GameObject.Find("Canvas").transform);
            turnEndText = GameObject.Find("TurnEnd").transform.GetChild(0).gameObject;
            transform.localScale = new Vector3(0.435f, 0.435f, 1);
            flag = true;
        }

        if (isLocalPlayer)
        {
            isPlayer = true;
            transform.localPosition = new Vector3(-577f, -300f, 0);
        }
        else
        {
            transform.localPosition = new Vector3(-577f, 300f, 0);
        }

        if (!turnAssignFlag)
        {
            if (isPlayer && isHost)
            {
                int check = getPlayers();
                if (check == 2)
                {
                    Cmdspawn_meditator();
                    assignTurn();
                    turnAssignFlag = true;
                }
            }
        }

        if (isPlayer)
        {
            if (getMeditator())
            {
                scr_meditator inv = med.GetComponent<scr_meditator>();
                myturn = (inv.getTurn() == isHost);
                if (sav_myturn != myturn) {
                    sav_myturn = myturn;
                    if (myturn) {
                        inv.createParticle(0, Vector3.zero, -1);                       
                        if (inv.turnCnt != 0)
                        {
                            gameObject.GetComponent<scr_playerInGame>().drawCard();
                            gameObject.GetComponent<scr_playerIdentity>().CmdgainTerial();
                        }
                    }
                    
                }
            }
            turnEndText.GetComponent<Text>().text = (myturn) ? "턴 종료" : "상대 턴";
        }
    }

    public void assignTurn()
    {
        int rand = (int)(Random.value * 2);
        bool turn = (rand == 1) ? true : false;
        if (getMeditator())
        {
            med.GetComponent<scr_meditator>().setTurn(turn);
        }
    }

    private int getPlayers()
    {
        int cnt = 0;
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
        {
            cnt++;
        }
        return cnt;
    }

    // if there's a meditator, return true.
    public bool getMeditator()
    {
        bool flag = false;
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Meditator"))
        {
            med = i;
            flag = true;
        }
        return flag;
    }

    [Command]
    private void Cmdspawn_meditator()
    {
        GameObject med = Instantiate(meditator, transform.position, Quaternion.identity);
        NetworkServer.Spawn(med);
    }

    public void direct_cmdrequest()
    {
        Cmdrequest_meditator();
    }

    public void direct_informHand(int cnt)
    {
        CmdInformHand(cnt);
    }

    // [later patch needed] Need to be use string Parameter and split it
    public void direct_informData(ArrayList data, Vector3 pos) {
        CmdInformData((int)data[0], (int)data[1], (int)data[2], (int)data[3],
            (string)data[4], (string)data[5], (string)data[6], (string)data[7], (string)data[8], pos);
    }

    [Command]
    private void Cmdrequest_meditator()
    {
        if (getMeditator())
        {
            med.GetComponent<scr_meditator>().changeTurn();
        }
    }

    [Command]
    private void CmdInformHand(int cnt) {
        if (getMeditator())
        {
            med.GetComponent<scr_meditator>().informHand(false, cnt);
        }
    }

    // [later patch needed] Need to be use string Parameter and split it
    [Command]
    private void CmdInformData(int a, int b, int c, int d, string e, string f, string g, string h, string i, Vector3 pos) {
        if (getMeditator()) {
            med.GetComponent<scr_meditator>().setData(a, b, c, d, e, f, g, h, i, pos);
        }
    }
}
