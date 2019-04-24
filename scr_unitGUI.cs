using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_unitGUI : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IPointerExitHandler, 
    IDragHandler, IEndDragHandler {

    public Sprite attack;
    public Sprite magic;
    public bool emerge_flag = true;

    private Vector3 savePos;
    private bool drag = false;
    private bool onMouse = false;

    Vector3 screenPoint;
    Vector3 offset;
    GameObject sav_parent;
    

    GameObject player;

    public void Start()
    {
        sav_parent = transform.parent.gameObject;
    }

    public void Update()
    {
        if (!onMouse && !drag)
        {
            SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();

            rend.sprite = null;
            rend.color = new Color(1f, 1f, 1f, 0f);
        }
        if (sav_parent == null) Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
        scr_unit data = sav_parent.GetComponent<scr_unit>();

        if (!data.isEnemy) {
            string checkEmerge = (string)DB_card.getIdentity(data.id)[5];
            if (data.canAttack())
            {
                rend.sprite = attack;
                rend.color = new Color(1f, 1f, 1f, 1f);
            }
            else if (checkEmerge.Contains("[능력") && emerge_flag) {
                if (data.canSpell()) {
                    rend.sprite = magic;
                    rend.color = new Color(1f, 1f, 1f, 1f);
                }              
            }
        }
        
        onMouse = true;
    }   

    public void OnPointerExit(PointerEventData eventData) {
        onMouse = false;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        savePos = gameObject.transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3
        (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        drag = true;
    }

    public void OnDrag(PointerEventData eventData) {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
        {
            gameObject.transform.SetParent(GameObject.Find("Canvas").transform);

            if (gameObject.GetComponent<SpriteRenderer>().sprite == attack)
            {
                if (!cur.GetComponent<scr_playerController>().isPlayer && canDirectAttack() &&
                transform.localPosition.y >= 360)
                {
                    scr_unit data = sav_parent.GetComponent<scr_unit>();
                    data.CmdDirectattack_health_calculation(cur);

                    if (data.id == "Falcifer")
                    {
                        data.CmdDamage(sav_parent, 2);
                        data.Cmdcharge();
                        break;
                    }
                    break;
                }
            }
        }

        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Unit")) {
            gameObject.transform.SetParent(GameObject.Find("Canvas").transform);

            if (gameObject.GetComponent<SpriteRenderer>().sprite == attack)
            {
                string checkRange = (string)DB_card.getIdentity(sav_parent.GetComponent<scr_unit>().id)[5];
                if (cur.GetComponent<scr_unit>().isEnemy && (checkRange.Contains("[원격") || canAttack(cur)) &&
                Vector3.Distance(gameObject.transform.localPosition, cur.transform.localPosition) <= 60f)
                {
                    scr_unit data = sav_parent.GetComponent<scr_unit>();
                    data.Cmdchange_spawn_turn();

                    string checkAttack = (string)DB_card.getIdentity(data.id)[5];
                    if (checkAttack.Contains("[공격")) {
                        switch (data.id)
                        {
                            case "moon_slayer":
                                data.CmdgetSelfStatus(1, 0);
                                break;
                            case "Revilla":
                                data.CmdgetSelfStatus(cur.GetComponent<scr_unit>().health, 0);
                                break;
                            case "dollmaker":
                                if (data.isMyturn()) scr_playerInGame.hand.Add("doll");                                                 
                                break;
                            case "sacrifice_summoner":
                                if (getPlayer()) player.GetComponent<scr_playerIdentity>().CmdSetSacrificeCnt(data.hosts, 2);
                                break;
                            case "wind_rider":
                                if (cur.GetComponent<scr_unit>().attack >= 7) data.CmdDamage(cur, cur.GetComponent<scr_unit>().health);
                                break;
                            case "Volcanic_twindragon":
                                if (cur.GetComponent<scr_unit>().health <= data.attack)
                                {
                                    data.CmdgetSelfStatus(1, 1);
                                    data.Cmdcharge();
                                }
                                break;
                            case "mancy":
                                data.CmdgetSelfStatus(2, 0);
                                break;
                            case "axe_zealot":
                                if (getPlayer())
                                {
                                    if (player.GetComponent<scr_playerController>().myturn)
                                    {
                                        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Unit"))
                                        {
                                            if(obj != gameObject) data.CmdDamage(obj, 1);
                                        }
                                    }
                                }
                                break;
                            case "Falcifer":
                                data.CmdDamage(sav_parent, 2);
                                data.Cmdcharge();
                                break;
                            case "mirat":
                                if (getPlayer())
                                {
                                    if (player.GetComponent<scr_playerController>().myturn)
                                    {
                                        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Unit"))
                                        {
                                            if (obj.GetComponent<scr_unit>().hosts == data.hosts &&
                                                obj.GetComponent<scr_unit>().id == "doll") data.CmdgetStatus(obj, 1, 1);
                                        }
                                    }
                                }
                                break;
                            case "Keiro":
                                if (getPlayer())
                                {
                                    if (data.isMyturn()) {
                                        if (cur.GetComponent<scr_unit>().health <= data.attack) {
                                            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
                                            {
                                                if (!obj.GetComponent<scr_playerController>().isPlayer)
                                                {
                                                    data.CmdDamage(obj, cur.GetComponent<scr_unit>().attack);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    data.Cmdattack_health_calculation(cur);
                    break;
                }
            }

            else if (gameObject.GetComponent<SpriteRenderer>().sprite == magic) {
                if ((Vector3.Distance(gameObject.transform.localPosition, cur.transform.localPosition) <= 60f) && cur != gameObject)
                {
                    scr_unit data = sav_parent.GetComponent<scr_unit>();
                    requestParticle(3, sav_parent, -1);

                    switch (data.id) {
                        case "little_healer":
                            data.CmdDamage(cur, -2);
                            emerge_flag = false;
                            break;
                        case "student_swordman":
                            data.CmdDamage(cur, 1);
                            emerge_flag = false;
                            break;
                        case "shasha":
                            data.CmdsetTerial(cur.GetComponent<scr_unit>().terial);
                            emerge_flag = false;
                            break;
                        case "hero_helper":
                            data.CmdgetStatus(cur, 1, 1);
                            emerge_flag = false;
                            break;
                        case "corpse_raven":
                            scr_unit curData = cur.GetComponent<scr_unit>();
                            if (data.hosts == curData.hosts &&
                                curData.card_tag.Contains("Beast")) {
                                data.CmdDamage(cur, curData.health);
                                if(getPlayer()) player.GetComponent<scr_playerIdentity>().CmdGainCard(data.hosts, "draw");
                                emerge_flag = false;
                            }
                            break;
                        case "admired_bishop":
                            curData = cur.GetComponent<scr_unit>();
                            if (data.hosts != curData.hosts) {
                                if (data.isMyturn()) scr_playerInGame.hand.Add(curData.id);
                                emerge_flag = false;
                            }
                            break;
                        case "waterdragon_priest":
                            curData = cur.GetComponent<scr_unit>();
                            if (data.hosts == curData.hosts &&
                                curData.card_tag.Contains("Water"))
                            {
                                data.CmdDamage(cur, curData.health);
                                if (getPlayer()) {
                                    player.GetComponent<scr_playerIdentity>().CmdGainCard(data.hosts, "draw");
                                    player.GetComponent<scr_playerIdentity>().CmdGainCard(data.hosts, "draw");
                                }
                                emerge_flag = false;
                            }
                            break;
                        case "little_mermaid":
                            curData = cur.GetComponent<scr_unit>();
                            if (curData.card_tag.Contains("Water"))
                            {
                                data.CmdgetStatus(cur, 1, 1);
                                emerge_flag = false;
                            }
                            break;
                        case "slum_dancer":
                            data.CmdgetStatus(cur, -2, 0);
                            emerge_flag = false;
                            break;
                        case "attack_eater":
                            curData = cur.GetComponent<scr_unit>();
                            bool temp_flag = false;
                            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Unit")) {
                                if (obj != gameObject && obj.GetComponent<scr_unit>().card_tag.Contains("Wind")) {                                   
                                    temp_flag = true;
                                    break;
                                }
                            }
                            if (temp_flag) {
                                int temp_curAttack = curData.attack;
                                int temp_attack = data.attack;
                                data.CmdgetSelfStatus(temp_curAttack - temp_attack, 0);
                                data.CmdgetStatus(cur, temp_attack - temp_curAttack, 0);
                                emerge_flag = false;
                            }                           
                            break;
                        case "ground_follower":
                            curData = cur.GetComponent<scr_unit>();
                            if (curData.hosts == data.hosts && curData.card_tag.Contains("Earth"))
                            {
                                curData.CmdgetSelfStatus(curData.health - curData.attack, 0);
                                emerge_flag = false;
                            }
                            break;
                        case "whitefur_fox":
                            curData = cur.GetComponent<scr_unit>();
                            if (curData.hosts == data.hosts && curData.card_tag.Contains("Beast"))
                            {
                                data.CmdgetStatus(cur, 1, 1);
                                emerge_flag = false;
                            }
                            break;
                        case "juen":
                            curData = cur.GetComponent<scr_unit>();
                            if (curData.hosts == data.hosts && curData.card_tag.Contains("Beast"))
                            {
                                curData.Cmdcharge();
                                emerge_flag = false;
                            }
                            break;
                        case "Toxie":
                            curData = cur.GetComponent<scr_unit>();
                            if (curData.health <= 2)
                            {
                                curData.CmdCamouflage("Devil-T_host");
                                emerge_flag = false;
                            }
                            break;
                        case "steel_sworder":
                            data.CmdDamage(cur, 2);
                            emerge_flag = false;
                            break;
                        case "rein":
                            data.CmdgetStatus(cur, 1 - cur.GetComponent<scr_unit>().attack, 0);
                            emerge_flag = false;
                            break;
                        case "dark_baron":
                            if (getPlayer()) {
                                int temp_health = cur.GetComponent<scr_unit>().health;
                                data.CmdDamage(player, temp_health);
                                data.CmdDamage(cur, temp_health);
                            }
                            emerge_flag = false;
                            break;
                        case "mitero":
                            curData = cur.GetComponent<scr_unit>();
                            if (curData.hosts == data.hosts && curData.card_tag.Contains("Wind"))
                            {
                                data.CmdDamage(cur, curData.health);
                                emerge_flag = false;
                            }
                            if (!emerge_flag) {
                                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Unit")) {
                                    scr_unit objData = obj.GetComponent<scr_unit>();
                                    if (data.hosts != objData.hosts && objData.attack >= 5) {
                                        data.CmdDamage(obj, objData.health);
                                    }
                                }
                            }
                            break;
                        case "Sein":
                            getPlayer();
                            curData = cur.GetComponent<scr_unit>();
                            if (data.isMyturn()) {
                                if (scr_playerInGame.hand.Contains("doll")) {
                                    scr_playerInGame.hand.Remove("doll");
                                    scr_playerInGame.deck.Add("doll");
                                    data.CmdDamage(cur, 2);
                                }
                            }
                            break;
                        case "Tenebera":
                            data.CmdrequestParticleByVector(7, cur.transform.localPosition, -1);
                            data.CmdDestroyUnit(cur);
                            emerge_flag = false;
                            break;
                        case "dark_mage":
                            data.CmdDamage(cur, 4);
                            emerge_flag = false;
                            break;
                        case "burying_socerer":
                            curData = cur.GetComponent<scr_unit>();
                            if (data.isMyturn()) {
                                scr_playerInGame.hand.Add(curData.id);
                            }
                            data.CmdrequestParticleByVector(7, cur.transform.localPosition, -1);
                            data.CmdDestroyUnit(cur);
                            emerge_flag = false;
                            break;
                        case "tigor":
                            curData = cur.GetComponent<scr_unit>();
                            if (curData.hosts == data.hosts && curData.card_tag.Contains("Beast"))
                            {
                                data.CmdgetStatus(cur, 3, 3);
                                emerge_flag = false;
                            }
                            break;
                        case "SENA":
                            if (data.isMyturn())
                            {
                                if (scr_playerInGame.hand.Count != 0)
                                {
                                    data.CmdDamage(cur, 2);
                                    getPlayer();
                                    player.GetComponent<scr_playerIdentity>().CmdSelectCard(data.hosts, data.id);
                                }
                            }
                            break;
                        case "BT":
                            if(cur.GetComponent<scr_unit>().attack <= 5) cur.GetComponent<scr_unit>().Cmdcharge();
                            emerge_flag = false;
                            break;
                        default:
                            break;
                    }
                    break;
                }
            }
        }

        gameObject.transform.position = savePos;
        drag = false;
    }

    private bool canAttack(GameObject cur) {
        bool flag = true;
        foreach (GameObject it in GameObject.FindGameObjectsWithTag("Unit")) {
            if (it.GetComponent<scr_unit>().isEnemy && (it.transform.localPosition.x == cur.transform.localPosition.x
                && it.transform.localPosition.y < cur.transform.localPosition.y)) {
                flag = false;
                break;
            }
        }
        return flag;
    }

    private bool canDirectAttack() {
        Vector3 pos = sav_parent.transform.localPosition;
        bool flag = true;
        foreach (GameObject it in GameObject.FindGameObjectsWithTag("Unit")) {
            if (it.GetComponent<scr_unit>().isEnemy && Mathf.Abs(it.transform.localPosition.x - pos.x) <= 10) {
                flag = false;
                break;
            }
        }
        return flag;
    }

    public void requestParticle(int particle, GameObject target, int value) {
        scr_unit data = sav_parent.GetComponent<scr_unit>();

        data.request_particle = particle;
        data.request_particle_target = target;
        data.request_particle_value = value;
    }

    public bool getPlayer()
    {
        bool flag = false;
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (cur.GetComponent<scr_playerController>().isPlayer)
            {
                player = cur;
                flag = true;
                break;
            }
        }
        return flag;
    }
}
