using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemManager : NetworkBehaviour {

    private const string PLAYER_ITEM_LAYER = "PlayerItem";

    [SerializeField]
	private Transform itemHolder;

	[SerializeField]
	private PlayerItem primaryItem;

	private PlayerItem currentItem;
	private ItemGraphics currentItemGraphics;
    private ItemSounds currentItemSounds;

	public bool isReloading = false;

	void Start () { 
		EquipItem(primaryItem);
	}

	public PlayerItem GetCurrentItem() {
		return currentItem;
	}

	public ItemGraphics GetCurrentGraphics() {
		return currentItemGraphics;
	}

    public ItemSounds GetCurrentSounds() {
        return currentItemSounds;
    }

	void EquipItem (PlayerItem _item) {
		currentItem = _item;

		GameObject _itemInstance = (GameObject)Instantiate(_item.graphics, itemHolder.position, itemHolder.rotation);
        _itemInstance.transform.SetParent(itemHolder);

		currentItemGraphics = _itemInstance.GetComponent<ItemGraphics>();
        if (currentItemGraphics == null) {
            Debug.LogError("ItemManager: No ItemGraphics component on item " + _itemInstance.name + "!");
        }
        currentItemSounds = _itemInstance.GetComponent<ItemSounds>();
        if (currentItemGraphics == null) {
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

		yield return new WaitForSeconds(currentItem.reloadTime);

		currentItem.ammo = currentItem.maxAmmo;

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
        currentItem.ammo = currentItem.maxAmmo;
    }

}
