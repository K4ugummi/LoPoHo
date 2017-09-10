using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] componentsToDisable;
    [SerializeField]
    string remoteLayerName = "RemotePlayer";
    [SerializeField]
    string dontDrawLayerName = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUiPrefab;
    [HideInInspector]
    public GameObject playerUiInstance;


    Camera sceneCamera;

    void Start() {
        // Disable components that sould only be active on our controlled player.
        if (!isLocalPlayer) {
            DisableComponents();
            AssignRemoteLayer();
        }
        else {
            // Disable playergraphics for local player, like Helmets etc.
            // TODO: Weapon is set to layer -1
            Util.SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            // Create Player UI
            playerUiInstance = Instantiate(playerUiPrefab);
            playerUiInstance.name = playerUiPrefab.name;

            GUIPlayer _ui = playerUiInstance.GetComponent<GUIPlayer>();
            if (_ui == null) {
                Debug.LogError("PlayerSetup: No PlayerUI component on PlayerUI prefab!");
            }
            else {
                _ui.SetPlayer(GetComponent<Player>());
            }

            GetComponent<Player>().SetupPlayer();

            string _userName = "Loading...";
            if (UserAccountManager.isLoggedIn) {
                _userName = UserAccountManager.playerUsername;
            }
            else {
                _userName = transform.name;
            }

            CmdSetUserName(transform.name, _userName);
        }
    }

    [Command]
    void CmdSetUserName(string _playerID, string _userName) {
        Player _player = GameManager.GetPlayer(_playerID);
        if (_player != null) {
            Debug.Log("PlayerSetup: " + _userName + " has joined!");
            _player.userName = _userName;
        }
    }


    // Called everytime a client is setup locally
    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID, _player);
    }

    // Called if the player is not the local player, and sets the layer according to it.
    void AssignRemoteLayer() {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    // Added components (inspector) will be disabled in start.
    // A player should have components activated, that belong to the local-,
    // not remote player.
    void DisableComponents() {
        for (int i = 0; i < componentsToDisable.Length; i++) {
            componentsToDisable[i].enabled = false;
        }
    }

    // OnDisable is called, when the object is destroyed
    void OnDisable() {
        Destroy(playerUiInstance);

        // (Re-)Enable Scene Camera
        if (isLocalPlayer) {
            GameManager.instance.SetSceneCameraActive(true);
        }

        GameManager.UnregisterPlayer(transform.name);
    }
}
