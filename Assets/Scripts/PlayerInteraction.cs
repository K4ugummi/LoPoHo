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
    private Item currentItem;
    private ItemManager itemManager;

    void Start() {
        if (cam == null) {
            Debug.LogError("Playershoot: No camera referenced!");
            this.enabled = false;
        }

        itemManager = GetComponent<ItemManager>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F12)) {
            if (isLocalPlayer) {
                Util.TakeScreenshot();
            }
        }

        currentItem = itemManager.GetCurrentItem();

        if (PauseMenu.isPauseMenu) {
            return;
        }
        #region Item Action
        if (currentItem != null) {
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

            if (Input.GetButtonDown("MousePrimary")) {
                Debug.Log("MousePrimary pressed!");
                VisItemPrimary _visItemPrimary = new VisItemPrimary();
                currentItem.Accept(_visItemPrimary);
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
        #region ItemSelect
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
        #endregion
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

    //// Is called on the server, when a player uses his primary mouse button
    //[Command]
    //void CmdOnPrimary() {
    //    RpcDoPrimaryEffect();
    //}

    //// Do on primary effect on all clients
    //[ClientRpc]
    //void RpcDoPrimaryEffect() {
    //    PlayerItem _item = itemManager.GetCurrentItem();
    //    if (_item == null) {
    //        return;
    //    }
    //    // TODO: FIX THIS SHIT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //    _item.GetMuzzleFlash().Play();
    //    AudioSource.PlayClipAtPoint(_item.primaryAudio.clip, transform.position, 0.5f);
    //}

    //// Is called on the server, when the primary mouse button action has hit something
    //[Command]
    //void CmdOnPrimaryHit(Vector3 _hitPosition, Vector3 _normalOfSurface) {
    //    RpcDoPrimaryHitEffect(_hitPosition, _normalOfSurface);
    //}

    //// Do on primary hit effect on all clients
    //[ClientRpc]
    //void RpcDoPrimaryHitEffect(Vector3 _hitPosition, Vector3 _normalOfSurface) {
    //    PlayerItem _item = itemManager.GetCurrentItem();
    //    if (_item == null) {
    //        return;
    //    }
    //    // TODO: Instantiation objects takes a lot of processing power
    //    // Look into "object pooling"
    //    GameObject _hitEffect = Instantiate(_item.hitEffectPrefab, _hitPosition, Quaternion.LookRotation(_normalOfSurface));
    //    AudioSource.PlayClipAtPoint(_item.primaryImpactAudio.clip, _hitPosition, 0.5f);
    //    Destroy(_hitEffect, 1f);
    //}

    //[Command]   // only called on the server!
    //void CmdOnPlayerShot(string _playerID, int _damage, string _sourceID) {
    //    Player _player = GameManager.GetPlayer(_playerID);
    //    _player.RpcTakeDamage(_damage, _sourceID);
    //} 

    public void CancelAllActionsOnDeath() {
        if (!isLocalPlayer) {
            return;
        }
        CancelInvoke("Primary");
    }
}
