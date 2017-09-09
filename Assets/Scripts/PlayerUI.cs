using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

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
    [HideInInspector]
    GuiSelectableItem[] selectableItems;

	[SerializeField]
	GameObject pauseMenu;
	[SerializeField]
	GameObject scoreboard;
    [SerializeField]
    GameObject inventory;

	private Player player;
	private PlayerController controller;
	private ItemManager itemManager;

	public void SetPlayer (Player _player) {
		player = _player;
		controller = player.GetComponent<PlayerController>();
		itemManager = player.GetComponent<ItemManager>();
	}

	void Start () {
		PauseMenu.isPauseMenu = false;
        Inventory.isInventory = false;
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
        }

        if (itemManager.isItemChangedGuiFlag) {
            RedrawSelectableItems();
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
        Inventory.isInventory = inventory.activeSelf;
        if (Inventory.isInventory) {
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

    void RedrawSelectableItems() {
        string[] _itemNames = itemManager.GetCurrentItemNames();
        for (int i = 0; i < 10; i++) {
            GuiSelectableItem _item = selectableItems[i];
            _item.itemNameText.text = _itemNames[i];
            Color _color = new Color(0f, 0f, 0f, 100f / 255f);
            if (i == itemManager.selectedItemGUIIndex) { //191 131 0
                _color.r = 191f/255f;
                _color.g = 131f/255f;
            }
            _item.itemIsSelectedImage.color = _color;
        }
        itemManager.isItemChangedGuiFlag = false;
    }

}
