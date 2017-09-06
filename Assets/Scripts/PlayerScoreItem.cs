using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerScoreItem : MonoBehaviour {

    [SerializeField]
    TMP_Text userNameText;
    [SerializeField]
    TMP_Text killsText;
    [SerializeField]
    TMP_Text deathsText;
    [SerializeField]
    TMP_Text pingText;

    public void Setup(string _userName, int _kills, int _deaths, int _ping) {
        userNameText.text = _userName;
        killsText.text = _kills.ToString();
        deathsText.text = _deaths.ToString();
        pingText.text = GetPingColored(_ping);
    }

    private string GetPingColored(int _ping) {
        string _result = "<color=#";
        if (_ping < 25) {
            _result += "04B404";
        }
        else if (_ping < 50) {
            _result += "31B404";
        }
        else if (_ping < 75) {
            _result += "5FB404";
        }
        else if (_ping < 100) {
            _result += "86B404";
        }
        else if (_ping < 125) {
            _result += "AEB404";
        }
        else if (_ping < 150) {
            _result += "B18904";
        }
        else if (_ping < 175) {
            _result += "B45F04";
        }
        else if (_ping < 200) {
            _result += "B43104";
        }
        else {
            _result += "B40404";
        }
        return _result + ">" + _ping.ToString() + "</color>";
    }

}
