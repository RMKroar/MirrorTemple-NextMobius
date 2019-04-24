using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_card : MonoBehaviour {
    public static Dictionary<string, ArrayList> cardData = new Dictionary<string, ArrayList>();

    void Start()
    {
        TextAsset csv = (TextAsset)Resources.Load("data/CardData") as TextAsset;
        string[] csvBundles = csv.text.Split('\n');
        int cnt = 0;
        foreach(string bundle in csvBundles) {
            if(cnt != 0)
            {
                string[] csvPieces = bundle.Split(',');
                Debug.Log(csvPieces[0]);
                if (csvPieces[0] == null || csvPieces[0] == "") break;
                ArrayList csvData = new ArrayList();
                for(int i = 1; i <= 8; i++) {
                    if(i > 4)
                    {
                        string datum = csvPieces[i];
                        if (datum != null)
                        {
                            if (i == 6) datum = datum.Replace("$", Environment.NewLine);
                            csvData.Add(datum);
                        }
                        else csvData.Add("");
                    }
                    else
                    {
                        int datum = Int32.Parse(csvPieces[i]);
                        csvData.Add(datum);
                    }
                }
                cardData.Add(csvPieces[0], csvData);
            }
            cnt++;
        }
    }

    public static ArrayList collectible = new ArrayList {
        "doll", "ren", "little_healer", "student_swordman", "student_fighter", "student_magician",
        "shasha",
        "dollmaker", "volcanic_dragonling", "volcanic_dragon", 
        "raysen_fisherman", "barefoot_boy", "purple_swarm", "kitten",
        "big_sworder", "student_knight", "ancient_magician", "dimension_summoner",
        "speedy_archer", "hero_helper", "moon_slayer", "archeologist",
        "sacrificer", "dollhugging_girl", "volcanic_monster", "lava_swarm", "triarodon_secretary", "tina",
        "cave_guardian", "sacrificed_ghost", "neprafti", "corpse_raven", "animal_lover",
        "Revilla", "Volcanic_twindragon", "tough_shieldman", "raysen_samurai",
        "cursed_mage", "synthesizer", "sacrifice_summoner",
        "slum_dancer", "wind_walker", "wave_fighter",
        "duret", "alcadrobot_typeA", "lary", "dragon_tamer", "fire_femalesworder",
        "waterdragon_priest", "little_mermaid", "waterdragon_son", "wind_rider", "attack_eater",
        "ground_priest", "ground_follower", "admired_bishop", "afterlife_guide", "mild_bear", "whitefur_fox",
        "little_lion", "Toxie", "Nyan",
        "crystal_fighter", "steel_sworder", "battlefield_commander", "sirai",
        "oldbook_analyzer", "fire_mage", "nature_mage", "carsus_priest", "violet",
        "mancy", "rein", "necky", "ken", "alcadrobot_typeB", "tetra", "little_firedragon", "waterpalace_guardian", "tornado_hero",
        "ground_sentinel", "mern_chief", "sacrifice_socerer", "grail_maker", "jack-o-lantern", "longlife_turtle",
        "Cardinel", "Sytron", "Sardis", "heavyarmor_knight", "penitent_devil",
        "wishful_bishop", "axe_zealot", "dark_baron", "siera", "mitero", "expert_healer",
        "lion_tamer", "Falcifer", "Sein",
        "Necropia", "Tenebera", "Pyrena", "mirat", "nature_musician",
        "investigator_pein", "burying_socerer", "dark_mage", "tigor",
        "Keres", "Hexator", "Resia", "knowledge_omniscence", "Triarodon",
        "Keiro", "Teradon",
        
        "#cost_control", "#elemental_reset", "#tales_river", "#draft",
        "#slash", "#initial_shine", "#volcano", "#salvage",
        "#widdy_breath", "#vitality",
        "#luminous_grail", "#dark_token", "#grave_robber", "#doll_sacrifice", "#dimension_summon", "#evolve:SaintResia",
        "#recall", "#shrink",
        "#blood_storm", "#ador_breath", "#nas_breath", "#dark_ora", "#doll_gathering", "#gladius", "#titanum_shield",
        "#assembly", "#wisdom_eye", "#heal_leaf", "#emergency_deposit",
        "#armored", "#selfburn", "#earthquake", "#luminous_badge",
        "#doll_regression", "#union_roar", "#death", "#ren_wakeup", "#elemental_book"
    };

    public static ArrayList getIdentity(string id) {
        ArrayList data = new ArrayList();

        // data {cost, attack, health, rank, name, description, url}
        // element {Fire, Water, Wind, Earth(자연 포함), Shock[충격], Saint, Dark, Dimension}
        // tribe {Warrior, Mage, Archer, Dragon, Beast, God, Devil}
        if (!id.Contains("#")) {
            switch (id)
            {
                case "doll":
                    data.Add(0); data.Add(1); data.Add(1); data.Add(0); data.Add("전투 인형");
                    data.Add("");
                    data.Add(""); data.Add("sprites/Units/Common/Doll");
                    break;
                case "ador":
                    data.Add(1); data.Add(1); data.Add(1); data.Add(0); data.Add("아도르");
                    data.Add("");
                    data.Add("Fire"); data.Add("sprites/Units/Common/Ador");
                    break;
                case "nas":
                    data.Add(1); data.Add(1); data.Add(1); data.Add(0); data.Add("나스");
                    data.Add("");
                    data.Add("Water"); data.Add("sprites/Units/Common/Nas");
                    break;
                case "widdy":
                    data.Add(1); data.Add(1); data.Add(1); data.Add(0); data.Add("위디");
                    data.Add("");
                    data.Add("Wind"); data.Add("sprites/Units/Common/Widdy");
                    break;
                case "daki":
                    data.Add(1); data.Add(1); data.Add(1); data.Add(0); data.Add("다키");
                    data.Add("");
                    data.Add("Dark"); data.Add("sprites/Units/Common/Daki");
                    break;
                case "pyren_zombie":
                    data.Add(1); data.Add(0); data.Add(1); data.Add(1); data.Add("파이렌 좀비");
                    data.Add("<color=green>[출격]</color> 사망한 비용 4 이하의 흑암 종족 유닛을 선택하여 제외합니다. 그 유닛과 동일한 유닛으로 변신합니다.");
                    data.Add("Dark"); data.Add("sprites/Units/Rare/PyrenZombie");
                    break;
                case "ren":
                    data.Add(1); data.Add(1); data.Add(1); data.Add(1); data.Add("꼬마 소년 렌");
                    data.Add("<color=green>[출격]</color> 내 테리얼이 6 이상이라면,\n<color=red>모든</color> 유닛에게 피해를 5 줍니다.");
                    data.Add(""); data.Add("sprites/Units/Rare/BoyRen");
                    break;
                case "little_healer":
                    data.Add(1); data.Add(1); data.Add(2); data.Add(0); data.Add("작은 회복사");
                    data.Add("<color=green>[능력]</color> 유닛 하나의 체력을 2 회복합니다.");
                    data.Add("Saint/Mage"); data.Add("sprites/Units/Common/LittleHealer");
                    break;
                case "student_swordman":
                    data.Add(1); data.Add(1); data.Add(1); data.Add(0); data.Add("견습 검사");
                    data.Add("<color=green>[능력]</color> 유닛 하나에게 피해를 1 줍니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Common/StudentSwordman");
                    break;
                case "student_fighter":
                    data.Add(1); data.Add(1); data.Add(2); data.Add(0); data.Add("견습 격투가");
                    data.Add("<color=green>[성장(1)]</color> 공격력을 1 얻습니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Common/StudentFighter");
                    break;
                case "shasha":
                    data.Add(1); data.Add(2); data.Add(1); data.Add(1); data.Add("중첩술사 샤샤");
                    data.Add("<color=green>[능력]</color> 유닛 하나의 비용과 같은 비용이 됩니다.");
                    data.Add("Mage"); data.Add("sprites/Units/Rare/Shasha");
                    break;
                case "dollmaker":
                    data.Add(1); data.Add(1); data.Add(1); data.Add(1); data.Add("인형 재봉사");
                    data.Add("<color=green>[출격]</color> '전투 인형' 카드를 손에 얻습니다.\n" +
                        "<color=green>[공격]</color> '전투 인형' 카드를 손에 얻습니다.");
                    data.Add("Mage"); data.Add("sprites/Units/Rare/Dollmaker");
                    break;
                case "volcanic_dragon":
                    data.Add(1); data.Add(3); data.Add(3); data.Add(0); data.Add("용암룡");
                    data.Add("<color=green>[출격]</color> 이 유닛을 처치합니다.\n<color=green>[환경 : '용암 분화구']</color> 처치를 무효로 합니다.");
                    data.Add("Fire/Dragon");  data.Add("sprites/Units/Common/VolcanicDragon");
                    break;
                case "volcanic_dragonling":
                    data.Add(1); data.Add(0); data.Add(2); data.Add(1); data.Add("용암의 새끼용");
                    data.Add("<color=green>[성장(0)]</color> 덱에서 '용암 분화구'를 뽑습니다.\n<color=green>[환경 : '용암 분화구']</color> 대신 '용암룡'을 뽑습니다.");
                    data.Add("Fire/Dragon"); data.Add("sprites/Units/Rare/VolcanicDragonling");
                    break;
                case "raysen_fisherman":
                    data.Add(1); data.Add(1); data.Add(1); data.Add(1); data.Add("레이센 어부");
                    data.Add("<color=green>[출격]</color> 유계의 비용 2 이하 물 종족 유닛을 선택하여 패로 되돌립니다.\n" +
                    "<color=green>[환경 : '탈레스의 강']</color> 중첩 수만큼 선택할 수 있는 유닛의 비용이 1 증가합니다.");
                    data.Add("Water"); data.Add("sprites/Units/Rare/RaysenFisherman");
                    break;
                case "barefoot_boy":
                    data.Add(1); data.Add(1); data.Add(1); data.Add(0); data.Add("맨발의 소년");
                    data.Add("<color=green>[돌진]</color>");
                    data.Add("Wind"); data.Add("sprites/Units/Common/BarefootBoy");
                    break;
                case "purple_swarm":
                    data.Add(1); data.Add(0); data.Add(3); data.Add(0); data.Add("자색 지룡");
                    data.Add("<color=green>[출격]</color> 손의 땅 종족 유닛의 수만큼 생명력을 +1 얻습니다.");
                    data.Add("Earth/Dragon"); data.Add("sprites/Units/Common/PurpleSwarm");
                    break;
                case "student_magician":
                    data.Add(1); data.Add(1); data.Add(2); data.Add(0); data.Add("견습 마법사");
                    data.Add("<color=green>[출격]</color> 손의 유닛 1장을 선택하여, 내 <color=red>모든</color> " +
                        "유닛을 그 유닛과 같은 종족으로 설정합니다.");
                    data.Add("Mage"); data.Add("sprites/Units/Common/StudentMagician");
                    break;
                case "kitten":
                    data.Add(1); data.Add(1); data.Add(1); data.Add(0); data.Add("작은 고양이");
                    data.Add("<color=green>[출격]</color> 덱에서 '작은 고양이'를 뽑습니다. ");
                    data.Add("Beast"); data.Add("sprites/Units/Common/Kitten");
                    break;
                case "speedy_archer":
                    data.Add(2); data.Add(3); data.Add(2); data.Add(0); data.Add("재빠른 궁수");
                    data.Add("<color=green>[원격]</color>");
                    data.Add("Archer"); data.Add("sprites/Units/Common/SpeedyArcher");
                    break;
                case "hero_helper":
                    data.Add(2); data.Add(1); data.Add(2); data.Add(0); data.Add("용사의 조력자");
                    data.Add("<color=green>[능력]</color> 유닛 하나에게 +1/+1을 부여합니다.");
                    data.Add(""); data.Add("sprites/Units/Common/HeroHelper");
                    break;
                case "moon_slayer":
                    data.Add(2); data.Add(2); data.Add(3); data.Add(0); data.Add("반월 전사");
                    data.Add("<color=green>[공격]</color> 공격력을 1 얻습니다.");
                    data.Add("Saint/Warrior"); data.Add("sprites/Units/Common/MoonSlayer");
                    break;
                case "archeologist":
                    data.Add(2); data.Add(1); data.Add(2); data.Add(0); data.Add("고고학자");
                    data.Add("<color=green>[출격]</color> 카드를 1장 뽑습니다.");
                    data.Add("Earth"); data.Add("sprites/Units/Common/Archeologist");
                    break;
                case "sacrificer":
                    data.Add(2); data.Add(2); data.Add(3); data.Add(0); data.Add("의식의 희생자");
                    data.Add("<color=green>[출격]</color> <color=blue>의식 카운트</color>를 1 얻습니다.");
                    data.Add("Saint"); data.Add("sprites/Units/Common/Sacrificer");
                    break;
                case "dollhugging_girl":
                    data.Add(2); data.Add(1); data.Add(1); data.Add(0); data.Add("인형을 안은 소녀");
                    data.Add("<color=green>[출격]</color> 덱의 '전투 인형'이라는 이름 또는 설명이 붙은 카드를 선택하여 뽑습니다.");
                    data.Add(""); data.Add("sprites/Units/Rare/DollhuggingGirl");
                    break;
                case "triarodon_secretary":
                    data.Add(2); data.Add(2); data.Add(2); data.Add(0); data.Add("트리아로돈의 비서");
                    data.Add("<color=green>[출격]</color> 덱의 '탈레스의 강'을 뽑습니다.");
                    data.Add("Water"); data.Add("sprites/Units/Common/TriarodonSecretary");
                    break;
                case "cave_guardian":
                    data.Add(2); data.Add(4); data.Add(2); data.Add(0); data.Add("깊은 동굴의 문지기");
                    data.Add("<color=green>[출격]</color> 손의 카드 1장을 선택하여 버립니다.");
                    data.Add("Dark"); data.Add("sprites/Units/Common/CaveGuardian");
                    break;
                case "Revilla":
                    data.Add(2); data.Add(1); data.Add(2); data.Add(2); data.Add("여전사 레빌라");
                    data.Add("<color=green>[돌진]</color>, <color=green>[공격]</color> 대상 유닛의 생명력만큼 공격력을 얻습니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Legend/Revilla");
                    break;
                case "big_sworder":
                    data.Add(2); data.Add(1); data.Add(2); data.Add(0); data.Add("대검 전사");
                    data.Add("<color=green>[출격]</color> '글라디우스'를 손에 얻습니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Common/BigSworder");
                    break;
                case "student_knight":
                    data.Add(2); data.Add(1); data.Add(2); data.Add(0); data.Add("견습 기사");
                    data.Add("<color=green>[출격]</color> '참격'을 손에 얻습니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Common/StudentKnight");
                    break;
                case "ancient_magician":
                    data.Add(2); data.Add(2); data.Add(2); data.Add(0); data.Add("고대의 마법사");
                    data.Add("<color=green>[출격]</color> 무작위 주문을 손에 얻습니다.");
                    data.Add("Earth/Mage"); data.Add("sprites/Units/Common/AncientMagician");
                    break;
                case "dimension_summoner":
                    data.Add(2); data.Add(2); data.Add(2); data.Add(0); data.Add("차원의 소환사");
                    data.Add("<color=green>[출격]</color> 무작위 유닛을 손에 얻습니다.");
                    data.Add("Dimension/Mage"); data.Add("sprites/Units/Common/DimensionSummoner");
                    break;
                case "volcanic_monster":
                    data.Add(2); data.Add(2); data.Add(1); data.Add(0); data.Add("분화구의 괴수");
                    data.Add("<color=green>[출격]</color> 불 종족 유닛을 덱에서 선택하여 뽑고, 그 비용만큼 내 플레이어에게 피해를 줍니다.");
                    data.Add("Fire/Devil"); data.Add("sprites/Units/Common/VolcanicMonster");
                    break;
                case "lava_swarm":
                    data.Add(2); data.Add(0); data.Add(2); data.Add(1); data.Add("용암 지룡");
                    data.Add("<color=green>[출격]</color> 내 '용암룡'이 있다면, '용암룡'으로 변신하고 그 효과를 발동시킵니다.");
                    data.Add("Fire/Dragon"); data.Add("sprites/Units/Rare/LavaSwarm");
                    break;
                case "tina":
                    data.Add(2); data.Add(1); data.Add(2); data.Add(0); data.Add("음속의 요정 티나");
                    data.Add("<color=green>[돌진]</color>, <color=green>[공격]</color> 공격 대상 유닛의 공격력이 5 이상이라면, 전투에 의한 피해를 0으로 합니다.");
                    data.Add("Wind"); data.Add("sprites/Units/Common/Tina");
                    break;
                case "sacrificed_ghost":
                    data.Add(2); data.Add(2); data.Add(1); data.Add(0); data.Add("의식에 희생한 귀신");
                    data.Add("<color=green>[출격]</color> 의식 카운트를 2 얻습니다.");
                    data.Add("Dark"); data.Add("sprites/Units/Common/SacrificedGhost");
                    break;
                case "neprafti":
                    data.Add(2); data.Add(2); data.Add(1); data.Add(0); data.Add("네프라프티");
                    data.Add("<color=green>[출격]</color> 덱에서 카드 3장을 넘겨, 그 중 1장을 선택하여 손에 넣고," +
                        " 남은 카드는 <color=red>모두</color> 버립니다.");
                    data.Add("Dark/Devil"); data.Add("sprites/Units/Common/Neprafti");
                    break;
                case "animal_lover":
                    data.Add(2); data.Add(2); data.Add(2); data.Add(1); data.Add("동물 애호가");
                    data.Add("<color=green>[출격]</color> 내 <color=red>모든</color> 야수에게 공격력을 1 부여합니다.");
                    data.Add(""); data.Add("sprites/Units/Common/AnimalLover");
                    break;
                case "corpse_raven":
                    data.Add(2); data.Add(3); data.Add(2); data.Add(1); data.Add("시체 까마귀");
                    data.Add("<color=green>[능력]</color> 내 야수 하나를 처치하고 카드를 1장 뽑습니다.");
                    data.Add("Beast"); data.Add("sprites/Units/Rare/CorpseRaven");
                    break;
                case "tough_shieldman":
                    data.Add(3); data.Add(1); data.Add(4); data.Add(0); data.Add("완고한 방패병");
                    data.Add("<color=green>[출격]</color> '티타늄 방패'를 손에 얻습니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Common/ToughShieldman");
                    break;
                case "cursed_mage":
                    data.Add(3); data.Add(3); data.Add(5); data.Add(0); data.Add("저주받은 마법사");
                    data.Add("<color=green>[사망]</color> 내 <color=red>모든</color> 유닛에게 피해를 2 줍니다.");
                    data.Add("Dark/Mage"); data.Add("sprites/Units/Common/CursedMage");
                    break;
                case "synthesizer":
                    data.Add(3); data.Add(2); data.Add(2); data.Add(0); data.Add("페인 합성술사");
                    data.Add("이 유닛은 합성 소환의 소재로 사용 가능합니다.");
                    data.Add("Dimension"); data.Add("sprites/Units/Common/Synthesizer");
                    break;
                case "sacrifice_summoner":
                    data.Add(3); data.Add(3); data.Add(4); data.Add(1); data.Add("의식소환사");
                    data.Add("<color=green>[공격]</color> <color=blue>의식 카운트</color>를 2 얻습니다.");
                    data.Add("Saint/Mage"); data.Add("sprites/Units/Rare/SacrificeSummoner");
                    break;
                case "slum_dancer":
                    data.Add(3); data.Add(2); data.Add(4); data.Add(0); data.Add("슬럼가의 댄서");
                    data.Add("<color=green>[능력]</color> 유닛 하나의 공격력을 2 감소시킵니다.\n0 미만으로 내려가지 않습니다.");
                    data.Add(""); data.Add("sprites/Units/Common/SlumDancer");
                    break;
                case "wind_walker":
                    data.Add(3); data.Add(4); data.Add(3); data.Add(0); data.Add("바람을 걷는 자");
                    data.Add("<color=green>[전투]</color> 7 이상의 피해를 입지 않습니다.");
                    data.Add("Wind"); data.Add("sprites/Units/Common/WindWalker");
                    break;
                case "wave_fighter":
                    data.Add(3); data.Add(4); data.Add(3); data.Add(0); data.Add("파동 격투가");
                    data.Add("<color=green>[출격]</color> 적 플레이어에게 피해를 1 줍니다.");
                    data.Add("Shock/Warrior"); data.Add("sprites/Units/Common/WaveFighter");
                    break;
                case "duret":
                    data.Add(3); data.Add(0); data.Add(7); data.Add(1); data.Add("종말을 부르는 듀렛");
                    data.Add("<color=green>[성장(5)]</color> <color=red>모든</color> 유닛을 처치합니다.");
                    data.Add("Dimension"); data.Add("sprites/Units/Rare/Duret");
                    break;
                case "alcadrobot_typeA":
                    data.Add(3); data.Add(1); data.Add(1); data.Add(1); data.Add("알카드로봇 A타입");
                    data.Add("<color=green>[출격]</color> 덱에서 비용 3 유닛을 선택하여 뽑습니다.");
                    data.Add(""); data.Add("sprites/Units/Rare/AlcadrobotTypeA");
                    break;
                case "lary":
                    data.Add(3); data.Add(3); data.Add(4); data.Add(0); data.Add("인형술사 래리");
                    data.Add("<color=green>[출격]</color> 덱에 '전투 인형' 3장을 섞어넣습니다.");
                    data.Add("Mage"); data.Add("sprites/Units/Rare/Lary");
                    break;
                case "fire_femalesworder":
                    data.Add(3); data.Add(2); data.Add(3); data.Add(0); data.Add("화염의 여전사");
                    data.Add("<color=green>[출격 / 환경 : '용암 분화구']</color> +2/+1을 얻습니다.");
                    data.Add("Fire/Warrior"); data.Add("sprites/Units/Common/FireFemalesworder");
                    break;
                case "wind_rider":
                    data.Add(3); data.Add(1); data.Add(3); data.Add(1); data.Add("바람 기수");
                    data.Add("<color=green>[돌진]</color>, <color=green>[공격]</color> 대상 유닛의 공격력이 7 이상이라면, 그 유닛을 처치합니다.");
                    data.Add("Wind/Warrior"); data.Add("sprites/Units/Rare/WindRider");
                    break;
                case "admired_bishop":
                    data.Add(3); data.Add(2); data.Add(3); data.Add(0); data.Add("존경받는 주교");
                    data.Add("<color=green>[능력]</color>, 적 유닛 하나를 지정하여, 그 유닛과 동일한 유닛을 손에 얻습니다.");
                    data.Add("Saint/Mage"); data.Add("sprites/Units/Common/AdmiredBishop");
                    break;
                case "afterlife_guide":
                    data.Add(3); data.Add(3); data.Add(3); data.Add(0); data.Add("저승의 인도자");
                    data.Add("<color=green>[사망]</color> 이 유닛을 복사하여 처치합니다.");
                    data.Add("Dark"); data.Add("sprites/Units/Common/AfterlifeGuide");
                    break;
                case "Toxie":
                    data.Add(3); data.Add(2); data.Add(3); data.Add(2); data.Add("역병술사 톡시");
                    data.Add("<color=green>[능력]</color> 생명력 2 이하의 유닛 하나를 '톡시-T 파편'으로 변신시킵니다.");
                    data.Add("Dark/Mage"); data.Add("sprites/Units/Legend/Toxie");
                    break;
                case "Devil-T_host":
                    data.Add(3); data.Add(7); data.Add(7); data.Add(2); data.Add("톡시-T 파편");
                    data.Add("[성장(0)] 내 플레이어에게 피해를 4 줍니다.");
                    data.Add("Dark"); data.Add("sprites/Units/Legend/ToxieHost");
                    break;
                case "dragon_tamer":
                    data.Add(3); data.Add(1); data.Add(3); data.Add(0); data.Add("용을 다루는 법사");
                    data.Add("<color=green>[출격]</color> 내 모든 용 유닛에게 +1/+1을 부여합니다.");
                    data.Add("Mage"); data.Add("sprites/Units/Common/DragonTamer");
                    break;
                case "Volcanic_twindragon":
                    data.Add(2); data.Add(3); data.Add(5); data.Add(2); data.Add("용암쌍룡");
                    data.Add("<color=#8800ea>[제물 : 용암룡 2체]</color>\n<color=green>[돌진]</color>, " +
                        "<color=green>[공격]</color> 이 유닛의 공격력이 대상 유닛의 생명력 이상이라면, +1/+1과 함께 공격 기회를 얻습니다.");
                    data.Add("Fire/Dragon"); data.Add("sprites/Units/Legend/VolcanicTwindragon");
                    break;
                case "waterdragon_priest":
                    data.Add(3); data.Add(2); data.Add(1); data.Add(1); data.Add("수룡을 섬기는 사제");
                    data.Add("<color=green>[능력]</color> 내 물 종족 유닛 하나를 처치하고 카드를 2장 뽑습니다.");
                    data.Add("Mage"); data.Add("sprites/Units/Rare/WaterdragonPriest");
                    break;
                case "little_mermaid":
                    data.Add(3); data.Add(1); data.Add(4); data.Add(0); data.Add("작은 인어");
                    data.Add("<color=green>[출격 / 환경 : '탈레스의 강']</color> 공격력을 1 얻습니다." +
                        "<color=green>[능력]</color> 내 물 종족 유닛 하나에게 +1/+1을 부여합니다.");
                    data.Add("Water"); data.Add("sprites/Units/Common/LittleMermaid");
                    break;
                case "waterdragon_son":
                    data.Add(3); data.Add(2); data.Add(3); data.Add(1); data.Add("수룡신의 아들");
                    data.Add("<color=green>[출격]</color> 덱에서 '수룡신 트리아로돈'을 뽑습니다.\n" +
                        "만약 '수룡신 트리아로돈'이 사망한 상태라면, 대신 유계의 '수룡신 트리아로돈'을 손으로 가져오고, 이 유닛을 처치합니다.");
                    data.Add("Water"); data.Add("sprites/Units/Rare/WaterdragonSon");
                    break;
                case "attack_eater":
                    data.Add(3); data.Add(2); data.Add(3); data.Add(1); data.Add("공격력을 먹는 비룡");
                    data.Add("<color=green>[능력]</color> 이 유닛을 제외한 바람 종족 유닛이 있다면, 유닛 하나와 공격력을 바꿉니다.");
                    data.Add("Wind/Dragon"); data.Add("sprites/Units/Rare/AttackEater");
                    break;
                case "ground_priest":
                    data.Add(3); data.Add(1); data.Add(4); data.Add(0); data.Add("대지의 사제");
                    data.Add("<color=green>[출격]</color> 땅 종족 유닛 하나를 덱에서 무작위로 뽑습니다.");
                    data.Add("Earth/Mage"); data.Add("sprites/Units/Common/GroundPriest");
                    break;
                case "ground_follower":
                    data.Add(3); data.Add(2); data.Add(2); data.Add(1); data.Add("대지의 추종자");
                    data.Add("<color=green>[능력]</color> 내 땅 종족 유닛 하나의 공격력을 생명력과 같게 설정합니다.");
                    data.Add("Earth"); data.Add("sprites/Units/Rare/GroundFollower");
                    break;
                case "raysen_samurai":
                    data.Add(3); data.Add(3); data.Add(4); data.Add(0); data.Add("레이센 무사");
                    data.Add("<color=green>[개방]</color> 테리얼을 2 얻습니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Common/RaysenSamurai");
                    break;
                case "mild_bear":
                    data.Add(3); data.Add(2); data.Add(2); data.Add(1); data.Add("온순한 곰");
                    data.Add("<color=green>[출격]</color> 덱에서 비용 3 이하의 야수 유닛을 선택하여 뽑습니다.");
                    data.Add("Beast"); data.Add("sprites/Units/Common/MildBear");
                    break;
                case "whitefur_fox":
                    data.Add(3); data.Add(3); data.Add(2); data.Add(0); data.Add("백발 여우");
                    data.Add("<color=green>[능력]</color> 내 야수 하나에게 +1/+1을 부여합니다.");
                    data.Add("Beast"); data.Add("sprites/Units/Common/WhitefurFox");
                    break;
                case "soulful_icefox":
                    data.Add(3); data.Add(3); data.Add(3); data.Add(1); data.Add("묘혼 눈꽃여우");
                    data.Add("유계의 이 카드를 사용하여 제외하는 것으로, 유계의 '작은 고양이'를 <color=red>모두</color>" +
                        " 제외하고, 그 수만큼 테리얼을 얻습니다.");
                    data.Add("Water/Beast"); data.Add("sprites/Units/Rare/SoulfulIcefox");
                    break;
                case "little_lion":
                    data.Add(3); data.Add(3); data.Add(2); data.Add(0); data.Add("작은 사자");
                    data.Add("<color=green>[돌진]</color>");
                    data.Add("Beast"); data.Add("sprites/Units/Common/LittleLion");
                    break;
                case "Nyan":
                    data.Add(3); data.Add(1); data.Add(1); data.Add(2); data.Add("냥냥이");
                    data.Add("<color=green>[출격]</color> 내 <color=red>모든</color> 야수가 공격 기회를 얻습니다.");
                    data.Add("Beast"); data.Add("sprites/Units/Legend/Nyan");
                    break;
                case "crystal_fighter":
                    data.Add(4); data.Add(3); data.Add(5); data.Add(0); data.Add("수정의 격투가");
                    data.Add("<color=green>[성장(1)]</color> +1/+1을 얻습니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Common/CrystalFighter");
                    break;
                case "steel_sworder":
                    data.Add(4); data.Add(4); data.Add(3); data.Add(0); data.Add("강철의 검사");
                    data.Add("<color=green>[능력]</color> 유닛 하나에게 피해를 2 줍니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Common/SteelSworder");
                    break;
                case "oldbook_analyzer":
                    data.Add(4); data.Add(4); data.Add(4); data.Add(0); data.Add("고서 분석가");
                    data.Add("<color=green>[출격]</color> 카드를 1장 뽑습니다.");
                    data.Add("Earth"); data.Add("sprites/Units/Common/OldbookAnalyzer");
                    break;
                case "fire_mage":
                    data.Add(4); data.Add(5); data.Add(4); data.Add(0); data.Add("화염을 다루는 법사");
                    data.Add("<color=green>[출격]</color> 이 유닛을 제외한 <color=red>모든</color> 유닛에게 피해를 1 줍니다.");
                    data.Add("Fire/Mage"); data.Add("sprites/Units/Common/FireMage");
                    break;
                case "nature_mage":
                    data.Add(4); data.Add(2); data.Add(6); data.Add(0); data.Add("자연을 노래하는 법사");
                    data.Add("<color=green>[출격]</color> 내 플레이어의 생명력을 3 회복합니다.");
                    data.Add("Earth/Mage"); data.Add("sprites/Units/Common/NatureMage");
                    break;
                case "carsus_priest":
                    data.Add(4); data.Add(5); data.Add(4); data.Add(0); data.Add("카서스 결정법사");
                    data.Add("<color=green>[사망]</color> <color=red>모든</color> 내 유닛이 공격력을 1 얻습니다.");
                    data.Add("Water/Mage"); data.Add("sprites/Units/Common/CarsusPriest");
                    break;
                case "mancy":
                    data.Add(4); data.Add(3); data.Add(5); data.Add(1); data.Add("달빛 아래의 맨시");
                    data.Add("<color=green>[공격]</color> 공격력을 2 얻습니다.");
                    data.Add("Saint/Warrior"); data.Add("sprites/Units/Rare/Mancy");
                    break;
                case "rein":
                    data.Add(4); data.Add(3); data.Add(4); data.Add(1); data.Add("댄서 레인");
                    data.Add("<color=green>[능력]</color> 유닛 하나의 공격력을 1로 설정합니다.");
                    data.Add(""); data.Add("sprites/Units/Rare/Rein");
                    break;
                case "necky":
                    data.Add(4); data.Add(2); data.Add(4); data.Add(1); data.Add("쾌속질주 네키");
                    data.Add("<color=green>[돌진]</color>, <color=green>[전투]</color> 4 이상의 피해는 그 수치가 3이 됩니다.");
                    data.Add("Wind"); data.Add("sprites/Units/Rare/Necky");
                    break;
                case "ken":
                    data.Add(4); data.Add(5); data.Add(3); data.Add(1); data.Add("일격파동 켄");
                    data.Add("<color=green>[출격]</color> 적 플레이어에게 피해를 2 줍니다.");
                    data.Add("Shock"); data.Add("sprites/Units/Rare/Ken");
                    break;
                case "alcadrobot_typeB":
                    data.Add(4); data.Add(2); data.Add(2); data.Add(1); data.Add("알카드로봇 B타입");
                    data.Add("<color=green>[출격]</color> 덱에서 비용 4 유닛을 선택하여 뽑습니다.");
                    data.Add(""); data.Add("sprites/Units/Rare/AlcadrobotTypeB");
                    break;
                case "tetra":
                    data.Add(4); data.Add(2); data.Add(3); data.Add(1); data.Add("인형기사 테트라");
                    data.Add("<color=green>[돌진]</color>, <color=green>[출격]</color> 내 '전투 인형'의 수만큼 공격력을 얻습니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Rare/Tetra");
                    break;
                case "battlefield_commander":
                    data.Add(4); data.Add(2); data.Add(3); data.Add(0); data.Add("전장의 지휘관");
                    data.Add("<color=green>[출격]</color> '견습 기사'와 '견습 검사'를 손에 얻습니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Common/BattlefieldCommander");
                    break;
                case "sirai":
                    data.Add(4); data.Add(3); data.Add(5); data.Add(1); data.Add("폭풍칼날 시라이");
                    data.Add("<color=green>[출격]</color> <color=red>모든</color> 적 유닛에게 피해를 1 줍니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Rare/Sirai");
                    break;
                case "violet":
                    data.Add(4); data.Add(3); data.Add(5); data.Add(1); data.Add("알쏭달쏭 바이올렛");
                    data.Add("<color=green>[출격]</color> 덱에 '대마법사 바이올렛' 1장을 섞어넣습니다.");
                    data.Add("Mage"); data.Add("sprites/Units/Rare/Violet");
                    break;
                case "Archmage_violet":
                    data.Add(4); data.Add(3); data.Add(5); data.Add(2); data.Add("대마법사 바이올렛");
                    data.Add("<color=green>[출격]</color> 손과 덱에 있는 <color=red>모든</color> 카드를 비용 3 이상의 무작위 유닛으로 바꿉니다." +
                        " '대마법사의 진'을 손에 얻습니다.");
                    data.Add("Mage"); data.Add("sprites/Units/Legend/ArchmageViolet");
                    break;
                case "little_firedragon":
                    data.Add(4); data.Add(2); data.Add(4); data.Add(0); data.Add("작은 염룡");
                    data.Add("<color=green>[출격 / 환경 : '용암 분화구']</color> 공격력을 +4 얻습니다.");
                    data.Add("Fire/Dragon"); data.Add("sprites/Units/Common/LittleFiredragon");
                    break;
                case "waterpalace_guardian":
                    data.Add(4); data.Add(3); data.Add(3); data.Add(0); data.Add("수궁의 수호병");
                    data.Add("<color=green>[출격 / 환경 : '탈레스의 강']</color> 중첩 수만큼 +1/+1을 얻습니다.");
                    data.Add("Water/Warrior"); data.Add("sprites/Units/Common/WaterpalaceGuardian");
                    break;
                case "tornado_hero":
                    data.Add(4); data.Add(3); data.Add(2); data.Add(1); data.Add("태풍을 부르는 용사");
                    data.Add("<color=green>[출격]</color> 이 유닛을 제외한 바람 종족 유닛이 있다면, 공격력 2 이하의 <color=red>모든</color> 유닛을 처치합니다.");
                    data.Add("Wind/Warrior"); data.Add("sprites/Units/Rare/TornadoHero");
                    break;
                case "ground_sentinel":
                    data.Add(4); data.Add(4); data.Add(5); data.Add(0); data.Add("대지의 파수병");
                    data.Add("이 유닛이 전장에 있는 한, 내 플레이어는 전투에 의한 피해를 입지 않습니다.");
                    data.Add("Earth/Warrior"); data.Add("sprites/Units/Common/GroundSentinel");
                    break;
                case "mern_chief":
                    data.Add(4); data.Add(3); data.Add(4); data.Add(0); data.Add("메른 부족장");
                    data.Add("<color=green>[출격]</color> '지진' 카드를 손에 얻습니다.");
                    data.Add("Earth"); data.Add("sprites/Units/Common/MernChief");
                    break;
                case "sacrifice_socerer":
                    data.Add(4); data.Add(3); data.Add(6); data.Add(0); data.Add("의식 환술사");
                    data.Add("<color=green>[출격]</color> 내 신성 종족 유닛의 수만큼 <color=blue>의식 카운트</color>를 1 얻습니다.");
                    data.Add("Saint/Mage"); data.Add("sprites/Units/Common/SacrificeSocerer");
                    break;
                case "grail_maker":
                    data.Add(4); data.Add(3); data.Add(3); data.Add(1); data.Add("성배 제작사");
                    data.Add("<color=green>[출격]</color> 내 신성 종족 유닛의 수만큼 '광휘의 성배'를 손에 얻습니다.");
                    data.Add("Saint"); data.Add("sprites/Units/Rare/GrailMaker");
                    break;
                case "jack-o-lantern":
                    data.Add(4); data.Add(5); data.Add(4); data.Add(1); data.Add("잭오랜턴");
                    data.Add("<color=green>[출격]</color> 내 플레이어에게 피해를 4 줍니다. <color=blue>의식 카운트</color>를 4 얻습니다.");
                    data.Add("Dark"); data.Add("sprites/Units/Rare/JackOLantern");
                    break;
                case "longlife_turtle":
                    data.Add(4); data.Add(2); data.Add(5); data.Add(0); data.Add("천년을 사는 거북");
                    data.Add("<color=green>[성장(1)]</color> 생명력을 모두 회복합니다.");
                    data.Add("Water/Beast"); data.Add("sprites/Units/Common/LonglifeTurtle");
                    break;
                case "Cardinel":
                    data.Add(4); data.Add(1); data.Add(4); data.Add(2); data.Add("광란정원사 카디넬");
                    data.Add("<color=green>[출격]</color> '광란꽃 베라' 2장을 손에 얻습니다.");
                    data.Add("Earth"); data.Add("sprites/Units/Legend/Cardinel");
                    break;
                case "Vera":
                    data.Add(1); data.Add(2); data.Add(1); data.Add(2); data.Add("광란꽃 베라");
                    data.Add("<color=green>[개방]</color> 적 플레이어에게 피해를 3 줍니다.");
                    data.Add("Earth"); data.Add("sprites/Units/Legend/Vera");
                    break;
                case "Sytron":
                    data.Add(4); data.Add(3); data.Add(5); data.Add(2); data.Add("빛의 사제 시트론");
                    data.Add("<color=#8800ea>[조건 : 덱에 겹치는 카드 없음]</color>\n" +
                        "<color=green>[출격]</color> 내 플레이어의 생명력을 전부 회복합니다.");
                    data.Add("Saint/Mage"); data.Add("sprites/Units/Legend/Sytron");
                    break;
                case "Sardis":
                    data.Add(4); data.Add(3); data.Add(5); data.Add(2); data.Add("빛의 사제 사르디스");
                    data.Add("<color=#8800ea>[조건 : 덱에 겹치는 카드 없음]</color>\n" +
                        "<color=green>[출격]</color> 내 플레이어의 최대 생명력이 15 증가합니다.");
                    data.Add("Saint/Mage"); data.Add("sprites/Units/Legend/Sardis");
                    break;
                case "heavyarmor_knight":
                    data.Add(5); data.Add(4); data.Add(7); data.Add(0); data.Add("중장갑 기사");
                    data.Add("");
                    data.Add("Warrior"); data.Add("sprites/Units/Common/HeavyarmorKnight");
                    break;
                case "penitent_devil":
                    data.Add(5); data.Add(3); data.Add(7); data.Add(1); data.Add("참회하는 악마");
                    data.Add("<color=green>[출격]</color> 이 유닛을 제외한 내 유닛의 수만큼 생명력을 잃습니다.\n이 유닛을 제외한 <color=red>모든</color> 내 유닛에게 +1/+1을 부여합니다.");
                    data.Add("Saint/Devil"); data.Add("sprites/Units/Rare/PenitentDevil");
                    break;
                case "wishful_bishop":
                    data.Add(5); data.Add(4); data.Add(6); data.Add(1); data.Add("염원하는 주교");
                    data.Add("<color=green>[출격]</color> <color=blue>의식 카운트</color>를 2 얻습니다.");
                    data.Add("Saint/Mage"); data.Add("sprites/Units/Rare/WishfulBishop");
                    break;
                case "axe_zealot":
                    data.Add(5); data.Add(5); data.Add(6); data.Add(0); data.Add("도끼를 든 광전사");
                    data.Add("<color=green>[공격]</color> 이 유닛을 제외한 <color=red>모든</color> 유닛에게 피해를 1 줍니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Common/AxeZealot");
                    break;
                case "dark_baron":
                    data.Add(5); data.Add(4); data.Add(2); data.Add(1); data.Add("검은 성의 남작");
                    data.Add("<color=green>[능력]</color> 대상 유닛의 생명력만큼의 피해를 내 플레이어에게 줍니다.\n이후, 대상 유닛을 처치합니다.");
                    data.Add("Dark/Devil"); data.Add("sprites/Units/Rare/DarkBaron");
                    break;
                case "siera":
                    data.Add(5); data.Add(6); data.Add(4); data.Add(1); data.Add("총잡이 시에라");
                    data.Add("<color=green>[원격]</color>, <color=green>[공격]</color> 전투로 인한 피해를 0으로 합니다.");
                    data.Add("Archer"); data.Add("sprites/Units/Rare/Siera");
                    break;
                case "mitero":
                    data.Add(5); data.Add(3); data.Add(3); data.Add(1); data.Add("환류법사 미테로");
                    data.Add("<color=green>[능력]</color> 내 바람 종족 유닛 하나를 처치하고, 공격력이 5 이상인 <color=red>모든</color> 적 유닛을 처치합니다.");
                    data.Add("Mage"); data.Add("sprites/Units/Rare/Mitero");
                    break;
                case "ancient_hero":
                    data.Add(5); data.Add(5); data.Add(7); data.Add(1); data.Add("고대의 영웅");
                    data.Add("이 유닛이 전장에 있는 한, 내 플레이어는 전투에 의한 피해를 입지 않습니다.");
                    data.Add("Earth/Warrior"); data.Add("sprites/Units/Rare/AncientHero");
                    break;
                case "expert_healer":
                    data.Add(5); data.Add(4); data.Add(5); data.Add(0); data.Add("숙련된 회복사");
                    data.Add("<color=green>[출격]</color> 내 플레이어의 생명력을 5 회복합니다.");
                    data.Add("Saint/Mage"); data.Add("sprites/Units/Common/ExpertHealer");
                    break;
                case "lion_tamer":
                    data.Add(5); data.Add(5); data.Add(4); data.Add(1); data.Add("사육사 라이언");
                    data.Add("<color=green>[출격]</color> 라이언이 기르는 야수를 선택하여 손에 얻습니다.");
                    data.Add(""); data.Add("sprites/Units/Rare/LionTamer");
                    break;
                case "petree":
                    data.Add(2); data.Add(4); data.Add(3); data.Add(1); data.Add("고양이 페트리");
                    data.Add("");
                    data.Add("Beast"); data.Add("sprites/Units/Common/Kitten");
                    break;
                case "juen":
                    data.Add(3); data.Add(2); data.Add(2); data.Add(1); data.Add("백여우 쥬엔");
                    data.Add("<color=green>[능력]</color> 내 야수 하나에게 공격 기회를 부여합니다.");
                    data.Add("Beast"); data.Add("sprites/Units/Common/WhitefurFox");
                    break;
                case "besto":
                    data.Add(4); data.Add(4); data.Add(2); data.Add(1); data.Add("사자 베스토");
                    data.Add("<color=green>[돌진]</color>");
                    data.Add("Beast"); data.Add("sprites/Units/Common/LittleLion");
                    break;
                case "Falcifer":
                    data.Add(5); data.Add(3); data.Add(1); data.Add(2); data.Add("마계기사 팔키페르");
                    data.Add("<color=green>[출격]</color> 손의 <color=red>모든</color> 카드를 버리고, 버린 유닛의 수만큼 생명력을 얻습니다." +
                        "\n<color=green>[공격]</color> 이 유닛이 피해를 2 입습니다. 이 효과는 플레이어를 공격할 때에도 적용됩니다.\n<color=green>이 유닛은 무한히 공격할 수 있습니다.</color>");
                    data.Add("Dark/Warrior"); data.Add("sprites/Units/Legend/Falcifer");
                    break;
                case "Sein":
                    data.Add(5); data.Add(3); data.Add(5); data.Add(2); data.Add("인형사수 세인");
                    data.Add("<color=green>[원격]</color>, <color=green>[출격]</color> '인형 집결' 카드를 1장 손에 얻습니다. " +
                        "<color=green>[능력]</color> 패의 '전투 인형' 하나를 덱에 섞어넣는 것으로, 유닛 하나에게 피해를 2 줍니다.\n" +
                        "<color=green>이 능력은 무한정 사용할 수 있습니다.</color>");
                    data.Add("Archer"); data.Add("sprites/Units/Legend/Sein");
                    break;
                case "Necropia":
                    data.Add(5); data.Add(4); data.Add(5); data.Add(2); data.Add("암룡 네크로피아");
                    data.Add("<color=green>[성장(0)]</color> 덱의 카드 하나를 무작위로 파괴하고 <color=red>모든</color> 적 유닛에게 피해를 2 줍니다." +
                        " 유계의 이 카드를 사용하여 제외하는 것으로, '사룡 네크로피아'를 손에 넣습니다.");
                    data.Add("Dark/Dragon"); data.Add("sprites/Units/Legend/Necropia");
                    break;
                case "Dead_Necropia":
                    data.Add(2); data.Add(0); data.Add(2); data.Add(2); data.Add("사룡 네크로피아");
                    data.Add("<color=#8800ea>[제물 : 유계의 흑암 유닛 2체] </color>\n" +
                        "<color=green>[출격]</color> '암룡 네크로피아'로 변신합니다.");
                    data.Add("Dark/Dragon"); data.Add("sprites/Units/Legend/DeadNecropia");
                    break;
                case "Tenebera":
                    data.Add(5); data.Add(3); data.Add(6); data.Add(2); data.Add("소멸의 테네벨라");
                    data.Add("<color=#8800ea>[제물 : 유계의 흑암 유닛 3체] </color>\n" +
                        "<color=green>[능력]</color> 유닛 하나를 제외합니다.\n");
                    data.Add("Dark"); data.Add("sprites/Units/Legend/Tenebera");
                    break;
                case "Pyrena":
                    data.Add(5); data.Add(5); data.Add(4); data.Add(2); data.Add("네크로슈터 파이레나");
                    data.Add("<color=#8800ea>[제물 : 유계의 흑암 유닛 3체] </color>\n" +
                        "<color=green>[출격]</color> '파이렌 좀비' 2장을 손에 얻습니다.");
                    data.Add("Dark/Archer"); data.Add("sprites/Units/Legend/Pyrena");
                    break;
                case "mirat":
                    data.Add(6); data.Add(3); data.Add(6); data.Add(1); data.Add("인형사 미라트");
                    data.Add("<color=green>[출격]</color> '전투 인형' 카드 2장을 손에 얻습니다.\n[공격] 내 <color=red>모든</color> '전투 인형'에게 +1/+1을 부여합니다.");
                    data.Add("Mage"); data.Add("sprites/Units/Common/Mirat");
                    break;
                case "nature_musician":
                    data.Add(6); data.Add(3); data.Add(7); data.Add(1); data.Add("대자연의 음유시인");
                    data.Add("<color=green>[성장(1)]</color> '고대의 영웅' 1장을 손에 얻습니다.");
                    data.Add("Earth"); data.Add("sprites/Units/Rare/NatureMusician");
                    break;
                case "investigator_pein":
                    data.Add(6); data.Add(4); data.Add(4); data.Add(1); data.Add("불사의 탐구자 페인");
                    data.Add("<color=green>[전투]</color> 피해를 입지 않습니다.");
                    data.Add("Dimension/God"); data.Add("sprites/Units/Rare/InvestigatorPein");
                    break;
                case "burying_socerer":
                    data.Add(6); data.Add(3); data.Add(4); data.Add(1); data.Add("매장의 환술사");
                    data.Add("<color=#8800ea>[제물 : 신성 종족 유닛 1체 이상] </color>\n" +
                        "<color=green>[능력]</color> 유닛 하나를 지정하여, 그 유닛을 제외하고 동일한 유닛을 손에 얻습니다.");
                    data.Add("Saint/Mage"); data.Add("sprites/Units/Rare/BuryingSocerer");
                    break;
                case "dark_mage":
                    data.Add(6); data.Add(5); data.Add(4); data.Add(0); data.Add("흑암을 다루는 법사");
                    data.Add("<color=green>[능력]</color> 유닛 하나에게 피해를 4 줍니다.");
                    data.Add("Dark/Mage"); data.Add("sprites/Units/Common/DarkMage");
                    break;
                case "tigor":
                    data.Add(6); data.Add(5); data.Add(4); data.Add(1); data.Add("맹수대장 호랑");
                    data.Add("<color=green>[능력]</color> 내 야수 하나에게 +3/+3을 부여합니다.\n" +
                        "<color=green>[성장(0)]</color> 내 <color=red>모든</color> 야수에게 공격력을 1 부여합니다.");
                    data.Add("Beast"); data.Add("sprites/Units/Rare/Tigor");
                    break;
                case "Keres":
                    data.Add(6); data.Add(6); data.Add(6); data.Add(2); data.Add("시간마법사 케레스");
                    data.Add("이 카드를 유계에서 사용하여 제외하는 것으로, '시간마법사 케레스'를 손에 얻습니다.");
                    data.Add("Dimension/Mage"); data.Add("sprites/Units/Legend/Keres");
                    break;
                case "Hexator":
                    data.Add(6); data.Add(6); data.Add(6); data.Add(2); data.Add("구속된 헥사토르");
                    data.Add("<color=green>[출격]</color> 내 플레이어의 생명력이 6 이하라면,\n<color=green>[돌진]</color>을 얻습니다.\n적 플레이어의 생명력이 6 이하라면,\n" +
                    "+3/+3을 얻습니다.");
                    data.Add("Dragon"); data.Add("sprites/Units/Legend/Hexator");
                    break;
                case "Resia":
                    data.Add(6); data.Add(6); data.Add(6); data.Add(2); data.Add("광채의 레시아");
                    data.Add("");
                    data.Add("Saint/Mage"); data.Add("sprites/Units/Legend/Resia");
                    break;
                case "knowledge_omniscence":
                    data.Add(7); data.Add(5); data.Add(5); data.Add(0); data.Add("지식의 선구자");
                    data.Add("<color=green>[출격]</color> 카드를 2장 뽑습니다.");
                    data.Add(""); data.Add("sprites/Units/Common/KnowledgeOmniscence");
                    break;
                case "Triarodon":
                    data.Add(7); data.Add(8); data.Add(8); data.Add(2); data.Add("수룡신 트리아로돈");
                    data.Add("<color=green>[출격 / 환경 : '탈레스의 강']</color> 중첩 수의 2배만큼 물 종족 유닛 이외의 <color=red>모든</color> 유닛에게 피해를 줍니다.");
                    data.Add("Water/God"); data.Add("sprites/Units/Legend/Triarodon");
                    break;
                case "Teradon":
                    data.Add(7); data.Add(7); data.Add(7); data.Add(2); data.Add("토룡 테라돈");
                    data.Add("<color=green>[성장(0)]</color> +2/+2를 얻습니다.\n이 유닛이 전장에 있는 한, 내 플레이어는 전투에 의한 피해를 입지 않습니다.");
                    data.Add("Earth/Dragon"); data.Add("sprites/Units/Legend/Teradon");
                    break;
                case "Keiro":
                    data.Add(7); data.Add(4); data.Add(7); data.Add(2); data.Add("풍신 케이로");
                    data.Add("<color=green>[돌진]</color>, <color=green>[공격]</color> 이 유닛의 공격력이 대상 유닛의 생명력 이상이면, 그 유닛의 공격력만큼의 피해를 적 플레이어에게 줍니다.");
                    data.Add("Wind/God"); data.Add("sprites/Units/Legend/Keiro");
                    break;
                case "Angel_resia":
                    data.Add(7); data.Add(4); data.Add(12); data.Add(2); data.Add("대천사 레시아");
                    data.Add("내 플레이어의 생명력이 회복될 때마다, <color=red>모든</color> 적 캐릭터에게 그 수치만큼 피해를 줍니다.");
                    data.Add("Saint/Mage"); data.Add("sprites/Units/Legend/AngelResia");
                    break;
                case "Devil_resia":
                    data.Add(7); data.Add(12); data.Add(4); data.Add(2); data.Add("타락천사 레시아");
                    data.Add("<color=green>[사망]</color> 덱에서 카드를 4장 버리고, 부활합니다. 만약 버린 카드 중 흑암 종족 유닛이 없다면, 이 유닛을 제외합니다.\n이 유닛은 사망 효과 외의 방법으로 제외되지 않습니다.");
                    data.Add("Dark/Devil"); data.Add("sprites/Units/Legend/DevilResia");
                    break;
                case "PEIN":
                    data.Add(0); data.Add(10); data.Add(10); data.Add(3); data.Add("전지자 페인");
                    data.Add("<color=#8800ea>합성 소환 : 불사의 탐구자 페인 X 2</color>\n" +
                    "<color=green>[출격]</color> 이 유닛을 제외한 <color=red>모든</color> 유닛을 제외합니다.\n<color=green>[전투]</color> 피해를 입지 않습니다.");
                    data.Add("Dimension/God"); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "BAN":
                    data.Add(0); data.Add(6); data.Add(8); data.Add(3); data.Add("페인 반");
                    data.Add("<color=#8800ea>합성 소환 : 반월전사 + 달빛 아래의 맨시</color>\n" +
                    "<color=green>[출격]</color> '반월각'을 손에 얻습니다.");
                    data.Add("Shock/Warrior"); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "PAN":
                    data.Add(0); data.Add(5); data.Add(10); data.Add(3); data.Add("페인 판");
                    data.Add("<color=#8800ea>합성 소환 : 슬럼가의 댄서 + 댄서 레인</color>\n" +
                    "<color=green>[출격]</color> <color=red>모든</color> 적 유닛의 공격력을 0으로 설정합니다.");
                    data.Add(""); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "NATIS":
                    data.Add(0); data.Add(4); data.Add(8); data.Add(3); data.Add("페인 나티스");
                    data.Add("<color=#8800ea>합성 소환 : 바람을 걷는 자 + 쾌속질주 네키</color>\n" +
                    "<color=green>[돌진]</color>, <color=green>[출격]</color> 남은 테리얼을 7로 설정합니다.");
                    data.Add("Wind"); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "ROGER":
                    data.Add(0); data.Add(10); data.Add(5); data.Add(3); data.Add("페인 로저");
                    data.Add("<color=#8800ea>합성 소환 : 파동 격투가 + 일격파동 켄</color>\n" +
                    "<color=green>[출격]</color> 적 플레이어에게 피해를 5 줍니다.");
                    data.Add("Shock"); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "LATIEN":
                    data.Add(0); data.Add(8); data.Add(8); data.Add(3); data.Add("초월자 라티엔");
                    data.Add("<color=#ba009e>중첩 소환 : 비용 6 유닛 2체 이상</color>\n" +
                    "<color=green>[성장(0)]</color> <color=red>모든</color> 적 유닛의 공격력을 4 감소시킵니다. " +
                    "이 때, 공격력이 0 이하로 내려간 유닛을 처치합니다.");
                    data.Add("Saint/God"); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "SENA":
                    data.Add(0); data.Add(3); data.Add(5); data.Add(3); data.Add("세나 라티엔");
                    data.Add("<color=#ba009e>중첩 소환 : 비용 4 유닛 2체 이상</color>\n" +
                    "<color=green>[능력]</color> 손의 카드를 선택하여 '적호 세리아'로 바꾸어 덱에 " +
                    "섞어넣고, 유닛 하나에게 피해를 2 줍니다. <color=green>[공격]</color> 피해를 입지 않습니다.\n" +
                    "<color=green>이 능력은 무한정 사용할 수 있습니다.</color>");
                    data.Add("Fire/Warrior"); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "Seria":
                    data.Add(1); data.Add(2); data.Add(1); data.Add(2); data.Add("적호 세리아");
                    data.Add("<color=green>[돌진]</color>, <color=green>[출격]</color> 카드를 1장 뽑습니다.");
                    data.Add("Fire/Beast"); data.Add("sprites/Units/Legend/Seria");
                    break;
                case "BT":
                    data.Add(0); data.Add(2); data.Add(2); data.Add(3); data.Add("B.T 라티엔");
                    data.Add("<color=#ba009e>중첩 소환 : 유계의 카드 12장 이상</color>\n" +
                    "<color=green>[능력]</color> 공격력이 5 이하인 유닛 하나에게 공격 기회를 부여합니다.");
                    data.Add("Warrior"); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "TONUS":
                    data.Add(0); data.Add(1); data.Add(1); data.Add(3); data.Add("토누스 라티엔");
                    data.Add("<color=#ba009e>중첩 소환 : 덱의 무작위 비용 7 카드 1장</color>\n" +
                    "<color=green>[출격]</color> 덱에서 카드를 선택하여 뽑고, 남은 테리얼을 모두 잃습니다.\n" +
                    "<color=green>이 유닛은 게임 중 1번만 출격할 수 있습니다.</color>");
                    data.Add("Shock/Mage"); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "LICA":
                    data.Add(0); data.Add(1); data.Add(1); data.Add(3); data.Add("리카 라티엔");
                    data.Add("<color=#4b4c00>특수 소환 : 손의 카드 2장 이하</color>\n" +
                        "<color=green>[출격]</color> 덱에서 카드를 2장 뽑습니다.\n" +
                        "<color=green>이 유닛은 게임 중 1번만 출격할 수 있습니다.</color>");
                    data.Add("Archer"); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "MACHINA":
                    data.Add(0); data.Add(0); data.Add(7); data.Add(3); data.Add("알케믹 마키나");
                    data.Add("<color=blue>의식 소환 : 의식 카운트 20 이상</color>\n" +
                    "<color=green>[사망]</color><color=red> 게임에서 승리합니다.</color>\n" +
                    "<color=green>[공격]</color> 피해를 입지 않습니다.");
                    data.Add("Saint/God"); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "EURIEL":
                    data.Add(0); data.Add(13); data.Add(13); data.Add(3); data.Add("알케믹 에우리엘");
                    data.Add("<color=blue>의식 소환 : 의식 카운트 10 이상, 덱의 모든 카드</color>\n" +
                    "<color=#4b4c00>자동 소환 : 내 플레이어의 체력 0</color>\n" +
                    "<color=green>[출격]</color> 내 플레이어의 패배 조건을 '이 유닛이 전장을 벗어날 때'로 설정하고, " +
                    "이 유닛을 제외한 <color=red>모든</color> 유닛을 처치합니다.");
                    data.Add("Water/God"); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "INFINITAS":
                    data.Add(0); data.Add(7); data.Add(7); data.Add(3); data.Add("알케믹 인피니타스");
                    data.Add("<color=blue>의식 소환 : 의식 카운트 10 이상</color>\n" +
                    "<color=green>[출격]</color> 유계의 <color=red>모든</color> 유닛을 덱에 넣고 섞습니다.\n" +
                    "<color=green>이 유닛은 게임 중 1번만 출격할 수 있습니다.</color>");
                    data.Add("Fire/God"); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "SAYRUN":
                    data.Add(0); data.Add(2); data.Add(6); data.Add(3); data.Add("알케믹 세이런");
                    data.Add("<color=blue>의식 소환 : 의식 카운트 5 이상</color>\n" +
                    "<color=green>[성장(0)]</color> 무작위 세이런 악곡을 손에 얻습니다.");
                    data.Add(""); data.Add("sprites/Units/Extra/" + id);
                    break;
                case "HETERO":
                    data.Add(0); data.Add(2); data.Add(2); data.Add(3); data.Add("알케믹 헤테로");
                    data.Add("<color=#4b4c00>특수 소환 : 덱에 겹치는 카드 없음</color>\n" +
                    "<color=green>[출격]</color> 유계의 유닛 하나를 선택하여 손으로 가져옵니다.\n" +
                    "<color=green>이 유닛은 게임 중 1번만 출격할 수 있습니다.</color>");
                    data.Add("Dark/Dragon"); data.Add("sprites/Units/Extra/" + id);
                    break;
                default:
                    break;
            }
        }
        if (cardData.ContainsKey(id)) return cardData[id];
        else return new ArrayList();
        //return data;
    }

    // if attack is -1, it means THAT spell is [for All].

    public static ArrayList getSpellIdentity(string id) {
        ArrayList data = new ArrayList();
        switch (id)
        {
            case "#cost_control":
                data.Add(0); data.Add(0); data.Add(0); data.Add(0); data.Add("비용 조작");
                data.Add("유닛 하나에게 남은 테리얼 수만큼 비용을 부여합니다.");
                data.Add(""); data.Add("sprites/Spells/cost_control");
                break;
            case "#elemental_reset":
                data.Add(0); data.Add(0); data.Add(0); data.Add(0); data.Add("속성 초기화");
                data.Add("유닛 하나의 종족을 전장에 있는 동안 무효로 합니다.");
                data.Add(""); data.Add("");
                break;
            case "#tales_river":
                data.Add(0); data.Add(-1); data.Add(0); data.Add(0); data.Add("탈레스의 강");
                data.Add("<color=green>[환경 변화]</color> 이 카드는 중첩할 수 있습니다. (최대 3중첩)");
                data.Add(""); data.Add("");
                break;
            case "#draft":
                data.Add(0); data.Add(0); data.Add(0); data.Add(0); data.Add("징병");
                data.Add("유닛 하나의 종족을 전장에 있는 동안 '전사'로 설정합니다.");
                data.Add(""); data.Add("");
                break;
            case "#Archmage_rune":
                data.Add(5); data.Add(-1); data.Add(0); data.Add(2); data.Add("대마법사의 진");
                data.Add("<color=green>[환경 변화]</color> 카드의 비용을 영구히 지불하지 않습니다. (단, 카드를 낼 때는" +
                    " 카드에 명시된 비용만큼의 테리얼이 있어야 합니다.)");
                data.Add(""); data.Add("");
                break;
            case "#initial_shine":
                data.Add(1); data.Add(-1); data.Add(0); data.Add(0); data.Add("시초의 빛");
                data.Add("카드를 1장 뽑습니다.");
                data.Add(""); data.Add("");
                break;
            case "#slash":
                data.Add(1); data.Add(0); data.Add(0); data.Add(0); data.Add("참격");
                data.Add("유닛 하나에게 피해를 2 줍니다.");
                data.Add(""); data.Add("sprites/Spells/slash");
                break;
            case "#blood_storm":
                data.Add(2); data.Add(-1); data.Add(0); data.Add(1); data.Add("역전의 폭풍");
                data.Add("<color=red>모든</color> 유닛에게 피해를 1 줍니다. 적 유닛이 4체 이상인 경우, 대신 피해를 3 줍니다.");
                data.Add(""); data.Add("");
                break;
            case "#volcano":
                data.Add(1); data.Add(-1); data.Add(0); data.Add(0); data.Add("용암 분화구");
                data.Add("<color=green>[환경 변화]</color>");
                data.Add(""); data.Add("");
                break;         
            case "#salvage":
                data.Add(1); data.Add(-1); data.Add(0); data.Add(0); data.Add("인양");
                data.Add("유계의 무작위 물 속성 유닛 하나를 손으로 가져옵니다.");
                data.Add(""); data.Add("");
                break;
            case "#widdy_breath":
                data.Add(1); data.Add(0); data.Add(0); data.Add(0); data.Add("위디의 숨결");
                data.Add("유닛 하나의 공격력과 생명력을 바꾸고, '위디'를 손에 얻습니다.");
                data.Add(""); data.Add("");
                break;
            case "#vitality":
                data.Add(1); data.Add(-1); data.Add(0); data.Add(0); data.Add("대지의 기운");
                data.Add("내 <color=red>모든</color> 땅 속성 유닛에게 생명력을 +2 부여합니다.");
                data.Add(""); data.Add("");
                break;
            case "#luminous_grail":
                data.Add(1); data.Add(-1); data.Add(0); data.Add(0); data.Add("광휘의 성배");
                data.Add("내 플레이어의 최대 체력이 3 증가하고, 체력을 3 회복합니다.");
                data.Add(""); data.Add("");
                break;
            case "#dark_token":
                data.Add(1); data.Add(-1); data.Add(0); data.Add(0); data.Add("암흑 토큰");
                data.Add("'다키' 2장을 손에 얻습니다.");
                data.Add(""); data.Add("");
                break;
            case "#grave_robber":
                data.Add(1); data.Add(-1); data.Add(0); data.Add(1); data.Add("도굴");
                data.Add("유계의 이 카드를 사용하여 제외하는 것으로, 유계의 비용 5 " +
                    "흑암 속성 유닛 하나를 무작위로 손에 가져옵니다. 이 카드는 손에서 사용하여 효과 없이 유계로 넣을 수 있습니다.");
                data.Add(""); data.Add("");
                break;    
            case "#dimension_summon":
                data.Add(1); data.Add(-1); data.Add(0); data.Add(0); data.Add("차원 소환");
                data.Add("무작위 유닛을 손에 얻습니다.");
                data.Add(""); data.Add("");
                break;
            case "#evolve:SaintResia":
                data.Add(1); data.Add(-1); data.Add(0); data.Add(1); data.Add("진화 : 레시아");
                data.Add("내 '광채의 레시아'를 '대천사 레시아'로 진화시킵니다.");
                data.Add(""); data.Add("");
                break;
            case "#evolve:DarkResia":
                data.Add(1); data.Add(-1); data.Add(0); data.Add(1); data.Add("타락 : 레시아");
                data.Add("내 '광채의 레시아'를 '타락천사 레시아'로 타락시킵니다.");
                data.Add(""); data.Add("");
                break;
            case "#recall":
                data.Add(2); data.Add(0); data.Add(0); data.Add(0); data.Add("귀환");
                data.Add("내 유닛 하나를 제외하고 동일한 유닛을 손에 얻습니다.");
                data.Add(""); data.Add("");
                break;
            case "#shrink":
                data.Add(2); data.Add(0); data.Add(0); data.Add(0); data.Add("위축");
                data.Add("유닛 하나의 공격력을 1로 설정합니다.");
                data.Add(""); data.Add("");
                break;
            case "#ador_breath":
                data.Add(2); data.Add(0); data.Add(0); data.Add(0); data.Add("아도르의 숨결");
                data.Add("유닛 하나에게 피해를 2 주고, '아도르'를 손에 얻습니다.");
                data.Add(""); data.Add("");
                break;
            case "#nas_breath":
                data.Add(2); data.Add(-1); data.Add(0); data.Add(0); data.Add("나스의 숨결");
                data.Add("내 플레이어의 생명력을 4 회복하고, " +
                    "'나스'를 손에 얻습니다.");
                data.Add(""); data.Add("");
                break;
            case "#dark_ora":
                data.Add(2); data.Add(0); data.Add(0); data.Add(0); data.Add("암흑진");
                data.Add("유닛 하나에게 피해를 2 줍니다. 유계의 이 카드를 사용하여 제외하는 것으로," +
                    " 유계의 비용 4 이하 흑암 속성 유닛 하나를 무작위로 복사하여 덱에 넣고 섞습니다.");
                data.Add(""); data.Add("");
                break;
            case "#doll_gathering":
                data.Add(2); data.Add(-1); data.Add(0); data.Add(0); data.Add("인형 집결");
                data.Add("덱에서 '전투 인형'을 최대 3장 뽑습니다.");
                data.Add(""); data.Add("");
                break;
            case "#doll_regression":
                data.Add(3); data.Add(-1); data.Add(0); data.Add(1); data.Add("인형 회귀");
                data.Add("유계의 '전투 인형'을 최대 3장까지 손으로 가져오고, 카드를 1장 뽑습니다.");
                data.Add(""); data.Add("");
                break;
            case "#gladius":
                data.Add(2); data.Add(0); data.Add(0); data.Add(0); data.Add("글라디우스");
                data.Add("전사 유닛 하나에게 +3/+2를 부여합니다.");
                data.Add(""); data.Add("");
                break;
            case "#titanum_shield":
                data.Add(2); data.Add(0); data.Add(0); data.Add(0); data.Add("티타늄 방패");
                data.Add("전사 유닛 하나에게 +1/+2를 부여하고, 카드를 1장 뽑습니다.");
                data.Add(""); data.Add("");
                break;
            case "#assembly":
                data.Add(2); data.Add(-1); data.Add(0); data.Add(1); data.Add("증원");
                data.Add("덱에서 무작위 비용 4 이하 전사 유닛을 2장 뽑습니다.");
                data.Add(""); data.Add("");
                break;
            case "#wisdom_eye":
                data.Add(3); data.Add(-1); data.Add(0); data.Add(1); data.Add("혜안");
                data.Add("카드를 2장 뽑습니다.");
                data.Add(""); data.Add("");
                break;
            case "#heal_leaf":
                data.Add(3); data.Add(-1); data.Add(0); data.Add(0); data.Add("회복의 나뭇잎");
                data.Add("내 플레이어의 체력을 8 회복합니다.");
                data.Add(""); data.Add("");
                break;
            case "#armored":
                data.Add(3); data.Add(0); data.Add(0); data.Add(0); data.Add("무장");
                data.Add("유닛 하나에게 +2/+4를 부여합니다.");
                data.Add(""); data.Add("");
                break;
            case "#selfburn":
                data.Add(3); data.Add(0); data.Add(0); data.Add(1); data.Add("분신");
                data.Add("내 유닛 하나를 처치합니다. 대상이 불 종족 유닛이라면, 그 유닛의 공격력만큼 <color=red>모든</color> 적 유닛에게 피해를 줍니다.");
                data.Add(""); data.Add("");
                break;
            case "#earthquake":
                data.Add(3); data.Add(-1); data.Add(0); data.Add(1); data.Add("지진");
                data.Add("땅 종족 유닛을 제외한 <color=red>모든</color> 유닛에게 피해를 2 줍니다.");
                data.Add(""); data.Add("");
                break;
            case "#luminous_badge":
                data.Add(3); data.Add(-1); data.Add(0); data.Add(1); data.Add("광휘의 휘장");
                data.Add("<color=green>[환경 변화]</color> 이번 게임에서 내가 [개방]을 사용할 때마다, 내 플레이어의 생명력을 2 회복합니다.");
                data.Add(""); data.Add("");
                break;
            case "#emergency_deposit":
                data.Add(3); data.Add(-1); data.Add(0); data.Add(1); data.Add("비상용 예치금");
                data.Add("유계의 이 카드를 사용하여 제외하는 것으로 테리얼을 2 얻습니다. (최대 테리얼 수를 초과할 수 없습니다.)" +
                    " 이 카드는 손에서 사용하여 효과 없이 유계로 넣을 수 있습니다.");
                data.Add(""); data.Add("");
                break;
            case "#doll_sacrifice":
                data.Add(1); data.Add(-1); data.Add(0); data.Add(1); data.Add("인형 각성");
                data.Add("내 모든 '전투 인형'과 '인형기사 테트라' 에게 +1/+1을 부여합니다.");
                data.Add(""); data.Add("");
                break;
            case "#union_roar":
                data.Add(5); data.Add(-1); data.Add(0); data.Add(1); data.Add("단결의 포효");
                data.Add("유계의 무작위 용 유닛 3체를 손으로 가져옵니다. 카드를 1장 뽑습니다.");
                data.Add(""); data.Add("");
                break;
            case "#death":
                data.Add(5); data.Add(0); data.Add(0); data.Add(1); data.Add("소멸");
                data.Add("유닛 하나를 제외합니다.");
                data.Add(""); data.Add("");
                break;
            case "#ren_wakeup":
                data.Add(6); data.Add(-1); data.Add(0); data.Add(0); data.Add("렌 각성");
                data.Add("<color=red>모든</color> 유닛에게 피해를 5 줍니다.");
                data.Add(""); data.Add("");
                break;
            case "#halfmoon_strike":
                data.Add(5); data.Add(-1); data.Add(0); data.Add(2); data.Add("반월각");
                data.Add("<color=red>모든</color> 적 유닛에게 피해를 6 줍니다. 카드를 2장 뽑습니다.");
                data.Add(""); data.Add("");
                break;
            case "#latien_prelude":
                data.Add(2); data.Add(-1); data.Add(0); data.Add(2); data.Add("세이런 전주곡");
                data.Add("카드를 2장 뽑습니다.");
                data.Add(""); data.Add("");
                break;
            case "#latien_choir":
                data.Add(2); data.Add(-1); data.Add(0); data.Add(2); data.Add("세이런 합창곡");
                data.Add("내 <color=red>모든</color> 유닛에게 +2/+2를 부여합니다.");
                data.Add(""); data.Add("");
                break;
            case "#latien_fantasia":
                data.Add(1); data.Add(-1); data.Add(0); data.Add(2); data.Add("세이런 환상곡");
                data.Add("유계의 무작위 카드를 1장 손으로 가져옵니다.");
                data.Add(""); data.Add("");
                break;
            case "#latien_symphony":
                data.Add(2); data.Add(0); data.Add(0); data.Add(2); data.Add("세이런 교향곡");
                data.Add("유닛 하나를 처치합니다.");
                data.Add(""); data.Add("");
                break;
            case "#latien_lyrics":
                data.Add(4); data.Add(-1); data.Add(0); data.Add(2); data.Add("세이런 가곡");
                data.Add("<color=red>모든</color> 적 유닛에게 피해를 3 줍니다.");
                data.Add(""); data.Add("");
                break;
        }
        return data;
    }

    public static void setIdentity(GameObject obj, string id, Vector3 pos, Vector3 scale)
    {      
        if(cardData.ContainsKey(id))
        {
            ArrayList data = getIdentity(id);
            if (obj.tag == "Unit")
            {
                obj.GetComponent<scr_unit>().setIdentity(obj.GetComponent<scr_unit>().hosts,
                    (int)data[0], (int)data[1], (int)data[2],
                (int)data[3], (string)data[4], (string)data[5], (string)data[6], (string)data[7], id);
            }
            else
            {
                obj.GetComponent<scr_card>().setIdentity((int)data[0], (int)data[1], (int)data[2],
                (int)data[3], (string)data[4], (string)data[5], (string)data[6], (string)data[7], id);
                obj.GetComponent<RectTransform>().localPosition = pos;
                obj.GetComponent<RectTransform>().localScale = scale;
            }
        }       
    }
}
