using System.Collections;
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

        // Loop through players and set up a list item for each one
        foreach (Player _player in _players) {
            GameObject _itemGO = (GameObject)Instantiate(playerScoreItem, playerScoreList);
            PlayerScoreItem _item = _itemGO.GetComponent<PlayerScoreItem>();
            // Setting the ui elements
            if (_item != null) {
                _item.Setup(_player.userName, _player.kills, _player.deaths, _player.currentRTT);
            }
        }
        StartCoroutine(UpdatePings());
    }

    IEnumerator UpdatePings() {
        while (true) {
            yield return new WaitForSeconds(1f);
            foreach (Transform _child in playerScoreList) {
                Destroy(_child.gameObject);
            }
            // Get an array of players
            Player[] _players = GameManager.GetAllPlayers();
            // Loop through players and set up a list item for each one
            foreach (Player _player in _players) {
                GameObject _itemGO = (GameObject)Instantiate(playerScoreItem, playerScoreList);
                PlayerScoreItem _item = _itemGO.GetComponent<PlayerScoreItem>();
                // Setting the ui elements
                if (_item != null) {
                    _item.Setup(_player.userName, _player.kills, _player.deaths, _player.currentRTT);
                }
            }
        }
    }

    void OnDisable() {
        StopCoroutine(UpdatePings());
        // Clean up our list of items
        foreach (Transform _child in playerScoreList) {
            Destroy(_child.gameObject);
        }
    }
}
