using UnityEngine;
using UnityEngine.UI;

public class LoginControls : MonoBehaviour {

    [SerializeField]
    LoginMenu loginMenu;

    [SerializeField]
    InputField loginName;
    [SerializeField]
    InputField loginPassword;

    int selected;

	void Start () {
        loginName.Select();
        selected = 0;
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            switch (selected) {
                case 0:
                    loginPassword.Select();
                    selected = 1;
                    break;
                case 1:
                    loginName.Select();
                    selected = 0;
                    break;
                default:
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            loginMenu.Login_LoginButtonPressed();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            loginMenu.Login_ExitButtonPressed();
        }
    }

}
