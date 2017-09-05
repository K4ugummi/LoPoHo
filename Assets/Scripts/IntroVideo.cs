using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroVideo : MonoBehaviour {

    void Start() {
        Util.HideCursor();
        StartCoroutine(WaitAndLoad(7.7f, "LoginMenu"));
    }

    void Update() {
        if (Input.anyKey) {
            Util.ShowCursor();
            SceneManager.LoadScene("LoginMenu");
        }
    }

    IEnumerator WaitAndLoad(float value, string scene) {
        yield return new WaitForSeconds(value);
        Util.ShowCursor();
        SceneManager.LoadScene(scene);
    }
}