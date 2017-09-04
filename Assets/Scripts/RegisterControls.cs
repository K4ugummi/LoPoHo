using UnityEngine;
using UnityEngine.UI;

public class RegisterControls : MonoBehaviour {

    [SerializeField]
    LoginMenu loginMenu;

    [SerializeField]
    InputField registerName;
    [SerializeField]
    InputField registerPassword;
    [SerializeField]
    InputField registerConfirmPassword;

    void Start() {
        registerName.Select();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (registerName.isFocused) {
                registerPassword.Select();
            }
            else if (registerPassword.isFocused) {
                registerConfirmPassword.Select();
            }
            else if (registerConfirmPassword.isFocused) {
                registerName.Select();
            }
            else {
                registerName.Select();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Return)) {
            loginMenu.Register_RegisterButtonPressed();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            loginMenu.Register_BackButtonPressed();
        }
    }
}
