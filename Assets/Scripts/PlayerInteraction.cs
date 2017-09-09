using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(ItemManager))]
public class PlayerInteraction : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";
    private const string PLAYER_ITEM_LAYER = "PlayerItem";

    [SerializeField]
    public Camera cam;
    [SerializeField]
    private LayerMask hitMask;

    private int selectedItemIndex = 0;
    private Item currentItem;
    private GameObject currentItemInstance;
    private ItemManager itemManager;

    void Start() {
        if (cam == null) {
            Debug.LogError("Playershoot: No camera referenced!");
            this.enabled = false;
        }

        itemManager = GetComponent<ItemManager>();
    }

    [Client]
    void Update() {
        if (Input.GetKeyDown(KeyCode.F12)) {
            Util.TakeScreenshot();
        }
        if (PauseMenu.isPauseMenu) {
            return;
        }
        if (Inventory.isInventory) {
            return;
        }
        #region Item Action
        currentItem = itemManager.GetCurrentItem();
        if (isLocalPlayer) {
            if (currentItem != null) {
                if (Input.GetButtonDown("Reload")) {
                    VisItemReload _visItemReload = new VisItemReload();
                    currentItem.Accept(_visItemReload);
                }

                if (Input.GetButtonDown("MousePrimary")) {
                    VisItemPrimaryDown _visItemPrimaryDown = new VisItemPrimaryDown();
                    currentItem.Accept(_visItemPrimaryDown);
                }
                else if (Input.GetButtonUp("MousePrimary")) {
                    VisItemPrimaryUp _visItemPrimaryUp = new VisItemPrimaryUp();
                    currentItem.Accept(_visItemPrimaryUp);
                }
            }
        }
        #endregion
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
        if (Input.GetButtonDown("Item1")) {
            selectedItemIndex = 0;
        }
        if (Input.GetButtonDown("Item2")) {
            selectedItemIndex = 1;
        }
        if (Input.GetButtonDown("Item3")) {
            selectedItemIndex = 2;
        }
        if (Input.GetButtonDown("Item4")) {
            selectedItemIndex = 3;
        }
        if (Input.GetButtonDown("Item5")) {
            selectedItemIndex = 4;
        }
        if (Input.GetButtonDown("Item6")) {
            selectedItemIndex = 5;
        }
        if (Input.GetButtonDown("Item7")) {
            selectedItemIndex = 6;
        }
        if (Input.GetButtonDown("Item8")) {
            selectedItemIndex = 7;
        }
        if (Input.GetButtonDown("Item9")) {
            selectedItemIndex = 8;
        }
        if (Input.GetButtonDown("Item10")) {
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
            //itemManager.selectedItemIndex = selectedItemIndex;
            SwitchItem(selectedItemIndex);
        }
        #endregion
    }

    [Client]
    void SwitchItem(int _itemIndex) {
        if (!isLocalPlayer || itemManager.isReloading) {
            return;
        }
        itemManager.CmdOnEquipItem(selectedItemIndex);
    }

    #region Weapon Actions
    #region Entrance For Weapons
    [Client]
    public void OnPrimaryWeapon() {
        if (!isLocalPlayer) {
            return;
        }
        CmdOnPrimaryWeapon();
    }
    [Client]
    public void DoPrimaryWeaponEmptyClipEffect() {
        if (!isLocalPlayer) {
            return;
        }
        CmdDoPrimaryWeaponEmptyClipEffect();
    }
    [Client]
    public void OnPrimaryWeaponHit(Vector3 _hitPosition, Vector3 _normalOfSurface) {
        if (!isLocalPlayer) {
            return;
        }
        CmdOnPrimaryWeaponHit(_hitPosition, _normalOfSurface);
    }
    [Client]
    public void OnReloadWeapon() {
        if (!isLocalPlayer) {
            return;
        }
        CmdOnReloadWeapon();
    }
    [Client]
    public void OnPlayerShotWithWeapon(string _playerID, float _damage, string _sourceID) {
        if (!isLocalPlayer) {
            return;
        }
        CmdOnPlayerShotWithWeapon(_playerID, _damage, _sourceID);
    }
    #endregion
    #region Commands
    [Command]
    void CmdOnPrimaryWeapon() {
        RpcDoPrimaryWeaponEffect();
    }
    [Command]
    void CmdDoPrimaryWeaponEmptyClipEffect() {
        RpcDoPrimaryWeaponEmptyClipEffect();
    }
    [Command]
    void CmdOnPrimaryWeaponHit(Vector3 _hitPosition, Vector3 _normalOfSurface) {
        RpcDoPrimaryHitEffect(_hitPosition, _normalOfSurface);
    }
    [Command]
    void CmdOnReloadWeapon() {
        RpcOnReloadWeapon();
    }
    [Command]   // only called on the server!
    void CmdOnPlayerShotWithWeapon(string _playerID, float _damage, string _sourceID) {
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage, _sourceID);
    }

    public void CancelAllActionsOnDeath() {
        if (!isLocalPlayer) {
            return;
        }
        CancelInvoke("Primary");
    }
    #endregion
    #region ClientRPCs
    // Do on primary effect on all clients
    [ClientRpc]
    void RpcDoPrimaryWeaponEffect() {
        GameObject _muzzleFlash = Instantiate(((ItemWeapon)currentItem).muzzleFlash, ((ItemWeapon)currentItem).actionOrigin.position, Quaternion.LookRotation(((ItemWeapon)currentItem).actionOrigin.position));
        if (isLocalPlayer) {
            _muzzleFlash.layer = LayerMask.NameToLayer(PLAYER_ITEM_LAYER);
        }
        AudioSource _audio = Instantiate(((ItemWeapon)currentItem).primaryAudio, ((ItemWeapon)currentItem).actionOrigin.transform, false);
        _audio.spatialBlend = 1.0f;
        _audio.Play();
        Destroy(_muzzleFlash, 1f);
        Destroy(_audio.gameObject, 1f);
    }

    [ClientRpc]
    void RpcDoPrimaryWeaponEmptyClipEffect() {
        AudioSource _audio = Instantiate(((ItemWeapon)currentItem).emptyClipAudio, currentItem.transform, false);
        _audio.spatialBlend = 1.0f;
        _audio.Play();
        Destroy(_audio.gameObject, 1f);
    }

    // Do on primary hit effect on all clients
    [ClientRpc]
    void RpcDoPrimaryHitEffect(Vector3 _hitPosition, Vector3 _normalOfSurface) {
        // TODO: Instantiation objects takes a lot of processing power
        // Look into "object pooling"
        GameObject _hitEffect = Instantiate(((ItemWeapon)currentItem).hitEffectPrefab, _hitPosition, Quaternion.LookRotation(_normalOfSurface));
        //AudioSource.PlayClipAtPoint(((ItemWeapon)currentItem).primaryImpactAudio.clip, _hitPosition, 0.5f);
        AudioSource _audio = Instantiate(((ItemWeapon)currentItem).primaryImpactAudio, _hitPosition, Quaternion.identity);
        _audio.spatialBlend = 1.0f;
        _audio.Play();
        Destroy(_hitEffect, 1f);
        Destroy(_audio.gameObject, 1f);
    }

    [ClientRpc]
    void RpcOnReloadWeapon() {
        AudioSource _audio = Instantiate(((ItemWeapon)currentItem).reloadAudio, currentItem.transform, false);
        _audio.spatialBlend = 1.0f;
        _audio.Play();
        Animator _animator = currentItem.GetComponent<Animator>();
        if (_animator != null) {
            _animator.SetTrigger("Reload");
        }
        Destroy(_audio.gameObject, ((ItemWeapon)currentItem).weaponReloadTime);
    }
    #endregion
    #endregion
}
