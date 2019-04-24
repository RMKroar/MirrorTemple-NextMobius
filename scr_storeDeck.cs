using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_storeDeck : MonoBehaviour {
    public static ArrayList store_deck = new ArrayList();
    public static string extraID = "PEIN";

    public static void structureDeck() {
        int structDeck = GameObject.Find("DStructureDropdown").GetComponent<Dropdown>().value;
        store_deck.Clear();

        switch (structDeck) {
            case 1: extraID = "TONUS";
                store_deck.Add("volcanic_dragonling"); store_deck.Add("volcanic_dragonling"); store_deck.Add("volcanic_dragon");
                store_deck.Add("volcanic_dragon"); store_deck.Add("volcanic_dragon"); store_deck.Add("volcanic_monster");
                store_deck.Add("volcanic_monster"); store_deck.Add("volcanic_monster"); store_deck.Add("lava_swarm");
                store_deck.Add("lava_swarm"); store_deck.Add("lava_swarm"); store_deck.Add("dragon_tamer"); store_deck.Add("dragon_tamer");
                store_deck.Add("dragon_tamer"); store_deck.Add("Volcanic_twindragon"); store_deck.Add("little_firedragon");
                store_deck.Add("little_firedragon"); store_deck.Add("little_firedragon"); store_deck.Add("knowledge_omniscence");
                store_deck.Add("knowledge_omniscence"); store_deck.Add("#volcano");
                store_deck.Add("#ador_breath"); store_deck.Add("#ador_breath"); store_deck.Add("#selfburn"); store_deck.Add("#heal_leaf");
                store_deck.Add("#union_roar"); store_deck.Add("#union_roar"); store_deck.Add("#death");
                break;
            case 2: extraID = "EURIEL";
                store_deck.Add("raysen_fisherman"); store_deck.Add("raysen_fisherman"); store_deck.Add("triarodon_secretary");
                store_deck.Add("triarodon_secretary"); store_deck.Add("triarodon_secretary"); store_deck.Add("little_mermaid");
                store_deck.Add("little_mermaid"); store_deck.Add("little_mermaid"); store_deck.Add("waterdragon_son");
                store_deck.Add("waterdragon_son"); store_deck.Add("waterdragon_priest"); store_deck.Add("waterdragon_priest");
                store_deck.Add("waterpalace_guardian"); store_deck.Add("waterpalace_guardian"); store_deck.Add("waterpalace_guardian");
                store_deck.Add("Triarodon"); store_deck.Add("#tales_river"); store_deck.Add("#tales_river"); store_deck.Add("#tales_river");
                store_deck.Add("#salvage"); store_deck.Add("#salvage"); store_deck.Add("#blood_storm"); store_deck.Add("#nas_breath");
                store_deck.Add("#nas_breath"); store_deck.Add("#death"); store_deck.Add("#ren_wakeup");
                break;
            case 3: extraID = "NATIS";
                store_deck.Add("barefoot_boy"); store_deck.Add("barefoot_boy"); store_deck.Add("barefoot_boy"); store_deck.Add("tina");
                store_deck.Add("tina"); store_deck.Add("tina"); store_deck.Add("wind_rider"); store_deck.Add("wind_rider");
                store_deck.Add("little_lion"); store_deck.Add("little_lion"); store_deck.Add("little_lion"); store_deck.Add("wind_walker");
                store_deck.Add("wind_walker"); store_deck.Add("attack_eater"); store_deck.Add("attack_eater"); store_deck.Add("necky");
                store_deck.Add("necky"); store_deck.Add("tornado_hero"); store_deck.Add("mitero"); store_deck.Add("Keiro");
                store_deck.Add("#wisdom_eye"); store_deck.Add("#wisdom_eye"); store_deck.Add("#death"); store_deck.Add("#ren_wakeup");
                break;
            case 4: extraID = "HETERO";
                store_deck.Add("little_healer"); store_deck.Add("moon_slayer"); store_deck.Add("Revilla");
                store_deck.Add("admired_bishop"); store_deck.Add("duret"); store_deck.Add("grail_maker"); store_deck.Add("alcadrobot_typeB");
                store_deck.Add("Sytron"); store_deck.Add("Sardis"); store_deck.Add("expert_healer");
                store_deck.Add("burying_socerer"); store_deck.Add("Resia"); store_deck.Add("#evolve:SaintResia"); store_deck.Add("#recall");
                store_deck.Add("#blood_storm"); store_deck.Add("#emergency_deposit"); store_deck.Add("#emergency_deposit"); store_deck.Add("#heal_leaf");
                store_deck.Add("#wisdom_eye"); store_deck.Add("#death"); store_deck.Add("#ren_wakeup");
                break;
            case 5: extraID = "MACHINA";
                store_deck.Add("sacrificer"); store_deck.Add("sacrificer"); store_deck.Add("sacrificer"); store_deck.Add("sacrificed_ghost");
                store_deck.Add("sacrificed_ghost"); store_deck.Add("sacrificed_ghost"); store_deck.Add("jack-o-lantern"); store_deck.Add("jack-o-lantern");
                store_deck.Add("tornado_hero"); store_deck.Add("sacrifice_summoner"); store_deck.Add("sacrifice_summoner");
                store_deck.Add("wishful_bishop"); store_deck.Add("wishful_bishop"); store_deck.Add("dark_baron"); store_deck.Add("#widdy_breath");
                store_deck.Add("#blood_storm"); store_deck.Add("#blood_storm"); store_deck.Add("#heal_leaf"); store_deck.Add("#heal_leaf");
                store_deck.Add("#wisdom_eye"); store_deck.Add("#wisdom_eye");
                break;
            case 6: extraID = "BT";
                store_deck.Add("cave_guardian"); store_deck.Add("cave_guardian"); store_deck.Add("cave_guardian");
                store_deck.Add("neprafti"); store_deck.Add("neprafti"); store_deck.Add("neprafti");
                store_deck.Add("afterlife_guide"); store_deck.Add("afterlife_guide"); store_deck.Add("afterlife_guide");
                store_deck.Add("Necropia"); store_deck.Add("Tenebera"); store_deck.Add("Pyrena");
                store_deck.Add("dark_mage"); store_deck.Add("dark_mage"); store_deck.Add("#dark_token"); store_deck.Add("#dark_token");
                store_deck.Add("#dark_token"); store_deck.Add("#grave_robber"); store_deck.Add("#grave_robber");
                store_deck.Add("#dark_ora"); store_deck.Add("#dark_ora"); store_deck.Add("#dark_ora");
                store_deck.Add("#emergency_deposit"); store_deck.Add("#emergency_deposit"); store_deck.Add("#ren_wakeup");
                break;
            case 7: extraID = "BAN";
                store_deck.Add("student_swordman"); store_deck.Add("student_swordman"); store_deck.Add("student_swordman"); store_deck.Add("student_fighter");
                store_deck.Add("student_fighter"); store_deck.Add("student_fighter"); store_deck.Add("big_sworder"); store_deck.Add("big_sworder");
                store_deck.Add("student_knight"); store_deck.Add("student_knight"); store_deck.Add("student_knight"); store_deck.Add("moon_slayer");
                store_deck.Add("moon_slayer"); store_deck.Add("Revilla"); store_deck.Add("tough_shieldman"); store_deck.Add("tough_shieldman");
                store_deck.Add("wind_rider"); store_deck.Add("wind_rider"); store_deck.Add("steel_sworder"); store_deck.Add("steel_sworder");
                store_deck.Add("battlefield_commander"); store_deck.Add("battlefield_commander"); store_deck.Add("battlefield_commander");
                store_deck.Add("mancy"); store_deck.Add("mancy"); store_deck.Add("#assembly"); store_deck.Add("#assembly"); store_deck.Add("#assembly");
                store_deck.Add("#death");
                break;
            case 8:
                extraID = "TONUS";
                store_deck.Add("student_magician"); store_deck.Add("student_magician"); store_deck.Add("student_magician"); store_deck.Add("ancient_magician");
                store_deck.Add("ancient_magician"); store_deck.Add("ancient_magician"); store_deck.Add("dimension_summoner"); store_deck.Add("dimension_summoner");
                store_deck.Add("dimension_summoner"); store_deck.Add("alcadrobot_typeB"); store_deck.Add("alcadrobot_typeB"); store_deck.Add("violet");
                store_deck.Add("violet"); store_deck.Add("knowledge_omniscence"); store_deck.Add("knowledge_omniscence"); store_deck.Add("#dimension_summon");
                store_deck.Add("#dimension_summon"); store_deck.Add("#dimension_summon"); store_deck.Add("#recall"); store_deck.Add("#recall");
                store_deck.Add("#wisdom_eye"); store_deck.Add("#wisdom_eye");
                break;
            case 9:
                extraID = "BT";
                store_deck.Add("kitten"); store_deck.Add("kitten"); store_deck.Add("kitten"); store_deck.Add("corpse_raven"); store_deck.Add("corpse_raven");
                store_deck.Add("animal_lover"); store_deck.Add("animal_lover"); store_deck.Add("mild_bear"); store_deck.Add("mild_bear"); store_deck.Add("whitefur_fox");
                store_deck.Add("soulful_icefox"); store_deck.Add("soulful_icefox");
                store_deck.Add("little_lion"); store_deck.Add("little_lion"); store_deck.Add("Nyan"); store_deck.Add("longlife_turtle");
                store_deck.Add("lion_tamer"); store_deck.Add("lion_tamer"); store_deck.Add("tigor"); store_deck.Add("tigor");
                store_deck.Add("#recall"); store_deck.Add("#recall");
                store_deck.Add("#wisdom_eye"); store_deck.Add("#death"); store_deck.Add("#ren_wakeup");
                break;
            case 10:
                extraID = "LICA";
                store_deck.Add("dollmaker"); store_deck.Add("dollmaker"); store_deck.Add("dollhugging_girl"); store_deck.Add("dollhugging_girl"); store_deck.Add("dollhugging_girl");
                store_deck.Add("lary"); store_deck.Add("lary"); store_deck.Add("lary"); store_deck.Add("tetra"); store_deck.Add("tetra"); store_deck.Add("Sein");
                store_deck.Add("mirat"); store_deck.Add("mirat"); store_deck.Add("#doll_gathering"); store_deck.Add("#doll_gathering"); store_deck.Add("#doll_gathering");
                store_deck.Add("#doll_regression"); store_deck.Add("#doll_regression"); store_deck.Add("#doll_sacrifice"); store_deck.Add("#doll_sacrifice");
                store_deck.Add("#wisdom_eye"); store_deck.Add("#wisdom_eye");
                break;
            default:
                break;
        }

        GameObject.Find("DeckPanels").GetComponent<scr_deckPanel>().refresh();
        GameObject extraEl = GameObject.Find("DExtraElement");
        extraEl.GetComponent<scr_deckShowcard>().id = extraID;
        extraEl.transform.GetChild(0).GetComponent<Text>().text = (string)DB_card.getIdentity(extraID)[4];
    }

    // saving deck data : {"card_[cardIndex]_[deckNum]", string}
    public static void saveDeck(int value) {
        Debug.Log("invoked");
        if (PlayerPrefs.HasKey("extra_" + value)) PlayerPrefs.DeleteKey("extra_" + value);
        Debug.Log("extraDelete");
        for (int i = 0; i < 30; i++)
        {
            if (PlayerPrefs.HasKey("card_" + i + "_" + value)) PlayerPrefs.DeleteKey("card_" + i + "_" + value);
            else break;
            Debug.Log("cardDelete_index: " + i);
        }

        Transform inpf = GameObject.Find("DLoadButton_" + value).transform.GetChild(1);
        if (inpf.gameObject.activeInHierarchy)
        {
            string input_txt = inpf.gameObject.GetComponent<InputField>().text;
            PlayerPrefs.SetString("name_" + value, input_txt);
            GameObject.Find("DLoadButton_" + value).transform.GetChild(0).gameObject.GetComponent<Text>().text = input_txt;
            inpf.gameObject.SetActive(false);
        }

        int j = 0;
        PlayerPrefs.SetString("extra_" + value, extraID);
        Debug.Log("extraSave");
        foreach (string cardID in store_deck) {
            PlayerPrefs.SetString("card_" + j + "_" + value, cardID);
            j++;
            Debug.Log("cardSave_index: " + j);
        }
        PlayerPrefs.Save();
        Debug.Log("SaveFinished");
    }
    
    public static void loadDeck(int value) {
        store_deck.Clear();
        Debug.Log("invoked");
        if (PlayerPrefs.HasKey("extra_" + value))
        {
            extraID = PlayerPrefs.GetString("extra_" + value);
            Debug.Log("extraLoad");
            int i = 0;
            while (true)
            {
                if (!PlayerPrefs.HasKey("card_" + i + "_" + value)) break;
                else store_deck.Add(PlayerPrefs.GetString("card_" + i + "_" + value));
                Debug.Log("cardLoad_index: " + i);
                i++;
            }

            GameObject.Find("DeckPanels").GetComponent<scr_deckPanel>().refresh();
            GameObject extraEl = GameObject.Find("DExtraElement");
            extraEl.GetComponent<scr_deckShowcard>().id = extraID;
            extraEl.transform.GetChild(0).GetComponent<Text>().text = (string)DB_card.getIdentity(extraID)[4];
        }
    }
}