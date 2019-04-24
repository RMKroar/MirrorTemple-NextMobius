using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_particles : MonoBehaviour {
    public static GameObject par_up;
    public static GameObject par_down;
    public static GameObject par_ability;
    public static GameObject par_spark;
    public static GameObject par_attack;
    public static GameObject par_charge;
    public static GameObject par_card;
    public static GameObject ef_damage;
    public static GameObject par_revilla;
    public static GameObject par_flower;
    public static GameObject par_magmaBurst;
    public static GameObject par_giantBreath;
    public static GameObject par_cyclone;
    public static GameObject par_giantMud;
    public static GameObject par_giantShine;
    public static GameObject par_darkMist;
    public static GameObject par_apocalipse;
    public static GameObject par_soulwhite;
    public static GameObject par_soulblue;
    public static GameObject par_soulred;
    public static GameObject par_spell;
    public static GameObject par_cross;
    public static GameObject par_myturn;

    void Start()
    {
        par_up = Resources.Load("FXonload/par_up", typeof(GameObject)) as GameObject;
        par_down = Resources.Load("FXonload/par_down", typeof(GameObject)) as GameObject;
        par_ability = Resources.Load("FXonload/par_ability", typeof(GameObject)) as GameObject;
        par_spark = Resources.Load("FXonload/par_damage", typeof(GameObject)) as GameObject;
        par_attack = Resources.Load("FXonload/par_attack", typeof(GameObject)) as GameObject;
        par_charge = Resources.Load("FXonload/par_charge", typeof(GameObject)) as GameObject;
        par_card = Resources.Load("FXonload/par_card", typeof(GameObject)) as GameObject;
        ef_damage = Resources.Load("FXonload/ef_damage", typeof(GameObject)) as GameObject;
        par_revilla = Resources.Load("FXonload/par_revilla", typeof(GameObject)) as GameObject;
        par_flower = Resources.Load("FXonload/par_flower", typeof(GameObject)) as GameObject;
        par_magmaBurst = Resources.Load("FXonload/par_magmaBurst", typeof(GameObject)) as GameObject;
        par_giantBreath = Resources.Load("FXonload/par_giantBreath", typeof(GameObject)) as GameObject;
        par_cyclone = Resources.Load("FXonload/par_cyclone", typeof(GameObject)) as GameObject;
        par_giantMud = Resources.Load("FXonload/par_giantMud", typeof(GameObject)) as GameObject;
        par_giantShine = Resources.Load("FXonload/par_giantShine", typeof(GameObject)) as GameObject;
        par_darkMist = Resources.Load("FXonload/par_darkMist", typeof(GameObject)) as GameObject;
        par_apocalipse = Resources.Load("FXonload/par_apocalipse", typeof(GameObject)) as GameObject;
        par_soulwhite = Resources.Load("FXonload/par_soulfall", typeof(GameObject)) as GameObject;
        par_soulblue = Resources.Load("FXonload/par_soulblue", typeof(GameObject)) as GameObject;
        par_soulred = Resources.Load("FXonload/par_soulred", typeof(GameObject)) as GameObject;
        par_spell = Resources.Load("FXonload/par_spell", typeof(GameObject)) as GameObject;
        par_cross = Resources.Load("FXonload/par_cross", typeof(GameObject)) as GameObject;
        par_myturn = Resources.Load("FXonload/par_myturn", typeof(GameObject)) as GameObject;
    }
}
