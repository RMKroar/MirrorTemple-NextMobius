using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class scr_playerInGame : NetworkBehaviour {
    public static ArrayList deck = new ArrayList();
    public static ArrayList hand = new ArrayList();
    public static ArrayList tomb = new ArrayList();
    public static Queue cardQueue = new Queue();

    public GameObject card;
    public GameObject other_card;
    public GameObject canvas;
    public GameObject med;
    public GameObject unit;

    // for spawn Card
    public GameObject spawn_card;
    public Vector3 spawn_pos;
    public ArrayList spawn_data;

    int sav_handcnt = 0;

    void Start() {
        if (!isLocalPlayer) enabled = false;
        Debug.Log(scr_storeDeck.store_deck.ToArray());
        deck.Clear();
        foreach (string cardId in scr_storeDeck.store_deck) {
            deck.Add(cardId);
        }
        if (isLocalPlayer) StartCoroutine(DelayDraw());
    }

    void Update()
    {
        if (sav_handcnt != hand.Count) {
            informHand();
            displayHand();

            sav_handcnt = hand.Count;
        }

        if (getMeditator()) {
            displayOtherHand();
        }

        if (spawn_card != null)
        {
            if (isServer) CmdSpawnUnit();
            else {
                CmdSpawnUnitByMed();
                hand.Remove((string)spawn_data[8]);
            }
            spawn_card = null;
        }
    }

    // informHand() and displayHand() need to be refreshed when stack 'hand' is changed.
    public void mulligan() {
        cardQueue.Enqueue("draw");
        cardQueue.Enqueue("draw");
        cardQueue.Enqueue("draw");
    }

    public void displayHand() {
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Card")) {
            Destroy(cur);
        }

        foreach(string cardId in hand)
        {
            int temp_rank = (int)DB_card.getIdentity(cardId)[3];
            if (temp_rank == 3) hand.Remove(cardId);
        }

        while(hand.Count > 10)
        {
            hand.RemoveAt(hand.Count - 1);
        }

        int handCnt = hand.Count;
        int posX = 0;
        canvas = GameObject.Find("Canvas");
        int expansion = 0;

        for (int it = 0; it < hand.Count; it++) {
            if (it == 0)
            {
                posX = 430 - 50 * handCnt;
                if (posX < 180) posX = 180;
            }
            else if (it == 5) {
                expansion = 1;
                posX = 430 - 50 * (handCnt - 5);
            }
            canvas.GetComponent<scr_canvas>().displayCard(card, (string)hand[it], posX, expansion);
            posX += 100;
        }
    }

    public void displayOtherHand() {
        int cnt = 0;

        if (getMeditator()) {
            if (isServer) cnt = med.GetComponent<scr_meditator>().clientHandcnt;
            else cnt = med.GetComponent<scr_meditator>().serverHandcnt;
        }

        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("OtherCard"))
        {
            Destroy(cur);
        }

        int posX = 430 - 50 * cnt;
        int expansion = 0;
        canvas = GameObject.Find("Canvas");

        for (int it = 0; it < cnt; it++)
        {
            if (it == 0)
            {
                posX = 430 - 50 * cnt;
                if (posX < 180) posX = 180;
            }
            else if (it == 5)
            {
                expansion = 1;
                posX = 430 - 50 * (cnt - 5);
            }
            GameObject c = Instantiate(other_card, new Vector3(0, 0, 0), Quaternion.identity);
            c.transform.SetParent(canvas.transform);
            c.GetComponent<Transform>().localPosition = new Vector3(posX, 335 - 160 * expansion, 0);
            c.GetComponent<Transform>().localScale = new Vector3(0.3f, 0.3f, 1);
            posX += 100;
        }
    }

    public void informHand() {
        if (getMeditator()) {
            if (isServer) med.GetComponent<scr_meditator>().informHand(true, hand.Count);
            else gameObject.GetComponent<scr_playerController>().direct_informHand(hand.Count);
        }

    }

    public void setCardSpawn(GameObject obj, Vector3 pos, ArrayList data)
    {
        spawn_card = obj;
        spawn_pos = pos;
        spawn_data = data;
    }

    IEnumerator DelayDraw() {
        while (true) {
            if (cardQueue.Count == 0)
            {
                yield return null;
            }
            else {
                string cardId = (string)cardQueue.Dequeue();
                if (cardId == "draw") {
                    drawCard();
                }
                yield return new WaitForSeconds(1f);
            }                      
        }     
    }

    public void drawCard() {
        if (deck.Count > 0)
        {
            int drawIndex = Random.Range(0, deck.Count);
            GameObject ins = Instantiate(card, GameObject.Find("Canvas").transform);
            DB_card.setIdentity(ins, (string)deck[drawIndex], new Vector3(172, 44, 0), new Vector3(0.3f, 0.3f, 1));
            ins.GetComponent<scr_cardMove>().enabled = true;
            ins.GetComponent<scr_cardGUI>().enabled = false;
            ins.tag = "Undef";
            deck.RemoveAt(drawIndex);
        }
        else {
            gameObject.GetComponent<scr_playerIdentity>().CmdEmptyDeck();
        }

        informHand();
        displayHand();
    }

    [Command]
    public void CmdSpawnUnit()
    {
        GameObject ins = Instantiate(unit, new Vector3(-4000, 0, 0), Quaternion.identity);
        ins.GetComponent<scr_unit>().start_pos = spawn_pos;
        ins.GetComponent<scr_unit>().setIdentity(true, (int)spawn_data[0], (int)spawn_data[1],
                   (int)spawn_data[2], (int)spawn_data[3], (string)spawn_data[4], (string)spawn_data[5],
                   (string)spawn_data[6], (string)spawn_data[7], (string)spawn_data[8]);
        NetworkServer.SpawnWithClientAuthority(ins, connectionToClient);
        if(spawn_card.tag == "Card") hand.Remove(spawn_card.GetComponent<scr_card>().id);
    }

    [Command]
    public void CmdSpawnUnitByMed()
    {
        if (getMeditator()) {
            GameObject ins = Instantiate(unit, new Vector3(-4000, 0, 0), Quaternion.identity);

            ins.GetComponent<scr_unit>().start_pos = med.GetComponent<scr_meditator>().spawn_pos;
            ins.GetComponent<scr_unit>().setIdentity(false, med.GetComponent<scr_meditator>().terial,
                med.GetComponent<scr_meditator>().attack, med.GetComponent<scr_meditator>().health,
                med.GetComponent<scr_meditator>().rank, med.GetComponent<scr_meditator>().cardname,
                med.GetComponent<scr_meditator>().description, med.GetComponent<scr_meditator>().spawn_tag,
                med.GetComponent<scr_meditator>().pic_url, med.GetComponent<scr_meditator>().id);
            NetworkServer.SpawnWithClientAuthority(ins, connectionToClient);
        }
    }

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
}
