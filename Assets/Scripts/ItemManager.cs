using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : NetworkBehaviour {

    private const string PLAYER_ITEM_LAYER = "PlayerItem";
    private const string DEFAULT_LAYER = "Default";

    [SerializeField]
	private Transform itemHolder;
    private GameObject currentItemInstance;
    
    [SerializeField]
    private Item[] items;
    [SerializeField]
    private Item[] inventoryItems;

	private Item currentItem;
    [HideInInspector]
    public int selectedItemIndex = 0;
    [HideInInspector]
	public bool isReloading = false;
    [HideInInspector]
    public bool isItemChangedGuiFlag = true;
    [HideInInspector]
    public bool isInventoryChangedGuiFlag = true;

    void Start() {
        if (!isLocalPlayer) {
            return;
        }
        foreach (Item _item in items) {
            if (_item == null) {
                continue;
            }
            // TODO: Entfernen!
            VisItemSetMaxAmmount _vis = new VisItemSetMaxAmmount();
            _item.Accept(_vis);
        }
        CmdOnEquipItem(0);
	}
    
    [Command]
	public void CmdOnEquipItem(int  _itemIndex) {
        RpcOnEquipItem(_itemIndex);
    }

    [ClientRpc]
    void RpcOnEquipItem(int _itemIndex) {
        StoreCurrentItemInstanceState();
        if (currentItemInstance != null) {
            Destroy(currentItemInstance);
        }

        Item _item = items[_itemIndex];
        if (isLocalPlayer) {
            isItemChangedGuiFlag = true;
            selectedItemIndex = _itemIndex;
        }
        if (_item == null) {
            currentItem = null;
            return;
        }

        GameObject _itemInstance = (GameObject)Instantiate(_item.gameObject, itemHolder.position, itemHolder.rotation);
        _itemInstance.transform.SetParent(itemHolder);
        currentItemInstance = _itemInstance;
        if (isLocalPlayer) {
            Util.SetLayerRecursively(_itemInstance, LayerMask.NameToLayer(PLAYER_ITEM_LAYER));
        }
        else {
            Util.SetLayerRecursively(_itemInstance, LayerMask.NameToLayer(DEFAULT_LAYER));
        }
        currentItem = currentItemInstance.GetComponent<Item>();
    }

    [ClientRpc]
    public void RpcAddItemToInventory() {
        foreach (Item _item in items) {
            if (_item == null) {

            }
        }
    }

    public int GetItemsLength() {
        return items.Length;
    }

    public Item GetCurrentItem() {
        return currentItem;
    }

    public GameObject GetCurrentItemInstance() {
        return currentItemInstance;
    }

    public string GetCurrentItemName() {
        if (currentItem != null) {
            return currentItem.itemName;
        }
        return "";
    }

    public string[] GetCurrentItemNames() {
        string[] _result = new string[10];
        int _i = 0;
        foreach (Item _item in items) {
            if (_item == null) {
                _result[_i] = "";
            }
            else {
                _result[_i] = _item.itemName;
            }
            _i++;
        }
        return _result;
    }

    public string[] GetCurrentInventoryItemNames() {
        string[] _result = new string[40];
        int _i = 0;
        foreach (Item _item in inventoryItems) {
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

    public GameObject[] GetCurrentItemImages() {
        GameObject[] _result = new GameObject[10];
        int _i = 0;
        foreach (Item _item in items) {
            if (_item == null) {
                _result[_i] = null;
            }
            else {
                _result[_i] = Instantiate(_item.itemImage);
            }
            _i++;
        }
        return _result;
    }

    public GameObject[] GetCurrentInventoryItemImages() {
        GameObject[] _result = new GameObject[30];
        int _i = 0;
        foreach (Item _item in inventoryItems) {
            if (_item == null) {
                _result[_i] = null;
            }
            else {
                _result[_i] = Instantiate(_item.itemImage);
            }
            _i++;
        }
        return _result;
    }

    public int GetSelectedItemIndex() {
        return selectedItemIndex;
    }

    public void ProcessSwitchedItems(ItemSwitchInfo _info) {
        Item _from = null;
        Item _to = null;
        bool _isCurrentSelectedItem = false;
        switch (_info.fromTypeID) {
            case 0:
                if (_info.fromID == selectedItemIndex) {
                    _isCurrentSelectedItem = true;
                    StoreCurrentItemInstanceState();
                }
                _from = items[_info.fromID];
                break;
            case 1:
                _from = inventoryItems[_info.fromID];
                break;
            default:
                Debug.LogError("ItemManager: ItemInfo 'fromTypeID' is unknown!");
                break;
        }
        switch (_info.toTypeID) {
            case 0:
                if (_info.toID == selectedItemIndex) {
                    _isCurrentSelectedItem = true;
                    StoreCurrentItemInstanceState();
                }
                _to = items[_info.toID];
                items[_info.toID] = _from;
                break;
            case 1:
                _to = inventoryItems[_info.toID];
                inventoryItems[_info.toID] = _from;
                break;
            default:
                Debug.LogError("ItemManager: ItemInfo 'fromTypeID' is unknown!");
                break;
        }
        switch (_info.fromTypeID) {
            case 0:
                items[_info.fromID] = _to;
                break;
            case 1:
                inventoryItems[_info.fromID] = _to;
                break;
            default:
                Debug.LogError("ItemManager: ItemInfo 'fromTypeID' is unknown!");
                break;
        }
        if (_isCurrentSelectedItem) {
            CmdOnEquipItem(selectedItemIndex);
        }
    }

    void StoreCurrentItemInstanceState() {
        if (isLocalPlayer) {
            if (currentItemInstance != null) {
                VisItemGetAmmo _visGet = new VisItemGetAmmo();
                currentItemInstance.GetComponent<Item>().Accept(_visGet);
                if (items[selectedItemIndex] != null) {
                    VisItemSetAmmo _visSet = new VisItemSetAmmo();
                    _visSet.currentAmmo = _visGet.currentClipSize;
                    items[selectedItemIndex].Accept(_visSet);
                }
            }
        }
    }
}
