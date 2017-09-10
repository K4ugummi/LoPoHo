using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUIPlayer : MonoBehaviour {

    [SerializeField]
    RectTransform staminaBarFill;
    [SerializeField]
    TMP_Text staminaText;
    [SerializeField]
    RectTransform healthBarFill;
    [SerializeField]
    TMP_Text healthText;
    [SerializeField]
    RectTransform ammoBarFill;
    [SerializeField]
    TMP_Text weaponText;
    [SerializeField]
    TMP_Text ammoText;
    [SerializeField]
    DragSlot[] selectableItemSlots;
    [SerializeField]
    DragSlot[] inventoryItemSLots;
    [SerializeField]
	GameObject pauseMenu;
	[SerializeField]
	GameObject scoreboard;
    [SerializeField]
    GameObject inventory;
    [SerializeField]
    public GameObject crosshairInteractable;

	private Player player;
	private PlayerController controller;
	private ItemManager itemManager;
    private PlayerInteraction playerInteraction;

	public void SetPlayer (Player _player) {
		player = _player;
		controller = player.GetComponent<PlayerController>();
		itemManager = player.GetComponent<ItemManager>();
        playerInteraction = player.GetComponent<PlayerInteraction>();
	}

	void Start () {
		PauseMenu.isPauseMenu = false;
        GUIInventory.isInventory = false;
        pauseMenu.SetActive(false);
        inventory.SetActive(false);
    }

	void Update () {
		SetStaminaAmount(controller.GetCurrentStaminaAmount(), controller.GetMaxStaminaAmount());
		SetHealthAmount(player.GetCurrentHealth(), player.GetMaxHealth());

        if (itemManager.GetCurrentItem() != null) {
            SetItemName(itemManager.GetCurrentItem().itemName);
            VisItemAmmo _visItemAmmo = new VisItemAmmo();
            itemManager.GetCurrentItemInstance().GetComponent<Item>().Accept(_visItemAmmo);
            SetAmmoAmount(_visItemAmmo.currentClipSize, _visItemAmmo.maxClipSize);
        }
        else {
            SetItemName("None");
            SetAmmoAmount(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
			TogglePauseMenu();
		}

		if (Input.GetKeyDown(KeyCode.Tab)) {
			scoreboard.SetActive(true);
        } else if (Input.GetKeyUp(KeyCode.Tab)) {
			scoreboard.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            ToggleInventory();
            RedrawItems();
        }

        if (itemManager.isItemChangedGuiFlag) {
            RedrawItems();
        }

        if (playerInteraction.GetCanInteract()) {
            crosshairInteractable.SetActive(true);
        }
        else {
            crosshairInteractable.SetActive(false);
        }
	}

	public void TogglePauseMenu () {
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		PauseMenu.isPauseMenu = pauseMenu.activeSelf;
        if (PauseMenu.isPauseMenu) {
            Util.ShowCursor();
        }
        else {
            Util.HideCursor();
        }
    }

    public void ToggleInventory() {
        inventory.SetActive(!inventory.activeSelf);
        GUIInventory.isInventory = inventory.activeSelf;
        if (GUIInventory.isInventory) {
            Util.ShowCursor();
            Cursor.lockState = CursorLockMode.None;
        }
        else {
            Util.HideCursor();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

	void SetStaminaAmount(float _amount, float _max) {
		staminaBarFill.localScale = new Vector3(Util.GetValueInPct(_amount, _max), 1f, 1f);
        staminaText.text = "Stamina (" + Mathf.RoundToInt(_amount) + "/" + Mathf.RoundToInt(_max) + ")";
    }

	void SetHealthAmount (float _amount, float _max) {
		healthBarFill.localScale = new Vector3(Util.GetValueInPct(_amount, _max), 1f, 1f);
        healthText.text = "Health (" + Mathf.RoundToInt(_amount) + "/" + Mathf.RoundToInt(_max) + ")";
    }

    void SetItemName(string _name) {
        weaponText.text = _name;
    }

    void SetAmmoAmount (int _amount, int _max) {
        if (_max == 0 && _amount == 0) {
            ammoBarFill.localScale = new Vector3(1f, 0f, 1f);
            ammoText.text = "- / -";
        }
        else {
            ammoBarFill.localScale = new Vector3(1f, Util.GetValueInPct(_amount, _max), 1f);
            ammoText.text = _amount.ToString() + " / " + _max.ToString();
        }
	}

    void RedrawItems() {
        string[] _itemNames = itemManager.GetCurrentItemNames();
        GameObject[] _itemImages = itemManager.GetCurrentItemImages();
        for (int i = 0; i < 10; i++) {
            DragSlot _itemSlot = selectableItemSlots[i];
            if (_itemSlot.transform.childCount > 0) {
                Destroy(_itemSlot.transform.GetChild(0).gameObject);
            }
            if (_itemImages[i] != null) {
                GameObject _currentItemImage = _itemImages[i];
                _currentItemImage.transform.SetParent(_itemSlot.transform);
                _currentItemImage.transform.position = _itemSlot.transform.position;
                _itemSlot.dragSlotHint = _itemNames[i];
            }

            Color _color = new Color(0f, 0f, 0f, 100f / 255f);
            if (i == itemManager.selectedItemIndex) { //191 131 0
                _color.r = 191f/255f;
                _color.g = 131f/255f;
            }
            _itemSlot.GetComponent<Image>().color = _color;
        }

        if (inventory.activeSelf) {
            string[] _inventoryItemNames = itemManager.GetCurrentInventoryItemNames();
            GameObject[] _inventoryItemImages = itemManager.GetCurrentInventoryItemImages();
            for (int i = 0; i < 30; i++) {
                DragSlot _inventoryItemSlot = inventoryItemSLots[i];
                if (_inventoryItemSlot.transform.childCount > 0) {
                    Destroy(_inventoryItemSlot.transform.GetChild(0).gameObject);
                }
                if (_inventoryItemImages[i] != null) {
                    GameObject _currentItemImage = _inventoryItemImages[i];
                    _currentItemImage.transform.SetParent(_inventoryItemSlot.transform);
                    _currentItemImage.transform.position = _inventoryItemSlot.transform.position;
                    _inventoryItemSlot.dragSlotHint = _inventoryItemNames[i];
                }
            }
        }

        itemManager.isItemChangedGuiFlag = false;
    }

    public void ProcessSwitchedItems(ItemSwitchInfo _info) {
        itemManager.ProcessSwitchedItems(_info);
    }

}
