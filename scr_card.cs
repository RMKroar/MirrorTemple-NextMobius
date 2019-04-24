using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class scr_card : NetworkBehaviour, IPointerClickHandler {
    // Identification
    public int terial;
    public int attack;
    public int health;
    public int rank;
    public string cardname;
    public string description;
    public string card_tag;
    public string pic_url;

    public string id;

    // Sprites
    public Sprite unit;
    public Sprite extra;
    public Sprite spell_unit;
    public Sprite spell_all;
    public static Sprite[] numbers;
    public static Sprite[] elements;

    public int time = 0;

	void Start () {
        SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer>();

        numbers = new Sprite[10];
        for (int i = 0; i <= 9; i++) {
            numbers[i] = Resources.Load<Sprite>("sprites/numbers/num_" + i);
        }

        elements = new Sprite[15];
        for (int i = 0; i <= 14; i++) {
            elements[i] = Resources.Load<Sprite>("sprites/Units/Elements/element_" + i);
        }        

        spr.sprite = unit;
        if (rank == 3) spr.sprite = extra;
        else if (id.Contains("#")) spr.sprite = (attack < 0)? spell_all : spell_unit;

        Transform pic = gameObject.transform.GetChild(0);
        SpriteRenderer picRend = pic.gameObject.GetComponent<SpriteRenderer>();
        picRend.sprite = Resources.Load<Sprite>(pic_url) as Sprite;
        if (id.Contains("#"))
        {
            picRend.sortingOrder = 6;
            picRend.color = new Color(1f, 1f, 1f, 0.8f);
            pic.localPosition = new Vector3(0, 43, 0);
            pic.localScale = new Vector3(0.8f, 0.8f, 1f);
        }
        setSprites();      
	}

    // Rank : 0 - normal, 1 - rare, 2 - legend, 3 - extra
    public void setIdentity(int terial, int attack, int health, int rank, string cardname, string description, string card_tag, string pic_url, string id) {
        this.terial = terial;
        this.attack = attack;
        this.health = health;
        this.rank = rank;
        this.cardname = cardname;
        this.description = description;
        this.card_tag = card_tag;
        this.pic_url = pic_url;
        this.id = id;
    }

    public void setSprites() {
        SpriteRenderer temp = getChild(1).GetComponent<SpriteRenderer>();
        if (rank != 3) {
            temp.sprite = numbers[terial];
            temp.color = Color.yellow;
        }

        if (!id.Contains("#"))
        {
            temp = getChild(2).GetComponent<SpriteRenderer>();
            if (attack < 10) {
                temp.sprite = (attack > -1) ? numbers[attack] : null;
                transform.GetChild(2).localPosition = new Vector3(131, 103, 0);
            }
            else {
                temp.sprite = numbers[attack % 10];
                SpriteRenderer temp_extend = getChild(2).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                temp_extend.sprite = numbers[attack / 10];
                transform.GetChild(2).localPosition = new Vector3(146, 103, 0);
            }
            temp = getChild(3).GetComponent<SpriteRenderer>();
            if (health < 10)
            {
                temp.sprite = (health > -1) ? numbers[health] : null;
                transform.GetChild(3).localPosition = new Vector3(131, -30, 0);
            }
            else
            {
                temp.sprite = numbers[health % 10];
                SpriteRenderer temp_extend = getChild(3).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                temp_extend.sprite = numbers[health / 10];
                transform.GetChild(3).localPosition = new Vector3(146, -30, 0);
            }
        }
        else temp.color = new Color(0.9f, 1f, 1f);

        Text temp2 = getChild(4).GetComponent<Text>();
        temp2.text = cardname;
        temp2.color = rankedName();

        temp2 = getChild(5).GetComponent<Text>();
        temp2.text = description;

        temp = getChild(6).GetComponent<SpriteRenderer>();
        ArrayList temp_index = getTag();
        if (temp_index != null)
        {
            if (temp_index.Count == 1) temp.sprite = elements[(int)temp_index[0]];
            else if (temp_index.Count == 2)
            {
                temp.sprite = (time <= 50) ? elements[(int)temp_index[0]] : elements[(int)temp_index[1]];
            }
        }
    }

    void Update() {
        time++;
        if (time > 100) time = 0;

        SpriteRenderer temp = getChild(6).GetComponent<SpriteRenderer>();
        ArrayList temp_index = getTag();
        if (temp_index != null)
        {
            if (temp_index.Count == 1) temp.sprite = elements[(int)temp_index[0]];
            else if (temp_index.Count == 2)
            {
                temp.sprite = (time <= 50) ? elements[(int)temp_index[0]] : elements[(int)temp_index[1]];
            }
        }
    }

    private Color rankedName() {
        switch (rank) {
            case 0: return new Color(0, 0, 0);
            case 1: return Color.blue;
            case 2: return Color.magenta;
            case 3: return new Color(165/255f, 0, 1f);
            default: return new Color(1, 1, 1);
        }
    }

    private ArrayList getTag() {
        ArrayList temp_index = new ArrayList();

        if (id.Contains("#")) return null;

        if (card_tag.Contains("Fire")) temp_index.Add(0);
        if (card_tag.Contains("Water")) temp_index.Add(1);
        if (card_tag.Contains("Wind")) temp_index.Add(2);
        if (card_tag.Contains("Earth")) temp_index.Add(3);
        if (card_tag.Contains("Shock")) temp_index.Add(4);
        if (card_tag.Contains("Saint")) temp_index.Add(5);
        if (card_tag.Contains("Dark")) temp_index.Add(6);
        if (card_tag.Contains("Dimension")) temp_index.Add(7);

        if (card_tag.Contains("Warrior")) temp_index.Add(8);
        if (card_tag.Contains("Mage")) temp_index.Add(9);
        if (card_tag.Contains("Archer")) temp_index.Add(10);
        if (card_tag.Contains("Dragon")) temp_index.Add(11);
        if (card_tag.Contains("Beast")) temp_index.Add(12);
        if (card_tag.Contains("God")) temp_index.Add(13);
        if (card_tag.Contains("Devil")) temp_index.Add(14);

        return temp_index;
    }

    private GameObject getChild(int index) {
        return gameObject.transform.GetChild(index).gameObject;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.parent.tag == "SelectList")
        {
            GameObject ancestor = transform.parent.parent.parent.parent.gameObject;
            ancestor.GetComponent<scr_select>().setId(id);
        }
    }
}
