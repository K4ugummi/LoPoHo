using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerSetup))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(ItemManager))]
public class Player : NetworkBehaviour {

    [SyncVar]
    private bool _isDead = false;
    public bool isDead {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private float maxHealth = 100;
    [SyncVar]
    private float currentHealth;

    public float GetCurrentHealth() {
        return currentHealth;
    }
    public float GetMaxHealth() {
        return maxHealth;
    }

    [SyncVar]
    public string userName = "Loading...";
    [SyncVar]
    public int kills;
    [SyncVar]
    public int deaths;

    [SerializeField]
    GameObject deathParticleEffect;
    [SerializeField]
    GameObject spawnParticleEffect;

    [SerializeField]
    private Behaviour[] disableComponentsOnDeath;
    private bool[] wasEnabledBeforeDeath;
    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;
    
    private bool isFirstSetupOnClient = true;
    [SyncVar]
    private bool isAlreadyAnnounced = false;

    public void SetupPlayer() {
        if (isLocalPlayer) {
            //Switch cameras
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUiInstance.SetActive(true);
        }
        CmdBroadcastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadcastNewPlayerSetup() {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients() {
        // Store if components were enabled in wasEnabled[]
        if (isFirstSetupOnClient) {
            wasEnabledBeforeDeath = new bool[disableComponentsOnDeath.Length];
            for (int i = 0; i < wasEnabledBeforeDeath.Length; i++) {
                wasEnabledBeforeDeath[i] = disableComponentsOnDeath[i].enabled;
            }
            if (!isAlreadyAnnounced) {
                GameManager.instance.onPlayerJoinedCallback(userName);
                isAlreadyAnnounced = true;
            }
            isFirstSetupOnClient = false;
        }
        SetDefaults();
    }


    // Testfunction to kill the player with shortcut "K"
    //void Update() {
    //    if (!isLocalPlayer) {
    //        return;
    //    }
    //    if (Input.GetKeyDown(KeyCode.K)) {
    //        RpcTakeDamage(99999, transform.name);
    //    }
    //}

    [ClientRpc]     // a method that is called on all clients
    public void RpcTakeDamage(int _dmg, string _sourceID) {
        if (isDead) {
            return;
        }
        currentHealth -= _dmg;

        if (currentHealth <= Mathf.Epsilon) {
            Die(_sourceID);
        }
    }

    private void Die(string _sourceID) {
        isDead = true;

        Player _sourcePlayer = GameManager.GetPlayer(_sourceID);
        if (_sourcePlayer != null) {
            _sourcePlayer.kills++;
            GameManager.instance.onPlayerKilledCallback.Invoke(userName, _sourcePlayer.userName);
        }
        deaths++;

        // Disable components
        for (int i = 0; i < disableComponentsOnDeath.Length; i++) {
            disableComponentsOnDeath[i].enabled = false;
        }

        // Disable GameObjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++) {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        // Disable the collider
        Collider _collider = GetComponent<Collider>();
        if (_collider != null) {
            _collider.enabled = false;
        }

        GameObject _gfxInstance = (GameObject)Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
        Destroy(_gfxInstance, 3f);

        // Switch cameras
        if (isLocalPlayer) {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUiInstance.SetActive(false);
            GetComponent<PlayerUI>();
        }

        Debug.Log("Player: " + transform.name + " died!");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn() {
        yield return new WaitForSeconds(GameManager.instance.serverSettings.respawnTime);

        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        SetupPlayer();

        Debug.Log(transform.name + " respawned at " + _spawnPoint.name + ".");
    }
    
    public void SetDefaults() {
        isDead = false;
        currentHealth = maxHealth;

        // TODO: check if this is only set on local player
        GetComponent<PlayerController>().ResetStamina();
        GetComponent<ItemManager>().ResetAmmo();


        // (Re-)Enable components.
        for (int i = 0; i < disableComponentsOnDeath.Length; i++) {
            disableComponentsOnDeath[i].enabled = wasEnabledBeforeDeath[i];
        }

        // (Re-)Enable the gameobjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++) {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        // Enable the collider
        Collider _collider = GetComponent<Collider>();
        if (_collider != null) {
            _collider.enabled = true;
        }

        // Create spawn effect
        GameObject _gfxIns = (GameObject)Instantiate(spawnParticleEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
    }

}
