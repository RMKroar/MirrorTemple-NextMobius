using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_deckPanel : MonoBehaviour {
    public GameObject element;
    public GameObject terialFilter;
    public GameObject rankFilter;
    public GameObject abilFilter;
    public GameObject tagFilter;
    public GameObject searchFilter;

    int sav_terialFilter;
    int sav_rankFilter;
    int sav_abilFilter;
    int sav_tagFilter;
    string sav_search;

	// Use this for initialization
	void Start () {
        GameObject insParent = GameObject.Find("DGridWithElements");
        foreach (string id in DB_card.collectible) {
            GameObject ins = Instantiate(element, insParent.transform);
            ins.GetComponent<scr_deckShowcard>().id = id;
            ins.transform.GetChild(0).gameObject.GetComponent<Text>().text = (string)DB_card.getIdentity(id)[4];
        }

        for (int i = 1; i <= 3; i++)
        {
            if (PlayerPrefs.HasKey("name_" + i))
            {
                GameObject.Find("DLoadButton_" + i).transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("name_" + i);
            }
        }

        Dropdown drp = terialFilter.GetComponent<Dropdown>();
        sav_terialFilter = drp.value;
        drp.onValueChanged.AddListener(delegate {
            valueChange(terialFilter);
        });

        drp = rankFilter.GetComponent<Dropdown>();
        sav_rankFilter = drp.value;
        drp.onValueChanged.AddListener(delegate {
            valueChange(rankFilter);
        });

        drp = abilFilter.GetComponent<Dropdown>();     
        sav_abilFilter = drp.value;
        drp.onValueChanged.AddListener(delegate {
            valueChange(abilFilter);
        });

        drp = tagFilter.GetComponent<Dropdown>();
        sav_tagFilter = drp.value;
        drp.onValueChanged.AddListener(delegate {
            valueChange(tagFilter);
        });

        InputField inpf = searchFilter.GetComponent<InputField>();
        sav_search = inpf.text;
        inpf.onValueChanged.AddListener(delegate {
            valueChange(searchFilter);
        });

        refresh();
    }

    void valueChange(GameObject target) {
        if (target == terialFilter) sav_terialFilter = terialFilter.GetComponent<Dropdown>().value;
        else if (target == rankFilter) sav_rankFilter = rankFilter.GetComponent<Dropdown>().value;
        else if (target == abilFilter) sav_abilFilter = abilFilter.GetComponent<Dropdown>().value;
        else if (target == tagFilter) sav_tagFilter = tagFilter.GetComponent<Dropdown>().value;
        else if (target == searchFilter) sav_search = searchFilter.GetComponent<InputField>().text;

        refresh();
    }

    public void refresh() {
        for (int i = 0; i <= 7; i++) {
            GameObject graph = GameObject.Find("TerialGraph_" + i);
            graph.GetComponent<RectTransform>().sizeDelta = new Vector2(60f, 0f);
            graph.transform.GetChild(0).gameObject.GetComponent<Text>().text = "0";
        }
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Card"))
        {
            Destroy(cur);
        }
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Element")) {
            Destroy(cur);
        }
        GameObject insParent = GameObject.Find("DGridWithElements");
        GameObject insParent2 = GameObject.Find("DGridWithSelections");
        foreach (string id in DB_card.collectible)
        {
            if (checkTerial(id)) {
                GameObject ins = Instantiate(element, insParent.transform);
                ins.GetComponent<scr_deckShowcard>().id = id;
                ins.transform.GetChild(0).gameObject.GetComponent<Text>().text = (string)DB_card.getIdentity(id)[4];         
            }
        }
        foreach (string id in scr_storeDeck.store_deck)
        {
            GameObject ins = Instantiate(element, insParent2.transform);
            ins.GetComponent<scr_deckShowcard>().id = id;
            ins.GetComponent<scr_deckShowcard>().inDeck = true;
            ins.transform.GetChild(0).gameObject.GetComponent<Text>().text = (string)DB_card.getIdentity(id)[4];

            int cost = (int)DB_card.getIdentity(id)[0];
            GameObject graph = GameObject.Find("TerialGraph_" + cost);
            GameObject graphText = graph.transform.GetChild(0).gameObject;
            int graphAmount = Int32.Parse(graphText.GetComponent<Text>().text);
            if(graphAmount <= 15) graph.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 24f);
            graphText.GetComponent<Text>().text = ""+ (graphAmount + 1);
        }
    }

    public bool checkTerial(string id) {
        string data_name = (string)DB_card.getIdentity(id)[4];
        string data = (string)DB_card.getIdentity(id)[5];
        string data_tag = (string)DB_card.getIdentity(id)[6];
        bool flag = true;

        if (sav_search != null || sav_search != "") {
            if (!data_name.Contains(sav_search) && !data.Contains(sav_search)) flag = false;
        }        

        if (flag) switch (sav_terialFilter) {
            case 1:
                if ((int)DB_card.getIdentity(id)[0] > 1) flag = false;
                break;
            case 2:
                if ((int)DB_card.getIdentity(id)[0] != 2) flag = false;
                break;
            case 3:
                if ((int)DB_card.getIdentity(id)[0] != 3) flag = false;
                break;
            case 4:
                if ((int)DB_card.getIdentity(id)[0] != 4) flag = false;
                break;
            case 5:
                if ((int)DB_card.getIdentity(id)[0] != 5) flag = false;
                break;
            case 6:
                if ((int)DB_card.getIdentity(id)[0] < 6) flag = false;
                break;
            default:
                break;
        }
        if (flag) switch (sav_rankFilter)
        {
            case 1:
                if ((int)DB_card.getIdentity(id)[3] != 0) flag = false;
                break;
            case 2:
                if ((int)DB_card.getIdentity(id)[3] != 1) flag = false;
                break;
            case 3:
                if ((int)DB_card.getIdentity(id)[3] != 2) flag = false;
                break;
            default:
                break;
        }
        if (flag) switch (sav_abilFilter)
        {
            case 1:
                if (!data.Contains("[출격")) flag = false;
                break;
            case 2:
                if (!data.Contains("[능력")) flag = false;
                break;
            case 3:
                if (!data.Contains("[돌진")) flag = false;
                break;
            case 4:
                if (!data.Contains("[원격")) flag = false;
                break;
            case 5:
                if (!data.Contains("[성장")) flag = false;
                break;
            case 6:
                if (!data.Contains("[사망")) flag = false;
                break;
            default:
                break;
        }
        if (flag) switch (sav_tagFilter)
            {
                case 1:
                    if (!id.Contains("#")) flag = false;
                    break;
                case 2:
                    if (!data_tag.Contains("Fire")) flag = false;
                    break;
                case 3:
                    if (!data_tag.Contains("Water")) flag = false;
                    break;
                case 4:
                    if (!data_tag.Contains("Wind")) flag = false;
                    break;
                case 5:
                    if (!data_tag.Contains("Earth")) flag = false;
                    break;
                case 6:
                    if (!data_tag.Contains("Shock")) flag = false;
                    break;
                case 7:
                    if (!data_tag.Contains("Saint")) flag = false;
                    break;
                case 8:
                    if (!data_tag.Contains("Dark")) flag = false;
                    break;
                case 9:
                    if (!data_tag.Contains("Dimension")) flag = false;
                    break;
                case 10:
                    if (!data_tag.Contains("Warrior")) flag = false;
                    break;
                case 11:
                    if (!data_tag.Contains("Mage")) flag = false;
                    break;
                case 12:
                    if (!data_tag.Contains("Archer")) flag = false;
                    break;
                case 13:
                    if (!data_tag.Contains("Dragon")) flag = false;
                    break;
                case 14:
                    if (!data_tag.Contains("Beast")) flag = false;
                    break;
                case 15:
                    if (!data_tag.Contains("God")) flag = false;
                    break;
                case 16:
                    if (!data_tag.Contains("Devil")) flag = false;
                    break;
                default:
                    break;
            }

        return flag;
    }
}
