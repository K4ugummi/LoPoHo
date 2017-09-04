using UnityEngine;
using UnityEngine.UI;

public class LoginControls : MonoBehaviour {

    [SerializeField]
    LoginMenu loginMenu;

    [SerializeField]
    InputField loginName;
    [SerializeField]
    InputField loginPassword;

	void Start () {
        loginName.Select();
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (loginName.isFocused) {
                loginPassword.Select();
            }
            else if (loginPassword.isFocused) {
                loginName.Select();
            }
            else {
                loginName.Select();
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
