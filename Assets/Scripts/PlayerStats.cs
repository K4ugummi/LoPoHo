using System.Collections;
using UnityEngine.UI;
using DatabaseControl;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public Text killStats;
    public Text deathStats;

	void Start() {
        if (UserAccountManager.isLoggedIn) {
            UserAccountManager.instance.GetData(OnReceivedData);
        }
    }

    void OnReceivedData(string _data) {
        if (killStats == null || deathStats == null) {
            return;
        } 
        killStats.text = "Kills: " + UserAccountDataHandling.DataToKills(_data).ToString();
        deathStats.text = "Deaths: " + UserAccountDataHandling.DataToDeaths(_data).ToString();
    }

}
