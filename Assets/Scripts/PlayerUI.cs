using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	RectTransform staminaBarFill;
    [SerializeField]
    Text staminaText;
	[SerializeField]
	RectTransform healthBarFill;
    [SerializeField]
    Text healthText;
    [SerializeField]
    RectTransform ammoBarFill;
    [SerializeField]
	Text ammoText;

	[SerializeField]
	GameObject pauseMenu;

	[SerializeField]
	GameObject scoreboard;

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
        pauseMenu.SetActive(false);
    }

	void Update () {
		SetStaminaAmount(controller.GetCurrentStaminaAmount(), controller.GetMaxStaminaAmount());
		SetHealthAmount(player.GetCurrentHealth(), player.GetMaxHealth());
		SetAmmoAmount(itemManager.GetCurrentItem().itemAmmo, itemManager.GetCurrentItem().itemMaxAmmo);

		if (Input.GetKeyDown(KeyCode.Escape)) {
			TogglePauseMenu();
		}

		if (Input.GetKeyDown(KeyCode.Tab)) {
			scoreboard.SetActive(true);
        } else if (Input.GetKeyUp(KeyCode.Tab)) {
			scoreboard.SetActive(false);
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

	void SetStaminaAmount(float _amount, float _max) {
		staminaBarFill.localScale = new Vector3(Util.GetValueInPct(_amount, _max), 1f, 1f);
        staminaText.text = "Stamina (" + Mathf.RoundToInt(_amount) + "/" + Mathf.RoundToInt(_max) + ")";
    }

	void SetHealthAmount (float _amount, float _max) {
		healthBarFill.localScale = new Vector3(Util.GetValueInPct(_amount, _max), 1f, 1f);
        healthText.text = "Health (" + Mathf.RoundToInt(_amount) + "/" + Mathf.RoundToInt(_max) + ")";
    }

	void SetAmmoAmount (int _amount, int _max) {
        ammoBarFill.localScale = new Vector3(1f, Util.GetValueInPct(_amount, _max), 1f);
        ammoText.text = _amount.ToString() + " / " + _max.ToString();
	}

}
