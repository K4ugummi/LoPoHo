using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Player))]
public class PlayerNameplate : MonoBehaviour {

	[SerializeField]
	private Text usernameText;

	[SerializeField]
	private RectTransform healthBarFill;

	[SerializeField]
	private Player player;
	
	// Update is called once per frame
	void Update () {
        usernameText.text = player.userName;
		healthBarFill.localScale = new Vector3(Util.GetValueInPct(player.GetCurrentHealth(), player.GetMaxHealth()), 1f, 1f);

        // Rotate nameplate into the direction the main camera looks
        Camera _cam = Camera.main;
        transform.LookAt(transform.position + _cam.transform.rotation * Vector3.forward, _cam.transform.rotation * Vector3.up);
    }

}
