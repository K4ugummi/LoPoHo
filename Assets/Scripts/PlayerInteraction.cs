using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(ItemManager))]
public class PlayerInteraction : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask hitMask;

    private int selectedItemIndex = 0;
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
        if (currentItem != null) {
            if (currentItem.itemAmmo < currentItem.itemMaxAmmo) {
                if (Input.GetButtonDown("Reload")) {
                    itemManager.Reload();
                    return;
                }
            }
            if (currentItem.itemFireRate <= 0f) {
                if (Input.GetButtonDown("Fire1")) {
                    Primary();
                }
            }
            else {
                if (Input.GetButtonDown("Fire1")) {
                    InvokeRepeating("Primary", 0f, 1 / currentItem.itemFireRate);
                }
                else if (Input.GetButtonUp("Fire1")) {
                    CancelInvoke("Primary");
                }
            }
        }
        #region Select Item
        int _prevSelectedItemIndex = selectedItemIndex;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
            if (selectedItemIndex >= itemManager.GetItemsLength() - 1) {
                selectedItemIndex = 0;
            }
            else {
                selectedItemIndex++;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedItemIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            selectedItemIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            selectedItemIndex = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            selectedItemIndex = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            selectedItemIndex = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            selectedItemIndex = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            selectedItemIndex = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            selectedItemIndex = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            selectedItemIndex = 8;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            selectedItemIndex = 9;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            if (selectedItemIndex <= 0) {
                selectedItemIndex = itemManager.GetItemsLength() - 1;
            }
            else {
                selectedItemIndex--;
            }
        }

        if (_prevSelectedItemIndex != selectedItemIndex) {
            itemManager.selectedItemGUIIndex = selectedItemIndex;
            SwitchItem(selectedItemIndex);
        }
        #endregion
    }
    // FIRE1
    [Client]    // only called on the client!
    void Primary() {

        if (!isLocalPlayer || itemManager.isReloading) {
            return;
        }
        currentItem.itemAmmo--;

        // Call the OnPrimary method on the server
        CmdOnPrimary();

        if (currentItem.itemAmmo <= 0) {
            itemManager.Reload();
            return;
        }

        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentItem.itemRange, hitMask)) {
            // TODO: Differentiate hit effect and actions!
            // Something has been hit by clicking primary mouse button! 
            // Spawn the on hit effect on the server
            CmdOnPrimaryHit(_hit.point, _hit.normal);
            switch (_hit.collider.tag) {
                case PLAYER_TAG:
                    CmdOnPlayerShot(_hit.collider.name, currentItem.itemDamage, transform.name);
                    break;
                default:
                    break;
            }
        }
    }

    [Client]
    void SwitchItem(int _itemIndex) {
        if (!isLocalPlayer || itemManager.isReloading) {
            return;
        }
        itemManager.CmdOnEquipItem(selectedItemIndex);
    }

    // Is called on the server, when a player uses his primary mouse button
    [Command]
    void CmdOnPrimary() {
        RpcDoPrimaryEffect();
    }

    // Do on primary effect on all clients
    [ClientRpc]
    void RpcDoPrimaryEffect() {
        PlayerItem _item = itemManager.GetCurrentItem();
        if (_item == null) {
            return;
        }
        // TODO: FIX THIS SHIT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        _item.GetMuzzleFlash().Play();
        AudioSource.PlayClipAtPoint(_item.primaryAudio.clip, transform.position, 0.5f);
    }

    // Is called on the server, when the primary mouse button action has hit something
    [Command]
    void CmdOnPrimaryHit(Vector3 _hitPosition, Vector3 _normalOfSurface) {
        RpcDoPrimaryHitEffect(_hitPosition, _normalOfSurface);
    }

    // Do on primary hit effect on all clients
    [ClientRpc]
    void RpcDoPrimaryHitEffect(Vector3 _hitPosition, Vector3 _normalOfSurface) {
        PlayerItem _item = itemManager.GetCurrentItem();
        if (_item == null) {
            return;
        }
        // TODO: Instantiation objects takes a lot of processing power
        // Look into "object pooling"
        GameObject _hitEffect = Instantiate(_item.hitEffectPrefab, _hitPosition, Quaternion.LookRotation(_normalOfSurface));
        AudioSource.PlayClipAtPoint(_item.primaryImpactAudio.clip, _hitPosition, 0.5f);
        Destroy(_hitEffect, 1f);
    }

    [Command]   // only called on the server!
    void CmdOnPlayerShot(string _playerID, int _damage, string _sourceID) {
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage, _sourceID);
    } 

    public void CancelAllActionsOnDeath() {
        if (!isLocalPlayer) {
            return;
        }
        CancelInvoke("Primary");
    }
}
