using UnityEngine;
using UnityEngine.UI;

public class UserAccountLobby : MonoBehaviour {

    public Text userNameText;

    void Start() {
        if (UserAccountManager.isLoggedIn) {
            userNameText.text = UserAccountManager.playerUsername;
        }
    }

    public void LogOut() {
        if (UserAccountManager.isLoggedIn) {
            UserAccountManager.instance.LogOut();
        }
    }
    
    public void ExitGame() {
        if (UserAccountManager.isLoggedIn) {
            UserAccountManager.instance.LogOut();
        }
        Application.Quit();
    }
}
