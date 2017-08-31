using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DatabaseControl;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour {

    public static UserAccountManager instance;

    [SerializeField]
    public string loginSceneName = "LoginMenu";
    [SerializeField]
    public string mainMenuSceneName = "MainMenu";

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public static string playerUsername { get; protected set; }
    private static string playerPassword = "";

    public static string LoggedIn_data { get; protected set; }

    public static bool isLoggedIn { get; protected set; }

    public delegate void OnDataRecievedCallback(string _data);

    public void LogOut() {
        string _username = playerUsername;
        playerUsername = "";
        playerPassword = "";
        isLoggedIn = false;
        SceneManager.LoadScene(loginSceneName);
        Debug.Log("UserAccountManager: " + _username + " logged out.");
    }

    public void LogIn(string _username, string _password) {
        playerUsername = _username;
        playerPassword = _password;
        isLoggedIn = true;
        SceneManager.LoadScene(mainMenuSceneName);
        Debug.Log("UserAccountManager: " + playerUsername + " logged in.");
    }

    public void SendData(string _data) {
        if (isLoggedIn) {
            StartCoroutine(SetData(_data));
        }
    }

    IEnumerator SetData(string _data) {
        IEnumerator e = DCF.SetUserData(playerUsername, playerPassword, _data); // << Send request to set the player's data string. Provides the username, password and new data string
        while (e.MoveNext()) {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success") {
            //The data string was set correctly. Goes back to LoggedIn UI
            Debug.Log("Succes sending data");
            // loggedInParent.gameObject.SetActive(true);
        }
        else {
            Debug.LogError("UserAccountManager: Could not send data!");
        }
    }

    public void GetData(OnDataRecievedCallback _onDataReceived) {
        //Called when the player hits 'Get Data' to retrieve the data string on their account. Switches UI to 'Loading...' and starts coroutine to get the players data string from the server
        if (isLoggedIn)
            StartCoroutine(GetData_numerator(_onDataReceived));
    }

    IEnumerator GetData_numerator(OnDataRecievedCallback _onDataReceived) {
        string _data = "ERROR";
        IEnumerator e = DCF.GetUserData(playerUsername, playerPassword); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext()) {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error") {
            Debug.LogError("UserAccountManager: Could not get data!");
        }
        else {
            //The player's data was retrieved. Goes back to loggedIn UI and displays the retrieved data in the InputField
            _data = response;
        }

        // TODO: "LoggedIn_data = _data;" vielleicht entfernen!?
        LoggedIn_data = _data;
        if (_onDataReceived != null) {
            _onDataReceived.Invoke(_data);
        }
    }
}