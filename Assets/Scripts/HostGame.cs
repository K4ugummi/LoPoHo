using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint serverSize = 6;
    private string serverName = "LoPoHo Server";
    private bool isServerVisible = true;
    private string serverPassword = "";

    [SerializeField]
    private Text guiServerNameText;
    [SerializeField]
    private Text guiServerPasswordText;
    [SerializeField]
    private Text guiServerSizeText;

    private NetworkManager networkManager;

    void Start() {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null) {
            networkManager.StartMatchMaker();
        }
    }

    public void SetServerName() {
        serverName = guiServerNameText.text;
    }

    public void SetServerPassword() {
        serverPassword = guiServerPasswordText.text;
    }

    public void SetServerSize() {
        serverSize = uint.Parse(guiServerSizeText.text);
    }

    public void SetServerVisible(bool _visible) {
        isServerVisible = true;
    }

    public void CreateServer() {
        if (serverName != "" && serverName != null) {
            Debug.Log("HostGame: Creating server " + serverName + " for " + serverSize + " players.");
            // Create Server
            networkManager.matchMaker.CreateMatch(
                serverName, 
                serverSize, 
                isServerVisible, 
                serverPassword,
                "",
                "",
                0,
                0,
                networkManager.OnMatchCreate);

        }
    }
}
