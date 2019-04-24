using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class scr_unit : NetworkBehaviour
{
    [SyncVar]
    public Vector3 start_pos;

    [SyncVar]
    public bool hosts;

    [SyncVar]
    public int terial;

    [SyncVar]
    public int attack;

    [SyncVar]
    public int health;

    [SyncVar]
    public int rank;

    [SyncVar]
    public string cardname;

    [SyncVar]
    public string description;

    [SyncVar]
    public string card_tag;

    [SyncVar]
    public string pic_url;

    [SyncVar]
    public string id;

    [SyncVar]
    public int spawn_turn;

    [SyncVar]
    public int max_health = 0;

    [SyncVar]
    public bool extracted = false;

    [SerializeField]
    public SyncListString abils = new SyncListString();

    public GameObject dead;
    public bool isEnemy = false;
    bool flag = false;
    bool preFlag = false;

    public Sprite enemy;
    public Sprite extract;
    public Sprite enemy_extract;
    GameObject player;

    public int request_particle = 0;
    public int request_particle_value = 0;
    public GameObject request_particle_target;
    
    bool tombFlag = false;

    // Use this for initialization
    void Start() {
        GameObject canvas = GameObject.Find("Canvas");
        transform.SetParent(canvas.transform);
        transform.localScale = new Vector3(0.435f, 0.435f, 1);

        GameObject med = GameObject.FindGameObjectWithTag("Meditator");
        start_pos = med.GetComponent<scr_meditator>().spawn_pos;

        spawn_turn = med.GetComponent<scr_meditator>().turnCnt;
    }

    void Update()
    {
        if (getPlayer())
        {
            transform.localScale = new Vector3(0.435f, 0.435f, 1);
            transform.localPosition = start_pos;
            if (player.GetComponent<scr_playerController>().isHost != hosts)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = (extracted) ? enemy_extract : enemy;
                isEnemy = true;
                Transform t = gameObject.GetComponent<Transform>();
                t.localPosition = new Vector3(t.localPosition.x, -t.localPosition.y, t.localPosition.z);
            }
            else if (extracted) gameObject.GetComponent<SpriteRenderer>().sprite = extract;
        }
        GameObject pic = gameObject.transform.GetChild(0).gameObject;
        pic.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(pic_url) as Sprite;
        setSprites();

        Color myColor = gameObject.GetComponent<SpriteRenderer>().color;

        if (request_particle != 0) {
            CmdrequestParticle(request_particle, request_particle_target, request_particle_value);
            request_particle = 0;
            request_particle_target = null;
            request_particle_value = 0;
        }

        // pre-emerge
        if (!preFlag) {
            string checkPreEmerge = (string)DB_card.getIdentity(id)[5];
            if (description.Contains("[제물")) preEmerge();
            Inform();
            preFlag = true;
        }

        // emerge Effect && charge
        if (!flag && max_health == 0 && myColor.a >= 1) {
            flag = true;
            CmdsetMaxhealth();

            if (description.Contains("[돌진")) Cmdcharge();
            emerge();
        }
    }

    public void Inform() {
        if (getPlayer()) {
            if (player.GetComponent<scr_playerController>().isHost != hosts) {
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("OtherDisp"))
                {
                    if (cur.GetComponent<scr_cardMove>().durability > 30) cur.GetComponent<scr_cardMove>().durability = 30;
                }
                GameObject ins = Instantiate(player.GetComponent<scr_playerIdentity>().otherDisp, GameObject.Find("Canvas").transform);
                DB_card.setIdentity(ins, id, new Vector3(-800, 400, 0), new Vector3(0.3f, 0.3f, 1));
                ins.GetComponent<scr_cardGUI>().enabled = false;
            }
        }
    }

    // invoked by parent object 'drag'
    public bool canAttack()
    {
        GameObject med = GameObject.FindGameObjectWithTag("Meditator");
        return spawn_turn != med.GetComponent<scr_meditator>().turnCnt && med.GetComponent<scr_meditator>().turn == hosts;
    }

    public bool canSpell() {
        GameObject med = GameObject.FindGameObjectWithTag("Meditator");
        return med.GetComponent<scr_meditator>().turn == hosts;
    }

    public void setSprites()
    {
        // death Effect
        if (health <= 0) {
            if (death()) {
                if(rank != 3) CmdAddTomb();                
                else CmdDestroyUnit(gameObject);    // need to be replaced by method supports unit devastation.
            }
        }
        SpriteRenderer at = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        if (attack < 10)
        {
            at.sprite = (attack > -1) ? scr_card.numbers[attack] : null;
            SpriteRenderer temp_extend = transform.GetChild(1).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            temp_extend.sprite = null;
            transform.GetChild(1).localPosition = new Vector3(-92, -184, 0);
        }
        else
        {
            at.sprite = scr_card.numbers[attack % 10];
            SpriteRenderer temp_extend = transform.GetChild(1).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            temp_extend.sprite = scr_card.numbers[attack / 10];
            transform.GetChild(1).localPosition = new Vector3(-71, -184, 0);
        }

        SpriteRenderer hp = transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>();
        if (health < 10)
        {
            hp.sprite = (health > -1) ? scr_card.numbers[health] : null;
            SpriteRenderer temp_extend = transform.GetChild(2).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            temp_extend.sprite = null;
            transform.GetChild(2).localPosition = new Vector3(98, -184, 0);
        }
        else
        {
            hp.sprite = scr_card.numbers[health % 10];
            SpriteRenderer temp_extend = transform.GetChild(2).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            temp_extend.sprite = scr_card.numbers[health / 10];
            transform.GetChild(2).localPosition = new Vector3(119, -184, 0);
        }
    }

    public void setIdentity(bool hosts, int terial, int attack, int health, int rank, string cardname, string description, string card_tag, string pic_url, string id) {
        this.hosts = hosts;
        this.terial = terial;
        this.health = health;
        this.attack = attack;
        this.rank = rank;
        this.cardname = cardname;
        this.description = description;
        this.card_tag = card_tag;
        this.pic_url = pic_url;
        this.id = id;
    }

    public bool getPlayer() {
        bool flag = false;
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player")) {
            if (cur.GetComponent<scr_playerController>().isPlayer) {
                player = cur;
                flag = true;
                break;
            }
        }
        return flag;
    }

    public bool isMyturn() {
        if (getPlayer()) {
            return player.GetComponent<scr_playerController>().myturn;
        } else return false;
    }

    public void preEmerge() {
        
        switch (id) {
            case "Volcanic_twindragon":            
                if (isMyturn()) {
                    GameObject preCheck = null;
                    bool spawnable = false;

                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit")) {
                        scr_unit curData = cur.GetComponent<scr_unit>();
                        if (curData.id == "volcanic_dragon") {
                            if (preCheck == null) preCheck = cur;
                            else {
                                preCheck.GetComponent<scr_unit>().CmdDestroyUnit(preCheck);
                                CmdrequestParticleByVector(7, cur.transform.localPosition, -1);
                                curData.CmdDestroyUnit(cur);
                                spawnable = true;
                                break;
                            }
                        } 
                    }
                    if (!spawnable) {
                        GainHand(id);
                        player.GetComponent<scr_playerIdentity>().CmdGainStatus(hosts, 2, 0);
                        CmdDestroyUnit(gameObject);
                    }
                } 
                break;
            case "Sytron":
                if (isMyturn()) {
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

                    if (!temp_flag)
                    {
                        GainHand(id);
                        player.GetComponent<scr_playerIdentity>().CmdGainStatus(hosts, 4, 0);
                        CmdDestroyUnit(gameObject);
                    }
                }
                break;
            case "Sardis":
                if (isMyturn())
                {
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

                    if (!temp_flag)
                    {
                        GainHand(id);
                        player.GetComponent<scr_playerIdentity>().CmdGainStatus(hosts, 4, 0);
                        CmdDestroyUnit(gameObject);
                    }
                }
                break;
            case "Dead_Necropia":
                if (isMyturn()) {
                    ArrayList table = new ArrayList();
                    foreach (string cardId in scr_playerInGame.tomb) {
                        string cardTag = (string)DB_card.getIdentity(cardId)[6];
                        if (cardTag.Contains("Dark")) table.Add(cardId);
                    }

                    if (table.Count >= 2) {
                        for (int i = 0; i < 2; i++) {
                            int removeIndex = Random.Range(0, table.Count);
                            scr_playerInGame.tomb.Remove(table[removeIndex]);
                        }
                    }
                    else {
                        player.GetComponent<scr_playerIdentity>().CmdGainStatus(hosts, 5, 0);
                        GainHand(id);
                        CmdDestroyUnit(gameObject);
                    }
                }
                break;
            case "Tenebera":
                if (isMyturn())
                {
                    ArrayList table = new ArrayList();
                    foreach (string cardId in scr_playerInGame.tomb)
                    {
                        string cardTag = (string)DB_card.getIdentity(cardId)[6];
                        if (cardTag.Contains("Dark")) table.Add(cardId);
                    }

                    if (table.Count >= 3)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            int removeIndex = Random.Range(0, table.Count);
                            scr_playerInGame.tomb.Remove(table[removeIndex]);
                        }
                    }
                    else
                    {
                        player.GetComponent<scr_playerIdentity>().CmdGainStatus(hosts, 5, 0);
                        GainHand(id);
                        CmdDestroyUnit(gameObject);
                    }
                }
                break;
            case "Pyrena":
                if (isMyturn())
                {
                    ArrayList table = new ArrayList();
                    foreach (string cardId in scr_playerInGame.tomb)
                    {
                        string cardTag = (string)DB_card.getIdentity(cardId)[6];
                        if (cardTag.Contains("Dark")) table.Add(cardId);
                    }

                    if (table.Count >= 3)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            int removeIndex = Random.Range(0, table.Count);
                            scr_playerInGame.tomb.Remove(table[removeIndex]);
                        }
                    }
                    else
                    {
                        player.GetComponent<scr_playerIdentity>().CmdGainStatus(hosts, 5, 0);
                        GainHand(id);
                        CmdDestroyUnit(gameObject);
                    }
                }
                break;
            case "burying_socerer":
                if (isMyturn()) {
                    bool spawnable = false;
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit")) {
                        scr_unit curData = cur.GetComponent<scr_unit>();
                        string curTag = (string)DB_card.getIdentity(curData.id)[6];
                        if (curData.hosts == hosts && curTag.Contains("Saint") && cur != gameObject) {
                            spawnable = true;
                            CmdrequestParticleByVector(7, cur.transform.localPosition, -1);
                            curData.CmdDestroyUnit(cur);
                        }
                    }
                    if (!spawnable) {
                        player.GetComponent<scr_playerIdentity>().CmdGainStatus(hosts, 6, 0);
                        GainHand(id);
                        CmdDestroyUnit(gameObject);
                    }
                }
                break;
        }
    }

    public void emerge()
    {
        if(getPlayer()) {
            scr_playerIdentity playerData = player.GetComponent<scr_playerIdentity>();
            if (playerData.environment == "Latien")
            {
                CmdgetSelfStatus(attack, health);
            }
            else if(playerData.environment.Contains("$"))
            {
                string tempTag = playerData.environment;
                tempTag = tempTag.Replace("$", "");
                CmdsetTag(tempTag);
            }
        }

        switch (id)
        {
            case "ren":
                if (isMyturn())
                {
                    if (player.GetComponent<scr_playerIdentity>().terial >= 6)
                    {
                        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                        {
                            if (cur != gameObject) CmdDamage(cur, 5);
                        }
                        CmdDamage(gameObject, 5);
                    }
                }
                break;
            case "volcanic_dragon":
                if (isMyturn())
                {
                    if (player.GetComponent<scr_playerIdentity>().environment != "volcano") CmdDamage(gameObject, 3);
                }
                break;
            case "raysen_fisherman":
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);
                break;
            case "purple_swarm":
                if (isMyturn())
                {
                    foreach (string cardId in scr_playerInGame.hand)
                    {
                        string temp_tag = (string)DB_card.getIdentity(cardId)[6];
                        if (temp_tag.Contains("Earth")) CmdgetSelfStatus(0, 1);
                    }
                }
                break;
            case "student_magician":
                if (isMyturn())
                {
                    player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);
                }
                break;
            case "kitten":
                if (isMyturn())
                {
                    if (scr_playerInGame.deck.Contains("kitten"))
                    {
                        scr_playerInGame.deck.Remove("kitten");
                        GainHand("kitten");
                    }
                }
                break;
            case "dollmaker":
                if (isMyturn()) GainHand("doll");
                break;
            case "pyren_zombie":
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);
                break;
            case "sacrificer":
                if (getPlayer())
                {
                    player.GetComponent<scr_playerIdentity>().CmdSetSacrificeCnt(hosts, 1);
                }
                break;
            case "archeologist":
                if (isMyturn()) scr_playerInGame.cardQueue.Enqueue("draw");
                break;
            case "dollhugging_girl":
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);
                break;
            case "triarodon_secretary":
                if (isMyturn())
                {
                    if (scr_playerInGame.deck.Contains("#tales_river"))
                    {
                        scr_playerInGame.deck.Remove("#tales_river");
                        GainHand("#tales_river");
                    }
                }
                break;
            case "waterdragon_son":
                if (isMyturn())
                {
                    if (scr_playerInGame.tomb.Contains("Triarodon"))
                    {
                        scr_playerInGame.tomb.Remove("Triarodon");
                        GainHand("Triarodon");
                        CmdDamage(gameObject, health);
                    }
                    else if (scr_playerInGame.deck.Contains("Triarodon")) {
                        scr_playerInGame.deck.Remove("Triarodon");
                        GainHand("Triarodon");
                    }
                }
                break;
            case "cave_guardian":
                if (isMyturn()) {
                    player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);
                }
                break;
            case "animal_lover":
                if (isMyturn())
                {
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        scr_unit curData = cur.GetComponent<scr_unit>();
                        if (curData.hosts == hosts && curData.card_tag.Contains("Beast")) CmdgetStatus(cur, 1, 0);
                    }
                }
                break;
            case "neprafti":
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);
                break;
            case "big_sworder":
                if (isMyturn()) GainHand("#gladius");
                break;
            case "student_knight":
                if (isMyturn()) GainHand("#slash");
                break;
            case "ancient_magician":
                if (isMyturn())
                {
                    ArrayList temp_table = new ArrayList();
                    foreach (string cardId in DB_card.collectible)
                    {
                        if (cardId.Contains("#")) temp_table.Add(cardId);
                    }
                    int drawIndex = Random.Range(0, temp_table.Count);
                    GainHand((string)temp_table[drawIndex]);
                }
                break;
            case "dimension_summoner":
                if (isMyturn())
                {
                    ArrayList temp_table = new ArrayList();
                    foreach (string cardId in DB_card.collectible)
                    {
                        if (!cardId.Contains("#")) temp_table.Add(cardId);
                    }
                    int drawIndex = Random.Range(0, temp_table.Count);
                    GainHand((string)temp_table[drawIndex]);
                }
                break;
            case "volcanic_monster":
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);
                break;
            case "lava_swarm":
                if (isMyturn())
                {
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        scr_unit data = cur.GetComponent<scr_unit>();
                        if (data.hosts == hosts && data.id == "volcanic_dragon")
                        {
                            CmdCamouflage("volcanic_dragon");
                            if (player.GetComponent<scr_playerIdentity>().environment != "volcano") CmdDamage(gameObject, 3);
                            break;
                        }
                    }
                }
                break;
            case "sacrificed_ghost":
                if (getPlayer())
                {
                    player.GetComponent<scr_playerIdentity>().CmdSetSacrificeCnt(hosts, 2);
                }
                break;
            case "Revilla":
                if (isMyturn()) CmdrequestParticle(100, gameObject, -1);
                break;
            case "Volcanic_twindragon":
                if (isMyturn()) CmdrequestParticle(102, gameObject, -1);
                break;
            case "tough_shieldman":
                if (isMyturn()) GainHand("#titanum_shield");
                break;
            case "alcadrobot_typeA":
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);
                break;
            case "lary":
                if (isMyturn())
                {
                    scr_playerInGame.deck.Add("doll");
                    scr_playerInGame.deck.Add("doll");
                    scr_playerInGame.deck.Add("doll");
                }
                break;
            case "fire_femalesworder":
                if (isMyturn())
                {
                    if (player.GetComponent<scr_playerIdentity>().environment == "volcano")
                    {
                        CmdgetSelfStatus(2, 1);
                    }
                }
                break;
            case "dragon_tamer":
                if (isMyturn())
                {
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        scr_unit curData = cur.GetComponent<scr_unit>();
                        if (curData.hosts == hosts && curData.card_tag.Contains("Dragon")) CmdgetStatus(cur, 1, 1);
                    }
                }
                break;
            case "ground_priest":
                if (isMyturn())
                {
                    ArrayList temp_table = new ArrayList();
                    foreach (string cardId in scr_playerInGame.deck)
                    {
                        string temp_tag = (string)DB_card.getIdentity(cardId)[6];
                        if (temp_tag.Contains("Earth")) temp_table.Add(cardId);
                    }
                    if (temp_table.Count == 0) break;
                    int drawIndex = Random.Range(0, temp_table.Count);
                    GainHand((string)temp_table[drawIndex]);
                    scr_playerInGame.deck.Remove(temp_table[drawIndex]);
                }
                break;
            case "mild_bear":
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);
                break;
            case "Nyan":
                if (isMyturn())
                {
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        scr_unit curData = cur.GetComponent<scr_unit>();
                        if (curData.hosts == hosts && curData.card_tag.Contains("Beast"))
                        {
                            curData.Cmdcharge();
                        }
                    }
                }
                break;
            case "oldbook_analyzer":
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdGainCard(hosts, "draw");
                break;
            case "battlefield_commander":
                if (isMyturn())
                {
                    GainHand("student_knight");
                    GainHand("student_swordman");
                }
                break;
            case "sirai":
                if (isMyturn())
                {
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        if (cur.GetComponent<scr_unit>().hosts != hosts) CmdDamage(cur, 1);
                    }
                }
                break;
            case "violet":
                if (isMyturn())
                {
                    scr_playerInGame.deck.Add("Archmage_violet");
                }
                break;
            case "Archmage_violet":
                if (isMyturn())
                {
                    ArrayList targets = new ArrayList();
                    foreach (string cardId in DB_card.collectible)
                    {
                        if ((int)DB_card.getIdentity(cardId)[0] >= 3) targets.Add(cardId);
                    }
                    int handCnt = scr_playerInGame.hand.Count;
                    int deckCnt = scr_playerInGame.deck.Count;
                    scr_playerInGame.hand.Clear();
                    scr_playerInGame.deck.Clear();
                    for (int i = 0; i < handCnt; i++)
                    {
                        GainHand((string)targets[Random.Range(0, targets.Count)]);
                    }
                    for (int i = 0; i < deckCnt; i++)
                    {
                        scr_playerInGame.deck.Add(targets[Random.Range(0, targets.Count)]);
                    }
                    GainHand("#Archmage_rune");
                    player.GetComponent<scr_playerInGame>().displayHand();
                }
                break;
            case "tetra":
                if (isMyturn()) {
                    int temp_cnt = 0;
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit")) {
                        if (cur.GetComponent<scr_unit>().id == "doll") temp_cnt++;
                    }
                    CmdgetSelfStatus(temp_cnt, 0);
                }
                break;
            case "little_firedragon":
                if (isMyturn())
                {
                    if (player.GetComponent<scr_playerIdentity>().environment == "volcano")
                    {
                        CmdgetSelfStatus(4, 0);
                    }
                }
                break;
            case "waterpalace_guardian":
                if (isMyturn())
                {
                    switch (player.GetComponent<scr_playerIdentity>().environment)
                    {
                        case "tales_1":
                            CmdgetSelfStatus(1, 1);
                            break;
                        case "tales_2":
                            CmdgetSelfStatus(2, 2);
                            break;
                        case "tales_3":
                            CmdgetSelfStatus(3, 3);
                            break;
                        default:
                            break;
                    }
                }
                break;
            case "little_mermaid":
                if (isMyturn())
                {
                    switch (player.GetComponent<scr_playerIdentity>().environment)
                    {
                        case "tales_1":
                            CmdgetSelfStatus(1, 0);
                            break;
                        case "tales_2":
                            CmdgetSelfStatus(1, 0);
                            break;
                        case "tales_3":
                            CmdgetSelfStatus(1, 0);
                            break;
                        default:
                            break;
                    }
                }
                break;
            case "tornado_hero":
                if (isMyturn())
                {
                    bool temp_flag = false;
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        scr_unit curData = cur.GetComponent<scr_unit>();
                        if (curData.hosts == hosts && curData.card_tag.Contains("Wind") && cur != gameObject)
                        {
                            temp_flag = true;
                            break;
                        }
                    }
                    if (temp_flag)
                    {
                        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                        {
                            scr_unit curData = cur.GetComponent<scr_unit>();
                            if (curData.attack <= 2)
                            {
                                CmdDamage(cur, curData.health);
                            }
                        }
                    }
                }
                break;
            case "mern_chief":
                if (isMyturn())
                {
                    GainHand("#earthquake");
                }
                break;
            case "sacrifice_socerer":
                if (getPlayer())
                {
                    int temp_cnt = 0;
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        scr_unit curData = cur.GetComponent<scr_unit>();
                        if (curData.hosts == hosts && curData.card_tag.Contains("Saint")) temp_cnt++;
                    }
                    player.GetComponent<scr_playerIdentity>().CmdSetSacrificeCnt(hosts, temp_cnt);
                }
                break;
            case "grail_maker":
                if (isMyturn())
                {
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        scr_unit curData = cur.GetComponent<scr_unit>();
                        if (curData.hosts == hosts && curData.card_tag.Contains("Saint"))
                        {
                            GainHand("#luminous_grail");
                        }
                    }
                }
                break;
            case "jack-o-lantern":
                if (getPlayer())
                {
                    player.GetComponent<scr_playerIdentity>().CmdGainStatus(hosts, 0, -4);
                    player.GetComponent<scr_playerIdentity>().CmdSetSacrificeCnt(hosts, 4);
                }
                break;
            case "Cardinel":
                if (isMyturn())
                {
                    CmdrequestParticle(101, gameObject, -1);
                    GainHand("Vera");
                    GainHand("Vera");
                }
                break;
            case "Sytron":
                if (getPlayer()) {
                    player.GetComponent<scr_playerIdentity>().CmdGainStatus(hosts, 0, 99);
                    if(isMyturn()) CmdrequestParticle(105, gameObject, -1);
                }
                break;
            case "Sardis":
                if (getPlayer())
                {
                    player.GetComponent<scr_playerIdentity>().CmdGainStatusExtended(hosts, 0, 0, 15);
                    if (isMyturn()) CmdrequestParticle(105, gameObject, -1);
                }
                break;
            case "knowledge_omniscence":
                if (getPlayer())
                {
                    scr_playerIdentity temp_playerData = player.GetComponent<scr_playerIdentity>();
                    temp_playerData.CmdGainCard(hosts, "draw");
                    temp_playerData.CmdGainCard(hosts, "draw");
                }
                break;
            case "fire_mage":
                if (isMyturn())
                {
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        if (cur != gameObject) CmdDamage(cur, 1);
                    }
                }
                break;
            case "nature_mage":
                if (getPlayer())
                {
                    player.GetComponent<scr_playerIdentity>().CmdGainStatus(hosts, 0, 3);
                }
                break;
            case "wave_fighter":
                if (getPlayer())
                {
                    if (player.GetComponent<scr_playerController>().myturn)
                    {
                        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
                        {
                            if (!cur.GetComponent<scr_playerController>().isPlayer)
                            {
                                CmdDamage(cur, 1);
                            }
                        }
                    }
                }
                break;
            case "ken":
                if (isMyturn())
                {
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        if (!cur.GetComponent<scr_playerController>().isPlayer)
                        {
                            CmdDamage(cur, 2);
                        }
                    }
                }
                break;
            case "alcadrobot_typeB":
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);
                break;
            case "penitent_devil":
                if (getPlayer())
                {
                    if (player.GetComponent<scr_playerController>().myturn)
                    {
                        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Unit"))
                        {
                            if (hosts == obj.GetComponent<scr_unit>().hosts && obj != gameObject)
                            {
                                CmdgetStatus(obj, 1, 1);
                                CmdDamage(gameObject, 1);
                            }

                        }
                    }
                }
                break;
            case "wishful_bishop":
                if (getPlayer())
                {
                    player.GetComponent<scr_playerIdentity>().CmdSetSacrificeCnt(hosts, 2);
                }
                break;
            case "lion_tamer":
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);
                break;
            case "expert_healer":
                if (getPlayer())
                {
                    player.GetComponent<scr_playerIdentity>().CmdGainStatus(hosts, 0, 5);
                }
                break;
            case "Falcifer":
                if (isMyturn())
                {
                    int gain_health = 0;
                    foreach (string cardId in scr_playerInGame.hand) {
                        if (!cardId.Contains("#")) gain_health++;
                        scr_playerInGame.tomb.Add(cardId);
                    }
                    scr_playerInGame.hand.Clear();
                    CmdgetSelfStatus(0, gain_health);
                }
                break;
            case "Sein":
                if (isMyturn()) GainHand("#doll_gathering");
                break;
            case "Dead_Necropia":
                if (isMyturn())
                {
                    CmdCamouflage("Necropia");
                    CmdrequestParticle(106, gameObject, -1);
                }
                break;
            case "Pyrena":
                if (isMyturn()) {
                    GainHand("pyren_zombie");
                    GainHand("pyren_zombie");
                    CmdrequestParticle(106, gameObject, -1);
                }
                break;
            case "Tenebera":
                if(isMyturn()) CmdrequestParticle(106, gameObject, -1);
                break;
            case "mirat":
                if (getPlayer())
                {
                    scr_playerIdentity temp_playerData = player.GetComponent<scr_playerIdentity>();
                    temp_playerData.CmdGainCard(hosts, "doll");
                    temp_playerData.CmdGainCard(hosts, "doll");
                }
                break;
            case "Hexator":
                if (getPlayer())
                {
                    if (player.GetComponent<scr_playerController>().myturn)
                    {
                        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
                        {
                            if (cur.GetComponent<scr_playerController>().isPlayer)
                            {
                                if (cur.GetComponent<scr_playerIdentity>().health <= 6) Cmdcharge();
                            }
                            else
                            {
                                if (cur.GetComponent<scr_playerIdentity>().health <= 6) CmdgetSelfStatus(3, 3);
                            }
                        }
                    }
                }
                break;
            case "Triarodon":
                if (isMyturn())
                {
                    int dam = 0;
                    switch (player.GetComponent<scr_playerIdentity>().environment)
                    {
                        case "tales_1":
                            dam = 2;
                            break;
                        case "tales_2":
                            dam = 4;
                            break;
                        case "tales_3":
                            dam = 6;
                            break;
                        default:
                            break;
                    }
                    if (dam != 0)
                    {
                        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                        {
                            scr_unit curData = cur.GetComponent<scr_unit>();
                            if (!curData.card_tag.Contains("Water")) CmdDamage(cur, dam);
                        }
                        CmdrequestParticle(103, gameObject, -1);
                    }
                }
                break;
            case "Keiro":
                if (isMyturn()) CmdrequestParticle(107, gameObject, -1);
                break;
            case "Teradon":
                if (isMyturn()) CmdrequestParticle(104, gameObject, -1);
                break;
            case "PEIN":
                if (isMyturn()) CmdrequestParticle(108, gameObject, -1);
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit temp_data = cur.GetComponent<scr_unit>();
                    if (temp_data.hosts == hosts && (temp_data.id == "investigator_pein" || temp_data.id == "synthesizer"))
                    {
                        CmdrequestParticleByVector(7, cur.transform.localPosition, -1);
                        temp_data.CmdDestroyUnit(cur);
                    }
                    else if (cur != gameObject) CmdDamage(cur, temp_data.health);
                }
                break;
            case "BAN":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit temp_data = cur.GetComponent<scr_unit>();
                    if (temp_data.hosts == hosts && (temp_data.id == "moon_slayer" || temp_data.id == "mancy" || temp_data.id == "synthesizer"))
                    {
                        CmdrequestParticleByVector(7, cur.transform.localPosition, -1);
                        temp_data.CmdDestroyUnit(cur);
                    }
                }
                if (isMyturn()) GainHand("#halfmoon_strike");
                break;
            case "PAN":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit temp_data = cur.GetComponent<scr_unit>();
                    if (temp_data.hosts == hosts && (temp_data.id == "slum_dancer" || temp_data.id == "rein" || temp_data.id == "synthesizer"))
                    {
                        CmdrequestParticleByVector(7, cur.transform.localPosition, -1);
                        temp_data.CmdDestroyUnit(cur);
                    }
                }
                break;
            case "NATIS":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit temp_data = cur.GetComponent<scr_unit>();
                    if (temp_data.hosts == hosts && (temp_data.id == "wind_walker" || temp_data.id == "necky" || temp_data.id == "synthesizer"))
                    {
                        CmdrequestParticleByVector(7, cur.transform.localPosition, -1);
                        temp_data.CmdDestroyUnit(cur);
                    }
                }
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdGainStatus(hosts, 7, 0);
                break;
            case "ROGER":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit temp_data = cur.GetComponent<scr_unit>();
                    if (temp_data.hosts == hosts && (temp_data.id == "wave_fighter" || temp_data.id == "ken" || temp_data.id == "synthesizer")) {
                        CmdrequestParticleByVector(7, cur.transform.localPosition, -1);
                        temp_data.CmdDestroyUnit(cur);
                    }
                }
                if (isMyturn())
                {
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        if (!cur.GetComponent<scr_playerController>().isPlayer)
                        {
                            CmdDamage(cur, 5);
                        }
                    }
                }
                break;
            case "LATIEN":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit temp_data = cur.GetComponent<scr_unit>();
                    if (temp_data.hosts == hosts && temp_data.terial == 6)
                    {
                        CmdrequestParticleByVector(7, cur.transform.localPosition, -1);
                        temp_data.CmdDestroyUnit(cur);
                    }

                    if(getPlayer())
                    {
                        player.GetComponent<scr_playerIdentity>().CmdSetEnvironment("Latien");
                    }
                }
                break;
            case "SENA":
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    scr_unit temp_data = cur.GetComponent<scr_unit>();
                    if (temp_data.hosts == hosts && temp_data.terial == 4)
                    {
                        CmdrequestParticleByVector(7, cur.transform.localPosition, -1);
                        temp_data.CmdDestroyUnit(cur);
                    }
                }
                break;
            case "Seria":
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdGainCard(hosts, "draw");
                break;
            case "BT":
                if (isMyturn()) scr_playerInGame.tomb.Clear();
                break;
            case "TONUS":
                if (isMyturn()) {
                    foreach (string cardId in scr_playerInGame.deck) {
                        if ((int)DB_card.getIdentity(cardId)[0] == 7)
                        {
                            scr_playerInGame.deck.Remove(cardId);
                            break;
                        }
                    }
                }
                if (getPlayer())
                {
                    int currentTerial = player.GetComponent<scr_playerIdentity>().terial;
                    player.GetComponent<scr_playerIdentity>().CmdGainStatus(hosts, -currentTerial, 0);

                    player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);                    
                }
                    break;
            case "LICA":
                if (isMyturn())
                {
                    scr_playerInGame.cardQueue.Enqueue("draw");
                    scr_playerInGame.cardQueue.Enqueue("draw");
                }
                break;
            case "MACHINA":
                if (isMyturn()) CmdrequestParticle(109, gameObject, -1);
                if (getPlayer()) {
                    scr_playerIdentity playerData = player.GetComponent<scr_playerIdentity>();
                    playerData.CmdSetSacrificeCnt(hosts, -playerData.sacrificeCnt);
                }
                break;
            case "EURIEL":
                if (isMyturn())
                {
                    CmdrequestParticle(110, gameObject, -1);
                    if (scr_playerInGame.hand.Contains("#elemental_book")) scr_playerInGame.hand.Remove("#elemental_book");
                    if (scr_playerInGame.deck.Contains("#elemental_book")) scr_playerInGame.deck.Remove("#elemental_book");

                    player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);
                }
                break;
            case "INFINITAS":
                if (isMyturn()) {
                    CmdrequestParticle(111, gameObject, -1);
                    foreach (string cardId in scr_playerInGame.tomb)
                    {
                        scr_playerInGame.deck.Add(cardId);
                    }
                    scr_playerInGame.tomb.Clear();
                    scr_playerIdentity playerData = player.GetComponent<scr_playerIdentity>();
                    playerData.CmdSetSacrificeCnt(hosts, -playerData.sacrificeCnt);                                  
                }
                break;
            case "SAYRUN":
                if (getPlayer()) {
                    scr_playerIdentity playerData = player.GetComponent<scr_playerIdentity>();
                    playerData.CmdSetSacrificeCnt(hosts, -playerData.sacrificeCnt);
                }
                break;
            case "HETERO":
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdSelectCard(hosts, id);
                break;
        }
    }

    public bool death() {
        switch (id)
        {
            case "cursed_mage":
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Unit")) {
                    if (hosts == obj.GetComponent<scr_unit>().hosts) CmdDamage(obj, 2);
                }
                return true;
            case "afterlife_guide":
                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdGainCardExtended(hosts, id, "GAINTOMB");
                return true;
            case "carsus_priest":
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Unit")) {
                    if (hosts == obj.GetComponent<scr_unit>().hosts) CmdgetStatus(obj, 1, 0);
                }
                return true;
            case "PAN":
                DB_card.setIdentity(gameObject, "PAN", transform.localPosition, transform.localScale);
                return false;
            case "MACHINA":
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player")) {
                    if (obj.GetComponent<scr_playerController>().isHost != hosts) {
                        CmdDamage(obj, obj.GetComponent<scr_playerIdentity>().health);
                        break;
                    }                    
                }
                return true;
            default:
                return true;
        }        
    }

    public void grow()
    {
        if (getPlayer()) {
            int remain_terial = player.GetComponent<scr_playerIdentity>().terial;
            GameObject target_player = null;
            switch (id) {
                case "student_fighter":
                    if (remain_terial >= 1) {
                        CmdgetSelfStatus(1, 0);
                    }
                    break;
                case "volcanic_dragonling":
                    if (player.GetComponent<scr_playerController>().isHost == hosts)
                    {
                        if (player.GetComponent<scr_playerIdentity>().environment == "volcano")
                        {
                            if (scr_playerInGame.deck.Contains("volcanic_dragon"))
                            {
                                scr_playerInGame.deck.Remove("volcanic_dragon");
                                GainHand("volcanic_dragon");
                            }
                        }
                        else {
                            if (scr_playerInGame.deck.Contains("#volcano"))
                            {
                                scr_playerInGame.deck.Remove("#volcano");
                                GainHand("#volcano");
                            }
                        }
                    }
                    break;
                case "duret":
                    if (remain_terial >= 5) {
                        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                        {
                            if (cur != gameObject) CmdDamage(cur, cur.GetComponent<scr_unit>().health);
                        }
                        CmdDamage(gameObject, health);
                    }
                    break;
                case "Devil-T_host":
                    foreach (GameObject ins in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        if (ins.GetComponent<scr_playerController>().isHost == hosts)
                        {
                            target_player = ins;
                            break;
                        }
                    }
                    CmdDamage(target_player, 4);
                    break;
                case "crystal_fighter":
                    if (remain_terial >= 1) {
                        CmdgetSelfStatus(1, 1);
                    }
                    break;
                case "longlife_turtle":
                    if (remain_terial >= 1)
                    {
                        CmdDamage(gameObject, -50);
                    }
                    break;
                case "Necropia":
                    foreach (GameObject ins in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        if (ins.GetComponent<scr_playerController>().isHost == hosts)
                        {
                            target_player = ins;
                            break;
                        }
                    }
                    if (player == target_player)
                    {
                        if (scr_playerInGame.deck.Count != 0)
                        {
                            int index = Random.Range(0, scr_playerInGame.deck.Count);
                            string cardId = (string)scr_playerInGame.deck[index];
                            scr_playerInGame.deck.Remove(cardId);
                            scr_playerInGame.tomb.Add(cardId);

                            foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                            {
                                scr_unit curData = cur.GetComponent<scr_unit>();
                                if (curData.hosts != hosts) CmdDamage(cur, 2);
                            }
                        }
                    }
                    break;
                case "nature_musician":
                    target_player = null;
                    if (getPlayer())
                    {
                        foreach (GameObject ins in GameObject.FindGameObjectsWithTag("Player"))
                        {
                            if (ins.GetComponent<scr_playerController>().isHost == hosts)
                            {
                                target_player = ins;
                                break;
                            }
                        }
                        if (player == target_player)
                        {
                            if (remain_terial >= 1) {
                                GainHand("ancient_hero");
                            }
                        }
                    }
                    break;
                case "tigor":
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit")) {
                        scr_unit curData = cur.GetComponent<scr_unit>();
                        if (curData.hosts == hosts && curData.card_tag.Contains("Beast")) {
                            CmdgetStatus(cur, 1, 0);
                        }
                    }                    
                    break;
                case "Teradon":
                    CmdgetSelfStatus(2, 2);
                    break;
                case "PAN":
                    foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit"))
                    {
                        scr_unit curData = cur.GetComponent<scr_unit>();
                        if (curData.hosts != hosts)
                        {
                            CmdgetStatus(cur, -2, 0);
                        }
                    }
                    break;
                case "SAYRUN":
                    if (player.GetComponent<scr_playerController>().isHost == hosts)
                    {
                        int rand = Random.Range(0, 5);
                        switch (rand) {
                            case 0:
                                GainHand("#latien_prelude");
                                break;
                            case 1:
                                GainHand("#latien_choir");
                                break;
                            case 2:
                                GainHand("#latien_fantasia");
                                break;
                            case 3:
                                GainHand("#latien_symphony");
                                break;
                            case 4:
                                GainHand("#latien_lyrics");
                                break;
                            default:
                                GainHand("#latien_prelude");
                                break;
                        }                        
                    }
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator DelayAttack(float time, GameObject target, int my_attack, int target_attack)
    {
        GameObject med = GameObject.FindGameObjectWithTag("Meditator");

        yield return new WaitForSeconds(time);
        med.GetComponent<scr_meditator>().createParticle(DB_particles.par_spark, transform.localPosition, -1);
        med.GetComponent<scr_meditator>().createParticle(DB_particles.par_spark, target.transform.localPosition, -1);
        med.GetComponent<scr_meditator>().createParticle(DB_particles.ef_damage, transform.localPosition, target_attack);
        med.GetComponent<scr_meditator>().createParticle(DB_particles.ef_damage, target.transform.localPosition, my_attack);
        //if (getPlayer()) {
        //    if(player.GetComponent<scr_playerController>().isHost) 
        //}
        if(isServer) CmdapplyDamage(target, my_attack, target_attack);
    }

    IEnumerator DelayDirectAttack(float time, GameObject target, int my_attack)
    {
        GameObject med = GameObject.FindGameObjectWithTag("Meditator");
        yield return new WaitForSeconds(time);
        med.GetComponent<scr_meditator>().createParticle(DB_particles.par_spark, target.transform.localPosition, -1);
        med.GetComponent<scr_meditator>().createParticle(DB_particles.ef_damage, target.transform.localPosition, my_attack);
        //if (getPlayer()) {
        //    if(player.GetComponent<scr_playerController>().isHost) 
        //}
        if (isServer) CmdapplyDirectDamage(target, my_attack);
    }

    public void GainHand(string cardId) {
        CmdrequestParticle(5, gameObject, -1);
        scr_playerInGame.hand.Add(cardId);
    }

    [Command]
    public void CmdAddTomb()
    {
        RpcAddTomb();
    }

    [ClientRpc]
    public void RpcAddTomb()
    {
        getPlayer();
        GameObject ins = Instantiate(dead, GameObject.Find("Canvas").transform);
        ins.transform.localPosition = transform.localPosition;
        ins.transform.localScale = new Vector3(0.435f, 0.435f, 1f);
        scr_playerController temp_data = player.GetComponent<scr_playerController>();
        if (hosts == temp_data.isHost)
        {
            if (!tombFlag)
            {
                scr_playerInGame.tomb.Add(id);
                tombFlag = true;
            }
            GameObject picChild = transform.GetChild(0).gameObject;
            picChild.GetComponent<scr_picGUI>().eraseCard();
            CmdLapse();
        }
    }

    [Command]
    public void CmdLapse() {
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    public void CmdsetTerial(int amount) {
        terial = amount;
    }

    [Command]
    public void CmdsetTag(string value)
    {
        card_tag = value;
    }

    [Command]
    public void Cmdcharge() {
        CmdrequestParticle(4, gameObject, -1);
        spawn_turn -= 1;
    }

    [Command]
    public void CmdgetSelfStatus(int gain_attack, int gain_health)
    {
        if (gain_attack >= 0 && gain_health >= 0) CmdrequestParticle(1, gameObject, -1);
        else if (gain_attack < 0) CmdrequestParticle(2, gameObject, -1); 
        attack += gain_attack;
        health += gain_health;
        if (health > max_health) max_health = health;
        if (attack < 0) attack = 0;
    }

    [Command]
    public void CmdgetStatus(GameObject cur, int gain_attack, int gain_health) {
        if (cur.transform.tag == "Unit")
        {
            if (gain_attack >= 0 && gain_health >= 0) CmdrequestParticle(1, cur, -1);
            else if (gain_attack < 0) CmdrequestParticle(2, cur, -1);
            scr_unit data = cur.GetComponent<scr_unit>();

            data.attack += gain_attack;
            data.health += gain_health;
            if (data.health > data.max_health) data.max_health = data.health;
            if (data.attack < 0) data.attack = 0;
        }
    }

    [Command]
    public void Cmdattack_health_calculation(GameObject cur)
    {

        scr_unit curData = cur.GetComponent<scr_unit>();

        Vector3 pos = transform.localPosition;
        Vector3 curPos = cur.transform.localPosition;

        int total_attack = attack;
        if (curData.id == "wind_walker" && attack >= 7) total_attack = 0;
        else if (curData.id == "necky" && attack >= 4) total_attack = 3;
        if (curData.id == "investigator_pein" || curData.id == "PEIN") total_attack = 0;

        int total_damage = curData.attack;
        if ((id == "wind_walker" && curData.attack >= 7) ||
            (id == "tina" && curData.attack >= 5)) total_damage = 0;
        else if (id == "necky" && curData.attack >= 4) total_damage = 3;
        if (id == "siera" || id == "SENA" || id == "investigator_pein" || id == "PEIN") total_damage = 0;

        RpcAttackParticle(cur, total_attack, total_damage);       
    }

    [Command]
    public void CmdapplyDamage(GameObject target, int my_attack, int target_attack) {
        scr_unit curData = target.GetComponent<scr_unit>();
        curData.health -= my_attack;
        health -= target_attack;
    }

    [Command]
    public void CmdapplyDirectDamage(GameObject target, int my_attack)
    {
        target.GetComponent<scr_playerIdentity>().health -= my_attack;
    }

    [ClientRpc]
    public void RpcAttackParticle(GameObject target, int my_attack, int target_attack)
    {
        GameObject med = GameObject.FindGameObjectWithTag("Meditator");
        med.GetComponent<scr_meditator>().createParticle(DB_particles.par_attack, transform.localPosition, -1);
        StartCoroutine(DelayAttack(1, target, my_attack, target_attack));
    }

    [ClientRpc]
    public void RpcDirectAttackParticle(GameObject target, int my_attack)
    {
        GameObject med = GameObject.FindGameObjectWithTag("Meditator");
        med.GetComponent<scr_meditator>().createParticle(DB_particles.par_attack, transform.localPosition, -1);

        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit")) {
            string curId = cur.GetComponent<scr_unit>().id;
            if (cur.GetComponent<scr_unit>().hosts == target.GetComponent<scr_playerController>().isHost &&
                (curId == "ground_sentinel" || curId == "Teradon" || curId == "ancient_hero")) {
                my_attack = 0;
                break;
            }
        }

        StartCoroutine(DelayDirectAttack(1, target, my_attack));
    }

    [Command]
    public void CmdrequestParticle(int particle, GameObject target, int value) {
        RpcrequestParticle(particle, target, value);
    }

    [Command]
    public void CmdrequestParticleByVector(int particle, Vector3 pos, int value)
    {
        RpcrequestParticleByVector(particle, pos, value);
    }
    
    [ClientRpc]
    public void RpcrequestParticle(int particle, GameObject target, int value)
    {
        GameObject med = GameObject.FindGameObjectWithTag("Meditator");
        Vector3 pos = target.transform.localPosition;
        if (particle == 102) pos -= new Vector3(0f, 140f, 0f);
        else if (particle == 103) pos = new Vector3(-1200f, 300f, 0f);
        else if (particle == 105) pos = new Vector3(0f, 70f, -700f);
        else if (particle == 107) pos = new Vector3(0f, -50f, -700f);
        else if (particle == 109 || particle == 110 || particle == 111) pos = new Vector3(0f, 55f, -700f);
        med.GetComponent<scr_meditator>().createParticle(particle, pos, value);
    }

    [ClientRpc]
    public void RpcrequestParticleByVector(int particle, Vector3 pos, int value)
    {
        GameObject med = GameObject.FindGameObjectWithTag("Meditator");
        med.GetComponent<scr_meditator>().createParticle(particle, pos, value);
    }

    [Command]
    public void CmdDirectattack_health_calculation(GameObject cur)
    {
        Cmdchange_spawn_turn();
        RpcDirectAttackParticle(cur, attack);
    }

    // if args[1] - amount is negative, it is applied as healing.
    [Command]
    public void CmdDamage(GameObject cur, int amount) {
        RpcDamage(cur, amount);
        if (cur.transform.tag == "Unit")
        {
            scr_unit scr = cur.GetComponent<scr_unit>();

            scr.health -= amount;
            if (scr.health > scr.max_health)
            {
                scr.health = scr.max_health;
            }
        }
        else if (cur.transform.tag == "Player") {
            scr_playerIdentity scr = cur.GetComponent<scr_playerIdentity>();

            scr.health -= amount;
            if (scr.health > scr.max_health)
            {
                scr.health = scr.max_health;
            }
        }
    }

    [ClientRpc]
    public void RpcDamage(GameObject cur, int amount) {
        GameObject med = GameObject.FindGameObjectWithTag("Meditator");
        med.GetComponent<scr_meditator>().createParticle(DB_particles.par_spark, cur.transform.localPosition, -1);
        med.GetComponent<scr_meditator>().createParticle(DB_particles.ef_damage, cur.transform.localPosition, amount);
    }

    [Command]
    public void CmdDestroyUnit(GameObject cur) {
        if(cur.GetComponent<scr_unit>().id != "PAN") NetworkServer.Destroy(cur);
    }

    [Command]
    public void Cmdchange_spawn_turn() {
        spawn_turn = GameObject.FindGameObjectWithTag("Meditator").GetComponent<scr_meditator>().turnCnt;
    }

    [Command]
    public void CmdsetMaxhealth() {
        max_health = health;
    }

    [Command]
    public void CmdaddAbils(GameObject target, string code) {
        scr_unit scr = target.GetComponent<scr_unit>();

        scr.abils.Add(code);
    }

    [Command]
    public void CmdgainCard(string id) {
        if (getPlayer()) {
            player.GetComponent<scr_playerIdentity>().RpcGaincard(hosts, id);
        }
    }

    [Command]
    public void CmdCamouflage(string id) {
        DB_card.setIdentity(gameObject, id, transform.localPosition, transform.localScale);
    }
}
