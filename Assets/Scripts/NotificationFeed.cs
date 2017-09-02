using UnityEngine;

public class NotificationFeed : MonoBehaviour {

    [SerializeField]
    private float notificationTime = 5f;
    [SerializeField]
    GameObject notificationItem;

	void Start () {
        GameManager.instance.onPlayerKilledCallback += OnPlayerKilled;
        GameManager.instance.onPlayerJoinedCallback += OnPlayerJoined;
        GameManager.instance.onPlayerLeftCallback += OnPlayerLeft;
    }
	
	public void OnPlayerKilled(string _playerKilled, string _dmgSource) {
        GameObject _go = Instantiate(notificationItem, this.transform);
        _go.GetComponent<NotificationItem>().PlayerKilled(_playerKilled, _dmgSource);
        Destroy(_go, notificationTime);
    }

    public void OnPlayerJoined(string _player) {
        if (_player == "Loading...") {
            return;
        }
        GameObject _go = Instantiate(notificationItem, this.transform);
        _go.GetComponent<NotificationItem>().PlayerJoined(_player);
        Destroy(_go, notificationTime);
    }

    public void OnPlayerLeft(string _player) {
        GameObject _go = Instantiate(notificationItem, this.transform);
        _go.GetComponent<NotificationItem>().PlayerLeft(_player);
        Destroy(_go, notificationTime);
    }
}
