using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class scr_playerPic : NetworkBehaviour {
    SpriteRenderer rend;

    [SyncVar]
    public string id;

    void Start()
    {
        if(isLocalPlayer) CmdSetId(scr_storeDeck.extraID);
        rend = transform.GetChild(8).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {
        if (rend.sprite == null) rend.sprite = Resources.Load<Sprite>("sprites/Units/Extra/" + id) as Sprite;
    }

    [Command]
    public void CmdSetId(string tempId) {
        id = tempId;
    }
}
