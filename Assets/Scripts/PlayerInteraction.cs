using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(ItemManager))]
public class PlayerInteraction : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

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

    void Update() {
        currentItemInstance = itemManager.GetCurrentItemInstance();
        if (Input.GetKeyDown(KeyCode.F12)) {
            Util.TakeScreenshot();
        }
        if (PauseMenu.isPauseMenu) {
            return;
        }
        #region Item Action
        if (currentItemInstance != null) {
            currentItem = currentItemInstance.GetComponent<Item>();
            //if (currentItem.itemAmmo < currentItem.itemMaxAmmo) {
            //    if (Input.GetButtonDown("Reload")) {
            //        itemManager.Reload();
            //        return;
            //    }
            //}
            //if (currentItem.itemFireRate <= 0f) {
            //    if (Input.GetButtonDown("MousePrimary")) {
            //        Primary();
            //    }
            //}
            //else {
            //    if (Input.GetButtonDown("MousePrimary")) {
            //        InvokeRepeating("Primary", 0f, 1 / currentItem.itemFireRate);
            //    }
            //    else if (Input.GetButtonUp("MousePrimary")) {
            //        CancelInvoke("Primary");
            //    }
            //}
            if (Input.GetButtonDown("Reload")) {
                VisItemReload _visItemReload = new VisItemReload();
                currentItem.Accept(_visItemReload);
            }

            if (Input.GetButtonDown("MousePrimary")) {
                VisItemPrimary _visItemPrimaryDown = new VisItemPrimary();
                currentItem.Accept(_visItemPrimaryDown);
                //CmdOnPrimary(_visItemPrimaryDown);
            }
            else if (Input.GetButtonUp("MousePrimary")) {

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
            itemManager.selectedItemGUIIndex = selectedItemIndex;
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

    [Client]
    void Primary() {
        VisItemPrimary _visItemPrimaryDown = new VisItemPrimary();
        currentItem.Accept(_visItemPrimaryDown);
    }

    #region Weapon
    [Command]
    public void CmdOnPrimaryWeapon() {
        RpcDoPrimaryWeaponEffect();
    }
    [Command]
    public void CmdDoPrimaryWeaponEmptyClipEffect() {
        RpcDoPrimaryWeaponEmptyClipEffect();
    }
    [Command]
    public void CmdOnPrimaryWeaponHit(Vector3 _hitPosition, Vector3 _normalOfSurface) {
        RpcDoPrimaryHitEffect(_hitPosition, _normalOfSurface);
    }
    [Command]
    public void CmdOnReloadWeapon() {
        RpcOnReloadWeapon();
    }

    // Do on primary effect on all clients
    [ClientRpc]
    void RpcDoPrimaryWeaponEffect() {
        // TODO: FIX THIS SHIT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //_item.GetMuzzleFlash().Play();
        AudioSource.PlayClipAtPoint(((ItemWeapon)currentItem).primaryAudio.clip, currentItem.transform.position, 0.5f);
    }

    [ClientRpc]
    void RpcDoPrimaryWeaponEmptyClipEffect() {
        AudioSource.PlayClipAtPoint(((ItemWeapon)currentItem).emptyClipAudio.clip, currentItem.transform.position, 0.5f);
    }

    // Do on primary hit effect on all clients
    [ClientRpc]
    void RpcDoPrimaryHitEffect(Vector3 _hitPosition, Vector3 _normalOfSurface) {
        // TODO: Instantiation objects takes a lot of processing power
        // Look into "object pooling"
        GameObject _hitEffect = Instantiate(((ItemWeapon)currentItem).hitEffectPrefab, _hitPosition, Quaternion.LookRotation(_normalOfSurface));
        AudioSource.PlayClipAtPoint(((ItemWeapon)currentItem).primaryImpactAudio.clip, _hitPosition, 0.5f);
        Destroy(_hitEffect, 1f);
    }

    [Command]   // only called on the server!
    public void CmdOnPlayerShotWithWeapon(string _playerID, float _damage, string _sourceID) {
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage, _sourceID);
    }

    public void CancelAllActionsOnDeath() {
        if (!isLocalPlayer) {
            return;
        }
        CancelInvoke("Primary");
    }

    [ClientRpc]
    void RpcOnReloadWeapon() {
        AudioSource.PlayClipAtPoint(((ItemWeapon)currentItem).reloadAudio.clip, currentItem.transform.position, ((ItemWeapon)currentItem).weaponReloadTime);
        Animator _animator = currentItem.GetComponent<Animator>();
        if (_animator != null) {
            _animator.SetTrigger("Reload");
        }
    }
    #endregion
}
