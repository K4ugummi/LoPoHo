using UnityEngine;
using System;

public class UserAccountDataHandling : MonoBehaviour {

    private static char IDENTIFIER_SEPARATOR = '/';
    private static string IDENTIFIER_KILLS = "[KILLS]";
    private static string IDENTIFIER_DEATHS = "[DEATHS]";

    public static int DataToKills(string _data) {
        return int.Parse(DataToValue(_data, IDENTIFIER_KILLS));
    }

    public static int DataToDeaths(string _data) {
        return int.Parse(DataToValue(_data, IDENTIFIER_DEATHS));
    }

    public static string ValuesToData(int _kills, int _deaths) {
        return IDENTIFIER_KILLS + _kills + IDENTIFIER_SEPARATOR
            + IDENTIFIER_DEATHS + _deaths;
    }

    private static string DataToValue(string _data, string _identifier) {
        string[] _dataPieces = _data.Split('/');
        foreach (string _piece in _dataPieces) {
            if (_piece.StartsWith(_identifier)) {
                return _piece.Substring(_identifier.Length);
            }
        }
        Debug.LogError("UserAccountDataHandling: Identifier " + _identifier + " not found in Data!");
        return "";
    }
}
