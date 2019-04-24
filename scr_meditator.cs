using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class scr_meditator : NetworkBehaviour {
    public GameObject player;
    bool flag = false;

    [SyncVar]
    public bool turn;
    [SyncVar]
    public int serverHandcnt = 3;
    [SyncVar]
    public int clientHandcnt = 3;
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
    public string pic_url;
    [SyncVar]
    public string spawn_tag;
    [SyncVar]
    public string id;
    [SyncVar]
    public Vector3 spawn_pos;
    [SyncVar]
    public int turnCnt = 0;

    void Update()
    {
        if (!flag)
        {
            scr_playerInGame.cardQueue.Enqueue("draw");
            scr_playerInGame.cardQueue.Enqueue("draw");
            scr_playerInGame.cardQueue.Enqueue("draw");
            flag = true;
        }
    }

    public void setTurn(bool set_turn) {
        turn = set_turn;
    }

    public bool getTurn() {
        return turn;
    }

    public void changeTurn() {
        turn = (turn) ? false : true;
        turnCnt++;
    }

    // [later patch needed] Need to be use string Parameter and split it
    public void setData(int a, int b, int c, int d, string e, string f, string g, string h, string i, Vector3 pos) {
        terial = a;
        attack = b;
        health = c;
        rank = d;
        cardname = e;
        description = f;
        spawn_tag = g;
        pic_url = h;
        id = i;
        spawn_pos = pos;
    }
    
    public void informHand(bool is_server, int cnt) {
        if (is_server)
        {
            serverHandcnt = cnt;
        }
        else
        {
            clientHandcnt = cnt;
        }
    }

    public void createParticle(GameObject create_target, Vector3 create_pos, int value) {
        GameObject ins = Instantiate(create_target);
        ins.transform.SetParent(GameObject.Find("Canvas").transform);
        ins.transform.localPosition = create_pos;
        if (ins.tag == "EffectDamage") ins.GetComponent<scr_effect_damage>().value = value;
        else ins.transform.localPosition += new Vector3(0f, 0f, -value);
    }

    public void createParticle(int par_id, Vector3 create_pos, int value)
    {
        GameObject create_target = null;
        Vector3 delta_pos = Vector3.zero;

        switch (par_id) {
            case 0: create_target = DB_particles.par_myturn;
                break;
            case 1: create_target = DB_particles.par_up;
                delta_pos = new Vector3(0f, -100f, 0f);
                break;
            case 2: create_target = DB_particles.par_down;
                delta_pos = new Vector3(0f, 100f, 0f);
                break;
            case 3: create_target = DB_particles.par_ability;
                break;
            case 4: create_target = DB_particles.par_charge;
                break;
            case 5:
                create_target = DB_particles.par_card;
                break;
            case 6:
                create_target = DB_particles.par_spell;
                break;
            case 7:
                create_target = DB_particles.par_cross;
                break;
            case 100: create_target = DB_particles.par_revilla;
                break;
            case 101: create_target = DB_particles.par_flower;
                break;
            case 102: create_target = DB_particles.par_magmaBurst;
                break;
            case 103: create_target = DB_particles.par_giantBreath;
                break;
            case 104:
                create_target = DB_particles.par_giantMud;
                break;
            case 105:
                create_target = DB_particles.par_giantShine;
                break;
            case 106:
                create_target = DB_particles.par_darkMist;
                break;
            case 107:
                create_target = DB_particles.par_cyclone;
                break;
            case 108:
                create_target = DB_particles.par_apocalipse;
                break;
            case 109:
                create_target = DB_particles.par_soulwhite;
                break;
            case 110:
                create_target = DB_particles.par_soulblue;
                break;
            case 111:
                create_target = DB_particles.par_soulred;
                break;
            default: Debug.Log("particle load failed");
                break;
        }

        GameObject ins = Instantiate(create_target);
        ins.transform.SetParent(GameObject.Find("Canvas").transform);
        ins.transform.localPosition = create_pos + delta_pos;
        if (value >= 0) ins.GetComponent<scr_effect_damage>().value = value;
    }
}
