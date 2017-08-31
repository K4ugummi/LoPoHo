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

        // Current Database values
        int _dataKills = UserAccountDataHandling.DataToKills(_data);
        int _dataDeaths = UserAccountDataHandling.DataToDeaths(_data);

        int _newDataKills = (lastKills - player.kills) + _dataKills;
        int _newDataDeaths = (lastDeaths - player.deaths) + _dataDeaths;

        string _newData = UserAccountDataHandling.ValuesToData(_newDataKills, _newDataDeaths);

        lastKills = player.kills;
        lastDeaths = player.deaths;
        
        Debug.Log("PlayerScore: Syncing new data: " + _newData);
        UserAccountManager.instance.SendData(_newData);
    }
}
