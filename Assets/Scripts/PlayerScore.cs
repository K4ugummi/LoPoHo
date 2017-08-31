using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerScore : MonoBehaviour {

    int lastKills = 0;
    int lastDeaths = 0;

    Player player;
    
	void Start () {
        player = GetComponent<Player>();
        StartCoroutine(SyncScoreLoop());
	}
	
    // Sync, when we leave or whatever
    void OnDestroy() {
        if (player != null) {
            SyncNow();
        }
    }

    // Sync player data with the database
	IEnumerator SyncScoreLoop() {
        while (true) {
            yield return new WaitForSeconds(5f);

            SyncNow();
        }
    }

    void SyncNow() {
        if (UserAccountManager.isLoggedIn) {
            UserAccountManager.instance.GetData(OnDataReceived);
        }
    }

    void OnDataReceived(string _data) {
        if (player.kills <= lastKills && player.deaths <= lastDeaths) {
            return;
        }

        int _killsSinceLastUpdate = player.kills;
        int _deathsSinceLastUpdate = player.deaths;

        int _kills = UserAccountDataHandling.DataToKills(_data);
        int _deaths = UserAccountDataHandling.DataToDeaths(_data);

        int _newKills = _killsSinceLastUpdate + _kills;
        int _newDeaths = _deathsSinceLastUpdate + _deaths;

        string _newData = UserAccountDataHandling.ValuesToData(_newKills, _newDeaths);

        Debug.Log("PlayerScore: Syncing new data: " + _newData);

        lastKills = player.kills;
        lastDeaths = player.deaths;

        UserAccountManager.instance.SendData(_newData);
    }
}
