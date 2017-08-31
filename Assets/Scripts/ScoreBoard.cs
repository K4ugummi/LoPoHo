using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour {

    [SerializeField]
    GameObject playerScoreItem;
    [SerializeField]
    Transform playerScoreList;

	void OnEnable() {
        // Get an array of players
        Player[] _players = GameManager.GetAllPlayers();

        foreach (Player _player in _players) {
            GameObject _itemGO = (GameObject)Instantiate(playerScoreItem, playerScoreList);
            PlayerScoreItem _item = _itemGO.GetComponent<PlayerScoreItem>();
            if (_item != null) {
                _item.Setup(_player.userName, _player.kills, _player.deaths, 999);
            }
        }

        // Loop through players and set up a list item for each one
            // Setting the ui elements
    }

    void OnDisable() {
        // Clean up our list of items
        foreach (Transform _child in playerScoreList) {
            Destroy(_child.gameObject);
        }
    }
}
