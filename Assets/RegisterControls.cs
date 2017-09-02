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

    int selected;

    void Start() {
        registerName.Select();
        selected = 0;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            switch (selected) {
                case 0:
                    registerPassword.Select();
                    selected = 1;
                    break;
                case 1:
                    registerConfirmPassword.Select();
                    selected = 2;
                    break;
                case 2:
                    registerName.Select();
                    selected = 0;
                    break;
                default:
                    break;
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
