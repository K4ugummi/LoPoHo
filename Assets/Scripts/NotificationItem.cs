using UnityEngine;
using UnityEngine.UI;

public class NotificationItem : MonoBehaviour {

    [SerializeField]
    Text notification;

    public void PlayerKilled(string _playerKilled, string _dmgSource) {
        if (_playerKilled != _dmgSource) {
            notification.text = "<color=red><b>" + _dmgSource + "</b></color> killed <color=red><b>" + _playerKilled + "</b></color>";
        }
        else {
            notification.text = "<color=red><b>" + _playerKilled + "</b></color> commited suicide!";
        }
    }

    public void PlayerJoined(string _player) {
        notification.text = "<color=red><b>" + _player + "</b></color> has joined the game";
    }
    public void PlayerLeft(string _player) {
        notification.text = "<color=red><b>" + _player + "</b></color> has left the game";
    }
}
