using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_select : MonoBehaviour {
    public GameObject card;
    public string code;
    ArrayList selectTable;

    public void setId(string id) {
        getPlayer().GetComponent<scr_playerIdentity>().selectEffect(id, code);
        gameObject.SetActive(false);
    }

    public void setTable(ArrayList newTable, string code) {
        if (newTable != null && newTable.Count != 0) selectTable = newTable;
        else Debug.Log("Cannot assign null table");
        this.code = code;

        if (selectTable == null || selectTable.Count == 0) Debug.Log("No elements Found");
        for (int i = 0; i < selectTable.Count; i++)
        {
            Debug.Log((string)selectTable[i]);
        }
        GameObject grid = GameObject.FindGameObjectWithTag("SelectList");
        //Debug.Log(grid);
        //foreach (Transform childTr in grid.transform)
        //{
        //    Destroy(childTr.gameObject);
        //}
        for (int i = 0; i < selectTable.Count; i++)
        {
            GameObject ins = Instantiate(card, grid.transform);
            Debug.Log(ins);
            Debug.Log(selectTable[i]);
            DB_card.setIdentity(ins, (string)selectTable[i], Vector3.zero, Vector3.one);
            Debug.Log(ins.GetComponent<scr_card>().id);
            Debug.Log(ins.GetComponent<scr_card>().name);
            ins.GetComponent<scr_cardGUI>().enabled = false;
        }
    }

    public GameObject getPlayer()
    {
        GameObject player = null;
        foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (cur.GetComponent<scr_playerController>().isPlayer)
            {
                player = cur;
                break;
            }
        }
        return player;
    }
}
