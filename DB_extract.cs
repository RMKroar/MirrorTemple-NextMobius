using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_extract : MonoBehaviour {
    public static ArrayList forUnit = new ArrayList {
        "doll", "student_swordman", "little_healer", "student_fighter", "shasha", "speedy_archer",
        "hero_helper", "big_sworder", "tough_shieldman", "cursed_mage", "wind_walker", "slum_dancer",
        "crystal_fighter", "steel_sworder", "fire_mage", "mancy", "rein", "necky", "heavyarmor_knight",
        "wishful_bishop", "axe_zealot", "dark_baron", "dark_mage", "investigator_pein"
    };



    public static ArrayList forAll = new ArrayList {
        "ren", "dollmaker", "moon_slayer", "archeologist", "sacrificer", "synthesizer", "sacrifice_summoner",
        "wave_fighter", "duret", "oldbook_analyzer", "nature_mage", "carsus_priest", "ken", "penitent_devil", "mirat" 
    };

    // Return Data = ArrayList(cost, name, description)
    public static ArrayList getIdentity(string id)
    {
        ArrayList data = new ArrayList();
        switch (id) {
            case "doll":
                data.Add(0); data.Add("인형술"); data.Add("유닛 하나에게 공격력을 1 부여합니다." +
                    "\n만약 대상 유닛이 '전투 인형'일 경우, 대신 +1/+1을 부여합니다.");
                break;
            case "ren":
                data.Add(5); data.Add("리틀 보이"); data.Add("모든 유닛에게 피해를 5 줍니다.");
                break;
            case "student_swordman":
                data.Add(2); data.Add("기초 검술"); data.Add("유닛 하나에게 피해를 3 줍니다.");
                break;
            case "little_healer":
                data.Add(1); data.Add("회복의 기도"); data.Add("유닛 하나의 체력을 2 회복합니다.");
                break;
            case "student_fighter":
                data.Add(1); data.Add("신체 강화"); data.Add("유닛 하나에게 +1/+1을 부여합니다.");
                break;
            case "shasha":
                data.Add(0); data.Add("비용 조작"); data.Add("유닛 하나의 비용을 내 남은 테리얼의 수로 설정합니다.");
                break;
            case "dollmaker":
                data.Add(3); data.Add("재봉"); data.Add("내 모든 '전투 인형'이 +1/+1을 얻습니다.\n" +
                    "'전투 인형' 2장을 손에 얻습니다.");
                break;
            case "speedy_archer":
                data.Add(1); data.Add("적중 사격"); data.Add("유닛 하나에게 피해를 1 줍니다. 대상 유닛이 후방에 있다면," +
                    " 대신 피해를 3 줍니다.");
                break;
            case "hero_helper":
                data.Add(2); data.Add("무장 보급"); data.Add("유닛 하나에게 +2/+2를 부여합니다.");
                break;
            case "moon_slayer":
                data.Add(3); data.Add("반월 지파진"); data.Add("적 유닛 모두에게 피해를 1 줍니다.\n" +
                    "내 전장의 '반월전사', '달빛 아래의 맨시', '페인 반' 1체 당 피해량이 1 증가합니다.");
                break;
            case "archeologist":
                data.Add(2); data.Add("고서 분석"); data.Add("카드를 1장 뽑습니다.");
                break;
            case "sacrificer":
                data.Add(2); data.Add("알카딕 1절"); data.Add("의식 카운트를 2 얻습니다.");
                break;
            case "big_sworder":
                data.Add(3); data.Add("대검 내려베기"); data.Add("유닛 하나에게 피해를 3 줍니다.\n" +
                    "만약 대상의 전방 또는 후방에 적 유닛이 있다면, 그 유닛에게도 피해를 3 줍니다.");
                break;
            case "tough_shieldman":
                data.Add(2); data.Add("중장갑 장착"); data.Add("유닛 하나에게 생명력을 4 부여합니다.");
                break;
            case "cursed_mage":
                data.Add(4); data.Add("저주의 연무"); data.Add("유닛 하나에게 +6/+6과 " +
                    "'[공격] 이 유닛이 피해를 3 받습니다'를 부여합니다.");
                break;
            case "synthesizer":
                data.Add(5); data.Add("회귀하는 합성"); data.Add("'페인 합성술사' 1장을 손에 얻습니다.");
                break;
            case "sacrifice_summoner":
                data.Add(3); data.Add("알카딕 2절"); data.Add("내 유닛 수만큼 의식 카운트를 얻습니다.");
                break;
            case "slum_dancer":
                data.Add(1); data.Add("스타일 스텝"); data.Add("유닛 하나의 공격력을 2 감소시킵니다.\n" +
                    "0 미만으로 내려가지 않습니다.");
                break;
            case "wind_walker":
                data.Add(3); data.Add("환류 태풍"); data.Add("유닛 하나에게 피해를 3 줍니다.\n" +
                    "만약 대상 유닛의 공격력이 7 이상이라면, 대신 그 유닛을 처치합니다.");
                break;
            case "wave_fighter":
                data.Add(1); data.Add("기공탄"); data.Add("적 플레이어에게 피해를 2 줍니다.");
                break;
            case "duret":
                data.Add(2); data.Add("종말의 불꽃"); data.Add("모든 유닛에게 적의 남은 테리얼만큼의 피해를 줍니다.");
                break;
            case "crystal_fighter":
                data.Add(1); data.Add("수정 타격"); data.Add("유닛 하나에게 피해를 1 줍니다.\n" +
                    "그 후 대상 유닛의 공격력이 생명력보다 높다면 '붉은 수정 레아'를 1장 손에 얻고, " +
                    "그렇지 않다면 '푸른 수정 오베론'을 1장 손에 얻습니다.");
                break;
            case "steel_sworder":
                data.Add(4); data.Add("참수형"); data.Add("유닛 하나에게 피해를 6 줍니다.");
                break;
            case "oldbook_analyzer":
                data.Add(4); data.Add("고서 연구"); data.Add("카드를 2장 뽑습니다.");
                break;
            case "fire_mage":
                data.Add(3); data.Add("2단 업화"); data.Add("유닛 하나에게 피해를 2 줍니다. " +
                    "그 후 대상이 살아있다면, 모든 적 유닛에게 피해를 1 줍니다.");
                break;
            case "nature_mage":
                data.Add(4); data.Add("자연의 축복"); data.Add("내 플레이어의 생명력을 3 회복합니다.\n" +
                    "전장의 유닛 수만큼 치유량이 증가합니다.");
                break;
            case "carsus_priest":
                data.Add(3); data.Add("카서스 주술 : 광란"); data.Add("모든 내 유닛이 공격력을 2 얻습니다.");
                break;
            case "mancy":
                data.Add(3); data.Add("명월청"); data.Add("유닛 하나에게 +2/+2를 부여합니다. " +
                    "대상 유닛이 '반월전사', '달빛 아래의 맨시', '페인 반' 중 하나라면, 대신 +4/+4를 부여합니다.");
                break;
            case "rein":
                data.Add(2); data.Add("스피드 스텝"); data.Add("유닛 하나의 공격력을 1로 설정합니다.");
                break;
            case "necky":
                data.Add(2); data.Add("풍활보"); data.Add("유닛 하나에게 +1/+1을 부여합니다.\n" +
                    "만약 대상의 공격력이 4 이하라면, 대신 공격 기회를 부여합니다.");
                break;
            case "ken":
                data.Add(3); data.Add("기공 연환"); data.Add("적 플레이어에게 피해를 2 줍니다.\n" +
                    "적 유닛의 수만큼 피해량이 증가합니다.");
                break;
            case "heavyarmor_knight":
                data.Add(2); data.Add("무기를 방패로"); data.Add("유닛 하나의 공격력을 절반으로 하고, 감소 수치의 2배만큼 " +
                    "생명력을 부여합니다. (소수는 버림합니다.)");
                break;
            case "penitent_devil":
                data.Add(3); data.Add("진실한 참회"); data.Add("모든 유닛의 생명력이 1이 됩니다.");
                break;
            case "wishful_bishop":
                data.Add(5); data.Add("알카딕 3절"); data.Add("유닛 하나의 사망 효과를 무효로 하고, 처치합니다.");
                break;
            case "axe_zealot":
                data.Add(1); data.Add("광폭화"); data.Add("유닛 하나의 생명력을 1로 설정하고, " +
                    "공격력을 4 부여합니다.");
                break;
            case "dark_baron":
                data.Add(2); data.Add("혈계약"); data.Add("유닛 하나를 처치합니다. 대상 유닛의 생명력 분의 피해를 내 플레이어에게 줍니다.");
                break;
            case "mirat":
                data.Add(5); data.Add("마리오네트"); data.Add("내 모든 '전투 인형'이 +2/+2를 얻습니다." +
                    "'전투 인형' 3장을 손에 얻습니다.");
                break;
            case "investigator_pein":
                data.Add(7); data.Add("광란의 빛 : 멸망"); data.Add("유닛 하나를 처치합니다. 만약 내 유닛이 3체 이상이라면, " +
                    "모든 유닛을 처치합니다.");
                break;
            case "dark_mage":
                data.Add(4); data.Add("타락"); data.Add("유닛 하나에게 피해를 4 줍니다. 대상 유닛이 처치되었다면, " +
                    "그 유닛을 1장 복사하여 내 손에 얻습니다.");
                break;
            default:
                break;
        }
        return data;
    } 
}
