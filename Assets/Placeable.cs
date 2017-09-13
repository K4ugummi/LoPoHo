using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Placeable : NetworkBehaviour {

    public GameObject itemToRecieve;

    [Client]
    public void OnPlayerTool(string _playerId) {
        CmdOnPlayerTool(_playerId);
    }

    [Command]
    public void CmdOnPlayerTool(string _playerId) {
        Player _player = GameManager.GetPlayer(_playerId);
        _player.GetComponent<ItemManager>();

        NetworkServer.Destroy(transform.gameObject);
    }

}
