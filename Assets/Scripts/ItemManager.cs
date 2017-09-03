using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemManager : NetworkBehaviour {

    private const string PLAYER_ITEM_LAYER = "PlayerItem";

    [SerializeField]
	private Transform itemHolder;
    private GameObject currentItemInstance;

	//[SerializeField]
	//private PlayerItem primaryItem;
    [SerializeField]
    private PlayerItem[] items;

	private PlayerItem currentItem;
    private int selectedItemIndex = 0;
    private ItemGraphics currentItemGraphics;
    private ItemSounds currentItemSounds;

	public bool isReloading = false;

	void Start() { 
        if (!isLocalPlayer) {
            return;
        }
		CmdOnEquipItem(0);
        if (isLocalPlayer) {
            foreach (PlayerItem _item in items) {
                _item.SetWeaponAmmoToMax();
            }
        }
	}

    //[Client]
    //void Update() {
    //    int _prevSelectedItemIndex = selectedItemIndex;

    //    if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
    //        if (selectedItemIndex >= items.Length - 1) {
    //            selectedItemIndex = 0;
    //        }
    //        else {
    //            selectedItemIndex++;
    //        }
    //    }
    //    if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
    //        if (selectedItemIndex <= 0) {
    //            selectedItemIndex = items.Length - 1;
    //        }
    //        else {
    //            selectedItemIndex--;
    //        }
    //    }

    //    if (_prevSelectedItemIndex != selectedItemIndex) {
    //        CmdOnEquipItem(selectedItemIndex);
    //    }
    //}
    
    [Command]
	public void CmdOnEquipItem (int  _itemIndex) {
        RpcOnEquipItem(_itemIndex);
    }

    [ClientRpc]
    void RpcOnEquipItem (int _itemIndex) {
        if (currentItemInstance != null) {
            Destroy(currentItemInstance);
        }

        PlayerItem _item = items[_itemIndex];

        if (_item == null) {
            Debug.LogError("ItemManager: Item with itemindex[" + _itemIndex + "] not found!");
        }

        currentItem = _item;

        GameObject _itemInstance = (GameObject)Instantiate(_item.itemGraphics, itemHolder.position, itemHolder.rotation);
        //GameObject _itemInstance = (GameObject)Instantiate(_item.itemGraphics, itemHolder.position, _item.transform.rotation);
        _itemInstance.transform.SetParent(itemHolder);
        currentItemInstance = _itemInstance;

        currentItemGraphics = _itemInstance.GetComponent<ItemGraphics>();
        if (currentItemGraphics == null) {
            Debug.LogError("ItemManager: No ItemGraphics component on item " + _itemInstance.name + "!");
        }
        currentItemSounds = _itemInstance.GetComponent<ItemSounds>();
        if (currentItemSounds == null) {
            Debug.LogError("ItemManager: No ItemSounds component on item " + _itemInstance.name + "!");
        }
        if (isLocalPlayer) {
            Util.SetLayerRecursively(_itemInstance, LayerMask.NameToLayer(PLAYER_ITEM_LAYER));
        }
    }

    public void Reload() {
        if (isReloading) {
            return;
        }

		StartCoroutine(ReloadCoroutine());
	}

	private IEnumerator ReloadCoroutine() {
		Debug.Log("Reloading...");

		isReloading = true;

		CmdOnReload();

		yield return new WaitForSeconds(currentItem.itemReloadTime);

		currentItem.itemAmmo = currentItem.itemMaxAmmo;

		isReloading = false;
	}

	[Command]
	void CmdOnReload () {
		RpcOnReload();
	}

	[ClientRpc]
	void RpcOnReload ()	{
		Animator _animator = currentItemGraphics.GetComponent<Animator>();
		if (_animator != null) {
			_animator.SetTrigger("Reload");
		}
    }

    public void ResetAmmo() {
        if (currentItem != null) {
            currentItem.itemAmmo = currentItem.itemMaxAmmo;
        }
    }

    public int GetItemsLength() {
        return items.Length;
    }

    public PlayerItem GetCurrentItem() {
        return currentItem;
    }

    public string GetCurrentItemName() {
        if (currentItem != null) {
            return currentItem.itemName;
        }
        return "None";
    }

    public ItemGraphics GetCurrentGraphics() {
        return currentItemGraphics;
    }

    public ItemSounds GetCurrentSounds() {
        return currentItemSounds;
    }

}
