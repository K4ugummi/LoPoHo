using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

    private const string PLAYER_ID_PREFIX = "Player ";

    public static GameManager instance;

    public ServerSettings serverSettings;
    
    private static Dictionary<string, Player> playerDictionary = new Dictionary<string, Player>();

    [SerializeField]
	private GameObject sceneCamera;

    public delegate void OnPlayerKilledCallback(string _playerKilled, string _dmgSource);
    public OnPlayerKilledCallback onPlayerKilledCallback;
    public delegate void OnPlayerJoinedCallback(string _player);
    public OnPlayerJoinedCallback onPlayerJoinedCallback;
    public delegate void OnPlayerLeftCallback(string _player);
    public OnPlayerLeftCallback onPlayerLeftCallback;

    void Awake() {
        if (instance != null) {
            Debug.LogError("GameManager: Another instance of GameManager found in Scene!");
        }
        else {
            instance = this;
        }
    }

    public void SetSceneCameraActive(bool _isActive) {
        if (sceneCamera == null) {
            Debug.LogError("GameManager: Scene camera not found!");
            return;
        }
        if (_isActive) {
            Util.ShowCursor();
        }
        else {
            Util.HideCursor();
        }
        sceneCamera.SetActive(_isActive);
    }

    #region Player Dictionary
    public static void ClearPlayerDictionaryOnStart() {
        if (playerDictionary != null) {
            playerDictionary.Clear();
        }
    }

    public static void RegisterPlayer(string _netId, Player _player) {
        string _playerId = PLAYER_ID_PREFIX + _netId;
        playerDictionary.Add(_playerId, _player);
        _player.transform.name = _playerId;
        Debug.Log("GameManager: " + _playerId + " has joined the game.");
    }

    public static void UnregisterPlayer(string _playerId)
    {
        Debug.Log("GameManager: " + _playerId + " has left the game.");
        playerDictionary.Remove(_playerId);
    }

    public static Player GetPlayer (string _playerId)
    {
        return playerDictionary[_playerId];
    }

    public static Player[] GetAllPlayers() {
        return playerDictionary.Values.ToArray();
    }
    #endregion
}
