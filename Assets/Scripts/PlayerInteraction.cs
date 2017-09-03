using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(ItemManager))]
public class PlayerInteraction : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask hitMask;

    private PlayerItem currentItem;
    private ItemManager itemManager;

    void Start() {
        if (cam == null) {
            Debug.LogError("Playershoot: No camera referenced!");
            this.enabled = false;
        }

        itemManager = GetComponent<ItemManager>();
    }

    void Update() {
        currentItem = itemManager.GetCurrentItem();

        if (PauseMenu.isPauseMenu) {
            return;
        }
        if (currentItem.ammo < currentItem.maxAmmo) {
            if (Input.GetButtonDown("Reload")) {
                itemManager.Reload();
                return;
            }
        }
        if (currentItem.fireRate <= 0f) {
            if (Input.GetButtonDown("Fire1")) {
                Primary();
            }
        }
        else {
            if (Input.GetButtonDown("Fire1")) {
                InvokeRepeating("Primary", 0f, 1 / currentItem.fireRate);
            }
            else if (Input.GetButtonUp("Fire1")) {
                CancelInvoke("Primary");
            } 
        }
    }
    // FIRE1
    [Client]    // only called on the client!
    void Primary() {

        if (!isLocalPlayer || itemManager.isReloading) {
            return;
        }

        if (currentItem.ammo <= 0) {
            itemManager.Reload();
            return;
        }

        currentItem.ammo--;

        // Call the OnPrimary method on the server
        CmdOnPrimary();

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentItem.range, hitMask)) {
            // TODO: Differentiate hit effect and actions!
            // Something has been hit by clicking primary mouse button! 
            switch (_hit.collider.tag) {
                case PLAYER_TAG:
                    CmdPlayerShot(_hit.collider.name, currentItem.damage, transform.name);
                    break;
                default:
                    break;
            }

            // Spawn the on hit effect on the server
            CmdOnPrimaryHit(_hit.point, _hit.normal);
        }
    }

    // Is called on the server, when a player uses his primary mouse button
    [Command]
    void CmdOnPrimary() {
        RpcDoPrimaryEffect();
    }

    // Do on primary effect on all clients
    [ClientRpc]
    void RpcDoPrimaryEffect() {
        itemManager.GetCurrentGraphics().muzzleFlash.Play();
        itemManager.GetCurrentSounds().primaryAudio.Play();
    }

    // Is called on the server, when the primary mouse button action has hit something
    [Command]
    void CmdOnPrimaryHit(Vector3 _hitPosition, Vector3 _normalOfSurface) {
        RpcDoPrimaryHitEffect(_hitPosition, _normalOfSurface);
    }

    // Do on primary hit effect on all clients
    [ClientRpc]
    void RpcDoPrimaryHitEffect(Vector3 _hitPosition, Vector3 _normalOfSurface) {
        // TODO: Instantiation objects takes a lot of processing power
        // Look into "object pooling"
        GameObject _hitEffect = Instantiate(itemManager.GetCurrentGraphics().hitEffectPrefab, _hitPosition, Quaternion.LookRotation(_normalOfSurface));
        AudioSource.PlayClipAtPoint(itemManager.GetCurrentSounds().primaryImpactAudio, _hitPosition, 0.5f);
        Destroy(_hitEffect, 2f);
    }

    [Command]   // only called on the server!
    void CmdPlayerShot(string _playerID, int _damage, string _sourceID) {
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage, _sourceID);
    } 
}
