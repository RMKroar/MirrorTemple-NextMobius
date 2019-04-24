using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class scr_playerIdentity : NetworkBehaviour {
    [SyncVar]
    public int terial = 1;
    [SyncVar]
    public int health = 15;
    [SyncVar]
    public int max_health = 15;
    [SyncVar]
    public int sav_turn = 0;
    [SyncVar]
    public int sacrificeCnt = 0;
    [SyncVar]
    public string environment = null;
    [SyncVar]
    public string id;

    public bool is_enemy;
    public GameObject otherDisp;
    SpriteRenderer rend;

    void Start()
    {
        rend = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        is_enemy = !isLocalPlayer;
        if (isLocalPlayer) Cmdinitialize(scr_storeDeck.extraID);

        if (is_enemy) transform.GetChild(3).GetComponent<scr_extractButton>().enabled = false;
    }

    void Update()
    {
        if (rend.sprite == null) rend.sprite = Resources.Load<Sprite>("sprites/Units/Extra/" + id) as Sprite;
        setSprites();
        if (health <= 0) GameOver();
    }
    
    [Command]
    public void CmdEmptyDeck() {
        health = 0;
    }

    public void GameOver()
    {
        GameObject resPanel = GameObject.Find("Canvas").transform.GetChild(8).gameObject;
        resPanel.SetActive(true);
        resPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = (isLocalPlayer) ? "패배..." : "승리!";
        enabled = false;
        scr_playerInGame.tomb.Clear();
        scr_playerInGame.hand.Clear();
        scr_guiController.extraFlag = true;
    }

    public void setSprites() {
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = scr_card.numbers[terial];
        SpriteRenderer hp = transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer temp_extend = transform.GetChild(2).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        if (health < 10)
        {
            hp.sprite = (health > -1) ? scr_card.numbers[health] : null;
            temp_extend.sprite = null;
            transform.GetChild(2).localPosition = new Vector3(97.5f, -185.6f, 0);
        }
        else
        {
            hp.sprite = scr_card.numbers[health % 10];          
            temp_extend.sprite = scr_card.numbers[health / 10];
            transform.GetChild(2).localPosition = new Vector3(118.5f, -185.6f, 0);
        }
    }

    public void selectEffect(string id, string code) {
        if (code == "DRAW")
        {
            scr_playerInGame.deck.Remove(id);
            scr_playerInGame.hand.Add(id);
        }
        else if (code == "GETHAND")
        {
            scr_playerInGame.hand.Add(id);
        }
        else if (code == "DISCARDHAND")
        {
            scr_playerInGame.hand.Remove(id);
            scr_playerInGame.tomb.Add(id);
        }
        else if (code == "DISCARDHANDTOSACRIFICE")
        {
            scr_playerInGame.hand.Remove(id);
            scr_playerInGame.tomb.Add(id);
            sacrificeCnt += (int)DB_card.getIdentity(id)[0];
        }
        else if (code == "DRAWPANELTY_COSTDAMAGE")
        {
            scr_playerInGame.deck.Remove(id);
            scr_playerInGame.hand.Add(id);
            health -= (int)DB_card.getIdentity(id)[0];
        }
        else if (code == "RECALL")
        {
            scr_playerInGame.tomb.Remove(id);
            scr_playerInGame.hand.Add(id);
        }
        else if (code == "SERIATODECK")
        {
            scr_playerInGame.hand.Remove(id);
            scr_playerInGame.deck.Add("Seria");
        }
        else if (code == "REMOVE_CAMOUFLAGE")
        {
            scr_playerInGame.tomb.Remove(id);
            foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
            {
                scr_unit curData = cur.GetComponent<scr_unit>();
                if (curData.id == "pyren_zombie") curData.CmdCamouflage(id);
            }
        }
        else if (code == "CHANGETAG") {
            string tempTag = (string)DB_card.getIdentity(id)[6];
            foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit")) {
                scr_unit curData = cur.GetComponent<scr_unit>();
                if (curData.hosts == GetComponent<scr_playerController>().isHost) {
                    curData.CmdsetTag(tempTag);
                }
            }
        }
        else if (code == "ELEMENTALFIELD")
        {
            string tempTag = (string)DB_card.getIdentity(id)[6];
            CmdSetEnvironment("$" + tempTag);
        }
        gameObject.GetComponent<scr_playerInGame>().displayHand();
    }

    [Command]
    public void Cmdinitialize(string id) {
        health = 15;
        max_health = 15;
        this.id = id;
    }
    
    [Command]
    public void CmdgainTerial() {
        GameObject med = GameObject.FindGameObjectWithTag("Meditator");
        terial = med.GetComponent<scr_meditator>().turnCnt / 2 + 1;
        if (terial > 7) terial = 7;
    }

    [Command]
    public void CmduseTerial(int amount) {
        if (environment != "archmage_rune") terial -= amount;
    }

    [Command]
    public void Cmdextract(GameObject unit) {
        scr_unit data = unit.GetComponent<scr_unit>();
        terial -= 2;
        data.extracted = true;
        data.CmdgetSelfStatus(1, 1);

        if (data.description.Contains("[개방")) {
            switch (data.id) {
                case "Vera":
                    data.CmdapplyDirectDamage(getOtherPlayer(gameObject.GetComponent<scr_playerController>().isHost), 3);
                    break;
                case "raysen_samurai":
                    getOtherPlayer(gameObject.GetComponent<scr_playerController>().isHost).GetComponent<scr_playerIdentity>().terial += 2;
                    break;
                default:
                    break;
            }
        }
    }

    [Command]
    public void CmdDamage(GameObject cur, int amount)
    {
        if (cur.transform.tag == "Unit")
        {
            scr_unit scr = cur.GetComponent<scr_unit>();

            scr.health -= amount;
            if (scr.health > scr.max_health)
            {
                scr.health = scr.max_health;
            }
        }
        else if (cur.transform.tag == "Player")
        {
            scr_playerIdentity scr = cur.GetComponent<scr_playerIdentity>();

            scr.health -= amount;
            if (scr.health > scr.max_health)
            {
                scr.health = scr.max_health;
            }
        }
    }

    // checking mine and not mine : cur.GetComponent<scr_unit>().hosts != hosts;
    [Command]
    public void CmdforUnitSpell(GameObject target, bool hosts, string id, int cost)
    {
        scr_unit data = target.GetComponent<scr_unit>();
        int cnt_flag = 0;
        bool used = true;

        switch (id)
        {
            case "#cost_control":
                data.CmdsetTerial(terial);
                break;
            case "#elemental_reset":
                data.CmdsetTag("");
                break;
            case "#draft":
                data.CmdsetTag("Warrior");
                break;
            case "#slash":
                data.CmdDamage(target, 2);
                break;
            case "#widdy_breath":
                data.CmdgetSelfStatus(data.health - data.attack, data.attack - data.health);
                RpcGaincard(hosts, "widdy");
                break;
            case "#shrink":
                data.CmdgetSelfStatus(1 - data.attack, 0);
                break;
            case "#ador_breath":
                data.CmdDamage(target, 2);
                RpcGaincard(hosts, "ador");
                break;
            case "#dark_ora":
                data.CmdDamage(target, 2);
                break;
            case "#brainwashing":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit curData = cur.GetComponent<scr_unit>();
                    if (curData.hosts == hosts && curData.card_tag.Contains("Shine"))
                    {
                        cnt_flag++;
                        NetworkServer.Destroy(cur);
                    }
                }
                if (cnt_flag > 0)
                {
                    RpcGaincard(hosts, data.id);
                    data.CmdDestroyUnit(target);
                    break;
                }
                else used = false;
                break;
            case "#recall":
                if (data.hosts != hosts) used = false;
                else {
                    RpcGaincard(hosts, data.id);
                    data.CmdDestroyUnit(target);
                }
                break;
            case "#dark_hand":
                data.CmdDamage(target, 3);
                break;
            case "#armored":
                data.CmdgetSelfStatus(2, 4);
                break;
            case "#gladius":
                if (data.card_tag.Contains("Warrior")) data.CmdgetSelfStatus(3, 2);
                else used = false;
                break;
            case "#titanum_shield":
                if (data.card_tag.Contains("Warrior")) {
                    data.CmdgetSelfStatus(1, 2);
                    RpcGaincard(hosts, "draw");
                }
                else used = false;
                break;
            case "#selfburn":
                if (data.card_tag.Contains("Fire")) {
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        if (cur.GetComponent<scr_unit>().hosts != hosts) {
                            data.CmdDamage(cur, data.attack);
                        }
                    }
                }
                CmdDamage(target, data.health);
                break;
            case "#death":
                data.CmdDestroyUnit(target);
                break;
            case "#latien_symphony":
                data.CmdDamage(target, data.health);
                break;
            default:
                break;
        }
        CmduseTerial(cost);
        if (used) RpcUseSpell(hosts, id);
    }

    [Command]
    public void CmdforAllSpell(bool hosts, string id, int cost)
    {
        bool used = true;

        switch (id) {
            case "#vitality":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit data = cur.GetComponent<scr_unit>();
                    if (data.hosts == hosts && data.card_tag.Contains("Earth"))
                    {
                        data.CmdgetSelfStatus(0, 2);
                    }
                }
                break;
            case "#luminous_grail":
                CmdGainStatusExtended(hosts, 0, 3, 3);
                break;
            case "#dark_token":
                RpcGaincard(hosts, "daki");
                RpcGaincard(hosts, "daki");
                break;
            case "#blood_storm":
                int temp_cnt = 0;
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit data = cur.GetComponent<scr_unit>();
                    if (data.hosts != hosts) temp_cnt++;
                }
                if (temp_cnt >= 4)
                {
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        scr_unit data = cur.GetComponent<scr_unit>();
                        data.CmdDamage(cur, 3);
                    }
                }
                else
                {
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        scr_unit data = cur.GetComponent<scr_unit>();
                        data.CmdDamage(cur, 1);
                    }
                }
                break;
            case "#volcano":
                CmdSetEnvironment("volcano");
                break;
            case "#dimension_summon":
                ArrayList temp_table = new ArrayList();
                foreach (string cardId in DB_card.collectible)
                {
                    if (!cardId.Contains("#")) temp_table.Add(cardId);
                }
                int drawIndex = Random.Range(0, temp_table.Count);
                CmdGainCard(hosts, (string)temp_table[drawIndex]);
                break;
            case "#tales_river":
                CmdSetEnvironment("tales_1");
                break;
            case "#salvage":
                RpcSelectCard(hosts, id);
                break;
            case "#evolve:SaintResia":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit")) {
                    scr_unit curData = cur.GetComponent<scr_unit>();
                    if (curData.hosts == hosts && curData.id == "Resia") {
                        curData.CmdCamouflage("Angel_resia");
                    }
                }
                break;
            case "#evolve:DarkResia":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit curData = cur.GetComponent<scr_unit>();
                    if (curData.hosts == hosts && curData.id == "Resia")
                    {
                        curData.CmdCamouflage("Dark_resia");
                    }
                }
                break;
            case "#nas_breath":
                RpcGaincard(hosts, "nas");
                CmdGainStatus(hosts, 0, 4);
                break;
            case "#assembly":
                RpcSelectCard(hosts, id);
                break;
            case "#initial_shine":
                RpcGaincard(hosts, "draw");
                break;
            case "#dangerous_contact":
                scr_meditator med = GameObject.FindGameObjectWithTag("Meditator").GetComponent<scr_meditator>();
                int sav_handCnt = (hosts)? med.serverHandcnt : med.clientHandcnt;
                for (int i = 0; i < 3; i++) {
                    RpcGaincardExtended(hosts, "draw", "NotDarkRemove");
                }
                if (sav_handCnt != -1) {
                    if (hosts) {
                        if (sav_handCnt == med.serverHandcnt) {
                            RpcGaincardExtended(hosts, "draw", "RemoveAll");
                        }
                    }
                    else {
                        if (sav_handCnt == med.clientHandcnt) {
                            RpcGaincardExtended(hosts, "draw", "RemoveAll");
                        }
                    }                       
                }
                break;
            case "#doll_gathering":
                for (int i = 0; i < 3; i++) {
                    RpcGaincardExtended(hosts, "doll", "IFINDECK");
                }
                break;
            case "#doll_regression":
                for (int i = 0; i < 3; i++) {
                    RpcGaincardExtended(hosts, "doll", "TOMBTOHAND");
                }
                RpcGaincard(hosts, "draw");
                break;
            case "#doll_sacrifice":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit curData = cur.GetComponent<scr_unit>();
                    if (curData.hosts == hosts && (curData.id == "doll" || curData.id == "tetra"))
                    {
                        curData.CmdgetSelfStatus(1, 1);
                    }
                }
                break;
            case "#wisdom_eye":
                RpcGaincard(hosts, "draw");
                RpcGaincard(hosts, "draw");
                break;
            case "#heal_leaf":
                CmdGainStatus(hosts, 0, 8);
                break;
            case "#earthquake":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit")) {
                    scr_unit data = cur.GetComponent<scr_unit>();
                    if (!data.card_tag.Contains("Earth")) {
                        data.CmdDamage(cur, 2);
                    }
                }
                break;
            case "#luminous_badge":
                CmdSetEnvironment("luminous_badge");
                break;
            case "#Archmage_rune":
                CmduseTerial(cost);
                CmdSetEnvironment("archmage_rune");
                break;
            case "#union_roar":
                CmdSelectCard(hosts, id);
                break;
            case "#ren_wakeup":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit")) {
                    cur.GetComponent<scr_unit>().CmdDamage(cur, 5);
                }
                break;
            case "#halfmoon_strike":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit curData = cur.GetComponent<scr_unit>();
                    if(curData.hosts != hosts) curData.CmdDamage(cur, 6);
                }
                RpcGaincard(hosts, "draw");
                RpcGaincard(hosts, "draw");
                break;
            case "#latien_prelude":
                RpcGaincard(hosts, "draw");
                RpcGaincard(hosts, "draw");
                break;
            case "#latien_choir":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit curData = cur.GetComponent<scr_unit>();
                    if (curData.hosts == hosts) curData.CmdgetSelfStatus(2, 2);
                }
                break;
            case "#latien_fantasia":
                RpcSelectCard(hosts, id);
                break;
            case "#latien_lyrics":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit curData = cur.GetComponent<scr_unit>();
                    if (curData.hosts != hosts) curData.CmdDamage(cur, 3);
                }
                break;
            default:
                break;
        }
        CmduseTerial(cost);
        if(used) RpcUseSpell(hosts, id);
    }

    [ClientRpc]
    public void RpcUseSpell(bool value, string id) {
        scr_playerController temp_data = gameObject.GetComponent<scr_playerController>();
        if (temp_data.isPlayer) {
            if (value == temp_data.isHost && id.Contains("#")) {
                scr_playerInGame.hand.Remove(id);
                scr_playerInGame.tomb.Add(id);
                gameObject.GetComponent<scr_playerInGame>().displayHand();
            }
        }
        else
        {
            if (value == temp_data.isHost) {
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("OtherDisp")) {
                    if (cur.GetComponent<scr_cardMove>().durability > 30) cur.GetComponent<scr_cardMove>().durability = 30;
                }
                GameObject ins = Instantiate(otherDisp, GameObject.Find("Canvas").transform);
                DB_card.setIdentity(ins, id, new Vector3(-800, 400, 0), new Vector3(0.3f, 0.3f, 1));
                ins.GetComponent<scr_cardGUI>().enabled = false;
            }           
        }
        GameObject med = GameObject.FindGameObjectWithTag("Meditator");
        med.GetComponent<scr_meditator>().
            createParticle(6, (transform.localPosition.y > 0)? new Vector3(-220f, 97f, -480f) : new Vector3(-220f, -155f, -480f), -1);
    }

    [Command]
    public void CmdSelectCard(bool hosts, string id)
    {
        RpcSelectCard(hosts, id);
    }

    private GameObject getOtherPlayer(bool value) {
        GameObject other_player = null;
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (cur.GetComponent<scr_playerController>().isHost != value) other_player = cur;
        }
        return other_player;
    }

    [ClientRpc]
    public void RpcGaincard(bool value, string id)
    {
        scr_playerController temp_data = gameObject.GetComponent<scr_playerController>();
        if (value == temp_data.isHost && temp_data.isPlayer)
        {
            if (id == "draw")
            {
                scr_playerInGame.cardQueue.Enqueue("draw");
            }
            else {
                scr_playerInGame.hand.Add(id);
            }
            gameObject.GetComponent<scr_playerInGame>().displayHand();
        }
    }

    [ClientRpc]
    public void RpcGaincardExtended(bool value, string id, string code)
    {
        scr_playerController temp_data = gameObject.GetComponent<scr_playerController>();
        if (value == temp_data.isHost && temp_data.isPlayer)
        {
            if (id == "draw")
            {
                gameObject.GetComponent<scr_playerInGame>().drawCard();
            }
            else {
                if (code == "IFINDECK")
                {
                    if (scr_playerInGame.deck.Contains(id))
                    {
                        scr_playerInGame.hand.Add(id);
                        scr_playerInGame.deck.Remove(id);
                    }
                }
                else if (code == "TOMBTOHAND")
                {
                    if (scr_playerInGame.tomb.Contains(id))
                    {
                        scr_playerInGame.hand.Add(id);
                        scr_playerInGame.tomb.Remove(id);
                    }
                }
                else if (code == "GAINTOMB") scr_playerInGame.tomb.Add(id);
                else scr_playerInGame.hand.Add(id);
            }
            gameObject.GetComponent<scr_playerInGame>().displayHand();
        }
    }

    [Command]
    public void CmdGainStatus(bool hosts, int gain_terial, int gain_health)
    {
        CmdGainStatusExtended(hosts, gain_terial, gain_health, 0);               
    }

    [Command]
    public void CmdGainStatusExtended(bool hosts, int gain_terial, int gain_health, int gain_maxHealth)
    {
        if (GetComponent<scr_playerController>().isHost == hosts) {
            int sav_health = health;
            GameObject spec_unit = null;

            max_health += gain_maxHealth;
            terial += gain_terial;
            if (terial > 7) terial = 7;
            health += gain_health;
            if (health > max_health) health = max_health;

            if (health - sav_health > 0)
            {
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit curData = cur.GetComponent<scr_unit>();
                    if (curData.hosts == hosts && curData.id == "Angel_resia")
                    {
                        spec_unit = cur;
                        break;
                    }
                }
                if (spec_unit != null)
                {
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        scr_unit curData = cur.GetComponent<scr_unit>();
                        if (curData.hosts != hosts) spec_unit.GetComponent<scr_unit>().CmdDamage(cur, health - sav_health);
                    }
                    spec_unit.GetComponent<scr_unit>().CmdDamage(getOtherPlayer(gameObject.GetComponent<scr_playerController>().isHost), health - sav_health);
                }
            }
        }       
    }

    [ClientRpc]
    public void RpcSelectCard(bool value, string id)
    {
        scr_playerController temp_data = gameObject.GetComponent<scr_playerController>();
        if (value == temp_data.isHost && temp_data.isPlayer)
        {
            ArrayList table = new ArrayList();
            switch (id) {               
                case "volcanic_monster":
                    foreach (string cardId in scr_playerInGame.deck) {
                        string cardTag = (string)DB_card.getIdentity(cardId)[6];                       
                        if (cardTag.Contains("Fire")) table.Add(cardId);
                    }
                    if (table.Count != 0)
                    {
                        GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                        GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(table, "DRAWPANELTY_COSTDAMAGE");
                    }
                    break;
                case "dollhugging_girl":
                    foreach (string cardId in scr_playerInGame.deck)
                    {
                        string cardName = (string)DB_card.getIdentity(cardId)[4];
                        string cardDesc = (string)DB_card.getIdentity(cardId)[5];
                        if (cardName.Contains("전투 인형") || cardDesc.Contains("전투 인형")) table.Add(cardId);
                    }
                    if (table.Count != 0) {
                        GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                        GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(table, "DRAW");
                    }                   
                    break;
                case "cave_guardian":
                    if (scr_playerInGame.hand.Count != 0)
                    {
                        GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                        GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(scr_playerInGame.hand, "DISCARDHAND");
                    }
                    break;
                case "neprafti":                   
                    for (int i = 0; i < 3; i++) {
                        if (scr_playerInGame.deck.Count != 0)
                        {
                            int drawIndex = Random.Range(0, scr_playerInGame.deck.Count);
                            table.Add(scr_playerInGame.deck[drawIndex]);
                            scr_playerInGame.tomb.Add(scr_playerInGame.deck[drawIndex]);
                            scr_playerInGame.deck.RemoveAt(drawIndex);
                        }
                        else break;
                    }
                    if (table.Count != 0)
                    {
                        GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                        GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(table, "RECALL");
                    }
                    break;
                case "raysen_fisherman":
                    int temp_cost = 2;

                    if (environment == "tales_1") temp_cost++;
                    else if (environment == "tales_2") temp_cost += 2;
                    else if (environment == "tales_3") temp_cost += 3;

                    foreach (string cardId in scr_playerInGame.tomb)
                    {
                        int cardCost = (int)DB_card.getIdentity(cardId)[0];
                        string cardTag = (string)DB_card.getIdentity(cardId)[6];
                        
                        if (cardTag.Contains("Water") && cardCost <= temp_cost) table.Add(cardId);
                    }
                    if (table.Count != 0)
                    {
                        GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                        GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(table, "RECALL");
                    }
                    break;
                case "student_magician":
                    foreach(string cardId in scr_playerInGame.hand)
                    {
                        if (!cardId.Contains("#")) table.Add(cardId);
                    }
                    if (table.Count != 0)
                    {
                        GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                        GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(table, "CHANGETAG");
                    }
                    break;
                case "pyren_zombie":
                    foreach (string cardId in scr_playerInGame.tomb)
                    {
                        int cardCost = (int)DB_card.getIdentity(cardId)[0];
                        string cardTag = (string)DB_card.getIdentity(cardId)[6];

                        if (cardTag.Contains("Dark") && cardCost <= 4) table.Add(cardId);
                    }
                    if (table.Count != 0)
                    {
                        GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                        GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(table, "REMOVE_CAMOUFLAGE");
                    }
                    break;
                case "mild_bear":
                    foreach (string cardId in scr_playerInGame.deck)
                    {
                        int cardCost = (int)DB_card.getIdentity(cardId)[0];
                        string cardTag = (string)DB_card.getIdentity(cardId)[6];

                        if (cardTag.Contains("Beast") && cardCost <= 3) table.Add(cardId);
                    }
                    if (table.Count != 0)
                    {
                        GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                        GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(table, "DRAW");
                    }
                    break;
                case "lion_tamer":
                    table.Add("petree"); table.Add("juen"); table.Add("besto");
                    GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                    GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(table, "GETHAND");
                    break;
                case "alcadrobot_typeA":
                    foreach (string cardId in scr_playerInGame.deck)
                    {
                        if ((int)DB_card.getIdentity(cardId)[0] == 3) table.Add(cardId);
                    }
                    if (table.Count != 0)
                    {
                        GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                        GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(table, "DRAW");
                    }
                    break;
                case "alcadrobot_typeB":
                    foreach (string cardId in scr_playerInGame.deck)
                    {
                        if ((int)DB_card.getIdentity(cardId)[0] == 4) table.Add(cardId);
                    }
                    if (table.Count != 0)
                    {
                        GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                        GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(table, "DRAW");
                    }
                    break;
                case "#latien_fantasia":
                    if (scr_playerInGame.tomb.Count != 0)
                    {
                        int temp_index = Random.Range(0, scr_playerInGame.tomb.Count);
                        scr_playerInGame.hand.Add(scr_playerInGame.tomb[temp_index]);
                        scr_playerInGame.tomb.RemoveAt(temp_index);
                    }
                    break;
                case "#assembly":
                    foreach (string cardId in scr_playerInGame.deck) {
                        string temp_tag = (string)DB_card.getIdentity(cardId)[6];
                        if (temp_tag.Contains("Warrior") && (int)DB_card.getIdentity(cardId)[0] <= 4) table.Add(cardId);
                    }
                    for (int i = 0; i < 2; i++) {
                        if (table.Count != 0)
                        {
                            int temp_index = Random.Range(0, table.Count);
                            scr_playerInGame.hand.Add(table[temp_index]);
                            scr_playerInGame.deck.Remove(table[temp_index]);
                        }
                        else break;
                    }
                    break;
                case "#salvage":
                    foreach (string cardId in scr_playerInGame.tomb)
                    {
                        string temp_tag = (string)DB_card.getIdentity(cardId)[6];
                        if (temp_tag.Contains("Water")) table.Add(cardId);
                    }
                    if (table.Count != 0)
                    {
                        int temp_index = Random.Range(0, table.Count);
                        scr_playerInGame.hand.Add(table[temp_index]);
                        scr_playerInGame.tomb.Remove(table[temp_index]);
                    }
                    break;
                case "#dimension_summon":
                    foreach (string cardId in DB_card.collectible) {
                        if (!cardId.Contains("#")) table.Add(cardId);
                    }
                    scr_playerInGame.hand.Add(table[Random.Range(0, table.Count)]);
                    break;
                case "#union_roar":
                    foreach (string cardId in scr_playerInGame.tomb)
                    {
                        string temp_tag = (string)DB_card.getIdentity(cardId)[6];
                        if (temp_tag.Contains("Dragon") && (int)DB_card.getIdentity(cardId)[0] <= 4) table.Add(cardId);
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        if (table.Count != 0)
                        {
                            int temp_index = Random.Range(0, table.Count);
                            scr_playerInGame.hand.Add(table[temp_index]);
                            scr_playerInGame.tomb.Remove(table[temp_index]);
                            table.RemoveAt(temp_index);
                        }
                        else break;
                    }
                    scr_playerInGame.cardQueue.Enqueue("draw");
                    break;
                case "SENA":
                    GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                    GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(scr_playerInGame.hand, "SERIATODECK");
                    break;
                case "TONUS":
                    GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                    GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(scr_playerInGame.deck, "DRAW");
                    break;
                case "EURIEL":
                    table.Add("ador"); table.Add("nas"); table.Add("widdy"); table.Add("glen"); table.Add("lumen");
                    table.Add("daki"); table.Add("karma"); table.Add("zic"); table.Add("rea"); table.Add("bao");
                    table.Add("spira"); table.Add("kaka"); table.Add("del");
                    GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                    GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(table, "ELEMENTALFIELD");
                    break;
                case "HETERO":
                    GameObject.Find("Canvas").GetComponent<scr_canvas>().enableSelect();
                    GameObject.Find("SelectPanel").GetComponent<scr_select>().setTable(scr_playerInGame.tomb, "RECALL");
                    break;
                default:
                    break;
            }
            table.Clear();
        }
    }

    [Command]
    public void CmdSetSacrificeCnt(bool value, int amount) {
        RpcSetSacrificeCnt(value, amount);
    }

    [ClientRpc]
    public void RpcSetSacrificeCnt(bool value, int amount)
    {
        scr_playerController temp_data = gameObject.GetComponent<scr_playerController>();
        if (value == temp_data.isHost && temp_data.isPlayer)
        {
            sacrificeCnt += amount;
        }
    }

    [Command]
    public void CmdSetEnvironment(string envId)
    {
        if (envId == "tales_1")
        {
            if (environment == "tales_1") environment = "tales_2";
            else if (environment == "tales_2") environment = "tales_3";
            else if (environment == "" || environment == null) environment = envId;
        }
        else environment = envId;
    }

    [Command]
    public void CmdGainCard(bool value, string id) {
        RpcGaincard(value, id);
    }

    [Command]
    public void CmdGainCardExtended(bool value, string id, string code) {
        RpcGaincardExtended(value, id, code);
    }
}
