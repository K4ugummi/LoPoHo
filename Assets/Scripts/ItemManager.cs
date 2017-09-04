using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemManager : NetworkBehaviour {

    private const string PLAYER_ITEM_LAYER = "PlayerItem";

    [SerializeField]
	private Transform itemHolder;
    private GameObject currentItemInstance;
    
    [SerializeField]
    private PlayerItem[] items;

	private PlayerItem currentItem;
    private int selectedItemIndex = 0;
    [HideInInspector]
    public int selectedItemGUIIndex = 0;
    [HideInInspector]
	public bool isReloading = false;
    [HideInInspector]
    public bool isItemChangedGuiFlag = true;

    void Start() { 
        if (!isLocalPlayer) {
            return;
        }
		CmdOnEquipItem(0);
        foreach (PlayerItem _item in items) {
            if (_item == null) {
                continue;
            }
            _item.SetWeaponAmmoToMax();
        }
	}
    
    [Command]
	public void CmdOnEquipItem(int  _itemIndex) {
        RpcOnEquipItem(_itemIndex);
    }

    [ClientRpc]
    void RpcOnEquipItem(int _itemIndex) {
        if (currentItemInstance != null) {
            Destroy(currentItemInstance);
        }

        PlayerItem _item = items[_itemIndex];

        currentItem = _item;
        if (isLocalPlayer) {
            isItemChangedGuiFlag = true;
            selectedItemGUIIndex = _itemIndex;
        }
        if (currentItem == null) {
            return;
        }

        GameObject _itemInstance = (GameObject)Instantiate(_item.itemGraphics, itemHolder.position, itemHolder.rotation);
        _itemInstance.transform.SetParent(itemHolder);
        currentItemInstance = _itemInstance;
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
		Animator _animator = currentItem.GetComponent<Animator>();
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

    public string[] GetCurrentItemNames() {
        string[] _result = new string[10];
        int _i = 0;
        foreach (PlayerItem _item in items) {
            if (_item == null) {
                _result[_i] = "None";
            }
            else {
                _result[_i] = _item.itemName;
            }
            _i++;
        }
        return _result;
    }

    public int GetSelectedItemIndex() {
        return selectedItemIndex;
    }

}
